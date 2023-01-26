#pragma warning disable 8600, 8602, 8603

public static class BaseService
{
    public const string DATA = "data.db";
    public const string HERO = "hero";
    public const string CHARACTERS = "characters";
    public const string INVENTORY = "inventory";
    public const string CITIES = "cities";
    public const string SERVICES = "services";
    public const string WORKS = "works";
 
    public static int LEVELCAP = 100;
    public static int LEVELXP = 100;
    public static double LEVELMP = 2.5;
    public static string[] returnOption = { "voltar", "cancelar", "volta", "return", "back", "can" };
    public static string[] posResp = {"sim", "s", "yes", "y"};
    public static string[] negResp = {"não", "nao", "n", "no"};

    /// <summary>
    /// Reads a option to write, able to exit on command
    /// </summary>
    public static string BWrite(string msg)
    {
        string s = "";
        while (true)
        {
            Console.Write("> ");
            s = Console.ReadLine().ToLower();
            if (s.Equals(""))
                Console.WriteLine(msg);
            else if (new[] { "x", "q", "quit", "exit", "sair" }.Contains(s))
            {
                Console.Write("Deseja sair do jogo? ");
                switch (Console.ReadLine().ToLower())
                {
                    case "y":
                    case "s":
                    case "yes":
                    case "sim":
                        throw new ForcedExitException();
                    default:
                        continue;
                }
            }
            else
                break;
        }
        return s;
    }
    
    public static void printTitleCard(){
        String[] titlecard = File.ReadAllLines("title-s.card");
        foreach (String l in titlecard)
            Console.WriteLine(l);
    }

    static string printDescription(Option[] options)
    {
        String descr = "";
        foreach (Option opt in options)
        {
            if (opt.descr.Length > 0)
                descr += opt.descr + "\n";
        }
        return descr + "(h) Ajuda";
    }

    public static void OptionInterface(
        String message,
        String msgFail = "Opção inválida.",
        params Option[] options)
    {
        Console.WriteLine(message);
        OptionInterface(msgFail, options);
    }

    public static void OptionInterface(
        String msgFail = "Opção inválida.",
        params Option[] options)
    {
        // Console.Clear();
        // printTitleCard();
        while (true)
        {
            bool isBreakable = false;
            bool isLoopable = false;
            Console.WriteLine(printDescription(options));
            String optionString = BWrite("Digite uma opção");
            if (new[] { "ajuda", "help", "h" }.Contains(optionString))
            {
                continue;
            }
            foreach (Option option in options)
            {
                if ((!option.IsRegex && option.cases.Contains(optionString))
                || (option.IsRegex && option.regex.IsMatch(optionString)))
                {
                    if(option.isAction){
                        option.methodVoid(optionString);
                    }
                    isLoopable = !option.isAction?option.methodReturn(optionString):option.isLoopable;
                    isBreakable = true;
                    break;
                }
            }
            if (isLoopable) continue;
            if (isBreakable) break;
            Console.WriteLine(msgFail);
        }
    }
}