
[Controller]
[Route("/app")]
[ServiceFilter(typeof(LoginRedirectFilter))]
public class GuiController : Controller
{
    /// <summary>
    /// GET: /app
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [RedirectAction]
    public async Task<IActionResult> AppPage()
    {
        ExamplePageModel pageModel = new()
        {
            UserId = 12,
        };

        ExampleLayoutModel<ExamplePageModel> viewModel = new("Html Title")
        {
            PageModel = pageModel,
        };
        
        return View("/Views/Pages/Layouts/SettingsPage.cshtml", viewModel);
    }

}