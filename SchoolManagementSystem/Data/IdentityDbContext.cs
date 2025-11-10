using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Data
{
    public class IdentityDbContext
    {
        private DbContextOptions<ApplicationDbContext> options;

        public IdentityDbContext(DbContextOptions<ApplicationDbContext> options)
        {
            this.options = options;
        }

        internal void OnModelCreating(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
    }
}