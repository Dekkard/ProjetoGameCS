#pragma warning disable 659, 8603
public class Value
{
    private int _platinum;
    private int _gold;
    private int _silver;
    private int _copper;

    public int Platinum { get => _platinum; set => _platinum = value; }
    public int Gold { get => _gold; set => _gold = value; }
    public int Silver { get => _silver; set => _silver = value; }
    public int Copper { get => _copper; set => _copper = value; }

    public Value(int platinum, int gold, int silver, int copper)
    {
        _platinum = platinum;
        _gold = gold;
        _silver = silver;
        _copper = copper;
    }
    public Value(String values)
    {
        string[] v = values.Split(",");
        _platinum = int.Parse(v[0]);
        _gold = int.Parse(v[1]);
        _silver = int.Parse(v[2]);
        _copper = int.Parse(v[3]);
    }

    public static Value operator +(Value v1, Value v2)
    {
        int c = 0, s = 0, g = 0, p = 0;
        c += v1.Copper + v2.Copper;
        if (c >= 100)
        {
            s += c / 100;
            c = c % 100;
        }
        s += v1.Silver + v2.Silver;
        if (s >= 100)
        {
            g += s / 100;
            s = s % 100;
        }
        g += v1.Gold + v2.Gold;
        if (g >= 100)
        {
            p += g / 100;
            g = g % 100;
        }
        p += v1.Platinum + v2.Platinum;
        return new Value(p, g, s, c);
    }
    public static Value operator -(Value v1, Value v2)
    {
        int c = 0, s = 0, g = 0, p = 0;
        p = v1.Platinum - v2.Platinum;
        g = v1.Gold - v2.Gold;
        s = v1.Silver - v2.Silver;
        c = v1.Copper - v2.Copper;
        if (c < 0)
        {
            s -= 1;
            c += 100;
        }
        if (s < 0)
        {
            g -= 1;
            s += 100;
        }
        if (g < 0)
        {
            p -= 1;
            g += 100;
        }
        if (p < 0 || g < 0 || s < 0 || c < 0)
        {
            Console.WriteLine("Transação impossível, dinheiro insuficiente.");
            return null;
        }
        return new Value(p, g, s, c);
    }

    public static Value operator *(Value v, int val)
    {
        return new Value(v.Platinum * val, v.Gold * val, v.Silver * val, v.Copper * val);
    }

    public override string ToString()
    {
        return _platinum + ", " + _gold + ", " + _silver + ", " + _copper;
    }

    public override bool Equals(object? obj)
    {
        return obj is Value value &&
               Platinum == value.Platinum &&
               Gold == value.Gold &&
               Silver == value.Silver &&
               Copper == value.Copper;
    }

    public bool CompareTo(Value value)
    {
        if (_platinum >= value.Platinum)
            if (_gold >= value.Gold)
                if (_silver >= value.Silver)
                    if (_copper >= value.Copper)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;
        else
            return false;
    }

    public static Value GenerateValue(Random rng, int rarity, int quality)
    {
        int platinum = Convert.ToInt32(rng.Next(1, 100000) > 90900 ? (0.0001 * rng.Next(rarity * quality)) : 0);
        int gold = Convert.ToInt32(rng.Next(1, 10000) > 9090 ? (0.01 * rng.Next(rarity * quality)) : 0);
        int silver = Convert.ToInt32(0.1 * rng.Next(Convert.ToInt32(rng.Next(rarity * quality))));
        int copper = Convert.ToInt32(10 * (1 + rng.Next(Convert.ToInt32(rng.Next(rarity * quality)))));
        if (copper >= 100)
        {
            silver += copper / 100;
            copper = copper % 100;
        }
        if (silver >= 100)
        {
            gold += silver / 100;
            silver = silver % 100;
        }
        if (gold >= 100)
        {
            platinum += gold / 100;
            gold = gold % 100;
        }
        return new Value(platinum, gold, silver, copper);
    }
}
