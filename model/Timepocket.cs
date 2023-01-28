public class Timepocket
{
    private int _second;
    private int _minute;
    private int _hour;
    private int _day;
    private int _weekday;
    private int _month;
    private int _year;
    private int _era;
    private int _cycle;

    public Timepocket(int second, int minute, int hour, int day, int month, int year, int era, int cycle)
    {
        _second = second;
        _minute = minute;
        _hour = hour;
        _day = day;
        _month = month;
        _year = year;
        _era = era;
        _cycle = cycle;
        _weekday = CalcWeekDay();
    }

    public Timepocket()
    {
        Random rng = new Random();
        _second = rng.Next(0, 60);
        _minute = rng.Next(0, 60);
        _hour = rng.Next(0, 24);
        _month = rng.Next(Enum.GetValues<Timepocket_Month>().Length + 1);
        _day = rng.Next(1, (_month % 2 == 0 ^ _month <= 6 ? 31 : 30) + 1);
        _year = rng.Next(1, 10000);
        _era = rng.Next(1, 100);
        _cycle = rng.Next(Enum.GetValues<Timepocket_Cycle>().Length + 1);
        _weekday = CalcWeekDay();
    }

    public int CalcWeekDay()
    {
        return (_year % 7 + (_day + (31 + 30) * (_month / 2) + (_month % 2 == 0 ^ _month <= 6 ? 31 : 0))) % 7;
    }

    public static Timepocket operator +(Timepocket t1, Timepocket t2)
    {
        int months = Enum.GetValues<Timepocket_Month>().Length;

        int second = t1._second + t2._second;
        int minute = t1._minute + t2._minute + second / 60;
        int hour = t1._hour + t2._hour + minute / 60;
        int day = t1._day + t2._day + hour / 24;// 
        int month = t1._month + t2._month;
        int cur_month = month % months;
        cur_month = (cur_month % 2 == 0 ^ cur_month <= 6 ? 31 : 30);
        month += day / cur_month;
        int year = t1._year + t2._year + month / months;
        int era = t1._era + t2._era + year / 10000;
        int cycle = t1._cycle + t2._cycle;

        second %= 60;
        minute %= 60;
        hour %= 24;
        day %= 60;
        month %= 60;
        year %= 60;
        era %= 60;
        cycle %= 60;

        return new Timepocket(second, minute, hour, day, month, year, era, cycle);
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return _second + ":"
        + _minute + ":"
        + _hour + ", "
        + (Timepocket_Week)_weekday + ", de"
        + (Timepocket_Month)_month + ", de "
        + _year + ", "
        + _era + "ª era de "
        + (Timepocket_Cycle)_cycle;
    }
}

public enum Timepocket_Week
{
    Mendim = 1, Rogh, Meddad, Gohrnam, Forust, Frestar, Sollari
}

public enum Timepocket_Month
{
    Eraccus = 1, Luggavian, Taeujead, Qenrantr, Julbam, Perllam, Fahllar, Hilbramnir, Yiidrar, Hvenma, Daemahnur, Reamahnur
}

public enum Timepocket_Cycle
{
    Primaerum = 1, Secardus, Tratarmin, Yilon, Kinmaera, Siekarvos, Shiekarvos, Oovlav, Ulkrat
}