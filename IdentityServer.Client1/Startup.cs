using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Client1
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
            services.AddHttpContextAccessor();
            services.AddScoped<IApiResourceHttpClient, ApiResourceHttpClient>();

            //Buradaki kod blogu IdentityServer ile haberle?mi? oluyoruz
            services.AddAuthentication(options =>
            {
                //Client1 de olu?acak cookimin ismi Cookies olacak
                options.DefaultScheme = "Cookies";
                //IdentityServer'dan gelen cookiler ile haberle?ecek
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies", opts =>
            {
                opts.AccessDeniedPath = "/Home/AccessDenied";
            }).AddOpenIdConnect("oidc", opts =>//Burada Client daki cookiler ile IdentityServerdaki clientla ileti?e ge?iriyoruz
            {
                opts.SignInScheme = "Cookies";
                opts.Authority = "https://localhost:5001";
                opts.ClientId = "Client1-Mvc";
                opts.ClientSecret = "secret";
                //ald???m?z id_token do?rulamak i?in token g?nderir bu i?e yarar
                opts.ResponseType = "code id_token";
                opts.GetClaimsFromUserInfoEndpoint = true;
                //Access token?m?z? kaydet
                opts.SaveTokens = true;
                //authserver belrttikten sonra hangi api'ye ba?lanacaksak onu bellirtiyoruz
                opts.Scope.Add("api1.read");
                opts.Scope.Add("offline_access");
                opts.Scope.Add("CountryAndCity");
                opts.Scope.Add("Roles");

                opts.ClaimActions.MapUniqueJsonKey("country", "country");
                opts.ClaimActions.MapUniqueJsonKey("city", "city");
                opts.ClaimActions.MapUniqueJsonKey("role", "role");

                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    RoleClaimType = "role"
                };

            });

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
            app.UseAuthentication();
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
