using LiteDB;
using System.Text.RegularExpressions;
using static BaseService;
#pragma warning disable 8600, 8602, 8604
public class HeroController
{
    private Hero _hero;
    private CityController cityController;
    private LiteDatabase _db;
    public InventoryController heroInventoryController;
    public EquipController equipController;

    public Hero Hero { get => _hero; }
    public HeroController(LiteDatabase db, Hero hero)
    {
        _db = db;
        _hero = hero;
        cityController = new CityController(db, this);
        heroInventoryController = new InventoryController(db, hero.Id, hero.Equip);
        equipController = new EquipController(db, hero.Equip);
    }
    public void CommandInput()
    {
        OptionInterface("O que deseja fazer?", "Escolha uma opção",
            new Option((o) => HeroInput(), true, "Herói: informações de herói", "herói", "heroi", "hero", "me"),
            new Option((o) => CityInput(), true, "Cidade: infomações sobre a cidade", "cidade", "city"),
            new Option((o) => SheetController.SaveSheet(_db, _hero, true), true, "Salvar: salva o jogo", "save", "salvar"),
            new Option((o) => false, "", returnOption)
        );
    }

    public void HeroInput()
    {
        OptionInterface("Informações de herói.", "Escolha uma opção",
            new Option((o) =>
                {
                    Console.WriteLine(_hero.DisplayHeroInfo());
                    Console.WriteLine(_hero.DisplayStatus());
                }, true
                , "Herói: informações de herói"
                , "heroi", "herói", "hero", "me"),
            new Option((o) =>
                {
                    List<Item> inv = heroInventoryController.Inventory.GetInv();
                    if (inv.Count == 0)
                        Console.WriteLine("Inventário vazio.");
                    else
                        heroInventoryController.Inventory.PrintInv();
                }, true
                , "Inventário: visualizar itens"
                , "inventário", "inventario", "inv", "i"),
            new Option((o) => Console.WriteLine(equipController.DispayEquipment())
                , true
                , ""
                , "equipamento", "equipment", "e"),
            new Option((o) =>
                {
                    bool cancel = false;
                    int slot = 0;
                    Option removeOption = new Option((o) => equipController.RemoveEquip(slot)
                        , false
                        , "0: remover"
                        , "remove", "none", "remover", "0");
                    Option cancelOption = new Option((o) => cancel = true
                        , false
                        , "cancelar"
                        , returnOption);
                    Console.WriteLine(equipController.DispayEquipment(true));
                    List<int> itensType = new List<int>();
                    // String slotTextBox = "1: cabeça\n2: corpo\n3: ombros\n4: pulso\n5: mãos\n6: pernas\n7: pé\n8: capa\n9: cinto\n10: anel direito\n11: anel esquerdo\n12: pescoço";
                    OptionInterface("Escolha um espaço",
                    "Deve-se escolher um espaço.",
                        new Option((o) => {
                            if(new Regex(@"\d+").IsMatch(o))
                                slot = int.Parse(o);
                            else if(new Regex(@"[\w_éáóíàò]+").IsMatch(o)){
                                try
                                {
                                    slot = (int)Enum.Parse<EquipSlot>(o);
                                }
                                catch(Exception e) when (e is ArgumentException | e is ArgumentNullException) {
                                    Console.WriteLine("Opção inválida, escolha outro espaço");
                                    return true;
                                }
                            }
                            itensType.AddRange(equipController.GetSlotType(slot));
                            if(itensType.Count <= 0){
                                Console.WriteLine("Opção inválida, escolha outro espaço");
                                return true;
                            }
                            if(o.Equals("13") || o.Equals("14"))
                                itensType.AddRange(equipController.GetSlotType(15));
                            return false;
                        }, "", new Regex(@"\d+|[\w_éáóíàò]+")),
                        cancelOption
                    );
                    if (cancel) return true;

                    List<Item> itens = heroInventoryController.Inventory.GetInv()
                        .Where(i => itensType.Contains((int)i.ItemType))
                        .ToList();

                    Dictionary<int, Item> itensDic = new Dictionary<int, Item>();
                    int index = 1;

                    if (itens.Count == 0)
                    {
                        Console.WriteLine("Nenhum item no inventário para esse espaço.");
                        if (equipController.GetEquipSlot(slot) != 0)
                            OptionInterface("Utilize o comando para remover o equipamento.",removeOption ,cancelOption);
                        else
                            return true;
                    }
                    if (cancel) return true;

                    string itensTextBox = "";
                    foreach (Item i in itens)
                    {
                        itensTextBox += (index) + ": " + i.ToString() + (itens.Count > index ? "\n" : "");
                        itensDic.Add(index++, i);
                    }
                    Item item = new Item();
                    OptionInterface("Escolha um item.",
                        new Option((o) =>
                        {
                            try
                            {
                                if (new Regex(@"\d+").IsMatch(o))
                                    item = itensDic[int.Parse(o)];
                                else
                                    item = itens.Find(i => i.Name.Equals(o));
                                equipController.ChangeEquip(item, slot);
                            }
                            catch (Exception e) when (e is NullReferenceException | e is KeyNotFoundException | e is ArgumentNullException)
                            {
                                Console.WriteLine("Item não encontrado.");
                                return true;
                            }
                            return false;
                        }
                        , itensTextBox
                        , new Regex(@"\d+")),
                        removeOption,
                        cancelOption
                    );
                    return true;
                }
                , "Equipar: Equipar arma, peças de armadura e jóias "
                , "equipar", "equip", "eq"),
            new Option((o) =>
                {
                    List<Item> inv = heroInventoryController.Inventory.GetInv();
                    Dictionary<int, Item> invDict = new Dictionary<int, Item>();
                    int index = 1;
                    foreach (Item i in inv)
                    {
                        invDict.Add(index++, i);
                    }
                    Item item;
                    if (inv.Count == 0)
                    {
                        Console.WriteLine("Inventário vazio.");
                        return true;
                    }
                    String itemTextblock = invDict.Select((kv) => kv.Key + ": " + kv.Value.Name + " [" + kv.Value.Qtd + "]").Aggregate((k1, k2) => k1 + "\n" + k2);
                    OptionInterface("Digite o item que deseja largar",
                    "Deve-se escolher um item",
                        new Option((o) =>
                        {
                            try
                            {
                                if (new Regex(@"\d+").IsMatch(o))
                                    item = invDict[int.Parse(o)];
                                else
                                    item = inv.Find(i => i.Name.Equals(o));
                            }
                            catch (Exception e) when (e is NullReferenceException
                                                    | e is KeyNotFoundException
                                                    | e is ArgumentNullException)
                            {
                                Console.WriteLine("Item não encontrado");
                                return true;
                            }
                            OptionInterface("Digite a quantidade que deseja largar.",
                            "Deve-se digitar um quantidade.",
                                new Option((o) =>
                                    {
                                        int qtd = int.Parse(o);
                                        if (qtd < 1 || qtd > item.Qtd)
                                        {
                                            Console.WriteLine("Escolha outra quantidade.");
                                            return true;
                                        }
                                        heroInventoryController.DropItem(item, qtd, _hero.Equip);
                                        return false;
                                    }, "", new Regex(@"\d+")),
                                new Option((o)=> false, "Voltar", returnOption)
                            );
                            return false;
                        }, itemTextblock, invDict.Keys.Select(k => k.ToString()).Union(inv.Select(i => i.Name)).ToArray())
                    );
                    return true;
                }
                , "Deixar: abandonar item"
                , "drop", "deixar", "d"),
            new Option((o) =>
                {
                    int itemCat = 0;
                    Console.Write("Digite o número do tipo de item\n> ");
                    try
                    {
                        string iCateg = Console.ReadLine();
                        itemCat = (int)Enum.Parse(typeof(ItemType), iCateg, true);
                    }
                    catch (Exception e) when (e is ArgumentException | e is IndexOutOfRangeException)
                    {
                        Console.WriteLine("Type de item não encontrado.");
                        return true;
                    }
                    Item item = InventoryController.GerarItem(
                        _hero.Id,
                        _hero.Level - 5,
                        _hero.Level + 5,
                        _hero.RarityModifier(),
                        _hero.QualityModifier(),
                        itemCat);
                    Console.WriteLine(item.Id + ", " + item);
                    heroInventoryController.AddItem(item);
                    return true;
                },
                "Geraritem: criar-se um item e adiciona-se ao inventário"
                , "geraritem", "medeitem", "generateitem", "item"),
            new Option((o) => SheetController.SaveSheet(_db, _hero, true)
                , true
                , "Save: salvar jogo"
                , "save", "salvar"),
            new Option((o) => false
                , "Voltar:"
                , returnOption)
        );
        SheetController.SaveSheet(_db, _hero, false);
        // if (command != 0) Command(command, secondOpt);

    }

