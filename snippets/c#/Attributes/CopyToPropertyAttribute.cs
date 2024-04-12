[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class CopyToPropertyAttribute<T> : Attribute
{
    public Type DestinationType => typeof(T);
    public string Name { get; }

    public CopyToPropertyAttribute([CallerMemberName] string name = "")
    {
        Name = name;
    }
}