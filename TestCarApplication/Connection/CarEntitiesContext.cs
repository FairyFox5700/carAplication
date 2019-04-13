using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using CarApplication.Entities;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
namespace CarApplication.Connection
{
    class CarEntitiesContext : DbContext
    {
        public DbSet<TradeMark> TradeMarks { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<SubFamily> SubFamilies { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<TechData> TechData { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //constanta&
            optionsBuilder.UseMySql("server=localhost;database=carDB;user=root;password=comuna");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TradeMark>(entity =>
            {
                entity.Property(e => e.TradeMarkId);
                entity.Property(e => e.TradeMarkUrl);
                entity.Property(e => e.TradeMarkName);


            });

            modelBuilder.Entity<Family>(entity =>
            {
                entity.Property(e => e.FamilyId);
                entity.Property(e => e.FamilyUrl);
                entity.Property(d => d.FamilyName);
                entity.HasOne(e => e.TradeMark).WithMany(p => p.CarFamilies);

            });
            modelBuilder.Entity<SubFamily>(entity =>
            {
                entity.Property(e => e.SubFamilyId);
                entity.Property(e => e.SubFamilyUrl);
                entity.Property(d => d.SubFamilyName);
                entity.HasOne(e => e.Family).WithMany(p => p.CarSubFamilies);

            });
            modelBuilder.Entity<Model>(entity =>
            {
                entity.Property(e => e.ModelId);
                entity.Property(e => e.ModelUrl);
                entity.Property(d => d.ModelName);
                entity.HasOne(e => e.SubFamily).WithMany(p => p.CarModels);

            });
            modelBuilder.Entity<TechData>(entity =>
            {
                entity.Property(e => e.TechDataId);
                entity.Property(e => e.TechInfo);
                entity.HasOne(e => e.Model).WithMany(p => p.TechData);

            });
        }
    }

}
