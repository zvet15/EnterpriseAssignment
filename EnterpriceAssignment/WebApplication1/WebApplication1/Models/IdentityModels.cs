using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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
                quality.Add(new Quality() { type = "Good" });
                quality.Add(new Quality() { type = "Excellent" });
                quality.Add(new Quality() { type = "Poor" });
                quality.Add(new Quality() { type = "Bad" });
                context.Qualities.AddRange(quality);
                context.SaveChanges();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                if (!roleManager.RoleExists("User"))
                {
                    IdentityRole role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = "User";
                    roleManager.Create(role);
                }
                for (int i = 1; i <= 5; i++)
                {
                    ApplicationUser seedingUser = new ApplicationUser()
                    {
                        Email = "zveti_" + i + "@hotmail.com",
                        UserName = "zveti_" + i + "@hotmail.com"
                    };
                    context.Users.Add(seedingUser);
                    userManager.Create(seedingUser, "Zvet123!");
                    userManager.AddToRole(seedingUser.Id, "User");
                }
                //------------------Categories-------------------------
                List<Categories> category = new List<Categories>();
                category.Add(new Categories() { CategoryName = "Food" });
                category.Add(new Categories() { CategoryName = "Clothes" });
                category.Add(new Categories() { CategoryName = "Hair" });
                category.Add(new Categories() { CategoryName = "Shoes" });
                category.Add(new Categories() { CategoryName = "Makeup" });
                context.Categories.AddRange(category);
                context.SaveChanges();
                //--------------ItemTypes-----------------------
                List<ItemTypes> itemTypes = new List<ItemTypes>();

                foreach (Categories categoryO in context.Categories.Local)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        itemTypes.Add(new ItemTypes() { Name = "TestCategory" + i, Categories = categoryO, Image = "https://drive.google.com/uc?id=1X8KEDqZ6ehyYkicXxcL-sFzytudNjMIy" });
                    }
                }

                context.ItemTypes.AddRange(itemTypes);
                context.SaveChanges();
                //----------------Items-----------------------------
                List<Items> item = new List<Items>();

                 Random rnd = new Random(); 
                foreach (ApplicationUser appUser in context.Users.Local)
                {
                    foreach (ItemTypes it in context.ItemTypes.Local)
                    {
                       
                        Items t = new Items();
                        DateTime dt = DateTime.Now;
                        t.ItemTypeId = it.ItemTypeId;
                        t.SellerId = appUser.Id;
                        t.Date = dt;
                        t.Quantity = rnd.Next(1, 9000);
                        t.QualityId = rnd.Next(1, 4);
                        t.Price = rnd.Next(50, 9000);
                        bool isInList = false;
                        foreach (Items itm in item)
                        {
                            if (itm.SellerId == t.SellerId && itm.QualityId == t.QualityId && itm.Quantity == t.Quantity
                                && itm.Price == t.Price)
                            {
                                isInList = true;
                            }
                        }
                        if (!isInList)
                        {                                
                             item.Add(t);                      
                        }                       
                     
                    }
                }

                context.Items.AddRange(item);
             context.SaveChanges();

            }
        }

        public Items generateItems(ApplicationDbContext context)
        {
            List<Items> item = new List<Items>();

            Random rnd = new Random();
            Random tnd = new Random();
            Random qu = new Random();
            Items t = new Items();
            foreach (ApplicationUser appUser in context.Users.Local)
            {
                foreach (ItemTypes it in context.ItemTypes.Local)
                {
                  
                    t.ItemTypeId = it.ItemTypeId;
                    t.SellerId = appUser.Id;
                    t.Quantity = rnd.Next(1, 9000);
                    t.QualityId = qu.Next(1, 4);
                    t.Price = tnd.Next(50, 9000);
               
                }
            }
                 return t;
                   
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public System.Data.Entity.DbSet<WebApplication1.Models.Categories> Categories { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Quality> Qualities { get; set; } 

        public System.Data.Entity.DbSet<WebApplication1.Models.ItemTypes> ItemTypes { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Items> Items { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Error> Errors { get; set; }
    }
}