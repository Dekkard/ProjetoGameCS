using LiteDB;
#pragma warning disable 8600, 8601, 8602, 8604, 8618
public class City
{
    private int _id;
    private int _heroId;
    private string _name;
    private CitySize _size;
    private bool _isCurrent;
    private List<int> _connections;

    public int Id { get => _id; set => _id = value; }
    public int HeroId { get => _heroId; set => _heroId = value; }
    public string Name { get => _name; set => _name = value; }
    public CitySize Size { get => _size; set => _size = value; }
    public bool IsCurrent { get => _isCurrent; set => _isCurrent = value; }
    public List<int> Connections { get => _connections; set => _connections = value; }

    /* public City(int heroId)
    {
        _id = ObjectId.NewObjectId().Increment % 10000;
        _heroId = heroId;
        _isCurrent = false;
        _connections = new List<int>();
    } */

    public City(int heroId, bool isCurrent = false)
    {
        _id = ObjectId.NewObjectId().Increment % 10000; // atribuição de id;
        _heroId = heroId;
        _name = CityNameGenerator(); // Gerador de nomes para a cidade
        _size = (CitySize)new Random().Next(Enum.GetValues(typeof(CitySize)).Length); // Escolhand aletória de tamanho da cidade
        _isCurrent = isCurrent; // Se a cidade é a qual o herói está atualmente
        _connections = new List<int>(); // Lista de conexões com outras cidades
    }

    public static string CityNameGenerator() // Gerador de Nomes de cidade a partir de sílabas já formadas
    {
        string name = "";
        Random rng = new Random();
        int syllables = rng.Next(1, 4);// Escolha de quantidade de sílabas aleatóriamente
        for (int i = 1; i <= syllables; i++)
        {
            Type type = Type.GetType("CityName" + i);// Criação da variável 'type' onde há a lista de sílabas
            name += Enum.GetName(type, rng.Next(Enum.GetValues(type).Length));// Escolha da sílaba aleatóriamente, a partir da lista em 'type'
        }
        if (syllables == 1)// Se o nome contém apenas uma sílaba lhe será dado um sufixo aleatório
            name += Enum.GetName(typeof(CitySuffix), rng.Next(Enum.GetValues(typeof(CitySuffix)).Length)).Replace("_", " ");
        return name;
    }

    public static void SyllableMaker(int reptition = 100) // Criação de sílabas aleatórias
    {
        string[] vowals = { "a", "e", "i", "o", "u" };// Lista de vogais
        string[] consoants = { "b", "c", "d", "f", "g", "h", "j", "l", "m", "n", "p", "q", "r", "s", "t", "v" };// Lista de consoantes comuns
        string[] c_inc = { "k", "w", "x", "y", "z" };//Lista de consoantes incomuns
        int v_len = vowals.Length;
        int c_len = consoants.Length;
        int ci_len = c_inc.Length;
        Random rng = new Random();
        List<string> syllables = new List<string>();
        for (int i = 0; i < reptition; i++) // Definido por usuário, a quantidade de sílabas a serem criadas
        {
            string syllable = "";
            int opt = rng.Next(1, 4);// Esolhe como será construido a sílaba aleatóriamente
            switch (opt)
            {
                case 1://uma consoante e mais uma vogal, e uma chance de ter-se uma letra 'u' no meio delas
                    syllable += consoants[rng.Next(c_len)] + (rng.Next(10) > 9 ? "u" : "") + vowals[rng.Next(v_len)];
                    break;
                case 2:// Uma consoante incomun, uma consoante e uma vogal
                    syllable += c_inc[rng.Next(ci_len)] + consoants[rng.Next(c_len)] + vowals[rng.Next(v_len)];
                    break;
                case 3:// Uma consoante incomun, uma vogal e uma consoante
                    syllable += c_inc[rng.Next(ci_len)] + vowals[rng.Next(v_len)] + consoants[rng.Next(c_len)];
                    break;
                case 4:// Uma consoante incomun, uma vogal, uma consoante e mais uma vogal
                    syllable += c_inc[rng.Next(ci_len)] + vowals[rng.Next(v_len)] + consoants[rng.Next(c_len)] + vowals[rng.Next(v_len)];
                    break;
                case 5:// Uma vogal, uma consoante incomun e uma consoante comum
                    syllable += vowals[rng.Next(v_len)] + c_inc[rng.Next(ci_len)] + consoants[rng.Next(c_len)];
                    break;
            }
            syllables.Add(syllable);
        }
    }

    /* public void AssignCityServices()
    {
        Random rng = new Random();
        int totalServ = (int)_size;
        while (totalServ > 0)
        {
            _services.Add((CityServices)rng.Next(Enum.GetValues(typeof(CityServices)).Length));
            totalServ--;
        }
    }

    public void AssignCityWorks()
    {
        Random rng = new Random();
        int totalWorks = (int)_size;
        while (totalWorks > 0)
        {
            _works.Add((CityWorks)rng.Next(Enum.GetValues(typeof(CityWorks)).Length));
            totalWorks--;
        }
    }
    */
    public override string ToString()
    {
        return _id + ": " + _name + ", " + _heroId + ", " + _isCurrent;
    }
}
public class Services
{
    private int _id;
    private int _cityId;
    private Value _riches;
    private CityServices _cityServices;

    public InventoryController inventory;

    public int Id { get => _id; set => _id = value; }
    public int CityId { get => _cityId; set => _cityId = value; }
    public Value Riches { get => _riches; set => _riches = value; }
    public CityServices CityServices { get => _cityServices; set => _cityServices = value; }

    public Services() { }

    public Services(int cityId, Value riches, CityServices cityServices)
    {
        _id = ObjectId.NewObjectId().Increment;// Id aleatório
        _cityId = cityId;
        _riches = riches;
        _cityServices = cityServices;
    }

    public override string ToString()
    {
        return "ID: " + _id + ", id Cidade: " + _cityId + ", " + _cityServices + ", " + _riches.ToString();
    }
}
public class Works
{
    private int _id;
    private int _cityId;
    private Value _workPay;
    private CityWorks _cityWorks;

    public int Id { get => _id; set => _id = value; }
    public int CityId { get => _cityId; set => _cityId = value; }
    public Value WorkPay { get => _workPay; set => _workPay = value; }
    public CityWorks CityWorks { get => _cityWorks; set => _cityWorks = value; }

    public Works() { _id = ObjectId.NewObjectId().Increment; }

    public Works(int citId, Value workPay, CityWorks cityWorks)
    {
        _id = ObjectId.NewObjectId().Increment;
        _cityId = citId;
        _workPay = workPay;
        _cityWorks = cityWorks;
    }

    public override string ToString()
    {
        return "ID: " + _id + ", id Cidade: " + _cityId + ", " + _cityWorks + ", " + _workPay.ToString();
    }
}