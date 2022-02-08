using LiteDB;
#pragma warning disable 659
public class Item
{
    private int _id;
    private int _ownerId;
    private string _name;
    private int _qtd;
    private Value _value;
    private ItemType _itemType;
    private int _weight;
    private int _quality;
    private int _rarity;
    private int _level;
    // private List<Item> _composition;
    private Modifiers _modifiers;
    private bool _isEquipable;

    public int Id { get => _id; set => _id = value; }
    public int ownerId { get => _ownerId; set => _ownerId = value; }
    public string Name { get => _name; set => _name = value; }
    public int Qtd { get => _qtd; set => _qtd = value; }
    public Value Value { get => _value; set => _value = value; }
    public ItemType ItemType { get => _itemType; set => _itemType = value; }
    public int Weight { get => _weight; set => _weight = value; }
    public int Quality { get => _quality; set => _quality = value; }
    public int Rarity { get => _rarity; set => _rarity = value; }
    public int Level { get => _level; set => _level = value; }
    public Modifiers Modifiers { get => _modifiers; set => _modifiers = value; }
    public bool IsEquipable { get => _isEquipable; set => _isEquipable = value; }


#pragma warning disable 8618
    public Item() { _id = ObjectId.NewObjectId().Increment; }

    public Item(int id, string name, int ownerId, int qtd, Value value, ItemType itemType, int weight, int quality, int rarity, int level, bool isEquipable)
    {
        _id = id;
        _ownerId = ownerId;
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
        if ((int)_itemType > 20 && (int)_itemType < 30)
        {
            return 10 + (_quality + _rarity + _level + _weight);
        }
        else
        {
            return 0;
        }
    }
    public int ItemAttackPoints()
    {
        if ((int)_itemType > 30 && (int)_itemType < 50)
        {
            return 10 + (_quality + _rarity + _level + _weight);
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
        _ownerId = ownerId;
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
#pragma warning disable 169, 659
public class Modifiers
{
    private int _str;
    private int _per;
    private int _end;
    private int _cha;
    private int _int;
    private int _agi;
    private int _luc;
    private int _critChance;
    private int _critHit;
    private int _hitpoints;
    private bool _cursed;
    private bool _blessed;

    public int Str { get => _str; }
    public int Per { get => _per; }
    public int End { get => _end; }
    public int Cha { get => _cha; }
    public int Int { get => _int; }
    public int Agi { get => _agi; }
    public int Luc { get => _luc; }
    public int CritChance { get => _critChance; }
    public int CritHit { get => _critHit; }
    public int Hitpoints { get => _hitpoints; }

    public Modifiers(int str, int per, int end, int cha, int @int, int agi, int luc, int critChance, int critHit, int hitpoints)
    {
        _str = str;
        _per = per;
        _end = end;
        _cha = cha;
        _int = @int;
        _agi = agi;
        _luc = luc;
        _critChance = critChance;
        _critHit = critHit;
        _hitpoints = hitpoints;
    }

    public Modifiers(int[] s)
    {
        _str = s[0];
        _per = s[1];
        _end = s[2];
        _cha = s[3];
        _int = s[4];
        _agi = s[5];
        _luc = s[6];
        _critChance = s[7];
        _critHit = s[8];
        _hitpoints = s[9];
    }

    public static Modifiers ItemStatusGen(int lMin, int lMax)
    {
        Random rng = new Random();
        int sttQtd = rng.Next(0, 3);
        int[] s = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < sttQtd; i++)
        {
            int sttOpt = rng.Next(1, 10);
            s[sttOpt] = rng.Next(lMin, lMax);
        }
        Modifiers mod = new Modifiers(s);
        return mod;
    }

    public bool isEnchanted()
    {
        if (_str > 0 || _per > 0 || _end > 0 || _cha > 0 || _int > 0 || _agi > 0 || _luc > 0 || _critChance > 0 || _critHit > 0 || _hitpoints > 0)
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        string returnString = "";
        returnString += _str > 0 ? "\n\tForça: " + _str : "";
        returnString += _per > 0 ? "\n\tPercepção: " + _per : "";
        returnString += _end > 0 ? "\n\tResistência: " + _end : "";
        returnString += _cha > 0 ? "\n\tCarisma: " + _cha : "";
        returnString += _int > 0 ? "\n\tInteligência: " + _int : "";
        returnString += _agi > 0 ? "\n\tAgilidade: " + _agi : "";
        returnString += _luc > 0 ? "\n\tSorte: " + _luc : "";
        returnString += _critChance > 0 ? "\n\tChance de crítico: " + _critChance : "";
        returnString += _critHit > 0 ? "\n\tDano crítico: " + _critHit : "";
        returnString += _hitpoints > 0 ? "\n\tVida adicional: " + _hitpoints : "";
        return returnString;
    }

    public override bool Equals(object? obj)
    {
        return obj is Modifiers modifiers &&
            _str == modifiers.Str &&
            _per == modifiers.Per &&
            _end == modifiers.End &&
            _cha == modifiers.Cha &&
            _int == modifiers.Int &&
            _agi == modifiers.Agi &&
            _luc == modifiers.Luc &&
            _critChance == modifiers.CritChance &&
            _critHit == modifiers.CritHit &&
            _hitpoints == modifiers.Hitpoints;
        // _cursed == modifiers.Cursed &&
        // _blessed == modifiers.Blessed &&
    }
}