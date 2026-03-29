using PaginaToros.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaginaToros.Server.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        private readonly (string Id, string Name, string NormalizedName)[] roles =
        {
            ("USUARIOMAESTRO", "USUARIOMAESTRO", "USUARIOMAESTRO"),
            ("ADMINISTRADOR", "ADMINISTRADOR", "ADMINISTRADOR"),
            ("Socio", "Socio", "SOCIO")
        };

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var rol in roles)
            {
                builder.Entity<IdentityRole>().HasData(new IdentityRole
                {
                    Id = rol.Id,
                    Name = rol.Name,
                    NormalizedName = rol.NormalizedName
                });
            }

            var user = CreateUserAdmin();
            builder.Entity<IdentityUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = user.Id, RoleId = "USUARIOMAESTRO" });

            ConfigureDomainUser(builder.Entity<User>());
            ConfigureUserSocio(builder.Entity<UserSocio>());

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

        public DbSet<User> User { get; set; }
        public DbSet<UserSocio> UserSocios { get; set; }

        private static void ConfigureDomainUser(EntityTypeBuilder<User> entity)
        {
            entity.Ignore(e => e.Socio);
            entity.Ignore(e => e.SocioIds);
            entity.Property(e => e.IdentityUserId).HasMaxLength(450);
        }

        private static void ConfigureUserSocio(EntityTypeBuilder<UserSocio> entity)
        {
            entity.ToTable("user_socios");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.SocioId }).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.SocioId).HasColumnName("socio_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
