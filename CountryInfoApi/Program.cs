using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.DAL;
using CountryInfoApi.Models;
using CountryInfoApi.Models.Base;
using CountryInfoApi.Repository;
using CountryInfoApi.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using NLog.Config;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using NLog.Extensions.Logging;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"), new MySqlServerVersion(new Version(8, 0, 33)));
});

builder.Services.Configure<ApiKeys>(builder.Configuration.GetSection("ApiKeys"));

builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IRecomendedPlacesService, RecomendedPlaceService>();
// Add services to the container.
builder.Services.AddScoped<IBaseRepository<City>, BaseRepository<City>>();
builder.Services.AddScoped<IBaseRepository<RecomendedPlace>, BaseRepository<RecomendedPlace>>();
builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();

});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtToken:Token").Value)),
           ValidateIssuer = false,
           ValidateAudience = false
       };
   });

builder.Services.AddCors(options => options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        }));

logger.Error("error");


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.UseStaticFiles();

app.MapControllers();

app.Run();

