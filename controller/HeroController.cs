using static BaseController;
using static EquipController;
using LiteDB;
#pragma warning disable 8600, 8602, 8604
public class HeroController
{
    private Hero hero;
    private CityController cityController;
    public HeroController(Hero hero)
    {
        this.hero = hero;
        cityController = new CityController();
    }
    public void CommandInput()
    {
        Console.WriteLine("Início.");
        while (true)
        {
            Console.WriteLine("O que deseja fazer?");
            string[] opt = OptRead("Escolha uma opção").ToLower().Split("");
            switch (opt[0])
            {
                case "help":
                case "ajuda":
                case "h":
                    Console.WriteLine("Comandos:"
                        + "\n  Herói: abre os comandos do herói."
                        + "\n  Cidade: abre comandos de interação com a cidade atual."
                        + "\n  Viajar: Viaja para uma cidade diferente." + "(Breve)"
                        + "\n  Voltar: Volta a tela inicial."
                        + "\n  Sair: Fecha o jogo."
                    );
                    break;
                case "herói":
                case "heroi":
                case "hero":
                case "me":
                    HeroInput();
                    break;
                case "cidade":
                case "city":
                    CityInput();
                    break;
                case "save":
                case "salvar":
                    SheetController.SaveSheet(hero, true);
                    break;
                case "voltar":
                case "back":
                case "volta":
                    return;
                case "exit":
                case "quit":
                case "sair":
                case "q":
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Comando desconhecido");
                    break;
            }
        }
    }

