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
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("СубДисп", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Диспетчер");
                    policy.RequireRole("Субъект");
                });
                opts.AddPolicy("ГорДисп", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Диспетчер");
                    policy.RequireRole("Город");
                });
                opts.AddPolicy("ФедДисп", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Диспетчер");
                    policy.RequireRole("Федерация");
                });
            });
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
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
