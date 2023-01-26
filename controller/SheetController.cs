using static BaseService;
using LiteDB;
#pragma warning disable 8600, 8602, 8603
public class SheetController
{
    public static Hero CreateHero(LiteDatabase db)
    {
        string nome;
        Hero hero = new Hero();

        Console.WriteLine("Escolha o seu nome: ");
        nome = BWrite("Seu herói deve ter um nome.");
            /* .Split(" ")
            .Select(n => n.Substring(0,1).ToUpper()+n.Substring(1))
            .Aggregate((n1,n2)=> n1 + " " + n2); */
        OptionInterface("Escolha sua Classe",
            "Deve-se escolher uma classe para seu herói.",
            new Option((o) => hero = new Hero(nome, 1, 1, 0, 0, 7, 6, 6, 4, 3, 4, 5), false,
            "1: Guerreiro", "1", "g", "warrior", "guerreiro"),
            new Option((o) => hero = new Hero(nome, 1, 2, 0, 0, 3, 5, 5, 4, 8, 5, 5), false,
            "2: Mago", "2", "m", "wizard", "mago"),
            new Option((o) => hero = new Hero(nome, 1, 3, 0, 0, 4, 6, 5, 4, 4, 7, 5), false,
            "3: Ladrão", "3", "l", "thief", "ladrao", "ladrão")
        );

        hero.Hitpoints = hero.TotalHitPoints();
        hero.Energypoints = hero.TotalEnergyPoints();
        hero.Manapoints = hero.TotalManaPoints();
        SaveSheet(db, hero, false);
        return hero;
    }
    public static void SaveSheet(LiteDatabase db, Hero hero, bool showmsg)
    {
        ILiteCollection<Hero> heros = db.GetCollection<Hero>(HERO);
        try
        {
            Hero hero_b = heros.Query().Where(x => x.Id.Equals(hero.Id)).First();
            bool saved = heros.Update(hero);
            if (saved && showmsg)
                Console.WriteLine("Salvo com sucesso.");
            else if (showmsg)
                Console.WriteLine("Falha em salvar");
        }
        catch (InvalidOperationException)
        {
            heros.Insert(hero);
            if (showmsg) Console.WriteLine("Salvo com sucesso.");
        }
    }

    public static Hero OpenSheet(LiteDatabase db)
    {
        ILiteCollection<Hero> heros = db.GetCollection<Hero>(HERO);
        Dictionary<int, Hero> heroDict = new Dictionary<int, Hero>();
        List<Hero> heroList = new List<Hero>();
        try{
        heroList = heros.Query().ToList();
        }
        catch(FormatException e){
            Console.WriteLine(e.Message);
        }
        if (heroList.Count <= 0)
        {
            Console.WriteLine("Nenhum herói encontrado");
            return null;
        }
        int index = 1;
        string heroTextblock = "";
        try
        {
            foreach (Hero h in heroList)
            {
                heroTextblock += index + ": " + h.Name + (heroList.Count > index ? "\n" : "");
                heroDict.Add(index++, h);
            }
        }
        catch (ArgumentException)
        {
            throw new ForcedExitException("Falha em carregar heróis.");
        }
        Hero hero = new Hero();
        bool cancelSelection = false;
        OptionInterface("Selecione o personagem: ",
        "Deve escolher um nome ou identificação.",
            new Option((o) =>
            {
                try
                {
                    hero = heroDict[int.Parse(o)];
                }
                catch (FormatException)
                {
                    hero = heros.Query().Where(x => x.Name.Equals(o)).First();
                }
                catch (Exception e) when (e is KeyNotFoundException
                                        | e is InvalidOperationException)
                {
                    Console.WriteLine("Número não consta na lista.");
                    return true;
                }
                return false;
            }, heroTextblock, heroDict.Keys.Select(k => k.ToString()).ToArray()),
            new Option((o) => true, "Voltar: cancelar seleção", returnOption)
        );
        if (cancelSelection) return null;
        return hero;
    }
}