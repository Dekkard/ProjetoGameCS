using LiteDB;
public class Work
{
    private int _id;
    private int _city;
    private Value _workPay;
    private CityWorks _cityWorks;

    public int Id { get => _id; set => _id = value; }
    public int City { get => _city; set => _city = value; }
    public Value WorkPay { get => _workPay; set => _workPay = value; }
    public CityWorks CityWorks { get => _cityWorks; set => _cityWorks = value; }

    public Work()
    {
        _id = ObjectId.NewObjectId().Increment;
        _workPay = new Value(0,0,0,0);
    }

    public Work(int city, Value workPay, CityWorks cityWorks)
    {
        // _id = ObjectId.NewObjectId().Increment;
        _city = city;
        _workPay = workPay;
        _cityWorks = cityWorks;
    }

    public override string ToString()
    {
        return "ID: " + _id + ", id Cidade: " + _city + ", " + _cityWorks + ", " + _workPay.ToString();
    }
}