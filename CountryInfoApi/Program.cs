using Com.CloudRail.SI.Services;
using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.DAL;
using CountryInfoApi.Models;
using CountryInfoApi.Models.Base;
using CountryInfoApi.Repository;
using CountryInfoApi.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<ApiKeys>(builder.Configuration.GetSection("ApiKeys"));

builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IRecomendedPlacesService, RecomendedPlaceService>();
// Add services to the container.
builder.Services.AddScoped<IBaseRepository<City>, BaseRepository<City>>();
builder.Services.AddScoped<IBaseRepository<RecomendedPlace>, BaseRepository<RecomendedPlace>>();
builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
builder.Services.AddControllers();
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

app.UseStaticFiles();

app.MapControllers();

app.Run();


//app.UseCors(x => x
//        .WithOrigins("https://localhost:3000") // путь к нашему SPA клиенту
//        .AllowCredentials()
//        .AllowAnyMethod()
//        .AllowAnyHeader());