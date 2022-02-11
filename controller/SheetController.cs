using static BaseController;
using LiteDB;
#pragma warning disable 8600, 8602, 8603
public class SheetController
{
    public static void CreateHero()
    {
        string nome;
        Hero hero = null;
        Console.WriteLine("Escolha sua Classe: ");
        while (true)
        {
            Console.WriteLine("1: Guerreiro ");
            Console.WriteLine("2: Mago ");
            Console.WriteLine("3: Ladrão ");
            string opt1 = OptRead("Deve-se escolher uma classe para seu herói.").ToLower();
            Console.WriteLine("Escolha o seu nome: ");
            nome = OptRead("Seu herói deve ter um nome.");
            switch (opt1)
            {
                case "1":
                case "g":
                case "warrior":
                case "guerreiro":
                    hero = new Hero(nome, 1, 1, 7, 6, 6, 4, 3, 4, 5);
                    break;
                case "2":
                case "m":
                case "mago":
                case "wizard":
                    hero = new Hero(nome, 1, 2, 3, 5, 5, 4, 8, 5, 5);
                    break;
                case "3":
                case "l":
                case "ladrão":
                case "ladrao":
                case "thief":
                    hero = new Hero(nome, 1, 3, 4, 6, 5, 4, 4, 7, 5);
                    break;
                default:
                    Console.WriteLine("Classe não encontrada, escolha outra.");
                    break;
            }
            if (!opt1.Equals("")) break;
        }
        hero.Hitpoints = hero.TotalHitPoints();
        hero.Energypoints = hero.TotalEnergyPoints();
        hero.Manapoints = hero.TotalManaPoints();
        SaveSheet(hero, false);
    }
    public static void SaveSheet(Hero hero, bool showmsg)
    {
        var db = new LiteDatabase("data.db");
        ILiteCollection<Hero> heros = db.GetCollection<Hero>(HERO);
        try
        {
            Hero hero_b = heros.Query().Where(x => x.Id.Equals(hero.Id)).First();
            bool saved = heros.Update(hero);
            if (showmsg)
            {
                if (saved)
                    Console.WriteLine("Salvo com sucesso.");
                else
                    Console.WriteLine("Falha em salvar");
            }
        }
        catch (System.InvalidOperationException)
        {
            heros.Insert(hero);
            if (showmsg) Console.WriteLine("Salvo com sucesso.");
        }
        db.Dispose();
    }

    public static Hero OpenSheet()
    {
        var db = new LiteDatabase("data.db");
        ILiteCollection<Hero> heros = db.GetCollection<Hero>(HERO);
        Dictionary<int, Hero> heroDict = new Dictionary<int, Hero>();
        List<Hero> heroList = heros.Query().ToList();
        if (heroList.Count > 0)
        {
            while (true)
            {
                int index = 0;
                foreach (Hero h in heroList)
                {
                    Console.WriteLine(++index + ": " + h.Nome);
                    try
                    {
                        heroDict.Add(index, h);
                    }
                    catch (System.ArgumentException) { }
                }
                Console.WriteLine("Selecione o personagem: ");
                string opt = OptRead("Deve escolher um nome ou identifacação.");
                Hero hero;
                try
                {
                    try
                    {
                        hero = heroDict[int.Parse(opt)];
                    }
                    catch (FormatException)
                    {
                        hero = heros.Query().Where(x => x.Nome.Equals(opt)).First();
                    }
                    catch (KeyNotFoundException)
                    {
                        Console.WriteLine("Número não consta na lista.");
                        continue;
                    }
                    hero.inventory = new InventoryController(hero.Id);
                    db.Dispose();
                    return hero;
                }
                catch (System.InvalidOperationException)
                {
                    Console.WriteLine("Nome não consta na lista.");
                    continue;
                }
            }
        }
        else
        {
            Console.WriteLine("Nenhum herói encontrado");
            db.Dispose();
            return null;
        }
    }
}