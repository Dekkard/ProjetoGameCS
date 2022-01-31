public class Program
{
    public static void Main(string[] args)
    {
        Warrior warrior = new Warrior("Derack", 6, 6, 5, 3, 3, 4, 4, 5);
        warrior.UpdateHitpoints();
        // Console.WriteLine(warrior.DisplayHeroInfo());
        // Console.WriteLine(warrior.DisplayStatus());
        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine("\t\t"+warrior.Attack()+" da dano.");
        }
    }
}