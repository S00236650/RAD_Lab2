namespace RAD_Lab2_Part5
{
    using Microsoft.EntityFrameworkCore;

    public class AdDb : DbContext
    {
        public AdDb(DbContextOptions<AdDb> options) : base(options) { }

        public DbSet<Ad> Ads => Set<Ad>();
        public DbSet<Seller> Selers => Set<Seller>();
        public DbSet<Category> Categories => Set<Category>();
    }
}
