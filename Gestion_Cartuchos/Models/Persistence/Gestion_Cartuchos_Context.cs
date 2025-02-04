using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Models
{
    public class Gestion_Cartuchos_Context : IdentityDbContext<ApplicationUser>
    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Env.Load(); // Cargar el archivo .env

            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? throw new InvalidOperationException("La variable de entorno 'CONNECTION_STRING' no está configurada.");

            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34)));
        }

        public DbSet<Cartucho> Cartuchos { get; set; }
        public DbSet<Asignar_Impresora> Asignar_Impresoras { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Impresora> Impresoras { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Recargas> Recargas { get; set; }
        public DbSet<ImpresoraModelo> ImpresoraModelos { get; internal set; }
    }
}
