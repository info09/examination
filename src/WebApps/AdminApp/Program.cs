using AdminApp;
using AdminApp.Core.Authentication;
using AdminApp.Services;
using AdminApp.Services.Interfaces;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

// Đăng ký Blazored Session Storage
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore();

// Đăng ký các service của bạn
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<TokenService>();

// Đăng ký TokenAuthenticationHandler
builder.Services.AddTransient<TokenAuthenticationHandler>();

builder.Services.AddHttpClient("MyHttpClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BackendApiUrl"]); // Cấu hình base URL cho API của bạn
})
    .AddHttpMessageHandler<TokenAuthenticationHandler>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

await builder.Build().RunAsync();
