public class StatusSPECIAL
{
    private int _strength;
    private int _perception;
    private int _endurance;
    private int _charisma;
    private int _intelligence;
    private int _agility;
    private int _luck;

    public StatusSPECIAL(int strength, int perception, int endurance, int charisma, int intelligence, int agility, int luck)
    {
        _strength = strength;
        _perception = perception;
        _endurance = endurance;
        _charisma = charisma;
        _intelligence = intelligence;
        _agility = agility;
        _luck = luck;
    }

    public StatusSPECIAL() { }
    public int Strength { get => _strength; set => _strength = value; }
    public int Perception { get => _perception; set => _perception = value; }
    public int Endurance { get => _endurance; set => _endurance = value; }
    public int Charisma { get => _charisma; set => _charisma = value; }
    public int Intelligence { get => _intelligence; set => _intelligence = value; }
    public int Agility { get => _agility; set => _agility = value; }
    public int Luck { get => _luck; set => _luck = value; }

    public string DisplayStatus()
    {
        return "Habilidade de Herói:\n"
        + "\tForça: " + Strength + "\n"
        + "\tPercepção: " + Perception + "\n"
        + "\tResistência: " + Endurance + "\n"
        + "\tCarisma: " + Charisma + "\n"
        + "\tInteligência: " + Intelligence + "\n"
        + "\tAgilidade: " + Agility + "\n"
        + "\tSorte: " + Luck;
    }
}