using FigureCollection;
using FigureCollection.Migrations;
using Microsoft.EntityFrameworkCore;


namespace FigureCollection.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Figure> Figures { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<FigureImage> FigureImages { get; set; }
    }


}
