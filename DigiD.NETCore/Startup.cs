using ComponentSpace.Saml2.Authentication;
using ComponentSpace.Saml2.Claims;
using ComponentSpace.Saml2.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DigiD.NETCore
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Use a unique identity cookie name rather than sharing the cookie across applications in the domain.
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.Name = "AppCookie.Identity";
            //});

            // Add the custom SAML claim factory.
            services.TryAddSingleton<ISamlClaimFactory, DigiDClaimFactory>();

            // Add SAML SSO services.
            services.AddSaml(ConfigureDigiD);


            // Add SAML authentication services.
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Cookie for everything...
                o.DefaultChallengeScheme = SamlAuthenticationDefaults.AuthenticationScheme; /// ... except challenge, which should be handled by SAML
                // Signout should be handled by SAML, but this does not work, so this is forced in AuthController
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddSaml(SamlAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.PartnerName = () => Configuration["PartnerName"];
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Delegate signin to cookie-based authentication handler
                });
        }

        private void ConfigureDigiD(SamlConfigurations samlConfig)
        {
            Configuration.Bind("SAML", samlConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }

    public static class HelperFunctions
    {
        public static void SetSiteUrl(this string url, string siteUrl)
        {
            url = url.Replace("%SITE_URL%", siteUrl);
        }
    }
}
