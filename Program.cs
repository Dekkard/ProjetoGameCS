using static BaseController;
using LiteDB;
public class Program
{
    public static void Main(string[] args)
    {
        game();
        /* Random rng = new Random();
        int [] list = {1,2,3,4,5};
        Console.WriteLine(list.Length + "["+list[0]+"..."+list[4]+"]");
        for(int i=0;i<25;i++){
            Console.Write(list[rng.Next(list.Length)] + ", ");
        }
        Console.WriteLine(); */
    }

#pragma warning disable 8600, 8602, 8604
    public static void game()
    {
        Hero hero = null;
        Console.WriteLine(TITLE);
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
                case "test":
                case "teste":
                case "t":
                    Console.WriteLine("Entrando em ambiente de teste.");
                    TestEnv();
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

    public static void TestEnv()
    {
        while (true)
        {
            string opt = OptRead("").ToLower();
            switch (opt)
            {
                case "database":
                case "db":
                    var db = new LiteDatabase("data.db");
                    foreach (Hero hero in db.GetCollection<Hero>(HERO).Query().ToList())
                    {
                        Console.WriteLine(hero.Id + ": " + hero.DisplayHeroInfo(", "));
                        foreach (Item item in db.GetCollection<Item>(INVENTORY).Query().Where(i => i.ownerId.Equals(hero.Id)).ToList())
                        {
                            Console.WriteLine("\t" + item.ToString());
                        }
                    }
                    foreach (City city in db.GetCollection<City>(CITIES).Query().ToList())
                    {
                        Console.WriteLine(city.ToString());
                        List<Services> ListServ = db.GetCollection<Services>(SERVICES).Query().Where(s => s.CityId.Equals(city.Id)).ToList();
                        Console.WriteLine("Serviços: " + ListServ.Count);
                        foreach (Services s in ListServ)
                        {
                            Console.WriteLine("\t" + s.ToString());
                        }
                        Console.WriteLine("Conexões: " + city.Connections.Count);
                        foreach (int i in city.Connections)
                        {
                            City cityc = db.GetCollection<City>(CITIES).Query().Where(ci => ci.Id.Equals(i)).First();
                            Console.WriteLine("\t" + cityc.Id + ": " + cityc.Name);
                        }
                    }
                    break;
                case "enemytest":
                case "enemy":
                    int qtd = 0;
                    while (true)
                    {
                        try
                        {
                            qtd = int.Parse(Console.ReadLine());
                            break;
                        }
                        catch (FormatException) { }
                    }
                    while (qtd-- > 0)
                    {
                        Enemy e = EnemyController.NewEnemy(1,10,10,1,new[] {0,1});
                        Console.WriteLine(e.Name);
                    }
                    break;
                case "game":
                    game();
                    break;
                case "sair":
                case "exit":
                case "quit":
                case "q":
                    System.Environment.Exit(0);
                    return;
                case "sound":
                    opt = OptRead("").ToLower();
                    switch (opt)
                    {
                        case "levelup":
                        case "lvl":
                        case "up":
                            Console.WriteLine("LevelUp: música de level up!");
                            Sounds.LevelUpSound();
                            break;
                        case "cave":
                            Sounds.Cave();
                            break;
                        case "play":
                            Sounds.Play();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Database: visualisa objetos inseridos no banco de dados."
                        + "\nSoundTest: realiza testes de som com beeps."
                        + "\nGame: Continua com o jogo."
                        + "\nSair: fecha o programa."
                    );
                    break;
            }
        }
    }
    private static string TITLE = @"
                                 _____________________________________________________________________________________
                                /  _______________________________________________________________________________   /
                               /  /  __         __________  _________  __________  ___   __  ______    ______    /  /
                              /  /  / /        / ________/ / _______/ / ________/ /   | / / / ____ \  (  ____)  /  /
                             /  /  / /        / /_______  / / _____  / /_______  / /| |/ / / /    \ \  \ \     /  /
                            /  /  / /        / ________/ / / /_  _/ / ________/ / / | | / / /     / /   ) )   /  /
                           /  /  / /______  / /_______  / /___/ /  / /_______  / /  |  / / /_____/ / ___\ \  /  /
                          /  /  /________/ /_________/ /_______/  /_________/ /_/   |_/ /_________/ /_____/ /  /
                         /  /   ___  ___     __________  __      __  __________  __________  ______        /  /
                        /  /   /  / /_      / ______  / / /     / / /___  ____/ / ________/ / ____ |      /  /
                       /  /   /__/ /       / /     / / / /     / /     / /     / /_______  / /___/ /     /  /
                      /  /  ____     __   / /     / / / /     / /     / /     / ________/ / __  __/     /  /
                     /  /    /  /_/ /_   / /_____/ / / /_____/ /     / /     / /_______  / /  \ \      /  /
                    /   \   /  / / /_   /_________/ /_________/     /_/     /_________/ /_/    \_\    /  /
                   /  _  \   __    __    __  _________  _______  __       _________    _______       /  /
                  /  / \  \  \ \   \ \   \ \ \  _____ \ \  ___ \ \ \      \  _____ \  (  _____)     /  /
                 /  /   \  \  \ \   \ \   \ \ \ \    \ \ \ \__\ | \ \      \ \    \ \  \ \         /  /
                /  /     \  \  \ \   \ \   \ \ \ \    \ \ \  __/\  \ \      \ \    \ \   ) )      /  /
               /  /       \  \  \ \___\ \___\ \ \ \____\ \ \ \ \ \  \ \_____ \ \___/ / ___\ \    /  /
              /  /         \  \  \_____________\ \________\ \_\ \_\  \______\ \_____/ (______)  /  /
             /  /___________\  \_______________________________________________________________/  /
            /____________________________________________________________________________________/
            
"; 
}