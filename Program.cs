using static BaseController;
using LiteDB;
public class Program
{
    public static void Main(string[] args)
    {
        game();
        /* var db = new LiteDatabase("data.db");
        foreach (City city in db.GetCollection<City>(CITIES).Query().ToList())
        {
            Console.WriteLine(city.ToString()+"[conn:"+city.Connections.Count+"]");
            foreach(Services s in db.GetCollection<Services>(SERVICES).Query().Where(s => s.CityId.Equals(city.Id)).ToList()){
                Console.WriteLine("\t"+s.ToString());
            }
            foreach (int i in city.Connections)
            {
                Console.WriteLine("\t"+i);
            }
        } */
    }

#pragma warning disable 8600
    public static void game()
    {
        Hero hero = null;
        Console.WriteLine("Bem-vindo Herói!");
        while (true)
        {
            Console.WriteLine("Carregar\nNovo Herói");
            string opt = OptRead("Digite uma opção.").ToLower();
            switch (opt)
            {
                case "carregar":
                case "load":
                case "c":
                    hero = SheetController.OpenSheet();
                    break;
                case "novo":
                case "new":
                case "n":
                    SheetController.CreateHero();
                    break;
                case "exit":
                case "quit":
                case "sair":
                case "q":
                    System.Environment.Exit(0);
                    break;
                case "help":
                case "ajuda":
                case "h":
                    Console.WriteLine("Comandos possíveis");
                    Console.WriteLine("\tCarregar: Carrega um herói.");
                    Console.WriteLine("\tNovo: Cria um novo herói.");
                    Console.WriteLine("\tSair: Fecha programa.");
                    break;
                default:
                    Console.WriteLine("Comando inválido.");
                    break;
            }
            if (hero != null)
            {
                break;
            }
        }
        Console.WriteLine("Começando aventura.\nDigite 'ajuda' para ver os comandos.");
        HeroController hc = new HeroController(hero);
        hc.CommandInput();
        game();
    }
}