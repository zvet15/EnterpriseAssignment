using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
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

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("EnterpriseDB", throwIfV1Schema: false)
        {
            Database.SetInitializer(new webApp());
        }

        public class webApp:DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {
            public override void InitializeDatabase(ApplicationDbContext context)
            {
                base.InitializeDatabase(context);
            }

            protected override void Seed(ApplicationDbContext context)
            {
                base.Seed(context);
                List<Quality> quality = new List<Quality>();
                quality.Add(new Quality() {type="Good"});
                quality.Add(new Quality() {type="Excellent"});
                quality.Add(new Quality() {type="Poor"});
                quality.Add(new Quality() {type="Bad"});
                context.Qualities.AddRange(quality);
            }
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public System.Data.Entity.DbSet<WebApplication1.Models.Categories> Categories { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Quality> Qualities { get; set; } //not in the db

        public System.Data.Entity.DbSet<WebApplication1.Models.ItemTypes> ItemTypes { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Items> Items { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Error> Errors { get; set; }
    }
}