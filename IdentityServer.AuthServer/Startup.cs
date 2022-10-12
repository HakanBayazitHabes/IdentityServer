using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repository;
using IdentityServer.AuthServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer
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

            services.AddScoped<ICustomUserRepository, CustomUserRepository>();

            services.AddDbContext<CustomDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            //IdentityServer'�m�z�n tokenler� vs leri veri taban�nda tutuyoruz. Bunun i�in context ba�lant�s� kullanca��z
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;



            services.AddIdentityServer()
                .AddConfigurationStore(opts => 
                {
                    opts.ConfigureDbContext = c => c.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlopts =>
                       {
                           sqlopts.MigrationsAssembly(assemblyName);
                       });
                })
                .AddOperationalStore(opts => 
                {
                    opts.ConfigureDbContext = c => c.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlopts =>
                       sqlopts.MigrationsAssembly(assemblyName));
                })
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddTestUsers(Config.GetUsers().ToList())
                //otomatik olarak public-key  private-key olu�turur,canl�ya alaca��m�z zaman public-key ve private-key i uzak sunucudan(azure gibi) al�nmas� gerekir. 
                .AddDeveloperSigningCredential()
                .AddProfileService<CustomProfileService>()
                .AddResourceOwnerValidator<ResourcesOwnerPasswordValidator>();//yazm�� oldu�umuz s�n�f�m�z�n i�erisine girecek b�yle bir kullan�c� vard�r veya yoktur diyecektir buna yapan identityserver olacakt�r


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
