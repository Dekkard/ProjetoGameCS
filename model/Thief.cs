public class Thief : Hero
{
    public Thief()
    {
        // super();
    }
    public Thief(string nome, int level) : base(nome, level)
    {
        // super(nome, level);
    }

    public Thief(string nome, int level, int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck) : base(nome, level, strength, perception, endurance, charisma, intelligence, agility, luck)
    {
    }
    public override void UpdateHitpoints()
    {
        Hitpoints = 25 + (Level * 2) + (Endurance * 3) + (Strength + Agility * 2) / 3;
    }
    public override int AttackPoints()
    {
        return 15 + (Strength + Agility * 3) / 2;
    }
    public override int DefencePoints()
    {
        return 25 + Endurance + (Strength + Agility * 2) / 3;
    }
}