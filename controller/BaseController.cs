#pragma warning disable 8600, 8602, 8603
public static class BaseController
{
    public static string DATA = "data.db";
    public static string HERO = "hero";
    public static string INVENTORY = "inventory";
    public static string CITIES = "cities";
    public static string SERVICES = "services";
    public static string WORKS = "works";

    public static string OptRead(string msg)
    {
        string s = "";
        while (true)
        {
            Console.Write("> ");
            s = Console.ReadLine();
            if (s.Equals(""))
                Console.WriteLine(msg);
            else if (s.ToLower().Equals("x")
                    || s.ToLower().Equals("q")
                    || s.ToLower().Equals("quit")
                    || s.ToLower().Equals("exit")
                    || s.ToLower().Equals("sair"))
                System.Environment.Exit(0);
            else
                break;
        }
        return s;
    }
}