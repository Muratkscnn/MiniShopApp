using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniShopApp.Business.Abstract;
using MiniShopApp.Business.Concrete;
using MiniShopApp.Data.Abstract;
using MiniShopApp.Data.Concrete.EfCore;
using MiniShopApp.WebUI.EmailServices;
using MiniShopApp.WebUI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlite(Configuration.GetConnectionString("SqLiteConnection")));
            services.AddDbContext<MiniShopContext>(options => options.UseSqlite(Configuration.GetConnectionString("SqLiteConnection")));

            //services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection")));
            //services.AddDbContext<MiniShopContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection")));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //Password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                //Lockout
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;

                //User
                options.User.RequireUniqueEmail = true;

                //SignIn
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder()
                {
                    HttpOnly = true,
                    Name="MiniShopApp.Security.Cookie",
                    SameSite=SameSiteMode.Strict
                };
            });

            services.AddScoped<IEmailSender, SmtpEmailSender>(i => new SmtpEmailSender(
                Configuration["EmailSender:Host"],
                Configuration.GetValue<int>("EmailSender:Port"),
                Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                Configuration["EmailSender:UserName"],
                Configuration["EmailSender:Password"]
                ));

            //services.AddScoped<IProductRepository, EfCoreProductRepository>();
            //services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            //services.AddScoped<ICardRepository, EfCoreCardRepository>();
            //services.AddScoped<IOrderRepository, EfCoreOrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICardService, CardManager>();
            services.AddScoped<IOrderService, OrderManager>();

            //Projemizin MVC yapýsýnda olmasýný saðlar.
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICardService cardService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
             
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "orders",
                    pattern: "orders",
                    defaults: new { controller = "Card", action = "GetOrders" }
                    );

                endpoints.MapControllerRoute(
                    name: "checkout",
                    pattern: "checkout",
                    defaults: new { controller = "Card", action = "CheckOut" }
                    );

                endpoints.MapControllerRoute(
                    name: "card",
                    pattern: "card",
                    defaults: new { controller = "Card", action = "Index" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminuserlist",
                    pattern: "admin/user/list",
                    defaults: new { controller = "Admin", action = "UserList" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminusercreate",
                    pattern: "admin/user/create",
                    defaults: new { controller = "Admin", action = "UserCreate" }
                    );


                endpoints.MapControllerRoute(
                    name: "adminuseredit",
                    pattern: "admin/user/{id}",
                    defaults: new { controller = "Admin", action = "UserEdit" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminrolelist",
                    pattern: "admin/role/list",
                    defaults: new { controller = "Admin", action = "RoleList" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminrolecreate",
                    pattern: "admin/role/create",
                    defaults: new { controller = "Admin", action = "RoleCreate" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminroleedit",
                    pattern: "admin/role/{id}",
                    defaults: new { controller = "Admin", action = "RoleEdit" }
                    );



                endpoints.MapControllerRoute(
                    name: "adminproductcreate",
                    pattern: "admin/products/create",
                    defaults: new { controller = "Admin", action = "ProductCreate" }
                    );
                endpoints.MapControllerRoute(
                    name: "adminproducts",
                    pattern: "admin/products",
                    defaults: new { controller = "Admin", action = "ProductList" }
                    );
                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new { controller = "MiniShop", action = "Search" }
                    );
                endpoints.MapControllerRoute(
                   name: "products",
                   pattern: "products/{category?}",
                   defaults: new { controller = "MiniShop", action = "List" }
                   );
                endpoints.MapControllerRoute(
                    name: "adminproductedit",
                    pattern: "admin/products/{id?}",
                    defaults: new { controller = "Admin", action = "ProductEdit" }
                    );
                endpoints.MapControllerRoute(
                    name: "productdetails",
                    pattern: "{url}",
                    defaults: new { controller = "MiniShop", action = "Details" }
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedIdentity.Seed(userManager, roleManager, cardService, Configuration).Wait();
        }
    }
}
