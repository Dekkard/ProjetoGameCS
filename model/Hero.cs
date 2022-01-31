public class Hero : StatusSPECIAL
{
    public static string classType;
    private string _nome;
    private int _level;
    private int _hitpoints;

    public string Nome { get => _nome; set => _nome = value; }
    public int Level { get => _level; set => _level = value; }
    public int Hitpoints { get => _hitpoints; set => _hitpoints = value; }

    public Hero()
    {
        // super();
    }
    public Hero(string nome, int level)
    {
        // super();
        _nome = nome;
        _level = level;
    }

    public Hero(string nome, int level, int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck) : base(strength, perception, endurance, charisma, intelligence, agility, luck)
    {
        // super(strength, perception, endurance, charisma, intelligence, agility, luck);
        _nome = nome;
        _level = level;
    }

    public virtual void UpdateHitpoints()
    {
        Hitpoints = 25 + (_level * 2) + (Endurance * 3) + (Strength + Agility) / 2;
    }
    public virtual int AttackPoints()
    {
        return 10 + (Strength * 2 + Agility) / 2;
    }
    public int AttackSpeed()
    {
        return 7 + (Strength + Agility) / 2;
    }
    public virtual int DefencePoints()
    {
        return 20 + Endurance + (Strength + Agility) / 2;
    }
    public int PersuasionPoints()
    {
        return 10 + Charisma + Intelligence / 2;
    }
    public int ItemDiscoveryPoints()
    {
        return 10 + Perception + Intelligence / 2 + Luck;
    }
    public string DisplayHeroInfo()
    {
        return "Nome: " + Nome + "\n"
        + "Nível: " + Level + "\n"
        + "Vida: " + Hitpoints;
    }
}