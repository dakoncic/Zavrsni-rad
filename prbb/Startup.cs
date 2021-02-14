using DataAccessLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelsLayer.Identity;
using prbb.Areas.Identity.Services;
using AutoMapper;
using ModelsLayer.AutoMapper;

namespace prbb
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
            if(Configuration.GetValue<bool>("UseInMemoryDb"))            
               services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase());
            else
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            //services.AddTransient<IGuestAccess,GuestAccessMock>();
            services.AddTransient<IGuestAccess, GuestAccessDB>();
            //services.AddTransient<IClubAccess,ClubAccessMock>();
            services.AddTransient<IClubAccess, ClubAccessDB>();
            //services.AddTransient<IAdminAccess,AdminAccessMock>();
            services.AddTransient<IAdminAccess, AdminAccessDB>();

            var config = new AutoMapper.MapperConfiguration(c =>
            {
                c.AddProfile(new ApplicationProfile());
            }
            );
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddIdentity<ApplicationUser, ApplicationRole>()

                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            });



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env

            , RoleManager<ApplicationRole> roleManager
            , UserManager<ApplicationUser> userManager

            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            RoleInitializer.Initialize(roleManager).Wait();
            SetAdmin.AdminAsync(userManager).Wait();


        }
    }
}
