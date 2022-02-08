using static BaseController;
using LiteDB;
#pragma warning disable 8618
public class Hero : Status
{
    public static int LEVELCAP = 100;
    public static int LEVELXP = 100;
    public static double LEVELMP = 2.5;
    private int _id;
    private string _nome;
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
    public string Nome { get => _nome; set => _nome = value; }
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
        _id = ObjectId.NewObjectId().Increment % 1000;
        equipment = new Equip(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        _experience = 0L;
        _riches = new Value(0,0,0,0);
    }
    public Hero(string nome, int level, int heroType)
    {
        _id = ObjectId.NewObjectId().Increment % 1000;
        _nome = nome;
        _level = level;
        _heroType = (HeroType)heroType;
        equipment = new Equip(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        _experience = 0L;
        _riches = new Value(0,0,0,0);
    }

    public Hero(string nome, int level, int heroType, int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck) : base(strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        _id = ObjectId.NewObjectId().Increment % 1000;
        _nome = nome;
        _level = level;
        _heroType = (HeroType)heroType;
        equipment = new Equip(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        _experience = 0L;
        _riches = new Value(0,0,0,0);
    }
    public void levelUp()
    {
        _level += 1;
        Console.WriteLine("Parabens! " + _nome + " alcançou o nível " + _level + "!");
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
        switch ((int)_heroType)
        {
            case 1:
                hp = 35 + (_level * 2) + (Endurance * 3) + (Strength * 2 + Agility) / 3;
                break;
            case 2:
                hp = 20 + (_level * 2) + (Endurance * 3) + (Strength + Agility) / 2;
                break;
            case 3:
                hp = 25 + (_level * 2) + (Endurance * 3) + (Strength + Agility * 2) / 3;
                break;
            default:
                hp = 25 + (_level * 2) + (Endurance * 3) + (Strength + Agility) / 2;
                break;
        }
        return hp;
    }
    public int TotalManaPoints()
    {
        int mp = 0;
        switch ((int)_heroType)
        {
            case 1:
                mp = 20 + (_level * 2) + Intelligence;
                break;
            case 2:
                mp = 35 + (_level * 2) + Intelligence * 3;
                break;
            case 3:
                mp = 25 + (_level * 2) + Intelligence;
                break;
            default:
                mp = 25 + (_level * 2) + Intelligence;
                break;
        }
        return mp;
    }
    public int TotalEnergyPoints()
    {
        int ep = 0;
        switch ((int)_heroType)
        {
            case 1:
                ep = 25 + (_level * 2) + (Endurance * 2) + (Strength + Agility) / 2;
                break;
            case 2:
                ep = 20 + (_level * 2) + (Endurance * 2) + (Strength + Agility) / 2;
                break;
            case 3:
                ep = 35 + (_level * 2) + (Endurance * 2) + (Strength + Agility * 2) / 3;
                break;
            default:
                ep = 25 + (_level * 2) + (Endurance * 2) + (Strength + Agility) / 2;
                break;
        }
        return ep;
    }
    public int TotalWeightPoints()
    {
        int wp = 0;
        switch ((int)_heroType)
        {
            case 1:
                wp = 60 + ((Endurance * 2) + (Strength * 2));
                break;
            case 2:
                wp = 40 + ((Endurance * 2) + (Intelligence));
                break;
            case 3:
                wp = 50 + ((Endurance * 2) + (Agility));
                break;
            default:
                wp = 40 + ((Endurance * 2) + (Strength + Agility + Intelligence) / 3);
                break;
        }
        return wp;
    }
    public int AttackPoints()
    {
        int ap = 0;
        switch ((int)_heroType)
        {
            case 1:
                ap = 10 + (Strength * 3 + Agility) / 4;
                break;
            case 2:
                ap = 10 + (Intelligence * 3 + Strength + Agility) / 5;
                break;
            case 3:
                ap = 10 + (Strength + Agility * 3) / 4;
                break;
            default:
                ap = 10 + (Strength + Agility) / 2;
                break;
        }
        return ap;
    }
    public int DefencePoints()
    {
        int dp = 0;
        switch ((int)_heroType)
        {
            case 1:
                dp = 15 + Endurance + (Strength * 2 + Agility) / 3;
                break;
            case 2:
                dp = 10 + Endurance + (Strength + Agility) / 2;
                break;
            case 3:
                dp = 12 + Endurance + (Strength + Agility * 2) / 3;
                break;
            default:
                dp = 20 + Endurance + (Strength + Agility) / 2;
                break;
        }
        return dp;
    }
    public int PersuasionPoints()
    {
        return 10 + Charisma + Intelligence / 2;
    }
    public int ItemDiscoveryPoints()
    {
        return 10 + Perception + Intelligence / 2 + Luck;
    }
    public int RarityModifier()
    {
        return (4 * Luck + ((Perception + Intelligence) / 2)) - (Strength + Agility);
    }
    public int QualityModifier()
    {
        return (Strength + Agility) / 4 + (Perception + Intelligence) / 2;
    }
    public string DisplayHeroInfo()
    {
        return _id + ": Nome: " + Nome
        + "\nClasse: " + (TipoHeroi)_heroType
        + "\nNível: " + Level
        + "\nVida: " + Hitpoints + "/" + TotalHitPoints()
        + "\nMana: " + Manapoints + "/" + TotalManaPoints()
        + "\nEnergia: " + Energypoints + "/" + TotalEnergyPoints()
        + "\nPeso carregado: " + Weightpoints + "/" + TotalWeightPoints()
        + "\nRiquesas totais (" + _riches.ToString()+")"
        ;
    }

    public string HeroPrint()
    {
        return _nome + ";" + _level + ";" + _hitpoints + ";" + (int)_heroType + ";";
    }
    public void HeroRead(string line)
    {
        string[] l = line.Split(";");
        _nome = l[0];
        _level = int.Parse(l[1]);
        _hitpoints = int.Parse(l[2]);
        _heroType = (HeroType)int.Parse(l[3]);
    }
}