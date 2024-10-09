using ClientIPChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientIPChecker
{
    public class AppDbContext :DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserIpAddress> UserIpAddresses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserIpAddress>()
                .HasIndex(u => u.IpAddress);

            modelBuilder.Entity<User>()
                .HasMany(u => u.IpAddresses)
                .WithOne(ip => ip.User)
                .HasForeignKey(ip => ip.UserId);
        }
    }
}
