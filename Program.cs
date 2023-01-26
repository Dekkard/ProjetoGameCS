using static BaseService;
using LiteDB;
public class Program
{
    public static void Main(string[] args)
    {
        LiteDatabase db = new LiteDatabase("data.db");
        try
        {
            game(db);
        }
        catch (ForcedExitException fee)
        {
            if (fee.HasMessage) Console.WriteLine(fee.Message);
        }
        db.Dispose();
    }

#pragma warning disable 8600, 8602, 8604
    public static void game(LiteDatabase db)
    {
        Hero hero = null;
        Console.Clear();
        printTitleCard();
        OptionInterface("Bem-vindo Herói!",
            new Option((o) =>
            {
                hero = SheetController.OpenSheet(db);
                return hero == null;
            }, "(c) Carregar", "carregar", "load", "c"),
            new Option((o) =>
            {
                hero = SheetController.CreateHero(db);
                CityController cc = new CityController(db, new HeroController(db, hero));
                cc.CreateCity(hero, true, hero.Level, hero.RarityModifier(), hero.QualityModifier());
                OptionInterface("Começar aventura agora?", "", 
                    new Option((o)=> game(db), false, "Sim", posResp),
                    new Option((o)=> false, "Não", negResp)
                );
                return true;
            }, "(n) Novo Herói", "novo", "new", "n"),
            new Option((o) =>
            {
                TestEnv(db);
                return true;
            }, "", "test", "teste")
        );
        Console.WriteLine("Começando aventura.\nDigite 'ajuda' para ver os comandos.");
        HeroController hc = new HeroController(db, hero);
        hc.CommandInput();
        game(db);
    }

    public static void TestEnv(LiteDatabase db)
    {
        OptionInterface("Entrando em ambiente de teste.",
            new Option((o) =>
            {
                foreach (Hero hero in db.GetCollection<Hero>(HERO).Query().ToList())
                {
                    Console.WriteLine(hero.Id + ": " + hero.DisplayHeroInfo(", "));
                    foreach (Item item in db.GetCollection<Item>(INVENTORY).Query().Where(i => i.owner.Equals(hero.Id)).ToList())
                    {
                        Console.WriteLine("\t" + item.ToString());
                    }
                }
                foreach (City city in db.GetCollection<City>(CITIES).Query().ToList())
                {
                    Console.WriteLine(city.ToString());
                    List<Services> ListServ = db.GetCollection<Services>(SERVICES).Query().Where(s => s.City.Equals(city.Id)).ToList();
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
                return true;
            }, "Database: visualisa objetos inseridos no banco de dados.", "database", "db"),
            new Option((o) =>
            {
                int qtd = 0;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Digite quantidade");
                        qtd = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch (FormatException) { }
                }
                while (qtd-- > 0)
                {
                    Enemy e = new Enemy();
                    EnemyController enemyController = new EnemyController(db, e);
                    enemyController.GenerateEnemy(1, 10, 10, 1, db, new[] { 0, 1 });
                    Console.WriteLine(e.Name);
                }
                return true;
            }, "Enemy: ", "enemytest", "enemy"),
            new Option((o) =>
            {
                bool back = true;
                OptionInterface("Selecione o teste de som",
                    new Option((o) => { Sounds.LevelUpSound(); return true; }, "LevelUp: música de level up!", "levelup", "lvl", "up"),
                    new Option((o) => { Sounds.Cave(); return true; }, "Cave: toca música de caverna", "cave"),
                    new Option((o) => { Sounds.Play(); return true; }, "Play: abre (MIDI) de som", "play"),
                    new Option((o) => { back = false; return false; }, "Voltar", returnOption)
                );
                return back;
            }, "SoundTest: realiza testes de som com beeps.", "sound"),
            new Option((o) => { game(db); return true; }, "Game: Continua com o jogo.", "game")
        );
    }
}