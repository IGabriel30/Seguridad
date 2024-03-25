using Microsoft.EntityFrameworkCore;

namespace Seguridad.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Email = "root@gmail.com", IdUsuario = 1, Nombre = "root", Apellido = "admin", Status = 1, Rol = "Administrador", Password = "0192023a7bbd73250516f069df18b500" }
                );
        }
    }


}
