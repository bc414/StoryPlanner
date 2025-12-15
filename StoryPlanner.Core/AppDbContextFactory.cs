using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StoryPlanner.Core
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            // Ensure this connection string matches the one in App.xaml.cs
            optionsBuilder.UseSqlite("Data Source=StoryPlanner.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}