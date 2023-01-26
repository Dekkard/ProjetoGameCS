using System.Text.RegularExpressions;
#pragma warning disable 8618
public class Option
{
    public String[] cases;
    public Action<String> methodVoid;
    public Func<String, bool> methodReturn;
    public String descr;
    public Regex regex;
    public bool isLoopable;
    private bool _isAction;
    private bool _isRegex;
    public bool isAction { get => _isAction; }
    public bool IsRegex { get => _isRegex; }

    public Option(Func<String, bool> method, String descr, params String[] cases)
    {
        this.cases = cases;
        this.methodReturn = method;
        this.descr = descr;
        _isAction = false;
        _isRegex = false;
    }
    public Option(Action<String> method, bool isLoopable, String descr, params String[] cases)
    {
        this.cases = cases;
        this.methodVoid = method;
        this.isLoopable = isLoopable;
        this.descr = descr;
        _isAction = true;
        _isRegex = false;
    }
    public Option(Func<String, bool> method, String descr, Regex regex)
    {
        this.regex = regex;
        this.methodReturn = method;
        this.descr = descr;
        _isAction = false;
        _isRegex = true;
    }
    public Option(Action<String> method, bool isLoopable, String descr, Regex regex)
    {
        this.regex = regex;
        this.methodVoid = method;
        this.isLoopable = isLoopable;
        this.descr = descr;
        _isAction = true;
        _isRegex = true;
    }
}