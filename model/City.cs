using LiteDB;
using static BaseService;
#pragma warning disable 8600, 8601, 8602, 8604, 8618
public class City
{
    private int _id;
    // private Character _hero;
    private String _name;
    private CitySize _size;
    private List<int> _connections;

    public int Id { get => _id; set => _id = value; }
    // public Character Hero { get => _hero; set => _hero = value; }
    public String Name { get => _name; set => _name = value; }
    public CitySize Size { get => _size; set => _size = value; }
    public List<int> Connections { get => _connections; set => _connections = value; }

    public City()
    {
        _id = ObjectId.NewObjectId().Increment % 10000; // atribuição de id;
        _name = CityNameGenerator(); // Gerador de nomes para a cidade
        _size = (CitySize)new Random().Next(Enum.GetValues(typeof(CitySize)).Length); // Escolhand aletória de tamanho da cidade
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

    public override string ToString()
    {
        return _id + ": " + _name + ", ";
    }
}