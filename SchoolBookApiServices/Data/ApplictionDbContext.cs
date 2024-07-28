
namespace SchoolBookShop.Data
{
    public class ApplictionDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplictionDbContext(DbContextOptions<ApplictionDbContext> options) : base(options) { }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{

        //    builder.Entity<ApplicationUser>()
        //        .HasMany<Book>()
        //        .WithOne()
        //        .HasForeignKey(b => b.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //    builder.Entity<Book>()
        //        .HasMany<BookPhoto>()
        //        .WithOne()
        //        .HasForeignKey(b => b.BookId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //   // Explicitly configure primary keys for Identity entities


        //   //builder.Entity<IdentityUser<string>>()
        //   //    .HasKey(user => user.Id);
        //   //builder.Entity<IdentityRole<string>>()
        //   //    .HasKey(role => role.Id);
        //   // builder.Entity<IdentityUserLogin<string>>()
        //   //     .HasKey(login => new { login.LoginProvider, login.ProviderKey });

        //   // builder.Entity<IdentityUserRole<string>>()
        //   //     .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

        //   // builder.Entity<IdentityUserToken<string>>()
        //   //     .HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name });

        //   // builder.Entity<IdentityUserClaim<string>>()
        //   //     .HasKey(userClaim => userClaim.Id);

        //   // builder.Entity<IdentityRoleClaim<string>>()
        //   //     .HasKey(roleClaim => roleClaim.Id);

        //}
        public DbSet<Book> Books { get; set; }
        public DbSet<BookPhoto> BookPhotos { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart>Carts { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }

}
