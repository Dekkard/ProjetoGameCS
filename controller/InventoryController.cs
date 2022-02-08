using static BaseController;
using LiteDB;
#pragma warning disable 8600, 8602, 8603, 8604
public class InventoryController
{
    // private List<Item> inventory = new List<Item>();
    private int _ownerId;
    public InventoryController(int id)
    {
        _ownerId = id;
    }
    private Value vault = new Value(0, 0, 0, 0);

    public void PrintInv()
    {
        var db = new LiteDatabase("data.db");
        List<Item> inventory = db.GetCollection<Item>(INVENTORY).Query().Where(x => x.ownerId.Equals(_ownerId) & x.Qtd > 0).ToList();
        foreach (Item item in inventory)
        {
            Console.WriteLine(item.ToString());
        }
        db.Dispose();
    }
    public List<Item> GetInv()
    {
        var db = new LiteDatabase("data.db");
        List<Item> inventory = db.GetCollection<Item>(INVENTORY).Query().Where(x => x.ownerId.Equals(_ownerId) & x.Qtd > 0).ToList();
        db.Dispose();
        return inventory;
    }
    public bool Empty()
    {
        var db = new LiteDatabase("data.db");
        List<Item> inventory = db.GetCollection<Item>(INVENTORY).Query().Where(x => x.ownerId.Equals(_ownerId) & x.Qtd > 0).ToList();
        bool isEmpty = inventory.Count == 0 ? true : false;
        db.Dispose();
        return isEmpty;
    }
    public void AddItem(Item item)
    {
        var db = new LiteDatabase("data.db");
        var inv = db.GetCollection<Item>(INVENTORY);
        try
        {
            var itemFound = inv.Query().Where(x => x.Name.Equals(item.Name)).First();
            itemFound.Qtd += item.Qtd;
            itemFound.Value += item.Value;
            itemFound.Weight += item.Weight;
            inv.Update(itemFound);
        }
        catch (System.InvalidOperationException)
        {
            inv.Insert(item);
        }
        /* if (inventory.Contains(item))
            inventory[item.Id].Qtd += item.Qtd;
        else
            inventory.Add(item); */
        db.Dispose();
    }
    public void DeleteItem(int id)
    {
        var db = new LiteDatabase("data.db");
        var inv = db.GetCollection<Item>(INVENTORY);
        inv.Delete(id);
        db.Dispose();
    }
    public Item GetItem(int id)
    {
        var db = new LiteDatabase("data.db");
        var inv = db.GetCollection<Item>(INVENTORY);
        try
        {
            Item item = inv.Query().Where(x => x.Id.Equals(id)).First();
            db.Dispose();
            return item;
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Item não encontrado");
            db.Dispose();
            return null;
        }
    }
    public void DropItem(Item item, int qtd, Equip equip)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<Item>(BaseController.INVENTORY);
        if (item.IsEquipable)
        {
            if (item.Qtd > qtd)
            {
                item.Qtd -= qtd;
                col.Update(item);
            }
            else if (item.Qtd <= qtd)
            {
                int slotId = 0;
                if (new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains((int)item.ItemType))
                {
                    slotId = EquipController.GetEquipSlot((int)item.ItemType, equip);
                }
                else if ((int)item.ItemType == 11)
                {
                    slotId = EquipController.GetEquipSlot(10, equip);
                    if (slotId == 0 || slotId != item.Id)
                    {
                        slotId = EquipController.GetEquipSlot(11, equip);
                    }
                }
                else if ((int)item.ItemType == 12)
                {
                    slotId = EquipController.GetEquipSlot(12, equip);
                }
                else if (((int)item.ItemType > 20 && (int)item.ItemType < 50) || (int)item.ItemType == 13)
                {
                    slotId = EquipController.GetEquipSlot(13, equip);
                    if (slotId == 0 || slotId != item.Id)
                    {
                        slotId = EquipController.GetEquipSlot(14, equip);
                    }
                }

                if (slotId == 0 || slotId != item.Id)
                {
                    col.Delete(item.Id);
                }
                else
                {
                    item.Qtd = 0;
                    col.Update(item);
                }
            }
        }
        else
        {
            if (item.Qtd > qtd)
            {
                item.Qtd -= qtd;
                col.Update(item);
            }
            else if (item.Qtd <= qtd)
            {
                col.Delete(item.Id);
            }
        }
        db.Dispose();
    }
    public static Item GerarItem(int ownerId, int lMin, int lMax, int rarityMod, int qualityMod, int choosenItem = 0)
    {
        Item item = new Item();
        item.ownerId = ownerId;
        Random rng = new Random();
        item.Level = rng.Next(lMin < 1 ? 1 : lMin, lMax > Hero.LEVELCAP ? Hero.LEVELCAP : lMax);
        // Console.Write("Entre(" + (lMin < 1 ? 1 : lMin) + "," + (lMax > Hero.LEVELCAP ? Hero.LEVELCAP : lMax) + ") lvl" + item.Level);
        item.Rarity = rng.Next(0, rarityMod);
        item.Quality = rng.Next(0, qualityMod);
        item.Weight = rng.Next(5, 10);
        item.Value = Value.GenerateValue(rng, item.Rarity, item.Quality);
        item.Qtd = 1;
        int itemCategory;
        if (choosenItem == 0)
            itemCategory = rng.Next(1, 7);
        else
            itemCategory = ((choosenItem - (choosenItem % 10)) / 10) + 1;
        string itemName = "Nome";
        int itemType = 0;
        Type t;
        Array et;
        // Console.Write(", categoria: " + itemCategory + ", ");

        // Processo de nomeação dos itens, cada tipo de item terá um processo diferente
        switch (itemCategory)
        {
            case 1:
                // Escolha do tipo do item, se não for definido antes
                itemType = choosenItem == 0 ? rng.Next(1, 9) : choosenItem;
                // A partir do tipo, formata o nome da lista a ser usada 
                t = Type.GetType(Enum.GetName(typeof(ItemType), itemType).ToString() + "Name");
                et = Enum.GetValues(t);
                // Nome base
                itemName = Enum.GetName(t, rng.Next(et.Length - 1));
                // Nome que expressa qualidade
                itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                // Nome do material que compôem o item, a partir da lista
                itemName += " de " + MaterialNaming(new[] { 65, 66, 67 }, item.Level, item.Rarity, item.Quality);
                // Nome da raridade do item
                itemName += " " + Enum.GetName(typeof(RarityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(RarityName)));
                item.IsEquipable = true;
                break;
            case 2:
                // Escolha do tipo do item, se não for definido antes
                itemType = choosenItem == 0 ? rng.Next(11, 13) : choosenItem;
                // A partir do tipo, formata o nome da lista a ser usada 
                t = Type.GetType(Enum.GetName(typeof(ItemType), itemType).ToString() + "Name");
                et = Enum.GetValues(t);
                // Nome base
                itemName = Enum.GetName(t, rng.Next(et.Length - 1));
                switch (itemType)
                {
                    case 11:
                    case 12:
                        // Nome do material que compôem o item, a partir da lista
                        itemName += " de " + MaterialNaming(new[] { 65 }, item.Level, item.Rarity, item.Quality);
                        // Nome que expressa qualidade
                        itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                        // Segundo nome do material que compôem o item, a partir da lista
                        itemName += " de " + MaterialNaming(new[] { 64 }, item.Level, item.Rarity, item.Quality);
                        // Nome da raridade do item
                        itemName += " " + Enum.GetName(typeof(RarityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(RarityName)));
                        break;
                    case 13:
                        // Nome que expressa qualidade
                        itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                        // Nome do material que compôem o item, a partir da lista
                        itemName += " de " + MaterialNaming(new[] { 65 }, item.Level, item.Rarity, item.Quality);
                        // Nome da raridade do item
                        itemName += " " + Enum.GetName(typeof(RarityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(RarityName)));
                        break;
                }
                item.IsEquipable = true;
                break;
            case 3:
                // Escolha do tipo do item, se não for definido antes
                itemType = choosenItem == 0 ? rng.Next(21, 28) : choosenItem;
                // A partir do tipo, formata o nome da lista a ser usada 
                t = Type.GetType(Enum.GetName(typeof(ItemType), itemType).ToString() + "Name");
                et = Enum.GetValues(t);
                // Nome base
                itemName = Enum.GetName(t, rng.Next(et.Length - 1));
                // Nome que expressa qualidade
                itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                // Nome do material que compôem o item, a partir da lista
                itemName += " de " + MaterialNaming(new[] { 65 }, item.Level, item.Rarity, item.Quality);
                // Nome da raridade do item
                itemName += " " + Enum.GetName(typeof(RarityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(RarityName)));
                item.IsEquipable = true;
                break;
            case 4:
                // Escolha do tipo do item, se não for definido antes
                itemType = choosenItem == 0 ? rng.Next(31, 34) : choosenItem;
                // A partir do tipo, formata o nome da lista a ser usada 
                t = Type.GetType(Enum.GetName(typeof(ItemType), itemType).ToString() + "Name");
                et = Enum.GetValues(t);
                // Nome base
                itemName = Enum.GetName(t, rng.Next(et.Length - 1));
                // Nome que expressa qualidade
                itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                // Nome do material que compôem o item, a partir da lista
                itemName += " de " + MaterialNaming(new[] { 61, 65 }, item.Level, item.Rarity, item.Quality);
                // Nome da raridade do item
                itemName += " " + Enum.GetName(typeof(RarityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(RarityName)));
                item.IsEquipable = true;
                break;
            case 5:
                // Escolha do tipo do item, se não for definido antes
                itemType = choosenItem == 0 ? rng.Next(41, 43) : choosenItem;
                // A partir do tipo, formata o nome da lista a ser usada 
                t = Type.GetType(Enum.GetName(typeof(ItemType), itemType).ToString() + "Name");
                et = Enum.GetValues(t);
                // Nome base
                itemName = Enum.GetName(t, rng.Next(et.Length - 1));
                // Nome do material que compôem o item, a partir da lista
                itemName += " de " + MaterialNaming(new[] { 61, 65 }, item.Level, item.Rarity, item.Quality);
                // Nome que expressa qualidade
                itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                // Segundo nome do material que compôem o item, a partir da lista
                itemName += " de " + MaterialNaming(new[] { 68, 69 }, item.Level, item.Rarity, item.Quality);
                // Nome da raridade do item
                itemName += " " + Enum.GetName(typeof(RarityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(RarityName)));
                item.IsEquipable = true;
                break;
            case 6:
                // Escolha do tipo do item, se não for definido antes
                itemType = choosenItem == 0 ? rng.Next(51, 53) : choosenItem;
                // A partir do tipo, formata o nome da lista a ser usada 
                t = Type.GetType(Enum.GetName(typeof(ItemType), itemType).ToString() + "Name");
                et = Enum.GetValues(t);
                // Nome base
                itemName = Enum.GetName(t, rng.Next(et.Length - 1));
                switch (itemType)
                {
                    case 51:
                        itemName += " " + Enum.GetName(typeof(EdiblesQualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(EdiblesQualityName)));
                        item.Qtd = rng.Next(1, 5);
                        break;
                    case 52:
                        itemName = Enum.GetName(t, MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, t));
                        itemName += " " + Enum.GetName(typeof(EdiblesQualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(EdiblesQualityName)));
                        item.Qtd = rng.Next(1, 5);
                        break;
                    case 53:
                        itemName = "Poção de " + itemName;
                        break;
                    case 54:
                        item.Modifiers = Modifiers.ItemStatusGen(lMin, lMax);
                        if (item.Modifiers.isEnchanted())
                            itemName += " Encantada";
                        itemName += " de " + MaterialNaming(new[] { 61, 65 }, item.Level, item.Rarity, item.Quality);
                        item.Qtd = rng.Next(1, 25);
                        break;
                }
                item.IsEquipable = false;
                break;
            case 7:
                // Escolha do tipo do item, se não for definido antes
                itemType = choosenItem == 0 ? rng.Next(61, 69) : choosenItem;
                // A partir do tipo, formata o nome da lista a ser usada 
                t = Type.GetType(Enum.GetName(typeof(ItemType), itemType).ToString() + "Name");
                et = Enum.GetValues(t);
                // Nome base
                itemName = Enum.GetName(t, MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, t));
                /* if (itemType >= 61 && itemType <= 63)
                    itemName = Enum.GetName(t, rng.Next(et.Length - 1));
                else if (itemType >= 64 && itemType <= 69)
                {
                    itemName = Enum.GetName(t, rng.Next(et.Length - 1*(10000/item.Level))%(item.Rarity*item.Quality));
                    double d = ((item.Level * 8) + ((item.Rarity + item.Quality) / 2)) / 100;
                    itemName = Enum.GetName(t, rng.Next((int)(Math.Floor(d > et.Length ? et.Length : (d < 0 ? 0 : d)))));
                } */
                item.IsEquipable = false;
                break;
        }
        if (new[] { 1, 2, 3, 4, 5 }.Contains(itemCategory))
            item.Modifiers = Modifiers.ItemStatusGen(lMin, lMax);
        else
            item.Modifiers = new Modifiers(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        item.Name = itemName.Replace('_', ' ');
        item.ItemType = (ItemType)itemType;
        // Console.WriteLine("tipo: " + (ItemType)itemType + ": ");
        return item;
    }
    public static string MaterialNaming(int[] possibleMaterials, int level, int rarity, int quality)
    {
        int mat = possibleMaterials[new Random().Next(possibleMaterials.Length - 1)];
        Type type = Type.GetType(Enum.GetName(typeof(ItemType), mat).ToString() + "Name");
        return Enum.GetName(type, MaterialLevelingRng(level, rarity, quality, 0.7, 0.7, 0.7, 1.0, type));
    }
    public static int MaterialLevelingRng(int level, int rarity, int quality, double levelrates, double rarityRates, double qualityRates, double capRates, Type t)
    {
        Random rng = new Random();
        int et = Enum.GetValues(t).Length - 1;
        double d = (((level * et) + rng.Next((int)(rarity + quality) / 2)) / (Hero.LEVELCAP)) * 1.5;
        int n = (int)(Math.Floor(d > et ? et : (d < 0 ? 0 : d)));
        level = (int)Math.Pow(level, levelrates);
        rarity = (int)Math.Pow(rarity, rarityRates);
        quality = (int)Math.Pow(quality, qualityRates);
        int cap = (int)Math.Pow(Hero.LEVELCAP, 3 * capRates);
        int chance = cap - Hero.LEVELCAP;
        double capRed = Math.Pow(cap - 1000, 1 / et);
        double chanceRed = Math.Pow(chance - 100, 1 / et);
        return rng.Next(cap) > chance - (level * rarity * quality) ? n : MLTHelper(rng, level, rarity, quality, (int)(cap / capRed), (int)(chance / chanceRed), n - 1, capRed, chanceRed);
    }
    public static int MLTHelper(Random rng, int level, int rarity, int quality, int cap, int chance, int num, double capRed, double chanceRed)
    {
        if (num <= 0)
            return 0;
        else
            return rng.Next(cap) > chance - (level * rarity * quality) ? num : MLTHelper(rng, level, rarity, quality, (int)(cap / capRed), (int)(chance / chanceRed), num - 1, capRed, chanceRed);
    }

    
}