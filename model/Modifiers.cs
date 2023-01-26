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
    private int _manapoints;
    private bool _cursed;
    private bool _blessed;
    private int attackPointsMod;
    private int defencePointsMod;

    public int Str { get => _str; set => _str = value; }
    public int Per { get => _per; set => _per = value; }
    public int End { get => _end; set => _end = value; }
    public int Cha { get => _cha; set => _cha = value; }
    public int Int { get => _int; set => _int = value; }
    public int Agi { get => _agi; set => _agi = value; }
    public int Luc { get => _luc; set => _luc = value; }
    public int CritChance { get => _critChance; set => _critChance = value; }
    public int CritHit { get => _critHit; set => _critHit = value; }
    public int Hitpoints { get => _hitpoints; set => _hitpoints = value; }
    public int Manapoints { get => _manapoints; set => _manapoints = value; }
    public bool Cursed { get => _cursed; set => _cursed = value; }
    public bool Blessed { get => _blessed; set => _blessed = value; }
    public int AttackPointsMod { get => attackPointsMod; set => attackPointsMod = value; }
    public int DefencePointsMod { get => defencePointsMod; set => defencePointsMod = value; }

    public Modifiers(int str = 0, int per = 0, int end = 0, int cha = 0, int @int = 0, int agi = 0, int luc = 0, int critChance = 0, int critHit = 0, int hitpoints = 0, int manapoints = 0, bool cursed = false, bool blessed = false)
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
        _manapoints = manapoints;
        _cursed = cursed;
        _blessed = blessed;
    }

    public Modifiers(int[] s, bool cursed = false, bool blessed = false)
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
        _manapoints = s[10];
        _cursed = cursed;
        _blessed = blessed;
    }

    public static Modifiers ItemStatusGen(int lMin, int lMax)
    {
        Random rng = new Random();
        int sttQtd = rng.Next(0, 3);
        bool cursed = false, blessed = false;
        int[] s = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < sttQtd; i++)
        {
            int sttOpt = rng.Next(1, 10);
            s[sttOpt] = rng.Next(lMin, lMax);
        }
        if (rng.Next(1) == 0)
        {
            cursed = rng.Next(999)>970?true:false;
        }
        else
        {
            blessed = rng.Next(999)>970?true:false;
        }
        Modifiers mod = new Modifiers(s,cursed,blessed);
        return mod;
    }

    public bool isEnchanted()
    {
        if (_str > 0 || _per > 0 || _end > 0 || _cha > 0 || _int > 0 || _agi > 0 || _luc > 0 || _critChance > 0 || _critHit > 0 || _hitpoints > 0)
            return true;
        else
            return false;
    }

    public string ToString(String sep = "\n", String tab = "\t")
    {
        string returnString = "";
        returnString += _str > 0 ? sep + tab + "Força: " + _str : "";
        returnString += _per > 0 ? sep + tab + "Percepção: " + _per : "";
        returnString += _end > 0 ? sep + tab + "Resistência: " + _end : "";
        returnString += _cha > 0 ? sep + tab + "Carisma: " + _cha : "";
        returnString += _int > 0 ? sep + tab + "Inteligência: " + _int : "";
        returnString += _agi > 0 ? sep + tab + "Agilidade: " + _agi : "";
        returnString += _luc > 0 ? sep + tab + "Sorte: " + _luc : "";
        returnString += _critChance > 0 ? sep + tab + "Chance de crítico: " + _critChance : "";
        returnString += _critHit > 0 ? sep + tab + "Dano crítico: " + _critHit : "";
        returnString += _hitpoints > 0 ? sep + tab + "Vida adicional: " + _hitpoints : "";
        return returnString;
    }
    public static Modifiers operator +(Modifiers mod1, Modifiers modd2)
    {
        mod1.Str += modd2.Str;
        mod1.Per += modd2.Per;
        mod1.End += modd2.End;
        mod1.Cha += modd2.Cha;
        mod1.Int += modd2.Int;
        mod1.Agi += modd2.Agi;
        mod1.Luc += modd2.Luc;
        mod1.CritChance += modd2.CritChance;
        mod1.CritHit += modd2.CritHit;
        mod1.Hitpoints += modd2.Hitpoints;
        return mod1;
    }
    public static Modifiers operator -(Modifiers mod1, Modifiers modd2)
    {
        mod1.Str -= modd2.Str;
        mod1.Per -= modd2.Per;
        mod1.End -= modd2.End;
        mod1.Cha -= modd2.Cha;
        mod1.Int -= modd2.Int;
        mod1.Agi -= modd2.Agi;
        mod1.Luc -= modd2.Luc;
        mod1.CritChance -= modd2.CritChance;
        mod1.CritHit -= modd2.CritHit;
        mod1.Hitpoints -= modd2.Hitpoints;
        return mod1;
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