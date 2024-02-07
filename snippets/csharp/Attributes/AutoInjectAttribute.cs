

public enum AutoInjectionType
{
    Singleton,
    Scoped,
    Transient,
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoInjectAttribute : Attribute
{
    public AutoInjectionType AutoInjectionType { get; }
    // public InjectionProject Project { get; } = InjectionProject.Always;
    public Type? InterfaceType { get; set; }

    public AutoInjectAttribute(AutoInjectionType injectionType, InjectionProject project = InjectionProject.Always)
    {
        AutoInjectionType = injectionType;
        Project = project;
    }
}