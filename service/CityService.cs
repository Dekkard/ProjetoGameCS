using LiteDB;
using static BaseService;
#pragma warning disable 8603
public class CityService
{
    private LiteDatabase _db;

    public LiteDatabase db {get=> _db;}

    public CityService(LiteDatabase db)
    {
        _db = db;
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
            City cityConn = new City();
            city.Connections.Add(cityConn.Id);
            cityConn.Connections.Add(city.Id);
            AddCity(cityConn);
            totalConn--;
            if (ismsg) Console.WriteLine("Conexão descoberta: " + cityConn.Name);
        }
    }

    public void CreateWorks(City city, int level, int rarity, int quality)
    {
        var workColl = _db.GetCollection<Work>(WORKS);

        Random rng = new Random();
        int totalWork = (int)city.Size * 3;
        while (totalWork > 0)
        {
            int cityWorks = rng.Next(Enum.GetValues(typeof(CityWorks)).Length);
            Work works = new Work(city.Id, Value.GenerateValue(rng, rarity, quality), (CityWorks)cityWorks);
            workColl.Insert(works);
            totalWork--;
        }
    }

    public void AddCity(City city)
    {
        var col = _db.GetCollection<City>(CITIES);
        // City city = new City();
        // city.AssignCityServices();
        // city.AssignCityWorks();
        // city.AssignCityConnections();
        col.Insert(city);
    }

    public City GetCity(int id)
    {
        var col = _db.GetCollection<City>(CITIES);
        City city;
        try
        {
            city = col.Query().Where(c => c.Id.Equals(id)).First();
            return city;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Cidade não consta no registro");
            return null;
        }
    }

    public void SaveCity(City city)
    {
        var col = _db.GetCollection<City>(CITIES);
        col.Update(city);
    }

    public void DeleteCity(City city)
    {
        var col = _db.GetCollection<City>(CITIES);
        foreach (int cityConn in city.Connections)
        {
            removeCityConnectionRegistry(city, cityConn);
        }
        col.Delete(city.Id);
    }

    public void removeCityConnectionRegistry(City city, int cityConn)
    {
        city.Connections.Remove(cityConn);
        SaveCity(city);
    }

    public City GetCurrentCity(Hero hero)
    {
        var col = _db.GetCollection<City>(CITIES);
        City city;
        try
        {
            city = col.Query().Where(c => c.Id.Equals(hero.CurrentCity)).First();
            return city;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Cidade não consta no registro");
            return null;
        }
    }

    public List<City> GetCities(City city)
    {
        var col = _db.GetCollection<City>(CITIES);
        List<City> cities = col.Query().ToList();
        return cities;
    }

    public Services GetServices(int id)
    {
        var col = _db.GetCollection<Services>(SERVICES);
        Services service;
        try
        {
            service = col.Query().Where(c => c.Id.Equals(id)).First();
            return service;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Cidade não consta no registro");
            return null;
        }
    }

    public List<Services> GetServicesList(City city)
    {
        var col = _db.GetCollection<Services>(SERVICES);
        List<Services> ListServices = col.Query().Where(s => s.City.Equals(city.Id)).ToList();
        return ListServices;
    }

    public Work GetWork(int id)
    {
        var col = _db.GetCollection<Work>(WORKS);
        Work work;
        try
        {
            work = col.Query().Where(w => w.Id.Equals(id)).First();
            return work;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Trabalho não consta no registro");
            return null;
        }
    }

    public List<Work> GetWorkList(City city)
    {
        var col = _db.GetCollection<Work>(WORKS);
        List<Work> ListWorks = col.Query().Where(w => w.City.Equals(city)).ToList();
        return ListWorks;
    }

    public void Estaleiro(int days, Hero _hero)
    {
        Value serviceCost = new Value(0, 0, 0, 20);
        Random rng = new Random();
        int serviceQuality = rng.Next(1, _hero.RarityModifier()) % 100;

        serviceCost *= (int)(_hero.Level * 0.5) * days * (serviceQuality / 10);

        if (_hero.Riches.CompareTo(serviceCost))
        {
            _hero.Riches -= serviceCost;

            _hero.Energypoints += serviceQuality * days;
            _hero.Energypoints = _hero.Energypoints > _hero.TotalEnergyPoints() ? _hero.TotalEnergyPoints() : _hero.Energypoints;

            _hero.Hitpoints += serviceQuality * days * 2;
            _hero.Hitpoints = _hero.Hitpoints > _hero.TotalHitPoints() ? _hero.TotalHitPoints() : _hero.Hitpoints;

            _hero.Manapoints += serviceQuality * days * 2;
            _hero.Manapoints = _hero.Manapoints > _hero.TotalManaPoints() ? _hero.TotalManaPoints() : _hero.Manapoints;
        }
        else
        {
            Console.Write("Fundos insuficientes para utilizar as hospedagens");
        }
    }
}