    public void HeroInput()
    {
        Console.WriteLine("Informações de herói.");
        bool isAgain = false;
        while (true)
        {
            Console.WriteLine("O que deseja saber" + (isAgain ? " mais" : " ") + "?");
            isAgain = true;
            string[] opt = OptRead("Escolha uma opção").ToLower().Split(" ");
            switch (opt[0])
            {
                case "voltar":
                case "back":
                case "volta":
                    return;
                case "heroi":
                case "herói":
                case "hero":
                case "me":
                    Console.WriteLine(hero.DisplayHeroInfo());
                    Console.WriteLine(hero.DisplayStatus());
                    break;
                case "inventário":
                case "inventario":
                case "inv":
                case "i":
                    List<Item> inv = hero.inventory.GetInv();
                    if (inv.Count == 0)
                        Console.WriteLine("Inventário vazio.");
                    else
                        hero.inventory.PrintInv();
                    break;
                case "equipamento":
                case "equipment":
                case "e":
                    Console.WriteLine(DispayEquipment(hero.inventory, hero.Equipment));
                    break;
                case "equipar":
                case "equip":
                case "eq":
                    int slot = 0;
                    List<int> itensType = new List<int>();
                    while (slot == 0 || slot < 0 || slot > 14)
                    {
                        string opt2;
                        try
                        {
                            opt2 = opt[1];
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            Console.WriteLine(DispayEquipment(hero.inventory, hero.Equipment, true));
                            Console.WriteLine("Escolha um espaço");
                            opt2 = OptRead("Deve escolher um espaço.").ToLower();
                        }
                        switch (opt2)
                        {
                            case "cabeça":
                            case "head":
                            case "1":
                                slot = 1;
                                itensType.Add(1);
                                break;
                            case "corpo":
                            case "body":
                            case "2":
                                slot = 2;
                                itensType.Add(2);
                                break;
                            case "shoulder":
                            case "ombro":
                            case "3":
                                slot = 3;
                                itensType.Add(3);
                                break;
                            case "wrist":
                            case "pulso":
                            case "4":
                                slot = 4;
                                itensType.Add(4);
                                break;
                            case "hand":
                            case "mão":
                            case "5":
                                slot = 5;
                                itensType.Add(5);
                                break;
                            case "legs":
                            case "leg":
                            case "pernas":
                            case "perna":
                            case "6":
                                slot = 6;
                                itensType.Add(6);
                                break;
                            case "feet":
                            case "pe":
                            case "pé":
                            case "7":
                                slot = 7;
                                itensType.Add(7);
                                break;
                            case "cape":
                            case "capa":
                            case "back":
                            case "costas":
                            case "8":
                                slot = 8;
                                itensType.Add(8);
                                break;
                            case "belt":
                            case "cinto":
                            case "cintura":
                            case "9":
                                slot = 9;
                                itensType.Add(9);
                                break;
                            case "rightRing":
                            case "aneldireito":
                            case "10":
                                slot = 10;
                                itensType.Add(11);
                                break;
                            case "anelesquerdo":
                            case "leftRing":
                            case "11":
                                slot = 11;
                                itensType.Add(11);
                                break;
                            case "pescoço":
                            case "neck":
                            case "12":
                                slot = 12;
                                itensType.Add(12);
                                break;
                            case "right":
                            case "rightHand":
                            case "direita":
                            case "mãodireita":
                            case "maodireita":
                            case "13":
                                slot = 13;
                                // itemType.Append(13);
                                itensType.AddRange(new[] { 21, 22, 23, 24, 25, 26, 27, 28, 31, 32, 33, 34, 41, 42, 43 });
                                break;
                            case "leftHand":
                            case "left":
                            case "esquerda":
                            case "mãoesquerda":
                            case "maoesquerda":
                            case "14":
                                slot = 14;
                                itensType.AddRange(new[] { 13, 21, 22, 23, 24, 25, 26, 27, 28, 31, 32, 33, 34, 41, 42, 43 });
                                break;
                            default:
                                Console.WriteLine("Deve-se escolher um espaço.");
                                break;
                        }
                    }
                    var db = new LiteDatabase("data.db");
                    var col = db.GetCollection<Item>(INVENTORY);
                    List<Item> itens = col.Query().Where(i => i.ownerId.Equals(hero.Id) && i.Qtd > 0).ToList();
                    itens.RemoveAll(i => !itensType.Contains((int)i.ItemType));
                    Dictionary<int, Item> itensDic = new Dictionary<int, Item>();
                    db.Dispose();
                    int index = 1;
                    foreach (Item i in itens)
                    {
                        Console.WriteLine((index) + ": " + i.ToString());
                        itensDic.Add(index++, i);
                    }
                    while (true)
                    {
                        if (itens.Count != 0)
                        {
                            Console.WriteLine("Escolha um item.");
                            try
                            {
                                Item item;
                                string opt2 = OptRead("Escolha um item.");
                                if (opt2.ToLower().Equals("cancelar"))
                                {
                                    break;
                                }
                                else if (new[] { "remove", "none", "remover", "0" }.Contains(opt2.ToLower()))
                                {
                                    RemoveEquip(slot, hero.Equipment);
                                }
                                else
                                {
                                    try
                                    {
                                        try
                                        {
                                            item = itensDic[int.Parse(opt2)];
                                        }
                                        catch (KeyNotFoundException)
                                        {
                                            Console.WriteLine("Item não encontrado.");
                                            continue;
                                        }
                                    }
                                    catch (FormatException)
                                    {
                                        item = itens.Find(i => i.Name.Equals(opt2));
                                    }
                                    try
                                    {
                                        ChangeEquip(item, slot, hero.Equipment);
                                    }
                                    catch (System.NullReferenceException)
                                    {
                                        Console.WriteLine("Item não encontrado.");
                                        continue;
                                    }
                                }
                            }
                            catch (System.ArgumentNullException)
                            {
                                Console.WriteLine("Item não encontrado.");
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nenhum item no inventário para esse espaço.");
                            if (GetEquipSlot(slot, hero.Equipment) != 0)
                            {
                                Console.WriteLine("Utilize o comando para remover o equipamento.");
                                string opt2 = OptRead("Escolha um item.");
                                if (opt2.ToLower().Equals("cancelar"))
                                {
                                    return;
                                }
                                else if (new[] { "remove", "none", "remover", "0" }.Contains(opt2.ToLower()))
                                {
                                    RemoveEquip(slot, hero.Equipment);
                                }
                            }
                        }
                        break;
                    }
                    break;
                case "drop":
                case "deixar":
                case "d":
                    List<Item> inv1 = hero.inventory.GetInv();
                    Item item1;
                    int qtd;
                    if (inv1.Count == 0)
                        Console.WriteLine("Inventário vazio.");
                    else
                        hero.inventory.PrintInv();
                    Console.WriteLine("Digite o item que deseja largar");
                    string opt3 = OptRead("Deve-se escolher um item");
                    if (new[] { "cancelar", "can" }.Contains(opt3.ToLower()))
                        break;
                    else
                    {
                        int inputInt1 = 0;
                        string inputStr1 = "";
                        try
                        {
                            inputInt1 = int.Parse(opt3);
                        }
                        catch (FormatException)
                        {
                            inputStr1 = opt3;
                        }
                        item1 = inv1.Find(i => i.Name.Equals(inputStr1) || i.Id.Equals(inputInt1));
                        while (true)
                        {
                            try
                            {
                                Console.WriteLine("Digite a quantidade que deseja largar.");
                                opt3 = OptRead("Deve-se digitar um quantidade.");
                                qtd = int.Parse(opt3);
                                if (qtd < 1)
                                {
                                    Console.WriteLine("Escolha outra quantidade.");
                                    continue;
                                }
                                break;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("A quantidade deve ser um número válido.");
                            }
                        }
                        hero.inventory.DropItem(item1, qtd, hero.Equipment);
                    }
                    break;
                case "help":
                case "ajuda":
                case "h":
                    Console.WriteLine("Comandos:"
                        + "\n  Herói: Mostra as informações os status."
                        + "\n  Inventário: Visualiza o inventário."
                        + "\n  Equipamento: Visualiza os equipamentos."
                        + "\n  Equipar: Equipa um item novo."
                        + "\n  GerarItem: adiciona um item aleatório ao inventário."
                        + "\n  Voltar: retorna à interação anterior."
                        + "\n  Sair: Fecha o jogo."
                    );
                    break;
                case "geraritem":
                case "medeitem":
                case "generateitem":
                case "item":
                    int itemCat = 0;
                    try
                    {
                        itemCat = (int)Enum.Parse(typeof(ItemType), opt[1], true);
                    }
                    catch (System.ArgumentException)
                    {
                        Console.WriteLine("Tipo de item não encontrado.");
                        break;
                    }
                    catch (System.IndexOutOfRangeException) { }

                    hero.inventory.AddItem(InventoryController.GerarItem(hero.Id, hero.Level - 5, hero.Level + 5, hero.RarityModifier(), hero.QualityModifier(), itemCat));
                    break;
                case "save":
                case "salvar":
                    SheetController.SaveSheet(hero, true);
                    break;
                case "exit":
                case "quit":
                case "sair":
                case "q":
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Comando desconhecido");
                    break;
            }
            SheetController.SaveSheet(hero, false);
            // if (command != 0) Command(command, secondOpt);
        }
    }

    public void CityInput()
    {
        List<City> cities = cityController.GetCities(hero.Id);
        City city = null;
        if (cities.Count == 0)
        {
            city = cityController.CreateCity(hero.Id, true, hero.Level, hero.RarityModifier(), hero.QualityModifier());
        }
        else
        {
            city = cities.Find(c => c.IsCurrent);
        }
        Console.WriteLine("Bem vindo, você está em " + city.Name);
        bool isAgain = false;
        while (true)
        {
            Console.WriteLine(isAgain ? "\nDeseja saber mais alguma coisa?" : "\nO que deseja fazer na cidade?");
            isAgain = true;
            string[] opt = OptRead("Escolha uma opção").ToLower().Split("");
            switch (opt[0])
            {
                case "help":
                case "ajuda":
                case "h":
                    Console.WriteLine("Comandos:"
                        + "\n  Nome: veja nome da cidade."
                        + "\n  Conexões: Veja quais lugares esta cidade está conectada"
                        + "\n  Serviços: Veja quais serviços esta cidade oferece."
                        + "\n  Viajar: Viaja para uma cidade diferente." + "(Breve)"
                        + "\n  Herói: Veja o status do seu herói."
                        + "\n  Voltar: Interação anterior:"
                        + "\n  Sair: Fecha o jogo."
                    );
                    break;
                case "nome":
                case "name":
                case "n":
                    Console.WriteLine("Você está em " + city.Name
                        + "\n Tamanho: " + (TamanhoCidade)city.Size
                    );
                    break;
                case "connectins":
                case "conexões":
                case "conexoes":
                case "conn":
                case "con":
                case "c":
                    Console.WriteLine("Total de conexões: " + city.Connections.Count);
                    foreach (int cityConnId in city.Connections)
                    {
                        Console.WriteLine(cityController.GetCity(cityConnId).Name);
                    }
                    cityController.SaveCity(city);
                    break;
                case "serviços":
                case "servicos":
                case "services":
                case "serv":
                case "s":
                    cityController.CityServicesController(city, hero);
                    break;
                case "herói":
                case "heroi":
                case "hero":
                case "me":
                    HeroInput();
                    break;
                case "voltar":
                case "back":
                case "volta":
                    return;
                case "exit":
                case "quit":
                case "sair":
                case "q":
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Comando desconhecido");
                    break;
            }
        }
    }
}