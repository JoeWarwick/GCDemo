using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CDWRepository
{
    public class CDWSVCModelFactory : IDesignTimeDbContextFactory<CDWSVCModel<CDWSVCUser>>
    {
        public CDWSVCModel<CDWSVCUser> CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data source=cdwapi.db");

            return new CDWSVCModel<CDWSVCUser>(optionsBuilder.Options);
        }
    }
}
