using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using PortalApp;
using PortalApp.Core;

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
            OnTokenValidated = context =>
            {
                var accessToken = context.TokenEndpointResponse.AccessToken;
                var refreshToken = context.TokenEndpointResponse.RefreshToken;
                // Bạn có thể xử lý thông tin người dùng tại đây sau khi đăng nhập thành công
                return Task.CompletedTask;
            }
        };
        
    });
builder.Services.AddHttpClient("BackendApi", options =>
{
    options.BaseAddress = new Uri(builder.Configuration["BackendApiUrl"]);
});
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
