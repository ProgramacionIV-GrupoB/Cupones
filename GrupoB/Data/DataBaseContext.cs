using CuponesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CuponesApi.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet <CuponModel> Cupones { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<Cupon_CategoriaModel> Cupones_Categorias { get; set; }
        public DbSet<Tipo_CuponModel> Tipo_Cupon { get; set; }
        public DbSet<Cupon_ClienteModel> Cupones_Clientes { get; set; }
        public DbSet<Cupones_DetallesModel> Cupones_Detalle { get; set; }
        public DbSet<ArticuloModel> Articulos { get; set; }
        public DbSet<PrecioModel> Precios { get; set; }
        public DbSet<Cupones_HistorialModel> Cupones_Historial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<CuponModel>()
                .HasKey(c => c.Id_Cupon);
            modelBuilder.Entity<CategoriaModel>()
                .HasKey(c => c.Id_Categoria);
            modelBuilder.Entity<Cupon_CategoriaModel>()
                .HasKey(c => c.Id_Cupones_Categorias);
            modelBuilder.Entity<Tipo_CuponModel>()
               .HasKey(c => c.Id_Tipo_Cupon);
            modelBuilder.Entity<Cupon_ClienteModel>()
              .HasKey(c => c.NroCupon);
            modelBuilder.Entity<Cupones_DetallesModel>()
              .HasKey(c => new{ c.Id_Cupon,c.Id_Articulo});
            modelBuilder.Entity<ArticuloModel>()
             .HasKey(c => c.Id_Articulo);
            modelBuilder.Entity<PrecioModel>()
             .HasKey(c => c.Id_Precio);
            modelBuilder.Entity<Cupones_HistorialModel>()
             .HasKey(c => new{ c.Id_Cupon, c.NroCupon });



            base.OnModelCreating(modelBuilder);
        }
        
    }

}
