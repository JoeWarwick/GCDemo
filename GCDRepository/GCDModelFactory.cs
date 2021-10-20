using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GCDRepository
{
    public class GCDModelFactory : IDesignTimeDbContextFactory<GCDModel>
    {
        public GCDModel CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data source=gcdapi.db");

            return new GCDModel(optionsBuilder.Options);
        }
    }
}
