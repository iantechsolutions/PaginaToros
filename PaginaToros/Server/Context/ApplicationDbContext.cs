using PaginaToros.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PaginaToros.Server.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        private string[] roles =
        {
            "USUARIOMAESTRO",
            "ADMINISTRADOR"
        };

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (string rol in roles)
            {
                builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = rol, Name = rol, NormalizedName = rol });
            }

            var user = CreateUserAdmin();
            builder.Entity<IdentityUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = user.Id, RoleId = "USUARIOMAESTRO" });

        }

        private IdentityUser CreateUserAdmin()
        {
            IdentityUser user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                NormalizedUserName = "admin".ToUpper(),
                Email = "info@ianconsulting.net",
                NormalizedEmail = "info@ianconsulting.net".ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, user.UserName);

            return user;
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
