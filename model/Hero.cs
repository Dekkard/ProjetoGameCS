using LiteDB;
using static BaseService;
#pragma warning disable 8618
public class Hero : Status
{
    private int _id;
    private int _heroType;
    private int _currentCity;
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
    public int HeroType { get => _heroType; set => _heroType = value; }
    public int CurrentCity { get => _currentCity; set => _currentCity = value; }
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

    public Hero(int id, string nome = "", int level = 0, int heroType = 0, int reputation = 0, int karma = 0, int strength = 0, int perception = 0, int endurance = 0, int charisma = 0, int intelligence = 0, int agility = 0, int luck = 0) : base(strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        _id = id;
        _equip = new Equip();
        _riches = new Value(0, 0, 0, 0);
        _heroType = heroType;
        _name = nome;
        _level = level;
        _experience = 0L;
        _reputation = reputation;
        _karma = karma;
    }
    public Hero(string nome = "", int level = 0, int heroType = 0, int reputation = 0, int karma = 0, int strength = 0, int perception = 0, int endurance = 0, int charisma = 0, int intelligence = 0, int agility = 0, int luck = 0) : base(strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        _id = ObjectId.NewObjectId().Increment;
        _equip = new Equip();
        _riches = new Value(0, 0, 0, 0);
        _heroType = heroType;
        _name = nome;
        _level = level;
        _experience = 0L;
        _reputation = reputation;
        _karma = karma;
    }

