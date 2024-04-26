using ApiCubosSegundoPractico.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCubosSegundoPractico.Data
{
    public class CuboContext:DbContext
    {
        public CuboContext(DbContextOptions<CuboContext> options)
            :base(options)
        {

        }

        public DbSet<Compra> Compras { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cubo> Cubos { get; set; }
    }
}