    public void CityInput()
    {
        City city = null;
        city = cityController.cityService.GetCity(_hero.CurrentCity);
        Console.WriteLine("Bem vindo, você está em " + city.Name);
        OptionInterface("O que deseja fazer na cidade?",
            new Option((o) => Console.WriteLine("Você está em " + city.Name + "\n Tamanho: " + (TamanhoCidade)city.Size), true, "Nome: veja nome da cidade.", "nome", "name", "n"),
            new Option((o) =>
            {
                Console.WriteLine("Total de conexões: " + city.Connections.Count);
                foreach (int cityConn in city.Connections)
                    Console.WriteLine(cityController.cityService.GetCity(cityConn).Name);
            }, true, "Conexões: Veja quais lugares esta cidade está conectada", "connectins", "conexões", "conexoes", "conn", "con", "c"),
            new Option((o) => cityController.CityServicesController(city), true, "Serviços: Veja quais serviços esta cidade oferece.", "serviços", "servicos", "services", "serv", "s"),
            new Option((o) => true, "Viajar: Viaja para uma cidade diferente." + "(Breve)", "viajar", "travel", "t"),
            new Option((o) => HeroInput(), true, "Herói: Veja o status do seu herói.", "herói", "heroi", "hero", "me"),
            new Option((o) => false, "Voltar: Interação anterior.", returnOption)
        );
    }
}