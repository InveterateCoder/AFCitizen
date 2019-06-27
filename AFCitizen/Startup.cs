using AFCitizen.Infrastructure;
using AFCitizen.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AFCitizen
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }
        public Startup(IConfiguration config) => Configuration = config;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserIdentityDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("Users")));
            services.AddDbContext<UserLevelDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("UserLevel")));
            services.AddDbContext<FirstLevelDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("FirstLevel")));
            services.AddDbContext<MidLevelDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("MidLevel")));
            services.AddDbContext<TopLevelDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("TopLevel")));
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("ПервДисп", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Диспетчер");
                    policy.RequireRole("ПервУровень");
                });
                opts.AddPolicy("СредДисп", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Диспетчер");
                    policy.RequireRole("СредУровень");
                });
                opts.AddPolicy("ТопДисп", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Диспетчер");
                    policy.RequireRole("ТопУровень");
                });
                opts.AddPolicy("ПервАг", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("ПервУровень");
                    policy.RequireAssertion(handler => !handler.User.IsInRole("Диспетчер"));
                });
                opts.AddPolicy("СредАг", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("СредУровень");
                    policy.RequireAssertion(handler => !handler.User.IsInRole("Диспетчер"));
                });
                opts.AddPolicy("ТопАг", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("ТопУровень");
                    policy.RequireAssertion(handler => !handler.User.IsInRole("Диспетчер"));
                });
            });
            services.AddIdentity<CitizenUser, IdentityRole>(opts =>
            {
                opts.User.AllowedUserNameCharacters = string.Empty;
                opts.User.RequireUniqueEmail = true;
            }).AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddEntityFrameworkStores<UserIdentityDbContext>();
            services.AddMvc(opts => opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
