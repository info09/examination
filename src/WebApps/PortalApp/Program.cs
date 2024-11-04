using AdminApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using PortalApp;
using PortalApp.Core;
using PortalApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;       // Sử dụng scheme mặc định là "Cookies"
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;  // Sử dụng scheme đăng nhập mặc định là "Cookies"
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;      // Scheme cho OpenID Connect
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Scheme đăng xuất mặc định là "Cookies"
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(options =>
    {
        options.Authority = builder.Configuration["KeyCloak:KeyCloakUrl"];
        options.ClientId = builder.Configuration["KeyCloak:ClientId"];
        options.ClientSecret = builder.Configuration["KeyCloak:ClientSecret"];

        //"email openid roles profile offline_access
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.RequireHttpsMetadata = false;
        options.SaveTokens = true;
        options.ResponseType = "code";

        // Xử lý sự kiện đăng nhập
        options.Events = new OpenIdConnectEvents
        {
            OnTokenValidated = async context =>
            {
                var expiresIn = context.TokenEndpointResponse.ExpiresIn;
                var accessToken = context.TokenEndpointResponse.AccessToken;
                var refreshToken = context.TokenEndpointResponse.RefreshToken;
                var expiresAt = DateTimeOffset.UtcNow.AddSeconds(int.Parse(context.TokenEndpointResponse.ExpiresIn));
                // Bạn có thể xử lý thông tin người dùng tại đây sau khi đăng nhập thành công

                context.Properties.StoreTokens(new[]
                {
                    new AuthenticationToken { Name = "access_token", Value = context.TokenEndpointResponse.AccessToken },
                    new AuthenticationToken { Name = "refresh_token", Value = context.TokenEndpointResponse.RefreshToken },
                    new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o") }
                });
            }
        };

    });

builder.Services.AddScoped<TokenService>();

// Đăng ký TokenAuthenticationHandler
builder.Services.AddTransient<TokenAuthenticationHandler>();

builder.Services.AddHttpClient("BackendApi", options =>
{
    options.BaseAddress = new Uri(builder.Configuration["BackendApiUrl"]);
}).AddHttpMessageHandler<TokenAuthenticationHandler>();
builder.Services.RegisterCustomServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
