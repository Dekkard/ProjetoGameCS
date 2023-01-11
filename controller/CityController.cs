using LiteDB;
using static BaseController;
#pragma warning disable 8600, 8602, 8603
public class CityController
{
    public City CreateCity(int heroId, bool current, int level, int rarity, int quality)
    {
        City city = new City(heroId, current);
        AssignCityConnections(city);
        CreateServices(city, level, rarity, quality);
        CreateWorks(city, level, rarity, quality);
        AddCity(city);
        return city;
    }

    public void AssignCityConnections(City city, bool ismsg = false)
    {
        Random rng = new Random();
        int citySize = (int)city.Size;
        int totalConn;
        try
        {
            totalConn = rng.Next(1, (int)((citySize + 1) * 2.5) - city.Connections.Count);
        }
        catch (ArgumentOutOfRangeException)
        {
            return;
        }
        while (totalConn > 0)
        {
            City cityConn = new City(city.HeroId);
            city.Connections.Add(cityConn.Id);
            cityConn.Connections.Add(city.Id);
            AddCity(cityConn);
            totalConn--;
            if (ismsg) Console.WriteLine("Conexão descoberta: " + cityConn.Name);
        }
    }

    public void CreateServices(City city, int level, int rarity, int quality)
    {
        var db = new LiteDatabase("data.db");
        var servColl = db.GetCollection<Services>(SERVICES);
        var invColl = db.GetCollection<Item>(INVENTORY);

        Random rng = new Random();
        int totalServ = (int)city.Size;
        while (totalServ > 0)
        {
            int cityService = rng.Next(Enum.GetValues(typeof(CityServices)).Length);
            Services service = new Services(city.Id, Value.GenerateValue(rng, rarity, quality), (CityServices)cityService);
            if (cityService >= 1)
                CreateInventory(level, rarity, quality, invColl, rng, service);
            servColl.Insert(service);
            totalServ--;
        }
        db.Dispose();
    }

    public void CreateWorks(City city, int level, int rarity, int quality)
    {
        var db = new LiteDatabase(DATA);
        var workColl = db.GetCollection<Works>(WORKS);

        Random rng = new Random();
        int totalWork = (int)city.Size * 3;
        while (totalWork > 0)
        {
            int cityWorks = rng.Next(Enum.GetValues(typeof(CityWorks)).Length);
            Works works = new Works(city.Id, Value.GenerateValue(rng, rarity, quality), (CityWorks)cityWorks);
            workColl.Insert(works);
            totalWork--;
        }
        db.Dispose();
    }

