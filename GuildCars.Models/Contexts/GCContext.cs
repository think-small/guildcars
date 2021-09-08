using GuildCars.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GuildCars.Models.Contexts
{
    public class GCContext : IdentityDbContext<ApplicationUser>
    {
        public class ApplicationUser : IdentityUser
        {
            public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
            {
                // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                // Add custom user claims here
                return userIdentity;
            }
        }
        public GCContext() : base("GCEF") { }

        public DbSet<ImagePath> ImagePaths { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<SaleRecord> SaleRecords { get; set; }
        public DbSet<PurchaseType> PurchaseTypes { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<BodyStyle> BodyStyles { get; set; }
        public DbSet<TransmissionType> TransmissionTypes { get; set; }
        public DbSet<Detail> Details { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.ImagePaths)
                .WithRequired()
                .HasForeignKey(i => i.VehicleId);
        }
    }
}
