using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GoogleAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }
        [HttpGet("/login")]
        public IActionResult Login()
        {
            return Challenge(
            new AuthenticationProperties
            {
                
                RedirectUri = "https://localhost:7091/"
            },
            authenticationSchemes: new string[] { "google" });
        }
        //[HttpGet("/")]
        //public IActionResult GoogleScopedAuthorize(HttpContext ctx)
        //{
        //    ctx.GetTokenAsync("access_token");
        //    var json = JsonSerializer.Serialize(ctx.User.Claims.Select(x => new { x.Type, x.Value }).ToList());
        //    return Ok(json);
        //}
    }
}