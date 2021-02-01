using Microsoft.EntityFrameworkCore;

namespace MyAddresses.Repositories.Contexts
{
    public class MyAddressesDbContext: DbContext
    {
        public MyAddressesDbContext(DbContextOptions<MyAddressesDbContext> options)
            :base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}