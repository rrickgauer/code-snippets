public static class ActionExecutingContextExtensions
{
    public static int GetProjectIdRouteValue(this ActionExecutingContext context)
    {
        return context.GetRequestRouteValue<int>("projectId");
    }

    public static T GetRequestRouteValue<T>(this ActionExecutingContext context, string key)
    {
        if (!context.ActionArguments.TryGetValue(key, out var routeValue))
        {
            throw new InvalidRouteKeyException(key);
        }

        return (T)routeValue;
    }
}