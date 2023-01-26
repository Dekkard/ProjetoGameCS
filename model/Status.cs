public class Status
{
    private int _strength;
    private int _perception;
    private int _endurance;
    private int _charisma;
    private int _intelligence;
    private int _agility;
    private int _luck;

    public Status(int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck)
    {
        _strength = strength;
        _perception = perception;
        _endurance = endurance;
        _charisma = charisma;
        _intelligence = intelligence;
        _agility = agility;
        _luck = luck;
    }

    public Status() { }
    public int Strength { get => _strength; set => _strength = value; }
    public int Perception { get => _perception; set => _perception = value; }
    public int Endurance { get => _endurance; set => _endurance = value; }
    public int Charisma { get => _charisma; set => _charisma = value; }
    public int Intelligence { get => _intelligence; set => _intelligence = value; }
    public int Agility { get => _agility; set => _agility = value; }
    public int Luck { get => _luck; set => _luck = value; }

    public string DisplayStatus()
    {
        return "Status:\n"
        + "   Força: " + Strength + "\n"
        + "   Percepção: " + Perception + "\n"
        + "   Resistência: " + Endurance + "\n"
        + "   Carisma: " + Charisma + "\n"
        + "   Inteligência: " + Intelligence + "\n"
        + "   Agilidade: " + Agility + "\n"
        + "   Sorte: " + Luck;
    }

    public string StatusPrint()
    {
        return _strength + ";" + _perception + ";" + _endurance + ";" + _charisma + ";" + _intelligence + ";" + _agility + ";" + _luck + ";";
    }
    /* public void StatusRead(string line)
    {
        string[] l = line.Split(";");
        _strength = int.Parse(l[0]);
        _perception = int.Parse(l[1]);
        _endurance = int.Parse(l[2]);
        _charisma = int.Parse(l[3]);
        _intelligence = int.Parse(l[4]);
        _agility = int.Parse(l[5]);
        _luck = int.Parse(l[6]);
    } */
}