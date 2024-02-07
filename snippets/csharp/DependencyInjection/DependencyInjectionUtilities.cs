public class DependencyInjectionUtilities
{
    public static Assembly? ServicesAssembly => Assembly.GetAssembly(typeof(IConfigs));

    public static void InjectAll(IServiceCollection services, bool isProduction, InjectionProject projects, Assembly? executingEssembly)
    {
        InjectAll(services, isProduction, projects, ServicesAssembly, executingEssembly);
    }

    private static void InjectAll(IServiceCollection services, bool isProduction, InjectionProject projects, params Assembly?[] assemblies)
    {
        InjectConfigs(services, isProduction);

        foreach (var assembly in assemblies)
        {
            if (assembly != null)
            {
                InjectServicesIntoAssembly(services, projects, assembly);
            }
        }
    }



    public static void InjectConfigs(IServiceCollection services, bool isProduction)
    {
        if (isProduction)
        {
            services.AddSingleton<IConfigs, ConfigurationProduction>();
        }
        else
        {
            services.AddSingleton<IConfigs, ConfigurationDev>();
        }
    }



    public static void InjectServicesIntoAssembly(IServiceCollection services, InjectionProject projectType, Assembly assembly)
    {
        var serviceTypes = assembly.GetTypes().Where(t => t.IsClass && t.GetCustomAttribute<AutoInjectAttribute>() != null).ToList() ?? new List<Type>();

        foreach (var serviceType in serviceTypes)
        {
            InjectService(services, projectType, serviceType);
        }
    }

    private static void InjectService(IServiceCollection services, InjectionProject project, Type serviceType)
    {
        var attr = serviceType.GetCustomAttribute<AutoInjectAttribute>();

        if (attr == null)
        {
            return;
        }

        if ((attr.Project & project) == 0)
        {
            return;
        }

        if (attr.InterfaceType != null)
        {
            GetInterfaceInjectionMethod(services, attr)(attr.InterfaceType, serviceType);
        }
        else
        {
            GetInjectionMethod(services, attr)(serviceType);
        }
    }

    private static Func<Type, IServiceCollection> GetInjectionMethod(IServiceCollection services, AutoInjectAttribute attr)
    {
        return attr.AutoInjectionType switch
        {
            AutoInjectionType.Singleton => services.AddSingleton,
            AutoInjectionType.Scoped => services.AddScoped,
            AutoInjectionType.Transient => services.AddTransient,
            _ => throw new NotImplementedException(),
        };
    }

    private static Func<Type, Type, IServiceCollection> GetInterfaceInjectionMethod(IServiceCollection services, AutoInjectAttribute attr)
    {
        return attr.AutoInjectionType switch
        {
            AutoInjectionType.Singleton => services.AddSingleton,
            AutoInjectionType.Scoped => services.AddScoped,
            AutoInjectionType.Transient => services.AddTransient,
            _ => throw new NotImplementedException(),
        };
    }

}