using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResortAPI.Models;

namespace ResortAPI.Data
{
  

        public class ApplicationDbContext : IdentityDbContext<IdentityUser>
        {
            public ApplicationDbContext(DbContextOptions dbContextOptions) :
                base(dbContextOptions)
            {

            }

        public DbSet<User> Users { get; set; }
        public DbSet<Resort> Resorts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {

                base.OnModelCreating(modelBuilder);
                SeedRoles(modelBuilder);

            }
            private static void SeedRoles(ModelBuilder builder)
            {
                builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole()
                {
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "Admin"
                },
                new IdentityRole()
                {
                    Name = "Customer",
                    ConcurrencyStamp = "2",
                    NormalizedName = "User"
                }
               
                );
            }


        }
    
}
