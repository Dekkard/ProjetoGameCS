using static BaseService;
#pragma warning disable 8600, 8602, 8603, 8604
public class InventoryController
{
    // private List<Item> inventory = new List<Item>();
    // private Value vault = new Value(0, 0, 0, 0);

    private ItemService _inventoryService;
    private EquipController _equipController;

    public ItemService Inventory { get => _inventoryService; }

    private int _owner;

    public InventoryController(LiteDB.LiteDatabase db, int owner, Equip equip)
    {
        _owner = owner;
        _inventoryService = new ItemService(db, owner);
        _equipController = new EquipController(db, equip);
    }

    public double TotalCarringWeight()
    {
        double totalCarry = 0.0;
        List<Item> inv = _inventoryService.GetInv();
        foreach (Item item in inv)
            totalCarry += item.Weight;
        return totalCarry;
    }

    public Item GetItemById(int itemId){
        return Inventory.GetItemById(itemId);
        
    } 

    public void AddItem(Item item)
    {
        Item itemFound;
        try
        {
            itemFound = _inventoryService.GetItem(item);
            item.owner = _owner;
            itemFound.Qtd += item.Qtd;
            itemFound.Value += item.Value;
            itemFound.Weight += item.Weight;
            _inventoryService.put(itemFound);
        }
        catch (InvalidOperationException)
        {
            _inventoryService.put(item);
        }
    }

    public void DropItem(Item item, int qtd, Equip equip)
    {
        if (item.IsEquipable)
        {
            if (item.Qtd > qtd)
            {
                item.Qtd -= qtd;
                _inventoryService.put(item);
            }
            else if (item.Qtd <= qtd)
            {
                int slot = _equipController.GetEquipSlot((int)item.ItemType);

                if (slot == 0 || slot != item.Id)
                {
                    _inventoryService.DeleteItem(item.Id);
                }
                else
                {
                    item.Qtd = 0;
                    _inventoryService.put(item);
                }
            }
        }
        else
        {
            if (item.Qtd > qtd)
            {
                item.Qtd -= qtd;
                _inventoryService.put(item);
            }
            else if (item.Qtd <= qtd)
            {
                _inventoryService.DeleteItem(item.Id);
            }
        }
    }

