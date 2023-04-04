using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.DAL;
using CountryInfoApi.Models;
using CountryInfoApi.Models.Base;
using CountryInfoApi.Repository;
using CountryInfoApi.Service;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
