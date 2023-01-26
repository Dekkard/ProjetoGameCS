using LiteDB;
using static BaseService;
#pragma warning disable 659
public class Item
{
    private Int32 _id;
    private int _owner;
    private string _name;
    private int _qtd;
    private Value _value;
    private ItemType _itemType;
    private double _weight;
    private int _quality;
    private int _rarity;
    private int _level;
    // private List<Item> _composition;
    private Modifiers _modifiers;
    private bool _isEquipable;

    public int Id { get => _id; set => _id = value; }
    public int owner { get => _owner; set => _owner = value; }
    public string Name { get => _name; set => _name = value; }
    public int Qtd { get => _qtd; set => _qtd = value; }
    public Value Value { get => _value; set => _value = value; }
    public ItemType ItemType { get => _itemType; set => _itemType = value; }
    public double Weight { get => _weight; set => _weight = value; }
    public int Quality { get => _quality; set => _quality = value; }
    public int Rarity { get => _rarity; set => _rarity = value; }
    public int Level { get => _level; set => _level = value; }
    public Modifiers Modifiers { get => _modifiers; set => _modifiers = value; }
    public bool IsEquipable { get => _isEquipable; set => _isEquipable = value; }


#pragma warning disable 8618
    public Item()
    {
        _id = ObjectId.NewObjectId().Increment;
    }

    public Item(int id, string name, int ownerId, int qtd, Value value, ItemType itemType, int weight, int quality, int rarity, int level, bool isEquipable)
    {
        _id = id;
        _owner = ownerId;
        _name = name;
        _qtd = qtd;
        _value = value;
        _itemType = itemType;
        _weight = weight;
        _quality = quality;
        _rarity = rarity;
        _level = level;
        _isEquipable = isEquipable;
    }

    public int ItemDefencePoints()
    {
        if (new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 13 }.Contains((int)_itemType))
        {
            return 10 + (_quality + _rarity + _level + (int)_weight);
        }
        else
        {
            return 0;
        }
    }
    public int ItemAttackPoints()
    {
        if ((int)_itemType > 20 && (int)_itemType < 50)
        {
            return 10 + (_quality + _rarity + _level + (int)_weight);
        }
        else
        {
            return 0;
        }
    }

    public override string ToString()
    {
        return _name + " lvl " + _level + ": total: " + _qtd + ", valor: (" + _value.ToString() + ") (Slot: " + _itemType + "[" + (int)_itemType + "])"
            + Modifiers.ToString();
    }
    public string Print()
    {
        return _id + ";" + _name + ";" + _qtd + ";" + _value.ToString() + ";" + (int)_itemType + ";" + _weight + ";" + _quality + ";" + _rarity + ";" + _level + ";";
    }

    public override bool Equals(object? obj)
    {
        return obj is Item item &&
            _name == item.Name &&
            _itemType == item.ItemType &&
            _quality == item.Quality &&
            _rarity == item.Rarity &&
            _level == item.Level &&
            _modifiers.Equals(item.Modifiers) &&
            _isEquipable == item.IsEquipable;
        // _qtd == item.Qtd && _weight == item.Weight && _value.Equals(item.Value) &&
    }

    public Item(string line, int ownerId = 0)
    {
        string[] l = line.Split(";");
        _id = int.Parse(l[0]);
        _owner = owner;
        _name = l[1];
        _qtd = int.Parse(l[2]);
        _value = new Value(l[3]);
        _itemType = (ItemType)int.Parse(l[4]);
        _weight = int.Parse(l[5]);
        _quality = int.Parse(l[6]);
        _rarity = int.Parse(l[7]);
        _level = int.Parse(l[8]);
    }
}