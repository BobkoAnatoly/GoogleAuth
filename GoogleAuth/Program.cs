
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie")
    .AddOAuth("google", o =>
    {
        o.SignInScheme = "cookie";
        o.ClientId = "698063505179-u92amnmfjfkk2q3k29psoi5vmogsh1md.apps.googleusercontent.com";
        o.ClientSecret = "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";

        o.AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        o.TokenEndpoint = "https://oauth2.googleapis.com/token";
        o.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
        o.CallbackPath = "/callback";
        o.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
        o.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
        o.SaveTokens= true;
        o.Events.OnCreatingTicket = async ctx =>
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            using var result = await ctx.Backchannel.SendAsync(request);
            var user = await result.Content.ReadFromJsonAsync<JsonElement>();
            
            ctx.RunClaimActions(user);
        };

        o.ClaimActions.MapJsonKey("id", "id");
        o.ClaimActions.MapJsonKey("email", "email");
        o.ClaimActions.MapJsonKey("verified_email", "verified_email");
        o.ClaimActions.MapJsonKey("name", "name");
        o.ClaimActions.MapJsonKey("given_name", "given_name");
        o.ClaimActions.MapJsonKey("given_name", "given_name");
        o.ClaimActions.MapJsonKey("family_name", "family_name");
        o.ClaimActions.MapJsonKey("picture", "picture");
        o.ClaimActions.MapJsonKey("locale", "locale");
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseAuthentication();
app.MapControllers();
app.MapGet("/", (HttpContext ctx) =>
{
    ctx.GetTokenAsync("acceess_token");
    return ctx.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
});
app.Run();
