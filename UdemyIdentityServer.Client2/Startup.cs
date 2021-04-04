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
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace UdemyIdentityServer.Client2
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
            services.AddControllersWithViews();

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = "Cookies"; // ismi opsiyonel
                    opt.DefaultChallengeScheme = "oidc";

                }).AddCookie("Cookies", opts =>
                {
                    opts.AccessDeniedPath = "/Home/AccessDenied";
                })
                .AddOpenIdConnect("oidc", opts =>
                {
                    opts.SignInScheme = "Cookies";
                    opts.Authority = "https://localhost:5001"; // AutServer adresi
                    opts.ClientId = "Client2-Mvc";
                    opts.ClientSecret = "secret";
                    opts.ResponseType = "code id_token"; // code auth serverdan al�n�r , id token ile do�rulu�u check edilir.
                    opts.GetClaimsFromUserInfoEndpoint = true; // Kullanc�� ile ilgili claimleri userinfo endpointeden al�r
                    opts.SaveTokens = true; //Ba�ar�l� bir auth i�leminden sonra acces token cookeiye kay�t edilir.
                    opts.Scope.Add("api1.read"); // Bu scope u istiyor.
                    opts.Scope.Add("offline_access"); // bu scope u istiyor
                    opts.Scope.Add("CountryAndCity"); //custom claim
                    opts.Scope.Add("Roles"); ////custom claim
                    opts.ClaimActions.MapUniqueJsonKey("country", "country");// custom claim i�in mapleme i�lemi
                    opts.ClaimActions.MapUniqueJsonKey("city", "city");// custom claim i�in mapleme i�lemi
                    opts.ClaimActions.MapUniqueJsonKey("role", "role"); // custom claim i�in mapleme i�lemi

                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        RoleClaimType = "role" //Token Bazli yetkilendirme yap�l�yor ve role type yukar�daki role claimi olsun
                    };




                });
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
