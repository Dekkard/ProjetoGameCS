using LiteDB;
#pragma warning disable 8618
public class Enemy : Character
{
    private int _iaLevel;
    private EnemyType _enemyType;

    public int IaLevel { get => _iaLevel; set => _iaLevel = value; }
    public EnemyType EnemyType { get => _enemyType; set => _enemyType = value; }

    public Enemy(int iaLevel, EnemyType enemyType, string nome = "", int level = 0, int reputation = 0, int karma = 0, int strength = 0, int perception = 0, int endurance = 0, int charisma = 0, int intelligence = 0, int agility = 0, int luck = 0) : base(nome, level, reputation, karma, strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        _iaLevel = iaLevel;
        _enemyType = enemyType;
    }

    public Enemy()
    {
    }

    public override int AttackPoints()
    {
        int ap = 0;
        // Modifiers mod = Equip.SumModifiers();
        int str = Strength + Equip.EquipMod.Str;
        int agi = Agility + Equip.EquipMod.Agi;
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

    public override int DefencePoints()
    {
        int dp = 0;
        // Modifiers mod = Equip.SumModifiers();
        int str = Strength + Equip.EquipMod.Str;
        int agi = Agility + Equip.EquipMod.Agi;
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

    public override int TotalHitPoints()
    {
        int hp = 0;
        // Modifiers mod = Equip.SumModifiers();
        int end = Endurance + Equip.EquipMod.End;
        int str = Strength + Equip.EquipMod.Str;
        int agi = Agility + Equip.EquipMod.Agi;
        int hpStatusMod = (Level * 2) + end * 3 + (str + agi) / 2;
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
        return Hitpoints = hp + Equip.EquipMod.Hitpoints;
    }

    public override int TotalManaPoints()
    {
        int mp = 0;
        // Modifiers mod = Equip.SumModifiers();
        int @int = Intelligence + Equip.EquipMod.Int;
        int manaStatusMod = (Level * 2) + @int;
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
        return Manapoints = mp /* + Equipment.SumModifiers */;
    }

}