    private static void CreateInventory(int level, int rarity, int quality, ILiteCollection<Item> invColl, Random rng, Services service)
    {
        int quantity = rng.Next(rarity);
        for (int i = 1; i <= quantity; i++)
        {
            Item item;
            switch ((int)service.CityServices)
            {
                /* case 1:
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality);
                    break; */
                case 2:
                    int[] itemMer = { 51, 52, 55 };
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality, itemMer[rng.Next(itemMer.Length)]);
                    break;
                case 3:
                    int[] itemArm = { 13, 21, 22, 23, 24, 25, 26, 27, 28, 54, 61, 64, 65, 66 };
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality, itemArm[rng.Next(itemArm.Length)]);
                    break;
                case 4:
                    int[] itemAlq = { 52, 53, 62, 68, 69 };
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality, itemAlq[rng.Next(itemAlq.Length)]);
                    break;
                case 5:
                    int[] itemRag = { 31, 32, 33, 34, 61 };
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality, itemRag[rng.Next(itemRag.Length)]);
                    break;
                case 6:
                    int[] itemMag = { 41, 42, 43, 52, 69 };
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality, itemMag[rng.Next(itemMag.Length)]);
                    break;
                default:
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality);
                    break;
            }
            invColl.Insert(item);
        }
    }

    public void UpdateInventory(int level, int rarity, int quality, ILiteCollection<Item> invColl, Random rng, Services service)
    {
        List<Item> inventory = invColl.Query().Where(inv => inv.ownerId.Equals(service.Id)).ToList();
        if (inventory.Count == 0)
            CreateInventory(level, rarity, quality, invColl, rng, service);
    }

    public void AddCity(City city)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<City>(CITIES);
        // City city = new City();
        // city.AssignCityServices();
        // city.AssignCityWorks();
        // city.AssignCityConnections();
        col.Insert(city);
        db.Dispose();
    }

    public City GetCity(int id)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<City>(CITIES);
        City city;
        try
        {
            city = col.Query().Where(c => c.Id.Equals(id)).First();
            db.Dispose();
            return city;
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Cidade não consta no registro");
            db.Dispose();
            return null;
        }
    }

    public void SaveCity(City city)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<City>(CITIES);
        col.Update(city);
        db.Dispose();
    }

    public void DeleteCity(City city)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<City>(CITIES);
        foreach (int cityConnId in city.Connections)
        {
            removeCityConnectionRegistry(GetCity(cityConnId), city.Id);
        }
        col.Delete(city.Id);
        db.Dispose();
    }

    public void removeCityConnectionRegistry(City city, int cityConnId)
    {
        city.Connections.Remove(cityConnId);
        SaveCity(city);
    }

    public City GetCurrentCity(int heroId)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<City>(CITIES);
        City city;
        try
        {
            city = col.Query().Where(c => c.HeroId.Equals(heroId) || c.IsCurrent).First();
            db.Dispose();
            return city;
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Cidade não consta no registro");
            return null;
        }
    }

    public List<City> GetCities(int heroId)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<City>(CITIES);
        List<City> city = col.Query().Where(c => c.HeroId.Equals(heroId)).ToList();
        db.Dispose();
        return city;
    }

    public Services GetServices(int id)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<Services>(SERVICES);
        Services service;
        try
        {
            service = col.Query().Where(c => c.Id.Equals(id)).First();
            db.Dispose();
            return service;
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Cidade não consta no registro");
            return null;
        }
    }

    public List<Services> GetServicesList(int cityId)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<Services>(SERVICES);
        List<Services> ListServices = col.Query().Where(s => s.CityId.Equals(cityId)).ToList();
        db.Dispose();
        return ListServices;
    }

    public Works GetWork(int id)
    {
        var db = new LiteDatabase(DATA);
        var col = db.GetCollection<Works>(WORKS);
        Works work;
        try
        {
            work = col.Query().Where(w => w.Id.Equals(id)).First();
            db.Dispose();
            return work;
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Trabalho não consta no registro");
            return null;
        }
    }

    public List<Works> GetWorkList(int cityId)
    {
        var db = new LiteDatabase(DATA);
        var col = db.GetCollection<Works>(WORKS);
        List<Works> ListWorks = col.Query().Where(w => w.CityId.Equals(cityId)).ToList();
        db.Dispose();
        return ListWorks;
    }

    public void CityServicesController(City city, Hero hero)
    {
        List<Services> ListServices = GetServicesList(city.Id);
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
        Console.WriteLine(servDic.Count);
        Console.WriteLine("Escolha o serviço que deseja utilizar");
        string opt;
        Services service;
        while (true)
        {
            try
            {
                opt = OptRead("Deve-se escolher uma opção");
                if (new[] { "voltar", "cancelar", "volta", "return", "back", "can" }.Contains(opt))
                {
                    return;
                }
                try
                {
                    service = servDic[int.Parse(opt)];
                }
                catch (FormatException)
                {
                    service = servDic.Values.ToList().Find(s => s.CityServices.Equals(Enum.GetName(typeof(CityServices), opt)));
                }
            }
            catch (System.ArgumentNullException)
            {
                Console.WriteLine("Serviço não compreende na lista.");
                continue;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Serviço não compreende na lista.");
                continue;
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Serviço não compreende na lista.");
                continue;
            }
            break;
        }
        switch ((int)service.CityServices)
        {
            case 0:
                Console.WriteLine("Escolha quantos dias quer se recuperar neste estabelecimento.");
                int days;
                while (true)
                {
                    try
                    {
                        days = int.Parse(OptRead("Deve-se escolher quantos dias."));
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
                Estaleiro(hero, days);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
                service.inventory = new InventoryController(service.Id);
                List<Item> lojaList = service.inventory.GetInv();
                Dictionary<int, Item> lojaDic = new Dictionary<int, Item>();
                int index = 1;
                foreach (Item item in lojaList)
                {
                    lojaDic.Add(index++, item);
                }
                LojaInterface(lojaDic, hero);
                break;
        }
    }

    public void Estaleiro(Hero hero, int days)
    {
        Value serviceCost = new Value(0, 0, 0, 20);
        Random rng = new Random();
        int serviceQuality = rng.Next(1, hero.RarityModifier()) % 100;

        serviceCost *= (int)(hero.Level * 0.5) * days * (serviceQuality / 10);

        if (hero.Riches.CompareTo(serviceCost))
        {
            hero.Riches -= serviceCost;

            hero.Energypoints += serviceQuality * days;
            hero.Energypoints = hero.Energypoints > hero.TotalEnergyPoints() ? hero.TotalEnergyPoints() : hero.Energypoints;

            hero.Hitpoints += serviceQuality * days * 2;
            hero.Hitpoints = hero.Hitpoints > hero.TotalHitPoints() ? hero.TotalHitPoints() : hero.Hitpoints;

            hero.Manapoints += serviceQuality * days * 2;
            hero.Manapoints = hero.Manapoints > hero.TotalManaPoints() ? hero.TotalManaPoints() : hero.Manapoints;
        }
        else
        {
            Console.Write("Fundos insuficientes para utilizar as hospedagens");
        }
    }

    public void LojaInterface(Dictionary<int, Item> loja, Hero hero)
    {
        foreach (int index in loja.Keys)
        {
            Console.WriteLine(index + ": " + loja[index].ToString());
        }
        List<Item> cart = new List<Item>();
        Value total = new Value(0, 0, 0, 0);
        Console.WriteLine("Escolha os itens que deseja adquirir, digite 'fechar' para terminar compra.");
        while (true)
        {
            try
            {
                string opt2 = OptRead("Escolha um item");
                if (new[] { "fechar", "close", "ok", "okay", "f" }.Contains(opt2.ToLower()))
                {
                    if (hero.Riches.CompareTo(total))
                        break;
                    else
                    {
                        Console.WriteLine("Fundos insuficientes. Faça uma nova seleção");
                        cart.Clear();
                        continue;
                    }
                }
                else if (new[] { "voltar", "cancelar", "volta", "return", "back", "can" }.Contains(opt2.ToLower()))
                {
                    Console.WriteLine("Transação cancelada");
                    return;
                }
                try
                {
                    Item item = loja[int.Parse(opt2)];
                    total += item.Value;
                    cart.Add(item);
                }
                catch (FormatException)
                {
                    try
                    {
                        Item item = loja.Values.ToList().Find(i => i.Name.Equals(opt2));
                        total += item.Value;
                        cart.Add(item);
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
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Item não encontrado.");
                continue;
            }

        }
        if (cart.Count > 0)
        {

            foreach (Item ic in cart)
            {
                Console.WriteLine(ic.Name);

            }
            while (true)
            {
                Console.WriteLine("Comfirmar compra?");
                string opt = OptRead("Sim ou não.");
                if (new[] { "sim", "s", "yes", "y" }.Contains(opt.ToLower()))
                {
                    foreach (Item ic in cart)
                    {
                        hero.inventory.AddItem(ic);
                        hero.Riches -= ic.Value;
                    }
                }
                else if (new[] { "não", "nao", "n", "no" }.Contains(opt.ToLower()))
                    break;
            }
        }
    }
    public void CityWorksController(City city, Hero hero)
    {
        var db = new LiteDatabase(DATA);
        var col = db.GetCollection<Works>(WORKS);
        List<Works> listWorks = col.Query().Where(w => w.CityId.Equals(city.Id)).ToList();
        Dictionary<int, Works> cwDict = new Dictionary<int, Works>();
        int index = 1;
        Console.WriteLine("Qual destas opções deseja trabalhar?");
        foreach (Works w in listWorks)
        {
            Console.WriteLine(index + ": " + (CityWorks)w.CityWorks);
            cwDict.Add(index, w);
        }
        Works work;
        while (true)
        {
            try
            {
                string opt1 = OptRead("Escolha uma opção.");
                if (new[] { "voltar", "cancelar", "volta", "return", "back", "can" }.Contains(opt1.ToLower()))
                {
                    return;
                }
                try
                {
                    work = cwDict[int.Parse(opt1)];
                    break;
                }
                catch (FormatException)
                {
                    try
                    {
                        work = cwDict.Values.ToList().Find(w => w.CityWorks.Equals(opt1));
                        break;
                    }
                    catch (System.NullReferenceException)
                    {
                        Console.WriteLine("Opção não encontrada.");
                        continue;
                    }
                }
            }
            catch (System.ArgumentNullException)
            {
                Console.WriteLine("Opção não encontrada.");
                continue;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Opção não encontrada.");
                continue;
            }
        }
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
                while (true)
                {
                    string opt2 = OptRead("Sim ou não.").ToLower();
                    if (new[] { "não", "no", "nao", "n" }.Contains(opt2))
                    {
                        return;
                    }
                    else if (new[] { "sim", "yes", "s", "y" }.Contains(opt2))
                        break;
                    else Console.WriteLine("Sim ou não");
                }
                if (rng.Next(20) > 15)
                {
                    switch (rng.Next(1, 10))
                    {
                        case 1:
                            Item gem = InventoryController.GerarItem(hero.Id, hero.Level - 5, hero.Level + 10, hero.RarityModifier(), hero.QualityModifier(), 64);
                            Console.WriteLine("Durante suas minerações você encontra uma " + gem.Name + ". Como foi lhe dito no começo da sua empreitada, tudo e qualquer coisa que for encontrada na mina é de propriedade de " + city.Name + ", então, naturalmente, você deve entregar a jóia para os guardas. Entretanto, ninguém percebeu que você encontrou uma " + gem.Name + ", se você conseguir oculta-lá e sair dali é possível fazer um bom dinheiro com ela, apesar de o correto ser entrega-lá para os proprietários, apesar de sua recompensa ser bem menor que o valor da jóia. Qual a sua decisão?");
                            Console.WriteLine("1: Entrega-lá.");
                            Console.WriteLine("2: Ficar com " + gem.Name + ".");
                            while (true)
                            {
                                string opt3 = OptRead("Faça sua escolha").ToLower();
                                if (new[] { "1", "deliver", "entregar" }.Contains(opt3))
                                {
                                    Console.WriteLine("Você decide que o correto é entregar a jóia para as autoridades, outros usos podem ser feitos com a pedra do que seus próprios ganhos pessoais. Ao entregar para o chefe da mina, ele lhe agradece e lhe entrega uma quantia do dinheiro.");
                                    richies += gem.Value / 5;
                                    hero.Karma += 5;
                                    hero.Reputation += 5;
                                    break;
                                }
                                else if (new[] { "2", "keep", "ficar" }.Contains(opt3))
                                {
                                    Console.WriteLine("Você decide ficar com a " + gem.Name + ". Você olha para o lados, certificando se alguém está olhando, e coloca jóia no seu bolso...");
                                    hero.Karma -= 5;
                                    Thread.Sleep(2000);
                                    eOutcome = rng.Next(100);
                                    if (eOutcome > 60 - (hero.Agility + hero.Perception))
                                    {
                                        Console.WriteLine("... e niguém se quer percebeu o que você fez. ");
                                        listGoodsFound.Add(gem);
                                        break;
                                    }
                                    else
                                    {
                                        Console.Write("... um dos trabalhadores notou algo brilhante em suas mãos, ");
                                        if (rng.Next(100) > 70 - hero.Perception)
                                        {
                                            Console.WriteLine("você percebeu suas intenções, e antes que ele pudesse avisar qualquer um, você tomou uma atitude.");
                                            Console.WriteLine("1: Raciocinar.");
                                            Console.WriteLine("2: Negociar.");
                                            Console.WriteLine("3: Intimidar.");
                                            Console.WriteLine("4: Esconder a jóia.");
                                            while (true)
                                            {
                                                string opt4 = OptRead("Faça sua escolha.").ToLower();
                                                switch (opt4)
                                                {
                                                    case "1":
                                                    case "racioncinar":
                                                    case "reason":
                                                        Console.WriteLine("Você se aproxima do trabalhador, tenta convencer ele de que a jóia já era sua, e só tinha pegado ela para lhe dar sorte.");
                                                        if (rng.Next(100) > 80 - hero.PersuasionPoints())
                                                        {
                                                            Console.WriteLine("Ele acreditou, dizendo que achava ter visto você encontrando a pedra. Ele se desculpa, você diz que está tudo bem, os dois voltam para o que estavam fazendo.");
                                                            listGoodsFound.Add(gem);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Ele não acredita nessa mentira, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                                            MinerFailEvent(hero, rng);
                                                            return;
                                                        }
                                                        break;
                                                    case "2":
                                                    case "negociar":
                                                    case "negociate":
                                                        Console.Write("Você fala com o trabalhador que, de fato, encontrou uma jóia e está disposto a negociar um acordo para seu silêncio. ");
                                                        if (rng.Next(100) > 50 - hero.PersuasionPoints())
                                                        {
                                                            int prt = rng.Next(2, 6);
                                                            Console.WriteLine("O trabalhador, ao observar a pedra, pede " + (prt == 2 ? "pela metade" : (prt == 3 ? "por dois terços" : (prt == 4 ? "por três quartos" : "por quatro quintos"))) + " do valor da " + gem.Name + ". Você aceita a contra-oferta do trabalhador?");
                                                            string opt6 = OptRead("Faça sua escolha").ToLower();
                                                            if (new[] { "sim", "s", "yes", "y" }.Contains(opt6))
                                                            {
                                                                Console.WriteLine("Relutantemente você aceita a oferta dele, discretamente passando a jóia para o trabalhador, que lhe passa uma parte do dinheiro.");
                                                                richies += gem.Value / prt;
                                                            }
                                                            else if (new[] { "não", "nao", "no", "n" }.Contains(opt6))
                                                            {
                                                                if (rng.Next(100) > 60 - hero.PersuasionPoints() || prt != 2)
                                                                {
                                                                    prt = rng.Next(2, prt);
                                                                    Console.WriteLine("Você tenta renegociar a partição, ele meio que entende, refaz sua oferta, agora ele pede " + (prt == 2 ? "pela metade" : (prt == 3 ? "por dois terços" : (prt == 4 ? "por três quartos" : "por quatro quintos"))) + ". Você aceita essa nova contra-oferta?");
                                                                    string opt7 = OptRead("Faça sua escolha").ToLower();
                                                                    if (new[] { "sim", "s", "yes", "y" }.Contains(opt7))
                                                                    {
                                                                        Console.WriteLine("Relutantemente você aceita a oferta dele, discretamente passando a jóia para o trabalhador, que lhe passa uma parte do dinheiro.");
                                                                        richies += gem.Value / prt;
                                                                        break;
                                                                    }
                                                                    else if (new[] { "não", "nao", "no", "n" }.Contains(opt7))
                                                                    {
                                                                        Console.WriteLine("O trabalhador ficou extremamente irritado com a sua oferta, correndo até os guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                                                        MinerFailEvent(hero, rng);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("Você tenta renegociar a partição, mas o trabalhador irritou-se com a sua negação, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                                                    MinerFailEvent(hero, rng);
                                                                    return;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Anters que pudesse falar qualquer coisa, o trabalhador mudou de ideia, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                                                MinerFailEvent(hero, rng);
                                                                return;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("O trabalhador não aceitou sua oferta, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                                            MinerFailEvent(hero, rng);
                                                            return;
                                                        }
                                                        break;
                                                    case "3":
                                                    case "intimidar":
                                                    case "intimidate":
                                                        Console.WriteLine("Você se aproxima do trabalhador, o agarrando pela gola da sua camisa e dizendo que se ele o entregar você vai quebrar suas pernas com a picareta.");
                                                        if (rng.Next(100) > 70 - hero.Strength)
                                                        {
                                                            Console.WriteLine("O trabalhador hesitou, dizendo que não vai falar nada com nignuém, você o solta, espanando o ombro dele, dizendo para voltar ao trabalho.");
                                                            listGoodsFound.Add(gem);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("o trabalhador não se assutou facilmente, ele desvencilhou do seu aguarrão com um golpe, correndo até os dois guardas que estavam ali perto, que vão em sua direção, já demandando que lhe mostrem a jóia. Os guardas te revistam, encontrando uma " + gem.Name + ". Os guardas lhe deram uma surra e então o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                                            MinerFailEvent(hero, rng);
                                                            return;
                                                        }
                                                        break;
                                                    case "4":
                                                    case "esconder":
                                                    case "hide":
                                                        Console.WriteLine("Antes que o trabalhador alcança-se os guardas, você teve a ideia de esconder a jóia na terra.");
                                                        if (rng.Next(100) > 70 - (hero.Agility * 2))
                                                        {
                                                            Console.WriteLine("Rapidamente colocou a pedra no chão e a cobriu com pouco de terra, apalpando com o pé, não dava para perceber que havia algo ali. Quando os guardas chegaram ali, te revistaram, mas não encontraram nada, punindo o trabalhador por contar mentira. Enquanto os guardas o levaram para fora, você recuperou a pedra, sem ninguém próximo para lhe dedurar.");
                                                            listGoodsFound.Add(gem);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Infelizmente você não foi rápido suficiente, os guardas lhe pegaram tentando esconder a jóia no chão, que imediantemente lhe pegaram e o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                                            MinerFailEvent(hero, rng);
                                                            return;
                                                        }
                                                        break;
                                                    case "5":
                                                    case "nada":
                                                    case "nothing":
                                                        Console.Write("Você descide não fazer nada, contiua trabalhando, como se não tivesse acontecido nada. Quando o trabalhador chegou com uma dupla de guardas, ");
                                                        if (rng.Next(100) > 90 - hero.Luck)
                                                        {
                                                            Console.WriteLine("ele não soube dizer quem estava com a pedra ou não, os guardas o puniram por mentir, o jogando para fora da mina");
                                                            listGoodsFound.Add(gem);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("o trabalhador apontou o dedo para você, os guardas interromperam o que estava fazendo, revistando-o, encontrando a jóia que havia escavado. Eles o jogaram para fora da mina, todos seus esforços não serviram para nada");
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
                                            Console.WriteLine("imediatamente foi ao encontro dos guardas. Uma dupla deles lhe confrontou, obrigando-o a esvaziar o seus bolsos, eventualmente encontrando a tal jóia. Os guardas lhe deram uma surra e então o jogaram para fora da mina, nenhum de seus esforços na mina lhe renderam nada.");
                                            MinerFailEvent(hero, rng);
                                            return;
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        case 2:
                            Console.Write("Enquanto você e outros mineradores cavavam em uma sessão de um túnel, uma viga de sustento se partiu, fazendo com que uma parte da mina colapsar.");
                            int escapeChance = rng.Next(100) + (hero.Perception + hero.Agility);
                            if (escapeChance >= 50)
                            {
                                Console.Write(" Pela sua habilidade, você conseguiu escapar ileso das rochas que deslizaram do teto.");
                            }
                            else if (escapeChance >= 20 && escapeChance < 50)
                            {
                                Console.Write(" Por pouco você é não pego pelas rochas, uma delas te pegou de raspão, deixando um pouco de sangue escorrer.");
                                hero.Hitpoints -= rng.Next(1, 5);
                            }
                            else if (escapeChance < 20)
                            {
                                Console.Write(" Você não foi ágil suficiente para escapar das rochas, sendo pego pelo deslizamento. Com dificuldade, você se rasteja para sua liberdade.");
                                hero.Hitpoints -= rng.Next(15, 25);
                            }
                            Console.WriteLine(" Olhando para trás, você vê que um dos trabalhadores ficou preso no deslizaemento, sem que houvesse um modo dele se soltar sozinho. Ele grita por socorro.");
                            bool beanIsFirm = (rng.Next(100) > 70 - (hero.Luck * 2)) ? true : false;
                            bool rockIsLiftable = (rng.Next(100) > 70 - (hero.Luck * 2)) ? true : false;
                            int time = rng.Next(4) + hero.Luck / 10;
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
                                    string opt2 = OptRead("Faça sua escolha.");
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
                                                string opt3 = OptRead("Faça sua escolha.").ToLower();
                                                if (new[] { "1", "pull", "puxar" }.Contains(opt3))
                                                {
                                                    if (rng.Next(100) > 70 - hero.Strength - (rockIsLiftable ? 30 : 0))
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
                                                    if (rng.Next(100) > 70 - hero.Strength - (rockIsLiftable ? 30 : 0))
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

    public void MinerFailEvent(Hero hero, Random rng)
    {
        hero.Hitpoints -= rng.Next(hero.TotalHitPoints() / 10);
        hero.Hitpoints = hero.Hitpoints < 1 ? 1 : hero.Hitpoints;
        hero.Reputation -= 10;
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