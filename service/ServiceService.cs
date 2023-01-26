using LiteDB;
using static BaseService;
public class ServiceService
{
    private LiteDatabase _db;

    public LiteDatabase Db { get => _db;}

    public ServiceService(LiteDatabase db){
        _db = db;
    }

    public void CreateServices(City city, int level, int rarity, int quality)
    {
        var servColl = Db.GetCollection<Services>(SERVICES);

        Random rng = new Random();
        int totalServ = (int)city.Size;
        while (totalServ > 0)
        {
            int cityService = rng.Next(Enum.GetValues(typeof(CityServices)).Length);
            Services service = new Services(city.Id, Value.GenerateValue(rng, rarity, quality), (CityServices)cityService);
            Character vendor = Character.generateCharacter(level, 10);
            _db.GetCollection<Character>(CHARACTERS).Insert(vendor);
            service.Vendor = vendor.Id;
            if (cityService >= 1)
                CreateInventory(level, rarity, quality, rng, service);
            servColl.Insert(service);
            totalServ--;
        }
    }

    private void CreateInventory(int level, int rarity, int quality, Random rng, Services service)
    {
        int quantity = rng.Next(1,rarity*level);
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
                    item = InventoryController.GerarItem(service.Vendor, level - 5, level + 5, rarity, quality, itemMer[rng.Next(itemMer.Length)]);
                    break;
                case 3:
                    int[] itemArm = { 13, 21, 22, 23, 24, 25, 26, 27, 28, 54, 61, 64, 65, 66 };
                    item = InventoryController.GerarItem(service.Vendor, level - 5, level + 5, rarity, quality, itemArm[rng.Next(itemArm.Length)]);
                    break;
                case 4:
                    int[] itemAlq = { 52, 53, 62, 68, 69 };
                    item = InventoryController.GerarItem(service.Vendor, level - 5, level + 5, rarity, quality, itemAlq[rng.Next(itemAlq.Length)]);
                    break;
                case 5:
                    int[] itemRag = { 31, 32, 33, 34, 61 };
                    item = InventoryController.GerarItem(service.Vendor, level - 5, level + 5, rarity, quality, itemRag[rng.Next(itemRag.Length)]);
                    break;
                case 6:
                    int[] itemMag = { 41, 42, 43, 52, 69 };
                    item = InventoryController.GerarItem(service.Vendor, level - 5, level + 5, rarity, quality, itemMag[rng.Next(itemMag.Length)]);
                    break;
                default:
                    item = InventoryController.GerarItem(service.Vendor, level - 5, level + 5, rarity, quality);
                    break;
            }
            item.owner = service.Vendor;
            _db.GetCollection<Item>(INVENTORY).Insert(item);
        }
    }

    public void UpdateInventory(int level, int rarity, int quality, Random rng, Services service)
    {
        List<Item> inventory = _db.GetCollection<Item>(INVENTORY)
            .Query()
            .Where(i=> i.owner == service.Vendor)
            .ToList();
        if (inventory.Count == 0)
             CreateInventory(level, rarity, quality, rng, service);
    }
}