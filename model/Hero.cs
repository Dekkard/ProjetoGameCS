using static BaseController;
using LiteDB;
#pragma warning disable 8618
public class Hero : Status
{
    public static int LEVELCAP = 100;
    public static int LEVELXP = 100;
    public static double LEVELMP = 2.5;
    private int _id;
    private string _name;
    private int _level;
    private int _hitpoints;
    private int _manapoints;
    private int _energypoints;
    private int _weightpoints;
    private HeroType _heroType;
    private Equip equipment;
    private long _experience;
    private Value _riches;

    public InventoryController inventory;
    // public EquipController equipment;

    public int Id { get => _id; set => _id = value; }
    public string Nome { get => _name; set => _name = value; }
    public int Level { get => _level; set => _level = value; }
    public int Hitpoints { get => _hitpoints; set => _hitpoints = value; }
    public int Manapoints { get => _manapoints; set => _manapoints = value; }
    public int Energypoints { get => _energypoints; set => _energypoints = value; }
    public int Weightpoints { get => _weightpoints; set => _weightpoints = value; }
    public HeroType HeroType { get => _heroType; set => _heroType = (HeroType)value; }
    public Equip Equipment { get => equipment; set => equipment = value; }
    public long Experience { get => _experience; set => _experience = value; }
    public Value Riches { get => _riches; set => _riches = value; }

    public Hero()
    {
        _id = ObjectId.NewObjectId().Increment;
        equipment = new Equip();
        _experience = 0L;
        _riches = new Value(0, 0, 0, 0);
    }
    public Hero(string nome, int level, int heroType)
    {
        _id = ObjectId.NewObjectId().Increment;
        _name = nome;
        _level = level;
        _heroType = (HeroType)heroType;
        equipment = new Equip();
        _experience = 0L;
        _riches = new Value(0, 0, 0, 0);
    }

