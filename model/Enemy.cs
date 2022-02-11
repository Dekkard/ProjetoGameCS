using LiteDB;
#pragma warning disable 8618
public class Enemy : Status
{
    private int _id;
    private int _iaLevel;
    private string _name;
    private int _level;
    private int _hitpoints;
    private int _manapoints;
    private EnemyType _enemyType;
    private Equip _equipment;
    private Value _riches;

    public InventoryController inventory;
 
    public int Id { get => _id; set => _id = value; }
    public int IaLevel { get => _iaLevel; set => _iaLevel = value; }
    public string Name { get => _name; set => _name = value; }
    public int Level { get => _level; set => _level = value; }
    public int Hitpoints { get => _hitpoints; set => _hitpoints = value; }
    public int Manapoints { get => _manapoints; set => _manapoints = value; }
    public EnemyType EnemyType { get => _enemyType; set => _enemyType = value; }
    public Equip Equipment { get => _equipment; set => _equipment = value; }
    public Value Riches { get => _riches; set => _riches = value; }

    // public Enemy()
    // {
    //     _id = ObjectId.NewObjectId().Increment;
    //     _equipment = new Equip(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    //     _riches = new Value(0, 0, 0, 0);
    // }

    public Enemy(int strength = 0, int perception = 0, int endurance = 0, int charisma = 0, int intelligence = 0, int agility = 0, int luck = 0) : base(strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        _id = ObjectId.NewObjectId().Increment;
        _equipment = new Equip();
        _riches = new Value(0, 0, 0, 0);
        _hitpoints = 0;
        _manapoints = 0;
    }

    public Enemy(string name, int level, Equip equipment, Value riches, int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck) : base(strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        _id = ObjectId.NewObjectId().Increment;
        _name = name;
        _level = level;
        _equipment = equipment;
        _riches = riches;
    }
    public int AttackPoints()
    {
        int ap = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int str = Strength + Equipment.EquipMod.Str;
        int agi = Agility + Equipment.EquipMod.Agi;
        int attStatusMod = (str + agi) / 2;
        switch ((int)_enemyType)
        {
            case 0:
                ap = 15 + attStatusMod;
                break;
            case 1:
                ap = 25 + attStatusMod;
                break;
            case 2:
                ap = 20 + attStatusMod;
                break;
            case 3:
                ap = 20 + attStatusMod;
                break;
            case 4:
                ap = 30 + attStatusMod;
                break;
            case 5:
                ap = 40 + attStatusMod;
                break;
            case 6:
                ap = 40 + attStatusMod;
                break;
            case 7:
                ap = 60 + attStatusMod;
                break;
        }
        return ap;
    }

    public int DefencePoints()
    {
        int dp = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int str = Strength + Equipment.EquipMod.Str;
        int agi = Agility + Equipment.EquipMod.Agi;
        int defStatusMod = (str + agi) / 2;
        switch ((int)_enemyType)
        {
            case 0:
                dp = 15 + defStatusMod;
                break;
            case 1:
                dp = 25 + defStatusMod;
                break;
            case 2:
                dp = 20 + defStatusMod;
                break;
            case 3:
                dp = 20 + defStatusMod;
                break;
            case 4:
                dp = 30 + defStatusMod;
                break;
            case 5:
                dp = 40 + defStatusMod;
                break;
            case 6:
                dp = 40 + defStatusMod;
                break;
            case 7:
                dp = 60 + defStatusMod;
                break;
        }
        return dp;
    }

    public void TotalHitPoints()
    {
        int hp = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int end = Endurance + Equipment.EquipMod.End;
        int str = Strength + Equipment.EquipMod.Str;
        int agi = Agility + Equipment.EquipMod.Agi;
        int hpStatusMod = (_level * 2) + end * 3 + (str + agi) / 2;
        switch ((int)_enemyType)
        {
            case 0:
                hp = 30 + hpStatusMod;
                break;
            case 1:
                hp = 40 + hpStatusMod;
                break;
            case 2:
                hp = 35 + hpStatusMod;
                break;
            case 3:
                hp = 50 + hpStatusMod;
                break;
            case 4:
                hp = 80 + hpStatusMod;
                break;
            case 5:
                hp = 70 + hpStatusMod;
                break;
            case 6:
                hp = 70 + hpStatusMod;
                break;
            case 7:
                hp = 100 + hpStatusMod;
                break;
        }
        _hitpoints = hp + Equipment.EquipMod.Hitpoints;
    }

    public void TotalManaPoints()
    {
        int mp = 0;
        // Modifiers mod = Equipment.SumModifiers();
        int @int = Intelligence + Equipment.EquipMod.Int;
        int manaStatusMod = (_level * 2) + @int;
        switch ((int)_enemyType)
        {
            case 0:
                mp = 30 + manaStatusMod;
                break;
            case 1:
                mp = 0 + manaStatusMod;
                break;
            case 2:
                mp = 10 + manaStatusMod;
                break;
            case 3:
                mp = 25 + manaStatusMod;
                break;
            case 4:
                mp = 50 + manaStatusMod;
                break;
            case 5:
                mp = 70 + manaStatusMod;
                break;
            case 6:
                mp = 75 + manaStatusMod;
                break;
            case 7:
                mp = 120 + manaStatusMod;
                break;
        }
        _manapoints = mp /* + Equipment.SumModifiers */;
    }

}
