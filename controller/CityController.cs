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
            int cityService = rng.Next(Enum.GetValues(typeof(CityServices)).Length - 1);
            Services service = new Services(city.Id, Value.GenerateValue(rng, rarity, quality), (CityServices)cityService);
            if (cityService >= 1)
                CreateInventory(level, rarity, quality, invColl, rng, service);
            servColl.Insert(service);
            totalServ--;
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
                case 1:
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality);
                    break;
                case 2:
                    int[] itemArm = { 13, 21, 22, 23, 24, 25, 26, 27, 28, 54, 61, 64, 65, 66 };
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality, itemArm[rng.Next(itemArm.Length - 1)]);
                    break;
                case 3:
                    int[] itemAlq = { 52, 53, 62, 68, 69 };
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality, itemAlq[rng.Next(itemAlq.Length - 1)]);
                    break;
                case 4:
                    int[] itemMer = { 51, 52, 55 };
                    item = InventoryController.GerarItem(service.Id, level - 5, level + 5, rarity, quality, itemMer[rng.Next(itemMer.Length - 1)]);
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
}