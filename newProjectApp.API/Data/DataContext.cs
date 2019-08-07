using Microsoft.EntityFrameworkCore;
using newProjectApp.API.Modles;

namespace newProjectApp.API.Data
{
    public class DataContext:DbContext
    {
         public DataContext(DbContextOptions<DataContext> options):base(options) {}
          public DbSet<Value> Values { get; set; }  
    }
}