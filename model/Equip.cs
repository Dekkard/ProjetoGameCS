using static BaseController;
#pragma warning disable CS8602
public class Equip
{
    private int _idHero;
    private int _head;
    private int _body;
    private int _shoulder;
    private int _wrist;
    private int _hand;
    private int _legs;
    private int _feet;
    private int _cape;
    private int _belt;
    private int _rightRing;
    private int _leftRing;
    private int _neck;
    private int _rightHand;
    private int _leftHand;

    private Modifiers _equipMod;

    public int IdHero { get => _idHero; set => _idHero = value; }
    public int Head { get => _head; set => _head = value; }
    public int Body { get => _body; set => _body = value; }
    public int Shoulder { get => _shoulder; set => _shoulder = value; }
    public int Wrist { get => _wrist; set => _wrist = value; }
    public int Hand { get => _hand; set => _hand = value; }
    public int Legs { get => _legs; set => _legs = value; }
    public int Feet { get => _feet; set => _feet = value; }
    public int Cape { get => _cape; set => _cape = value; }
    public int Belt { get => _belt; set => _belt = value; }
    public int RightRing { get => _rightRing; set => _rightRing = value; }
    public int LeftRing { get => _leftRing; set => _leftRing = value; }
    public int Neck { get => _neck; set => _neck = value; }
    public int RightHand { get => _rightHand; set => _rightHand = value; }
    public int LeftHand { get => _leftHand; set => _leftHand = value; }
    public Modifiers EquipMod { get => _equipMod; set => _equipMod = value; }

    public Equip()
    {
        _idHero = 0;
        _head = 0;
        _body = 0;
        _shoulder = 0;
        _wrist = 0;
        _hand = 0;
        _legs = 0;
        _feet = 0;
        _cape = 0;
        _belt = 0;
        _rightRing = 0;
        _leftRing = 0;
        _neck = 0;
        _rightHand = 0;
        _leftHand = 0;
        _equipMod = new Modifiers();
    }

    public Equip(int idHero, int head, int body, int shoulder, int wrist, int hand, int legs, int feet, int cape, int belt, int rightRing, int leftRing, int neck, int rightHand, int leftHand, Modifiers equipMod)
    {
        _idHero = idHero;
        _head = head;
        _body = body;
        _shoulder = shoulder;
        _wrist = wrist;
        _hand = hand;
        _legs = legs;
        _feet = feet;
        _cape = cape;
        _belt = belt;
        _rightRing = rightRing;
        _leftRing = leftRing;
        _neck = neck;
        _rightHand = rightHand;
        _leftHand = leftHand;
        _equipMod = equipMod;
    }

    public void AddTotalMod(Item item)
    {
        EquipMod += item.Modifiers;
    }
    public void ReduceTotalMod(Item item)
    {
        EquipMod -= item.Modifiers;
    }
}
public enum EquipSlot
{
    Head = 1, Body, Shoulder, Wrist, Hand, Legs, Feet, Cape, Belt, RightRing, LeftRing, Neck, RightHand, LeftHand
}