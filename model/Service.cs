using LiteDB;
using static BaseService;
public class Services
{
    private int _id;
    private int _city;
    private Value _riches;
    private CityServices _cityServices;
    private int _vendor;
    // private List<int> inventory;

    public int Id { get => _id; set => _id = value; }
    public int City { get => _city; set => _city = value; }
    public Value Riches { get => _riches; set => _riches = value; }
    public CityServices CityServices { get => _cityServices; set => _cityServices = value; }
    public int Vendor { get => _vendor; set => _vendor = value; }
    // public List<int> Inventory { get => inventory; set => inventory = value; }

    public Services() {
        _riches = new Value(0,0,0,0);
    }

    public Services(int city, Value riches, CityServices cityServices)
    {
        _id = ObjectId.NewObjectId().Increment;// Id aleatório
        _city = city;
        _riches = riches;
        _cityServices = cityServices;
    }

    public override string ToString()
    {
        return "ID: " + _id + ", id Cidade: " + _city + ", " + _cityServices + ", " + _riches.ToString();
    }
}