using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Models
{
    public class Gestion_Cartuchos_Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DotNetEnv.Env.Load();

            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34)));
        }

        public DbSet<Cartucho> Cartuchos { get; set; }
        public DbSet<Asignar_Impresora> Asignar_Impresoras { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Impresora> Impresoras { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Recargas> Recargas { get; set; }
    }
}
