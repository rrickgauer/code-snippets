[ApiController]
[Route("api/auth")]
public class ApiAuthController : ControllerBase
{
    /// <summary>
    /// POST: /auth/login
    /// </summary>
    /// <returns></returns>
    [HttpPost("login/{userId}")]
    public async Task<IActionResult> PostLoginAsync([FromRoute] int userId, [FromBody] LoginRequestForm loginCredentials)
    {
        return Ok("Logged In");
    }

}