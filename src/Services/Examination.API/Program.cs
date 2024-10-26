using Examination.API;
using Examination.API.Extensions;
using Examination.Application.Commands.ExamResults.StartExam;
using Examination.Application.Mapping;
using Examination.Infrastructure.MongoDb.SeedWorks;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var user = builder.Configuration.GetValue<string>("DatabaseSettings:User");
var password = builder.Configuration.GetValue<string>("DatabaseSettings:Password");
var server = builder.Configuration.GetValue<string>("DatabaseSettings:Server");
var databaseName = builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName");
var mongodbConnectionString = "mongodb://" + user + ":" + password + "@" + server + "/" + databaseName + "?authSource=admin";

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IMongoClient>(c =>
{
    return new MongoClient(mongodbConnectionString);
});

builder.Services.AddScoped(c => c.GetService<IMongoClient>()?.StartSession());
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile(new MappingProfile()); });
builder.Services.AddMediatR(typeof(StartExamCommandHandler).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ExamSettings>(builder.Configuration);
builder.Services.RegisterCustomServices();

//
builder.Services.AddHttpContextAccessor();


builder.Services.AddSingleton<IMongoClient>(c =>
{
    return new MongoClient(mongodbConnectionString);
});

builder.Services.AddScoped(c => c.GetService<IMongoClient>()?.StartSession());
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile(new MappingProfile()); });
builder.Services.AddMediatR(typeof(StartExamCommandHandler).Assembly);
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Examination.API V1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Examination.API V2", Version = "v2" });

});

var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults
        .AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults
        .AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = identityUrl;
    options.RequireHttpsMetadata = false;
    options.Audience = "exam_api";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false
    };
    //Fix SSL
    options.BackchannelHttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = delegate { return true; }
    };
});



builder.Services.Configure<ExamSettings>(builder.Configuration);

//Health check
builder.Services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy())
        .AddMongoDb(mongodbConnectionString: mongodbConnectionString,
                    name: "mongo",
                    failureStatus: HealthStatus.Unhealthy);

builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
    opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
    opt.SetApiMaxActiveRequests(1); //api requests concurrency

    opt.AddHealthCheckEndpoint("Exam API", "/hc"); //map health check api
})
        .AddInMemoryStorage();

builder.Services.RegisterCustomServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseErrorWrapping();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase();
app.Run();