    public Hero(string nome, int level, int heroType, int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck) : base(strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        _id = ObjectId.NewObjectId().Increment;
        _name = nome;
        _level = level;
        _heroType = (HeroType)heroType;
        equipment = new Equip();
        _experience = 0L;
        _riches = new Value(0, 0, 0, 0);
    }
    public void levelUp()
    {
        _level += 1;
        Console.WriteLine("Parabens! " + _name + " alcançou o nível " + _level + "!");
        int ttlAtt = ((_level / 10) + 1) * 2;
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
            string opt = OptRead("Deve-se escolher um atributo").ToLower();
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
        // Modifiers mod = Equipment.SumModifiers();
        int endurance = Endurance + Equipment.EquipMod.End;
        int strength = Strength + Equipment.EquipMod.Str;
        int agility = Agility + Equipment.EquipMod.Agi;
        switch ((int)_heroType)
        {
            case 1:
                hp = 35 + (_level * 2) + endurance * 3 + (strength * 2 + agility) / 3;
                break;
            case 2:
                hp = 20 + (_level * 2) + endurance * 3 + (strength + agility) / 2;
                break;
            case 3:
                hp = 25 + (_level * 2) + endurance * 3 + (strength + agility * 2) / 3;
                break;
            default:
                hp = 25 + (_level * 2) + endurance * 3 + (strength + agility) / 2;
                break;
        }
        return hp + Equipment.EquipMod.Hitpoints;
    }
    public int TotalManaPoints()
    {
        int mp = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int intelligence = Intelligence + Equipment.EquipMod.Int;
        switch ((int)_heroType)
        {
            case 1:
                mp = 20 + (_level * 2) + intelligence;
                break;
            case 2:
                mp = 35 + (_level * 2) + intelligence * 3;
                break;
            case 3:
                mp = 25 + (_level * 2) + intelligence;
                break;
            default:
                mp = 25 + (_level * 2) + intelligence;
                break;
        }
        return mp + Equipment.EquipMod.Manapoints;
    }
    public int TotalEnergyPoints()
    {
        int ep = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int endurance = Endurance + Equipment.EquipMod.End;
        int strength = Strength + Equipment.EquipMod.Str;
        int agility = Agility + Equipment.EquipMod.Agi;
        switch ((int)_heroType)
        {
            case 1:
                ep = 25 + (_level * 2) + endurance * 2 + (strength + agility) / 2;
                break;
            case 2:
                ep = 20 + (_level * 2) + endurance * 2 + (strength + agility) / 2;
                break;
            case 3:
                ep = 35 + (_level * 2) + endurance * 2 + (strength + agility * 2) / 3;
                break;
            default:
                ep = 25 + (_level * 2) + endurance * 2 + (strength + agility) / 2;
                break;
        }
        return ep;
    }
    public int TotalWeightPoints()
    {
        int wp = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int endurance = Endurance + Equipment.EquipMod.End;
        int strength = Strength + Equipment.EquipMod.Str;
        int intelligence = Intelligence + Equipment.EquipMod.Int;
        int agility = Agility + Equipment.EquipMod.Agi;
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
        int strength = Strength + Equipment.EquipMod.Str;
        int agility = Agility + Equipment.EquipMod.Agi;
        int intelligence = Intelligence + Equipment.EquipMod.Int;
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
        return ap + Equipment.EquipMod.AttackPointsMod;
    }
    public int DefencePoints()
    {
        int dp = 0;
        // Modifiers mod = equipment.SumModifiers();
        int strength = Strength + Equipment.EquipMod.Str;
        int agility = Agility + Equipment.EquipMod.Agi;
        int endurance = Endurance + Equipment.EquipMod.End;
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
        int charisma = Charisma + Equipment.EquipMod.Cha;
        int intelligence = Intelligence + Equipment.EquipMod.Int;
        return 10 + charisma + intelligence / 2;
    }
    public int ItemDiscoveryPoints()
    {
        // Modifiers mod = equipment.SumModifiers();
        int perception = Perception + Equipment.EquipMod.Per;
        int intelligence = Intelligence + Equipment.EquipMod.Int;
        int luck = Luck + Equipment.EquipMod.Luc;
        return 10 + perception + intelligence / 2 + luck;
    }
    public int RarityModifier()
    {
        // Modifiers mod = equipment.SumModifiers();
        int luck = Luck + Equipment.EquipMod.Luc;
        int perception = Perception + Equipment.EquipMod.Per;
        int intelligence = Intelligence + Equipment.EquipMod.Int;
        int strength = Strength + Equipment.EquipMod.Str;
        int agility = Agility + Equipment.EquipMod.Agi;
        return (4 * luck + ((perception + intelligence) / 2)) - (strength + agility);
    }
    public int QualityModifier()
    {
        // Modifiers mod = equipment.SumModifiers();
        int strength = Strength + Equipment.EquipMod.Str;
        int agility = Agility + Equipment.EquipMod.Agi;
        int perception = Perception + Equipment.EquipMod.Per;
        int intelligence = Intelligence + Equipment.EquipMod.Int;
        return (strength + agility) / 4 + (perception + intelligence) / 2;
    }
    public void EnergyDrain()
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
    public string DisplayHeroInfo(string sep = "\n")
    {
        return "Nome: " + Nome
        + sep + "Classe: " + (TipoHeroi)_heroType
        + sep + "Nível: " + Level
        + sep + "Vida: " + Hitpoints + "/" + TotalHitPoints()
        + sep + "Mana: " + Manapoints + "/" + TotalManaPoints()
        + sep + "Energia: " + Energypoints + "/" + TotalEnergyPoints()
        + sep + "Peso carregado: " + Weightpoints + "/" + TotalWeightPoints()
        + sep + "Riquesas totais (" + _riches.ToString() + ")"
        ;
    }

    public string HeroPrint()
    {
        return _name + ";" + _level + ";" + _hitpoints + ";" + (int)_heroType + ";";
    }
    public void HeroRead(string line)
    {
        string[] l = line.Split(";");
        _name = l[0];
        _level = int.Parse(l[1]);
        _hitpoints = int.Parse(l[2]);
        _heroType = (HeroType)int.Parse(l[3]);
    }
}