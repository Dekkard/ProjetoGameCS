using LiteDB;
using System.Text.RegularExpressions;
using static BaseService;
#pragma warning disable 8600, 8602, 8603
public class CityController
{
    public CityService cityService;
    public ServiceService serviceService;
    public ItemService itemService;
    public HeroController _hero;
    public CityController(LiteDatabase db, HeroController hero)
    {
        this.cityService = new CityService(db);
        this.serviceService = new ServiceService(db);
        this.itemService = new ItemService(db, hero.Hero.Id);
        _hero = hero;
    }
    public City CreateCity(Hero heroId, bool current, int level, int rarity, int quality)
    {
        City city = new City();
        cityService.AssignCityConnections(city);
        serviceService.CreateServices(city, level, rarity, quality);
        cityService.CreateWorks(city, level, rarity, quality);
        cityService.AddCity(city);
        heroId.CurrentCity = city.Id;
        return city;
    }

    public void CityServicesController(City city)
    {
        List<Services> ListServices = cityService.GetServicesList(city);
        if (ListServices.Count == 0)
        {
            Console.WriteLine("Esta cidade não oferece serviço algum.");
            return;
        }
        Console.WriteLine("Serviços que esta cidade oferece.");
        Dictionary<int, Services> servDic = new Dictionary<int, Services>();
        int servIndex = 1;
        foreach (Services cityServices in ListServices)
        {
            Console.WriteLine(servIndex + ": " + cityServices.CityServices + "[" + (int)cityServices.CityServices + "]");
            servDic.Add(servIndex++, cityServices);
        }

        Services service = new Services();
        bool serviceSelectorCancel = false;

        OptionInterface("Escolha o serviço que deseja utilizar",
            new Option((o) =>
                {
                    try
                    {
                        if (new Regex(@"\d+").IsMatch(o))
                            service = servDic[int.Parse(o)];
                        else
                            service = servDic.Values.ToList()
                                .Find(s => s.CityServices.Equals(Enum.GetName(typeof(CityServices), o)));
                    }
                    catch (Exception e) when (e is ArgumentNullException
                                            | e is KeyNotFoundException
                                            | e is NullReferenceException)
                    {
                        Console.WriteLine("Serviço não compreende na lista.");
                        return true;
                    }
                    return false;
                }, "Serviço", new Regex(@"\d+")),
            new Option((o) => serviceSelectorCancel = true, false, "Voltar", returnOption)
        );
        if (serviceSelectorCancel) return;

        switch ((int)service.CityServices)
        {
            case 0:
                Console.WriteLine("Escolha quantos dias quer se recuperar neste estabelecimento.");
                int days;
                while (true)
                {
                    try
                    {
                        days = int.Parse(BWrite("Deve-se escolher quantos dias."));
                        if (days < 1)
                        {
                            Console.WriteLine("Escolha outra quantidade.");
                            continue;
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Quantidade inválida");
                    }
                }
                cityService.Estaleiro(days, _hero.Hero);
                break;
            case 1:
                StoreInterface(service);
                break;
            case 2:
                StoreInterface(service, 51, 52, 55);
                break;
            case 3:
                StoreInterface(service, 13, 21, 22, 23, 24, 25, 26, 27, 28, 54, 61, 64, 65, 66);
                break;
            case 4:
                StoreInterface(service, 52, 53, 62, 68, 69);
                break;
            case 5:
                StoreInterface(service, 31, 32, 33, 34, 61);
                break;
            case 6:
                StoreInterface(service, 41, 42, 43, 52, 69);
                break;
        }
    }

    public void StoreInterface(Services service, params int[] allowedItemType)
    {

        itemService.changeCaracter(service.Vendor);
        OptionInterface("Deseja comprar ou vender?", "Escolha um opção",
            new Option((o) =>
                {
                    List<Item> storeList = itemService.GetInv();
                    // List<Item> storeList = service.Inventory.Select(iv => itemService.GetItemById(iv)).ToList();
                    Dictionary<int, Item> storeDic = new Dictionary<int, Item>();
                    int index = 1;
                    foreach (Item item in storeList)
                        storeDic.Add(index++, item);
                    StoreBuyInterface(storeDic, _hero.Hero.Riches);
                }, true
                , "Comprar: Acessa a loja e selecione os item que deseja adquirir"
                , "comprar", "compra", "buy", "c", "b"),
            new Option((o) =>
                {
                    List<Item> heroInv = _hero.heroInventoryController.Inventory.GetInv();
                    if (allowedItemType.Length > 0)
                        heroInv = heroInv.Where(i => allowedItemType.Contains((int)i.ItemType)).ToList();
                    int index = 1;
                    Dictionary<int, Item> inventory = heroInv.ToDictionary(v => index++, item => item);
                    StoreSellInterface(inventory, service.Riches);
                }, true
                , "Vender: Seleciona os items do seu inventário para vender"
                , "vender", "vende", "sell", "v", "s"),
            new Option((o) => false
                , "Voltar: retorna à interação anterior"
                , returnOption)
        );
    }

    public void StoreBuyInterface(Dictionary<int, Item> store, Value pValue)
    {
        List<Item> cart = new List<Item>();
        Value total = new Value(0, 0, 0, 0);
        Console.WriteLine(pValue + ": total " + total);
        Console.WriteLine(store
            .Select(v => v.Key + ": " + v.Value)
            .Aggregate((v1, v2) => v1 + "\n" + v2));
        bool cancelService = false;
        OptionInterface("Escolha os itens que deseja adquirir",
            new Option((o) =>
                {
                    try
                    {
                        Item item;
                        int index;
                        Regex rNumber = new Regex(@"\d+");
                        if (rNumber.IsMatch(o))
                        {
                            index = int.Parse(o);
                            item = store[index];
                        }
                        else
                        {
                            var val = store.ToList().Find(i => i.Value.Name.Equals(o));
                            index = val.Key;
                            item = val.Value;
                        }
                        total += item.Value;
                        cart.Add(item);
                        store.Remove(index);
                        store = store.ToDictionary(v => v.Key > index ? v.Key - 1 : v.Key, v => v.Value);
                        if (store.Count >= 1)
                            Console.WriteLine(store
                            .Select(v => v.Key + ": " + v.Value)
                            .Aggregate((v1, v2) => v1 + "\n" + v2));
                        else
                            Console.WriteLine("Loja vazia");
                        return true;
                    }
                    catch (Exception e) when (e is ArgumentNullException
                                            | e is KeyNotFoundException
                                            | e is NullReferenceException
                                            | e is InvalidOperationException)
                    {
                        Console.WriteLine("Item não encontrado.");
                        return true;
                    }
                }
                , ""
                , new Regex(@"\d+")),
            new Option((o) => cart.ForEach(i => Console.WriteLine(i))
                , true
                , "lista: Listar itens a serem comprados"
                , "lista", "listar", "list", "ls"),
            new Option((o) => Console.WriteLine(pValue + ": total " + total + "\n" +
                (store.Count > 0 ? store
                    .Select(v => v.Key + ": " + v.Value)
                    .Aggregate((v1, v2) => v1 + "\n" + v2) : "Loja vazia"))
                , true
                , "loja: mostra itens disponíveis na loja"
                , "loja", "store", "lo", "st"),
            new Option((o) =>
                {
                    if (!_hero.Hero.Riches.CompareTo(total))
                    {
                        Console.WriteLine("Fundos insuficientes. Faça uma nova seleção");
                        cart.ForEach(i => store.Add(store.Count + 1, i));
                        total = new Value(0, 0, 0, 0);
                        cart.Clear();
                        return true;
                    }
                    return false;
                }
                , "Fechar: completar pedido"
                , "fechar", "close", "ok", "okay", "f"),
            new Option((o) =>
                {
                    Console.WriteLine("Transação cancelada");
                    cancelService = true;
                }, false
                , "Cancelar: cancelar pedido"
                , returnOption)
        );
        if (cart.Count <= 0 || cancelService) return;

        foreach (Item ic in cart)
        {
            Console.WriteLine(ic.Name);

        }

        OptionInterface("Comfirmar compra?",
            new Option((o) =>
                {
                    cart.ForEach(i => _hero.heroInventoryController.AddItem(i));
                    _hero.Hero.Riches -= total;
                }, false
                , "Sim"
                , posResp),
            new Option((o) => true
                , "Não"
                , negResp)
        );
    }

    public void StoreSellInterface(Dictionary<int, Item> inventory, Value vValue)
    {
        List<Item> cart = new List<Item>();
        Value total = new Value(0, 0, 0, 0);
        Console.WriteLine(vValue + ": total " + total);
        Console.WriteLine(inventory
            .Select(v => v.Key + ": " + v.Value)
            .Aggregate((v1, v2) => v1 + "\n" + v2));
        bool cancelService = false;
        OptionInterface("Escolha os itens que deseja vender",
            new Option((o) =>
                {
                    try
                    {
                        Item item;
                        int index;
                        Regex rNumber = new Regex(@"\d+");
                        if (rNumber.IsMatch(o))
                        {
                            index = int.Parse(o);
                            item = inventory[index];
                        }
                        else
                        {
                            var val = inventory.ToList().Find(i => i.Value.Name.Equals(o));
                            index = val.Key;
                            item = val.Value;
                        }
                        total += item.Value;
                        cart.Add(item);
                        inventory.Remove(index);
                        inventory = inventory.ToDictionary(v => v.Key > index ? v.Key - 1 : v.Key, v => v.Value);
                        if (inventory.Count > 0)
                            Console.WriteLine(inventory
                            .Select(v => v.Key + ": " + v.Value)
                            .Aggregate((v1, v2) => v1 + "\n" + v2));
                        else
                            Console.WriteLine("Inventário vazio");
                        return true;
                    }
                    catch (Exception e) when (e is ArgumentNullException
                                            | e is KeyNotFoundException
                                            | e is NullReferenceException
                                            | e is InvalidOperationException)
                    {
                        Console.WriteLine("Item não encontrado.");
                        return true;
                    }
                }
                , ""
                , new Regex(@"\d+")),
            new Option((o) =>
                {
                    if (cart.Count > 0)
                        cart.ForEach(i => Console.WriteLine(i));
                    else
                        Console.WriteLine("lista vazia");
                }
                , true
                , "lista: Listar itens a serem vendidos"
                , "lista", "listar", "list", "ls"),
            new Option((o) => Console.WriteLine(vValue + ": total " + total + "\n" + (inventory.Count > 0 ? inventory
                        .Select(v => v.Key + ": " + v.Value)
                        .Aggregate((v1, v2) => v1 + "\n" + v2) : "inventário vazio"))
                , true
                , "Inventário: mostra itens disponíveis no inventario"
                , "inventario", "inventary", "inv", "i"),
            new Option((o) =>
                {
                    if (!vValue.CompareTo(total))
                    {
                        Console.WriteLine("Fundos insuficientes. Faça uma nova seleção");
                        cart.ForEach(i => inventory.Add(inventory.Count + 1, i));
                        total = new Value(0, 0, 0, 0);
                        cart.Clear();
                        return true;
                    }
                    return false;
                }
                , "Fechar: completar pedido"
                , "fechar", "close", "ok", "okay", "f"),
            new Option((o) =>
                {
                    Console.WriteLine("Transação cancelada");
                    cancelService = true;
                    return false;
                }
                , "Cancelar: cancelar pedido"
                , returnOption)
        );
        if (cart.Count <= 0 || cancelService) return;

        foreach (Item ic in cart)
        {
            Console.WriteLine(ic.Name);

        }

        OptionInterface("Comfirmar venda?",
            new Option((o) =>
                {
                    cart.ForEach(i => itemService.changeItemOwnership(i));
                    _hero.Hero.Riches += total;
                }, false
                , "Sim"
                , posResp),
            new Option((o) => true
                , "Não"
                , negResp)
        );
    }

    public void CityWorksController(City city)
    {
        var col = cityService.db.GetCollection<Work>(WORKS);
        List<Work> listWorks = col.Query().Where(w => w.City.Equals(city)).ToList();
        Dictionary<int, Work> cwDict = new Dictionary<int, Work>();
        int index = 1;
        foreach (Work w in listWorks)
        {
            Console.WriteLine(index + ": " + (CityWorks)w.CityWorks);
            cwDict.Add(index++, w);
        }
        Work work = new Work();
        bool workCancel = false;
        OptionInterface("Em qual destas opções deseja trabalhar?",
            new Option((o) =>
                {
                    try
                    {
                        work = cwDict[int.Parse(o)];
                    }
                    catch (FormatException)
                    {
                        try
                        {
                            work = cwDict.Values.ToList().Find(w => w.CityWorks.Equals(o));
                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("Opção não encontrada.");
                            return true;
                        }
                    }
                    catch (Exception e) when (e is ArgumentNullException | e is KeyNotFoundException)
                    {
                        Console.WriteLine("Opção não encontrada.");
                        return true;
                    }
                    return true;
                }
                , ""
                , cwDict.Keys.Select(k => k.ToString()).ToArray()),
            new Option((o) => !(workCancel = true)
                , "Voltar: cancelar escolha"
                , returnOption)
        );
        if (workCancel) return;

        // Trabalho está relacionado aos atributos do herói, com isso, quando um trabalho é bem executado um ponto é adicionado ao atributo relacionado. Dependendo do trabalho, mais de um atributo é involvido.
        // Mineirador: Força e Resistência
        // Ferreiro: Força, Resistência e Percepção
        // Tecelão: Agilidade, percepção
        // Alfaiate: Agilidade, percepção e inteligência
        // Herbalista: Inteligência e Percepção
        // Alquimista: Inteligência, agilidade, percepção
        // Caçador: Agilidade e Percepção, Resistência
        // Coureiro: Resistência, Percepção
        // Orives: Percepção, agilidade e inteligência
        // Talheiro: Percepção, 
        // Madeireiro: Força e Resistência
        // Catalizante: Inteligência, percepção,
        Value richies = new Value(0, 0, 0, 0);
        int eOutcome;
        Random rng = new Random();
        List<Item> listGoodsFound = new List<Item>();
        switch ((int)work.CityWorks)
        {
            case 0:
                Console.WriteLine("As minas abertas aos arredores da " + ((TamanhoCidade)city.Size).ToString().ToLower() + " contribuem para o fornecimento de alguns metais e carvão para região. Trabalhar aqui exigirá muito do seu físico, a cada golpe de sua ferramenta irá drenar suas energias com grande facilidade se não o fizer com cuidado, porém, seus esforçor podem lhe render um bom dinheiro se fizer seu trabalho corretamente. Deseja continuar?");
                bool startWork = false;
                OptionInterface("Começar aventura agora?", "Sim ou não.",
                    new Option((o) => !(startWork = true), "Sim", posResp),
                    new Option((o) => false, "Não", negResp)
                );
                if (!startWork) return;
                if (rng.Next(20) > 15)
                {
                    switch (rng.Next(1, 10))
                    {
                        case 1:
                            Item gem = InventoryController.GerarItem(_hero.Hero.Id, _hero.Hero.Level - 5, _hero.Hero.Level + 10, _hero.Hero.RarityModifier(), _hero.Hero.QualityModifier(), 64);
                            Console.WriteLine("Durante suas minerações você encontra uma " + gem.Name + ". Como foi lhe dito no começo da sua empreitada, tudo e qualquer coisa que for encontrada na mina é de propriedade de " + city.Name + ", então, naturalmente, você deve entregar a jóia para os guardas. Entretanto, ninguém percebeu que você encontrou uma " + gem.Name + ", se você conseguir oculta-lá e sair dali é possível fazer um bom dinheiro com ela, apesar de o correto ser entrega-lá para os proprietários, apesar de sua recompensa ser bem menor que o valor da jóia. Qual a sua decisão?");
                            Console.WriteLine("1: Entrega-lá.");
                            Console.WriteLine("2: Ficar com " + gem.Name + ".");
                            while (true)
                            {
                                string opt3 = BWrite("Faça sua escolha").ToLower();
                                if (new[] { "1", "deliver", "entregar" }.Contains(opt3))
                                {
                                    Console.WriteLine("Você decide que o correto é entregar a jóia para as autoridades, outros usos podem ser feitos com a pedra do que seus próprios ganhos pessoais. Ao entregar para o chefe da mina, ele lhe agradece e lhe entrega uma quantia do dinheiro.");
                                    richies += gem.Value / 5;
                                    _hero.Hero.Karma += 5;
                                    _hero.Hero.Reputation += 5;
                                    break;
                                }
                                else if (new[] { "2", "keep", "ficar" }.Contains(opt3))
                                {
                                    Console.WriteLine("Você decide ficar com a " + gem.Name + ". Você olha para o lados, certificando se alguém está olhando, e coloca jóia no seu bolso...");
                                    _hero.Hero.Karma -= 5;
                                    Thread.Sleep(2000);
                                    eOutcome = rng.Next(100);
                                    if (eOutcome > 60 - (_hero.Hero.Agility + _hero.Hero.Perception))
                                    {
                                        Console.WriteLine("... e niguém se quer percebeu o que você fez. ");
                                        listGoodsFound.Add(gem);
                                        break;
                                    }
                                    else
                                    {
                                        Console.Write("... um dos trabalhadores notou algo brilhante em suas mãos, ");
                                        if (rng.Next(100) > 70 - _hero.Hero.Perception)
                                        {
                                            Console.WriteLine("você percebeu suas intenções, e antes que ele pudesse avisar qualquer um, você tomou uma atitude.");
                                            Console.WriteLine("1: Raciocinar.");
                                            Console.WriteLine("2: Negociar.");
                                            Console.WriteLine("3: Intimidar.");
                                            Console.WriteLine("4: Esconder a jóia.");
                                            while (true)
                                            {
                                                string opt4 = BWrite("Faça sua escolha.").ToLower();
                                                switch (opt4)
                                                {
                                                    case "1":
                                                    case "racioncinar":
                                                    case "reason":
                                                        Console.WriteLine("Você se aproxima do trabalhador, tenta convencer ele de que a jóia já era sua, e só tinha pegado ela para lhe dar sorte.");
                                                        if (rng.Next(100) > 80 - _hero.Hero.PersuasionPoints())
                                                        {
                                                            Console.WriteLine("Ele acreditou, dizendo que achava ter visto você encontrando a pedra. Ele se desculpa, você diz que está tudo bem, os dois voltam para o que estavam fazendo.");
                                                            listGoodsFound.Add(gem);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Ele não acredita nessa mentira, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, seus esforços na mina lhe renderam nada.");
                                                            MinerFailEvent(rng);
                                                            return;
                                                        }
                                                        break;
                                                    case "2":
                                                    case "negociar":
                                                    case "negociate":
                                                        Console.Write("Você fala com o trabalhador que, de fato, encontrou uma jóia e está disposto a negociar um acordo para seu silêncio. ");
                                                        if (rng.Next(100) > 50 - _hero.Hero.PersuasionPoints())
                                                        {
                                                            int prt = rng.Next(2, 6);
                                                            Console.WriteLine("O trabalhador, ao observar a pedra, pede " + (prt == 2 ? "pela metade" : (prt == 3 ? "por dois terços" : (prt == 4 ? "por três quartos" : "por quatro quintos"))) + " do valor da " + gem.Name + ". Você aceita a contra-oferta do trabalhador?");
                                                            string opt6 = BWrite("Faça sua escolha").ToLower();
                                                            if (new[] { "sim", "s", "yes", "y" }.Contains(opt6))
                                                            {
                                                                Console.WriteLine("Relutantemente você aceita a oferta dele, discretamente passando a jóia para o trabalhador, que lhe passa uma parte do dinheiro.");
                                                                richies += gem.Value / prt;
                                                            }
                                                            else if (new[] { "não", "nao", "no", "n" }.Contains(opt6))
                                                            {
                                                                if (rng.Next(100) > 60 - _hero.Hero.PersuasionPoints() || prt != 2)
                                                                {
                                                                    prt = rng.Next(2, prt);
                                                                    Console.WriteLine("Você tenta renegociar a partição, ele meio que entende, refaz sua oferta, agora ele pede " + (prt == 2 ? "pela metade" : (prt == 3 ? "por dois terços" : (prt == 4 ? "por três quartos" : "por quatro quintos"))) + ". Você aceita essa nova contra-oferta?");
                                                                    string opt7 = BWrite("Faça sua escolha").ToLower();
                                                                    if (new[] { "sim", "s", "yes", "y" }.Contains(opt7))
                                                                    {
                                                                        Console.WriteLine("Relutantemente você aceita a oferta dele, discretamente passando a jóia para o trabalhador, que lhe passa uma parte do dinheiro.");
                                                                        richies += gem.Value / prt;
                                                                        break;
                                                                    }
                                                                    else if (new[] { "não", "nao", "no", "n" }.Contains(opt7))
                                                                    {
                                                                        Console.WriteLine("O trabalhador ficou extremamente irritado com a sua oferta, correndo até os guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, seus esforços na mina lhe renderam nada.");
                                                                        MinerFailEvent(rng);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("Você tenta renegociar a partição, mas o trabalhador irritou-se com a sua negação, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, seus esforços na mina lhe renderam nada.");
                                                                    MinerFailEvent(rng);
                                                                    return;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Anters que pudesse falar qualquer coisa, o trabalhador mudou de ideia, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, seus esforços na mina lhe renderam nada.");
                                                                MinerFailEvent(rng);
                                                                return;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("O trabalhador não aceitou sua oferta, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, seus esforços na mina lhe renderam nada.");
                                                            MinerFailEvent(rng);
                                                            return;
                                                        }
                                                        break;
                                                    case "3":
                                                    case "intimidar":
                                                    case "intimidate":
                                                        Console.WriteLine("Você se aproxima do trabalhador, o agarrando pela gola da sua camisa e dizendo que se ele o entregar você vai quebrar suas pernas com a picareta.");
                                                        if (rng.Next(100) > 70 - _hero.Hero.Strength)
                                                        {
                                                            Console.WriteLine("O trabalhador hesitou, dizendo que não vai falar nada com nignuém, você o solta, espanando o ombro dele, dizendo para voltar ao trabalho.");
                                                            listGoodsFound.Add(gem);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("o trabalhador não se assutou facilmente, ele desvencilhou do seu aguarrão com um golpe, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, seus esforços na mina lhe renderam nada.");
                                                            MinerFailEvent(rng);
                                                            return;
                                                        }
                                                        break;
                                                    case "4":
                                                    case "esconder":
                                                    case "hide":
                                                        Console.WriteLine("Antes que o trabalhador alcança-se os guardas, você teve a ideia de esconder a jóia na terra.");
                                                        if (rng.Next(100) > 70 - (_hero.Hero.Agility * 2))
                                                        {
                                                            Console.WriteLine("Rapidamente colocou a pedra no chão e a cobriu com pouco de terra, apalpando com o pé, não dava para perceber que havia algo ali. Quando os guardas chegaram ali, te revistaram, mas não encontraram nada, punindo o trabalhador por mentir. Enquanto os guardas o levaram para fora, você recuperou a pedra, sem ninguém próximo para lhe dedurar.");
                                                            listGoodsFound.Add(gem);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Infelizmente você não foi rápido suficiente, os guardas lhe pegaram tentando esconder a jóia no chão, que imediantemente lhe pegaram e o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                                            MinerFailEvent(rng);
                                                            return;
                                                        }
                                                        break;
                                                    case "5":
                                                    case "nada":
                                                    case "nothing":
                                                        Console.Write("Você descide não fazer nada, contiua trabalhando, como se não tivesse acontecido nada. Quando o trabalhador chegou com uma dupla de guardas, ");
                                                        if (rng.Next(100) > 90 - _hero.Hero.Luck)
                                                        {
                                                            Console.WriteLine("ele não soube dizer quem estava com a pedra ou não, os guardas o puniram por mentir, o jogando para fora da mina");
                                                            listGoodsFound.Add(gem);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("o trabalhador apontou o dedo para você, os guardas interromperam o que estava fazendo, revistando-o, encontrando a jóia que havia escavado. Eles o jogaram para fora da mina, seus esforços não serviram para nada");
                                                        }
                                                        break;
                                                    default:
                                                        Console.WriteLine("Faça sua escolha.");
                                                        continue;
                                                }
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("imediatamente foi ao encontro dos guardas. Uma dupla deles lhe confrontou, obrigando-o a esvaziar o seus bolsos, eventualmente encontrando a tal jóia. Os guardas lhe deram uma surra e então o jogaram para fora da mina, seus esforços na mina lhe renderam nada.");
                                            MinerFailEvent(rng);
                                            return;
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        case 2:
                            Console.Write("Enquanto você e outros mineradores cavavam em uma sessão de um túnel, uma viga de sustento se partiu, fazendo com que uma parte da mina colapsar.");
                            int escapeChance = rng.Next(100) + (_hero.Hero.Perception + _hero.Hero.Agility);
                            if (escapeChance >= 50)
                            {
                                Console.Write(" Pela sua habilidade, você conseguiu escapar ileso das rochas que deslizaram do teto.");
                            }
                            else if (escapeChance >= 20 && escapeChance < 50)
                            {
                                Console.Write(" Por pouco você é não pego pelas rochas, uma delas te pegou de raspão, deixando um pouco de sangue escorrer.");
                                _hero.Hero.Hitpoints -= rng.Next(1, 5);
                            }
                            else if (escapeChance < 20)
                            {
                                Console.Write(" Você não foi ágil suficiente para escapar das rochas, sendo pego pelo deslizamento. Com dificuldade, você se rasteja para sua liberdade.");
                                _hero.Hero.Hitpoints -= rng.Next(15, 25);
                            }
                            Console.WriteLine(" Olhando para trás, você vê que um dos trabalhadores ficou preso no deslizaemento, sem que houvesse um modo dele se soltar sozinho. Ele grita por socorro.");
                            bool beanIsFirm = (rng.Next(100) > 70 - (_hero.Hero.Luck * 2)) ? true : false;
                            bool rockIsLiftable = (rng.Next(100) > 70 - (_hero.Hero.Luck * 2)) ? true : false;
                            int time = rng.Next(4) + _hero.Hero.Luck / 10;
                            while (time > 1)
                            {
                                Console.WriteLine("O que você faz?");
                                Console.WriteLine("1: Tentar resgatar o trabalhador.");
                                Console.WriteLine("2: Chamar alguém para ajudar.");
                                Console.WriteLine("3: Inspecionar as rochas colapsadas.");
                                Console.WriteLine("4: Inspecionar as outras vigas.");
                                Console.WriteLine("5: Inspecionar a caverna colapsada.");
                                Console.WriteLine("6: Não fazer nada.");
                                while (true)
                                {
                                    string opt2 = BWrite("Faça sua escolha.");
                                    switch (opt2)
                                    {
                                        case "1":
                                        case "resgatar":
                                        case "rescue":
                                            Console.WriteLine("Você corre em direção ao trabalhador caido, e nota que a rocha esmagou a perna dele, talvez ele não seja capaz de andar depois dessa. Como você vai tira-lo dali?");
                                            Console.WriteLine("1: Puxar");
                                            Console.WriteLine("2: Levantar a rocha.");
                                            while (true)
                                            {
                                                string opt3 = BWrite("Faça sua escolha.").ToLower();
                                                if (new[] { "1", "pull", "puxar" }.Contains(opt3))
                                                {
                                                    if (rng.Next(100) > 70 - _hero.Hero.Strength - (rockIsLiftable ? 30 : 0))
                                                    {
                                                        Console.WriteLine("");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                else if (new[] { "2", "lift", "levantar" }.Contains(opt3))
                                                {
                                                    if (rng.Next(100) > 70 - _hero.Hero.Strength - (rockIsLiftable ? 30 : 0))
                                                    {
                                                        Console.WriteLine("Você conseguiu levantar a rocha");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Você não conseguiu levantar a rocha");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Faça sua escolha");
                                                    continue;
                                                }
                                                break;
                                            }
                                            break;
                                        case "2":
                                        case "chamar":
                                        case "call":
                                            break;
                                        case "3":
                                        case "inspecionar":
                                        case "inspect":
                                            break;
                                        case "4":
                                        case "nothing":
                                        case "nada":
                                            break;
                                        default:
                                            Console.WriteLine("Faça sua escolha.");
                                            continue;
                                    }
                                    break;
                                }
                            }
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                    }
                }
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
        }
    }

    public void MinerFailEvent(Random rng)
    {
        _hero.Hero.Hitpoints -= rng.Next(_hero.Hero.TotalHitPoints() / 10);
        _hero.Hero.Hitpoints = _hero.Hero.Hitpoints < 1 ? 1 : _hero.Hero.Hitpoints;
        _hero.Hero.Reputation -= 10;
    }

    public void MinerRockFallTime(int time)
    {
        if (time >= 5)
            Console.WriteLine("As rochas não parecem estáveis, mas estão segurando bem.");
        else if (time >= 4 && time < 5)
            Console.WriteLine("Um pedaço pequeno de pedra caiu, ao olhar para cima, um ruido fraco é audível.");
        else if (time >= 3 && time < 4)
            Console.WriteLine("Mais pedaços de pedra cairam, de tamanho mais ou menos médio.");
        else if (time >= 2 && time < 3)
            Console.WriteLine("Pedaços grandes de pedra cairam, quase acertaram você ou o trabalhador caído, ruidos altos são ouvidos.");
        else if (time >= 1 && time < 2)
            Console.WriteLine("As rochas maiores estão prestes a cair, não há mais tempo.");
    }
}