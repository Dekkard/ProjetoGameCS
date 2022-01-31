public class Warrior : Hero
{
    public Warrior()
    {
        // super();
    }
    public Warrior(string nome, int level) : base(nome, level)
    {
        // super(nome, level);
    }

    public Warrior(string nome, int level, int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck) : base(nome, level, strength, perception, endurance, charisma, intelligence, agility, luck)
    {

    }
    public override void UpdateHitpoints()
    {
        Hitpoints = 35 + (Level * 2) + (Endurance * 3) + (Strength * 2 + Agility) / 3;
    }
    public override int AttackPoints()
    {
        return 15 + (Strength * 3 + Agility) / 2;
    }
    public override int DefencePoints()
    {
        return 25 + Endurance + (Strength * 2 + Agility) / 3;
    }

    public int Attack()
    {
        Console.WriteLine(Nome + " ataca!...");
        Random rnd = new Random();
        int attLand = rnd.Next(0,20+Perception / Level);
        if (attLand >= 10)
        {
            Console.WriteLine("\t...e acerta!");
            return AttackPoints() + rnd.Next(0,Level);
        } else {
            Console.WriteLine("\t...e erra!");
            return 0;
        }
    }
}