    public void levelUp()
    {
        Level += 1;
        Console.WriteLine("Parabens! " + Name + " alcançou o nível " + Level + "!");
        int ttlAtt = ((Level / 10) + 1) * 2;
        Console.WriteLine("Escolha quais atributos deseja aumetar"
        + "\n\t1: Força"
        + "\n\t2: Percepção"
        + "\n\t3: Resistência"
        + "\n\t4: Carisma"
        + "\n\t5: Inteligência"
        + "\n\t6: Agilidade"
        + "\n\t7: Sorte");
        while (ttlAtt > 0)
        {
            string opt = BWrite("Deve-se escolher um atributo").ToLower();
            switch (opt)
            {
                case "1":
                case "força":
                case "forca":
                case "for":
                case "strength":
                case "str":
                case "s":
                    Strength++;
                    ttlAtt--;
                    break;
                case "2":
                case "perceção":
                case "Percecao":
                case "per":
                case "perception":
                case "p":
                    Perception++;
                    ttlAtt--;
                    break;
                case "3":
                case "resistência":
                case "resistencia":
                case "res":
                case "endurace":
                case "end":
                case "e":
                    Endurance++;
                    ttlAtt--;
                    break;
                case "carisma":
                case "car":
                case "charisma":
                case "c":
                    Charisma++;
                    ttlAtt--;
                    break;
                case "inteligência":
                case "inteligencia":
                case "int":
                case "intelligence":
                case "i":
                    Intelligence++;
                    ttlAtt--;
                    break;
                case "agilidade":
                case "agility":
                case "agi":
                case "a":
                    Agility++;
                    ttlAtt--;
                    break;
                case "sorte":
                case "sor":
                case "luck":
                case "l":
                    Luck++;
                    ttlAtt--;
                    break;
                default:
                    Console.WriteLine("Atributo desconhecido.");
                    break;
            }
        }
    }
    public int TotalHitPoints()
    {
        int hp = 0;
        // Modifiers mod = Equip.SumModifiers();
        int endurance = Endurance + Equip.EquipMod.End;
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        switch ((int)_heroType)
        {
            case 1:
                hp = 35 + (Level * 2) + endurance * 3 + (strength * 2 + agility) / 3;
                break;
            case 2:
                hp = 20 + (Level * 2) + endurance * 3 + (strength + agility) / 2;
                break;
            case 3:
                hp = 25 + (Level * 2) + endurance * 3 + (strength + agility * 2) / 3;
                break;
            default:
                hp = 25 + (Level * 2) + endurance * 3 + (strength + agility) / 2;
                break;
        }
        return hp + Equip.EquipMod.Hitpoints;
    }
    public int TotalManaPoints()
    {
        int mp = 0;
        // Modifiers mod = Equip.SumModifiers();
        int intelligence = Intelligence + Equip.EquipMod.Int;
        switch ((int)_heroType)
        {
            case 1:
                mp = 20 + (Level * 2) + intelligence;
                break;
            case 2:
                mp = 35 + (Level * 2) + intelligence * 3;
                break;
            case 3:
                mp = 25 + (Level * 2) + intelligence;
                break;
            default:
                mp = 25 + (Level * 2) + intelligence;
                break;
        }
        return mp + Equip.EquipMod.Manapoints;
    }
    public int TotalEnergyPoints()
    {
        int ep = 0;
        // Modifiers mod = Equip.SumModifiers();
        int endurance = Endurance + Equip.EquipMod.End;
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        switch ((int)_heroType)
        {
            case 1:
                ep = 25 + (Level * 2) + endurance * 2 + (strength + agility) / 2;
                break;
            case 2:
                ep = 20 + (Level * 2) + endurance * 2 + (strength + agility) / 2;
                break;
            case 3:
                ep = 35 + (Level * 2) + endurance * 2 + (strength + agility * 2) / 3;
                break;
            default:
                ep = 25 + (Level * 2) + endurance * 2 + (strength + agility) / 2;
                break;
        }
        return ep;
    }
    public int TotalWeightPoints()
    {
        int wp = 0;
        // Modifiers mod = Equip.SumModifiers();
        int endurance = Endurance + Equip.EquipMod.End;
        int strength = Strength + Equip.EquipMod.Str;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        int agility = Agility + Equip.EquipMod.Agi;
        switch ((int)_heroType)
        {
            case 1:
                wp = 60 + (endurance * 2 + strength * 2);
                break;
            case 2:
                wp = 40 + (endurance * 2 + intelligence);
                break;
            case 3:
                wp = 50 + (endurance * 2 + agility);
                break;
            default:
                wp = 40 + (endurance * 2 + (strength + agility + intelligence) / 3);
                break;
        }
        return wp;
    }
    public int AttackPoints()
    {
        int ap = 0;
        // Modifiers mod = equipment.SumModifiers();
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        switch ((int)_heroType)
        {
            case 1:
                ap = 10 + (strength * 3 + agility) / 4;
                break;
            case 2:
                ap = 10 + (intelligence * 3 + strength + agility) / 5;
                break;
            case 3:
                ap = 10 + (strength + agility * 3) / 4;
                break;
            default:
                ap = 10 + (strength + agility) / 2;
                break;
        }
        return ap + Equip.EquipMod.AttackPointsMod;
    }
    public int DefencePoints()
    {
        int dp = 0;
        // Modifiers mod = equipment.SumModifiers();
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        int endurance = Endurance + Equip.EquipMod.End;
        switch ((int)_heroType)
        {
            case 1:
                dp = 15 + endurance + (strength * 2 + agility) / 3;
                break;
            case 2:
                dp = 10 + endurance + (strength + agility) / 2;
                break;
            case 3:
                dp = 12 + endurance + (strength + agility * 2) / 3;
                break;
            default:
                dp = 20 + endurance + (strength + agility) / 2;
                break;
        }
        return dp;
    }
    public int PersuasionPoints()
    {
        // Modifiers mod = equipment.SumModifiers();
        int charisma = Charisma + Equip.EquipMod.Cha;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        return 10 + charisma + intelligence / 2;
    }
    public int ItemDiscoveryPoints()
    {
        // Modifiers mod = equipment.SumModifiers();
        int perception = Perception + Equip.EquipMod.Per;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        int luck = Luck + Equip.EquipMod.Luc;
        return 10 + perception + intelligence / 2 + luck;
    }
    public int RarityModifier()
    {
        // Modifiers mod = equipment.SumModifiers();
        int luck = Luck + Equip.EquipMod.Luc;
        int perception = Perception + Equip.EquipMod.Per;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        return (4 * luck + ((perception + intelligence) / 2)) - (strength + agility);
    }
    public int QualityModifier()
    {
        // Modifiers mod = equipment.SumModifiers();
        int strength = Strength + Equip.EquipMod.Str;
        int agility = Agility + Equip.EquipMod.Agi;
        int perception = Perception + Equip.EquipMod.Per;
        int intelligence = Intelligence + Equip.EquipMod.Int;
        return (strength + agility) / 4 + (perception + intelligence) / 2;
    }
    public void EnergyDrain()
    {
        int ttlWeight = TotalWeightPoints();
        if (Weightpoints <= ttlWeight)
        {
            Energypoints -= 1;
        }
        else
        {
            Energypoints -= 2 * (Weightpoints / ttlWeight);
        }
    }
    public string DisplayHeroInfo(string sep = "\n")
    {
        return "Nome: " + Name
        + sep + "Classe: " + (HeroType)_heroType
        + sep + "Nível: " + Level
        + sep + "Vida: " + Hitpoints + "/" + TotalHitPoints()
        + sep + "Mana: " + Manapoints + "/" + TotalManaPoints()
        + sep + "Energia: " + Energypoints + "/" + TotalEnergyPoints()
        + sep + "Peso carregado: " + Weightpoints + "/" + TotalWeightPoints()
        + sep + "Riquesas totais (" + Riches.ToString() + ")"
        ;
    }
    public string HeroPrint()
    {
        return Name + ";" + Level + ";" + Hitpoints + ";" + (int)_heroType + ";";
    }
    public void HeroRead(string line)
    {
        string[] l = line.Split(";");
        Name = l[0];
        Level = int.Parse(l[1]);
        Hitpoints = int.Parse(l[2]);
        _heroType = int.Parse(l[3]);
    }

    public override bool Equals(object? obj)
    {
        return obj is Hero hero &&
            Id == hero.Id &&
            Strength == hero.Strength &&
            Perception == hero.Perception &&
            Endurance == hero.Endurance &&
            Charisma == hero.Charisma &&
            Intelligence == hero.Intelligence &&
            Agility == hero.Agility &&
            Luck == hero.Luck &&
            Name == hero.Name &&
            Level == hero.Level &&
            Hitpoints == hero.Hitpoints &&
            Manapoints == hero.Manapoints &&
            Energypoints == hero.Energypoints &&
            Weightpoints == hero.Weightpoints &&
            Equip.Equals(hero.Equip) &&
            Experience == hero.Experience &&
            Riches.Equals(hero.Riches) &&
            Reputation == hero.Reputation &&
            Karma == hero.Karma &&
            _heroType == hero._heroType;
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
        hash.Add(_heroType);
        hash.Add(_currentCity);
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