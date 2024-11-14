using ClientesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet<ClientesModel> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<ClientesModel>()
                .HasKey(c => c.CodCliente);
         

            base.OnModelCreating(modelBuilder);
        }
        
    }

}
