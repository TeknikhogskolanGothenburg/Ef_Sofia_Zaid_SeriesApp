using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using SeriesApp.Domain;

namespace SeriesApp.Data
{
    public class SeriesContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ProductionCompany> ProductionCompanies { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<SeriesActor> SeriesActors { get; set; }
        public DbSet<SeriesGenre> SeriesGenres { get; set; }

        public static readonly LoggerFactory MovieLoggerFactory =
           new LoggerFactory(new[]{
            new ConsoleLoggerProvider((category, level)
                => category == DbLoggerCategory.Database.Command.Name &&
                level == LogLevel.Information, true)
           });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(MovieLoggerFactory)
                .UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = TvSeriesDb; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeriesActor>().HasKey(s => new { s.SeriesId, s.ActorId });
            modelBuilder.Entity<SeriesGenre>().HasKey(s => new { s.SeriesId, s.GenreId });

            modelBuilder.Entity<Country>()
                .HasIndex(n => n.Name)
                .IsUnique();

            modelBuilder.Entity<ProductionCompany>()
                .HasIndex(p => new { p.Name, p.CountryId })
                .IsUnique();
        }
    }
}

