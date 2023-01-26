using LiteDB;
using static BaseService;
public class ItemService{

    private LiteDatabase _db;
    private int _character;

    public ItemService(LiteDatabase db, int character){
        _db = db;
        _character = character;
    }

    public void changeCaracter(int character){
        _character = character;
    }

    public void changeItemOwnership(Item item){
        item.owner = _character;
        _db.GetCollection<Item>(INVENTORY).Update(item);
    }

    public void PrintInv()
    {
        List<Item> inventory = _db.GetCollection<Item>(INVENTORY).Query().Where(x => x.owner.Equals(_character) & x.Qtd > 0).ToList();
        foreach (Item item in inventory)
        {
            Console.WriteLine(item.ToString());
        }
    }
    public List<Item> GetInv()
    {
        List<Item> inventory = _db.GetCollection<Item>(INVENTORY)
            .Query()
            .Where(x => x.owner == _character & x.Qtd > 0)
            .ToList();
        return inventory;
    }
    public Item GetItem(Item item)
    {
        return _db.GetCollection<Item>(INVENTORY)
            .Query()
            .Where(x => x.owner == _character & x.Qtd > 0 & x.Name.Equals(item.Name))
            .First();
    }
    public Item GetItemById(int itemId){
        return _db.GetCollection<Item>(INVENTORY).Query().Where(i => i.Id == itemId).First();
    }
    public bool Empty()
    {
        List<Item> inventory = _db.GetCollection<Item>(INVENTORY).Query().Where(x => x.owner.Equals(_character) & x.Qtd > 0).ToList();
        bool isEmpty = inventory.Count == 0 ? true : false;
        return isEmpty;
    }

    public bool put(Item item){
        bool isSuccess = false;
        ILiteCollection<Item> itemDb = _db.GetCollection<Item>(INVENTORY);
        isSuccess = itemDb.Update(item);
        if(!isSuccess){
            BsonValue insertItem = itemDb.Insert(item);
            if(insertItem != null) isSuccess = true;
        }
        Console.WriteLine(isSuccess?"Item Inserido":"Falha na inserção");
        return isSuccess;
    }
    public void DeleteItem(int id)
    {
        var inv = _db.GetCollection<Item>(INVENTORY);
        inv.Delete(id);
    }
}