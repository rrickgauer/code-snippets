[AutoInject(Domain.Enum.AutoInjectionType.Scoped, Domain.Enum.InjectionProject.Always)]
public class ExampleAuthFilter : IAsyncActionFilter
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var projectId = context.GetProjectIdRouteValue();

        if (projectId != 5)
        {
            context.Result = new BadRequestObjectResult(new {
                ErrorMessage = "Invalid project id",
            });
            
            return;
        }

        await next();
    }
}