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

    public Equip()
    {

    }

    public Equip(int idHero, int head, int body, int shoulder, int wrist, int hand, int legs, int feet, int cape, int belt, int rightRing, int leftRing, int neck, int rightHand, int leftHand)
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
    }
}
public enum EquipSlot
{
    Head = 1, Body, Shoulder, Wrist, Hand, Legs, Feet, Cape, Belt, RightRing, LeftRing, Neck, RightHand, LeftHand
}