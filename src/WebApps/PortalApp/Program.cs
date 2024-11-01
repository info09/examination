using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
        options.Authority = "http://localhost:8181/realms/exam";
        options.ClientId = "web-app";
        options.ClientSecret = "oyVn2O01PppwKsuirfXforq4ZWOMFN91";

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        // Xử lý sự kiện đăng nhập
        options.Events = new OpenIdConnectEvents
        {
            OnTokenValidated = context =>
            {
                // Bạn có thể xử lý thông tin người dùng tại đây sau khi đăng nhập thành công
                return Task.CompletedTask;
            }
        };
        options.RequireHttpsMetadata = false;
        options.SaveTokens = true;
    });

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
