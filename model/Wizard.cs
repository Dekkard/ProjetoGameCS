public class Wizard : Hero
{
    public Wizard()
    {
        // super();
    }
    public Wizard(string nome, int level) : base(nome, level)
    {
        // super(nome, level);
    }

    public Wizard(string nome, int level, int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck) : base(nome, level, strength, perception, endurance, charisma, intelligence, agility, luck)
    {
    }
    public override void UpdateHitpoints()
    {
        Hitpoints  = 20 + (Level * 2) + (Endurance * 3) + (Strength + Agility) / 2;
    }
    public override int AttackPoints()
    {
        return 10 + Intelligence * 3 + (Strength + Agility) / 4;
    }

    public override int DefencePoints()
    {
        return 10 + Endurance + (Strength + Agility) / 2;
    }
}