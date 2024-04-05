public abstract class Enumeration
{
    public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : Enumeration
    {
        var fields = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);

        var result = fields.Select(f => f.GetValue(null)).Cast<TEnum>();

        return result;
    }
}

public abstract class Enumeration<T>(T value, [CallerMemberName] string name="") : Enumeration, IComparable
{
    public string Name { get; } = name;
    public T Value { get; } = value;

    public static Type ValueType => typeof(T);

    public int CompareTo(object obj) => Name.CompareTo(((Enumeration<T>) obj).Name);

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration<T> other)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Name.Equals(other.Name);

        return typeMatches && valueMatches;
    }

    public static bool operator ==(Enumeration<T> left, Enumeration<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Enumeration<T> left, Enumeration<T> right)
    {
        return !left.Equals(right);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}


public class FriendEnum(uint value, [CallerMemberName] string name = "") : Enumeration<uint>(value, name)
{
    public static readonly FriendEnum Ryan = new(1);
    public static readonly FriendEnum Tim = new(2);
    public static readonly FriendEnum Dyl = new(3);
    public static readonly FriendEnum Gura = new(4);
}

public class CarEnum(uint value, [CallerMemberName] string name = "") : Enumeration<uint>(value, name)
{
    public static readonly CarEnum Ford   = new(1);
    public static readonly CarEnum Chevy  = new(2);
    public static readonly CarEnum Toyota = new(3);
    public static readonly CarEnum Honda  = new(4);
}


public class WeekDayEnum(string value, [CallerMemberName] string name = "") : Enumeration<string>(value, name)
{
    public static readonly WeekDayEnum Sunday    = new("Sunday");
    public static readonly WeekDayEnum Monday    = new("Monday");
    public static readonly WeekDayEnum Tuesday   = new("Tuesday");
    public static readonly WeekDayEnum Wednesday = new("Wednesday");
    public static readonly WeekDayEnum Thursday  = new("Thursday");
    public static readonly WeekDayEnum Friday    = new("Friday");
    public static readonly WeekDayEnum Saturday  = new("Saturday");
}