    public static Item GerarItem(int owner, int lMin, int lMax, int rarityMod, int qualityMod, int choosenItem = 0)
    {
        Item item = new Item();
        item.owner = owner;

        Random rng = new Random();

        item.Level = rng.Next(lMin < 1 ? 1 : lMin, lMax > LEVELCAP ? LEVELCAP : lMax);
        // Console.Write("Entre(" + (lMin < 1 ? 1 : lMin) + "," + (lMax > Hero.LEVELCAP ? Hero.LEVELCAP : lMax) + ") lvl" + item.Level);
        item.Rarity = rng.Next(0, rarityMod);
        item.Quality = rng.Next(0, qualityMod);
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
                itemName = Enum.GetName(t, rng.Next(et.Length));
                // Nome que expressa qualidade
                itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                // Nome do material que compôem o item, a partir da lista
                int[] materialsApp = { 65, 66, 67 };
                int mat = materialsApp[new Random().Next(materialsApp.Length)];
                item.Weight = WeightByMaterial(rng, mat);
                itemName += " de " + MaterialNaming(mat, item.Level, item.Rarity, item.Quality);
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
                itemName = Enum.GetName(t, rng.Next(et.Length));
                switch (itemType)
                {
                    case 11:
                    case 12:
                        // Nome do material que compôem o item, a partir da lista
                        itemName += " de " + MaterialNaming(65, item.Level, item.Rarity, item.Quality);
                        // Nome que expressa qualidade
                        itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                        // Segundo nome do material que compôem o item, a partir da lista
                        itemName += " de " + MaterialNaming(64, item.Level, item.Rarity, item.Quality);
                        item.Weight = (WeightByMaterial(rng, 64) + WeightByMaterial(rng, 65)) / 5;
                        // Nome da raridade do item
                        itemName += " " + Enum.GetName(typeof(RarityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(RarityName)));
                        break;
                    case 13:
                        // Nome que expressa qualidade
                        itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                        // Nome do material que compôem o item, a partir da lista
                        itemName += " de " + MaterialNaming(65, item.Level, item.Rarity, item.Quality);
                        WeightByMaterial(rng, 65);
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
                itemName = Enum.GetName(t, rng.Next(et.Length));
                // Nome que expressa qualidade
                itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                // Nome do material que compôem o item, a partir da lista
                itemName += " de " + MaterialNaming(65, item.Level, item.Rarity, item.Quality);
                WeightByMaterial(rng, 65);
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
                itemName = Enum.GetName(t, rng.Next(et.Length));
                // Nome que expressa qualidade
                itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                // Nome do material que compôem o item, a partir da lista
                int[] materialsRang = { 61, 65 };
                int matRang = materialsRang[new Random().Next(materialsRang.Length)];
                item.Weight = WeightByMaterial(rng, matRang);
                itemName += " de " + MaterialNaming(matRang, item.Level, item.Rarity, item.Quality);
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
                itemName = Enum.GetName(t, rng.Next(et.Length));
                // Nome do material que compôem o item, a partir da lista
                int[] materialsMag = { 61, 65 };
                int matMag = materialsMag[new Random().Next(materialsMag.Length)];
                item.Weight = WeightByMaterial(rng, matMag) / 3;
                itemName += " de " + MaterialNaming(matMag, item.Level, item.Rarity, item.Quality);
                // Nome que expressa qualidade
                itemName += " " + Enum.GetName(typeof(QualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(QualityName)));
                // Segundo nome do material que compôem o item, a partir da lista
                materialsMag = new[] { 68, 69 };
                matMag = materialsMag[new Random().Next(materialsMag.Length)];
                item.Weight += WeightByMaterial(rng, matMag);
                itemName += " de " + MaterialNaming(matMag, item.Level, item.Rarity, item.Quality);
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
                itemName = Enum.GetName(t, rng.Next(et.Length));
                switch (itemType)
                {
                    case 51:
                        itemName += " " + Enum.GetName(typeof(EdiblesQualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(EdiblesQualityName)));
                        item.Qtd = rng.Next(1, 5);
                        item.Weight = (WeightByMaterial(rng, 62)) * item.Qtd;
                        break;
                    case 52:
                        itemName = Enum.GetName(t, MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, t));
                        itemName += " " + Enum.GetName(typeof(EdiblesQualityName), MaterialLevelingRng(item.Level, item.Rarity, item.Quality, 0.7, 0.7, 0.7, 1.0, typeof(EdiblesQualityName)));
                        item.Qtd = rng.Next(1, 5);
                        item.Weight = (WeightByMaterial(rng, 62)) * item.Qtd;
                        break;
                    case 53:
                        itemName = "Poção de " + itemName;
                        item.Qtd = rng.Next(1, 3);
                        item.Weight = (WeightByMaterial(rng, 63)) * item.Qtd;
                        break;
                    case 54:
                        item.Modifiers = Modifiers.ItemStatusGen(lMin, lMax);
                        if (item.Modifiers.isEnchanted())
                            itemName += " Encantada";
                        int[] materialsAmmo = { 61, 65 };
                        int matAmmo = materialsAmmo[new Random().Next(materialsAmmo.Length)];

                        itemName += " de " + MaterialNaming(matAmmo, item.Level, item.Rarity, item.Quality);
                        item.Qtd = rng.Next(1, 25);
                        item.Weight = (WeightByMaterial(rng, matAmmo) / 10) * item.Qtd;
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
                item.Weight = (WeightByMaterial(rng, itemType));
                /* if (itemType >= 61 && itemType <= 63)
                    itemName = Enum.GetName(t, rng.Next(et.Length ));
                else if (itemType >= 64 && itemType <= 69)
                {
                    itemName = Enum.GetName(t, rng.Next(et.Length*(10000/item.Level))%(item.Rarity*item.Quality));
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
        item.Name = itemName.Replace('_', ' ') + (item.Modifiers.Cursed ? " Amaldiçoado" : item.Modifiers.Blessed ? " Abençoado" : "");
        item.ItemType = (ItemType)itemType;
        // Console.WriteLine("tipo: " + (ItemType)itemType + ": ");
        return item;
    }

    public static string MaterialNaming(int mat, int level, int rarity, int quality)
    {
        // int mat = possibleMaterials[new Random().Next(possibleMaterials.Length)];
        Type type = Type.GetType(Enum.GetName(typeof(ItemType), mat).ToString() + "Name");
        return Enum.GetName(type, MaterialLevelingRng(level, rarity, quality, 0.7, 0.7, 0.7, 1.0, type));
    }

    public static int MaterialLevelingRng(int level, int rarity, int quality, double levelrates, double rarityRates, double qualityRates, double capRates, Type t)
    {
        Random rng = new Random();
        int et = Enum.GetValues(t).Length - 1;
        double d = (((level * et) + rng.Next((int)(rarity + quality) / 2)) / (LEVELCAP)) * 1.5;
        int n = (int)(Math.Floor(d > et ? et : (d < 0 ? 0 : d)));
        level = (int)Math.Pow(level, levelrates);
        rarity = (int)Math.Pow(rarity, rarityRates);
        quality = (int)Math.Pow(quality, qualityRates);
        int cap = (int)Math.Pow(LEVELCAP, 3 * capRates);
        int chance = cap - LEVELCAP;
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

    public static double WeightByMaterial(Random rng, int material)
    {
        switch (material)
        {
            case 61:
                return (rng.NextDouble() + 0.5) * 2;
            case 62:
                return (rng.NextDouble() + 0.05) * 2;
            case 63:
                return (rng.NextDouble() + 0.01) * 2;
            case 64:
                return (rng.NextDouble() + 0.1) * 5;
            case 65:
                return (rng.NextDouble() + 1.0) * 5;
            case 66:
                return (rng.NextDouble() + 0.5) * 4;
            case 67:
                return (rng.NextDouble() + 0.3) * 2;
            case 68:
                return (rng.NextDouble() + 0.1) * 2;
            case 69:
                return (rng.NextDouble() + 0.05) * 2;
            default:
                return (rng.NextDouble() + 1.0) * 5.0;
        }
    }
}