using CountryInfoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CountryInfoApi.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<CityImg> CityImgs { get; set; }
        public DbSet<RecomendedPlace> RecomendedPlaces { get; set; }
        public DbSet<PlaceImg> PlaceImgs { get; set; }
    }
}
