

public enum AutoInjectionType
{
    Singleton,
    Scoped,
    Transient,
}

public abstract class AutoInjectBaseAttribute(AutoInjectionType autoInjectionType) : Attribute
{
    public AutoInjectionType AutoInjectionType { get; protected set; } = autoInjectionType;
    public abstract Type? ImplementationType { get; }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoInjectAttribute(AutoInjectionType autoInjectionType) : AutoInjectBaseAttribute(autoInjectionType)
{
    public override Type? ImplementationType => null;
}


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoInjectAttribute<T>(AutoInjectionType autoInjectionType) : AutoInjectBaseAttribute(autoInjectionType)
{
    public override Type? ImplementationType => typeof(T);
}