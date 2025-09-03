using Microsoft.EntityFrameworkCore;

namespace CarInventory
{
    public class CarDB: DbContext
    {
        public CarDB(DbContextOptions<CarDB> options) 
            : base(options) { }

        public DbSet<Car> Cars { get; set; } // create cars table in the data base
    }
}
