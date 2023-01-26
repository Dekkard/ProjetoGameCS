using static BaseService;
using LiteDB;
#pragma warning disable 8618
public class Character : Status
{    
    private int _id;
    private string _name;
    private int _level;
    private int _hitpoints;
    private int _manapoints;
    private int _energypoints;
    private int _weightpoints;
    private Equip _equip;
    private long _experience;
    private Value _riches;
    private int _reputation;
    private int _karma;

    public int Id { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public int Level { get => _level; set => _level = value; }
    public int Hitpoints { get => _hitpoints; set => _hitpoints = value; }
    public int Manapoints { get => _manapoints; set => _manapoints = value; }
    public int Energypoints { get => _energypoints; set => _energypoints = value; }
    public int Weightpoints { get => _weightpoints; set => _weightpoints = value; }
    public Equip Equip { get => _equip; set => _equip = value; }
    public long Experience { get => _experience; set => _experience = value; }
    public Value Riches { get => _riches; set => _riches = value; }
    public int Reputation { get => _reputation; set => _reputation = value; }
    public int Karma { get => _karma; set => _karma = value; }

    public Character(string nome = "", int level = 0, int reputation = 0, int karma = 0, int strength = 0, int perception = 0, int endurance = 0, int charisma = 0, int intelligence = 0, int agility = 0, int luck = 0) : base(strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        _id = ObjectId.NewObjectId().Increment;
        _equip = new Equip();
        _riches = new Value(0, 0, 0, 0);
        _name = nome;
        _level = level;
        _experience = 0L;
        _reputation = reputation;
        _karma = karma;
    }
    public virtual int TotalHitPoints()
    {
        int hp = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int endurance = Endurance + Equip.EquipMod.End;
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        return hp + Equip.EquipMod.Hitpoints;
    }
    public virtual int TotalManaPoints()
    {
        int mp = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int intelligence = Intelligence + Equip.EquipMod.Int;
        return mp + Equip.EquipMod.Manapoints;
    }
    public virtual int TotalEnergyPoints()
    {
        int ep = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int endurance = Endurance + Equip.EquipMod.End;
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        return ep;
    }
    public virtual int TotalWeightPoints()
    {
        int wp = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int endurance = Endurance + Equip.EquipMod.End;
        int strength = Strength + Equip.EquipMod.Str;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        int agility = Agility + Equip.EquipMod.Agi;
        return wp;
    }
    public virtual int AttackPoints()
    {
        int ap = 0;
        // Modifiers mod = equipment.SumModifiers();
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        return ap + Equip.EquipMod.AttackPointsMod;
    }
    public virtual int DefencePoints()
    {
        int dp = 0;
        // Modifiers mod = equipment.SumModifiers();
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        int endurance = Endurance + Equip.EquipMod.End;
        return dp;
    }
    public virtual int PersuasionPoints()
    {
        // Modifiers mod = equipment.SumModifiers();
        int charisma = Charisma + Equip.EquipMod.Cha;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        return 10 + charisma + intelligence / 2;
    }
    public virtual int ItemDiscoveryPoints()
    {
        // Modifiers mod = equipment.SumModifiers();
        int perception = Perception + Equip.EquipMod.Per;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        int luck = Luck + Equip.EquipMod.Luc;
        return 10 + perception + intelligence / 2 + luck;
    }
    public virtual int RarityModifier()
    {
        // Modifiers mod = equipment.SumModifiers();
        int luck = Luck + Equip.EquipMod.Luc;
        int perception = Perception + Equip.EquipMod.Per;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        return (4 * luck + ((perception + intelligence) / 2)) - (strength + agility);
    }
    public virtual int QualityModifier()
    {
        // Modifiers mod = equipment.SumModifiers();
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        int perception = Perception + Equip.EquipMod.Per;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        return (strength + agility) / 4 + (perception + intelligence) / 2;
    }
    public virtual void EnergyDrain()
    {
        int ttlWeight = TotalWeightPoints();
        if (_weightpoints <= ttlWeight)
        {
            _energypoints -= 1;
        }
        else
        {
            _energypoints -= 2 * (_weightpoints / ttlWeight);
        }
    }
    public string DisplayCharacterInfo(string sep = "\n")
    {
        return "Nome: " + Name
        + sep + "Nível: " + Level
        + sep + "Vida: " + Hitpoints + "/" + TotalHitPoints()
        + sep + "Mana: " + Manapoints + "/" + TotalManaPoints()
        + sep + "Energia: " + Energypoints + "/" + TotalEnergyPoints()
        + sep + "Peso carregado: " + _weightpoints + "/" + TotalWeightPoints()
        + sep + "Riquesas totais (" + _riches.ToString() + ")"
        ;
    }
    public string CharacterPrint()
    {
        return _name + ";" + _level + ";" + _hitpoints + ";";
    }
    public void CharacterRead(string line)
    {
        string[] l = line.Split(";");
        _name = l[0];
        _level = int.Parse(l[1]);
        _hitpoints = int.Parse(l[2]);
    }

    public static Character generateCharacter(int level, int reputation)
    {
        Random rng = new Random();
        int lvlMod = rng.Next(level - 50 < 1 ? 1 : level - 50, level);
        return new Character(NameService.nameMaker(rng.Next(2, 5)), level, reputation, 0, 7 * lvlMod, 15 * lvlMod, 6 * lvlMod, 20 * lvlMod, 17 * lvlMod, 5 * lvlMod, 5 * lvlMod);
    }    

    public override bool Equals(object? obj)
    {
        return obj is Character character &&
            Strength == character.Strength &&
            Perception == character.Perception &&
            Endurance == character.Endurance &&
            Charisma == character.Charisma &&
            Intelligence == character.Intelligence &&
            Agility == character.Agility &&
            Luck == character.Luck &&
            _id == character._id &&
            _name == character._name &&
            _level == character._level &&
            _hitpoints == character._hitpoints &&
            _manapoints == character._manapoints &&
            _energypoints == character._energypoints &&
            _weightpoints == character._weightpoints &&
            _equip.Equals(character._equip) &&
            _experience == character._experience &&
            _riches.Equals(character._riches) &&
            _reputation == character._reputation &&
            _karma == character._karma;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Strength);
        hash.Add(Perception);
        hash.Add(Endurance);
        hash.Add(Charisma);
        hash.Add(Intelligence);
        hash.Add(Agility);
        hash.Add(Luck);
        hash.Add(_id);
        hash.Add(_name);
        hash.Add(_level);
        hash.Add(_hitpoints);
        hash.Add(_manapoints);
        hash.Add(_energypoints);
        hash.Add(_weightpoints);
        hash.Add(_equip);
        hash.Add(_experience);
        hash.Add(_riches);
        hash.Add(_reputation);
        hash.Add(_karma);
        return hash.ToHashCode();
    }
}