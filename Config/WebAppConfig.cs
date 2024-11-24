using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Soufieb.Webapp.Config
{
    public static class WebAppConfig
    {
        public static void AddWebAppConfig(this IServiceCollection services, IConfiguration configuration, IWebHostBuilder builder, IWebHostEnvironment env)
        {

            services.AddMvc().AddRazorOptions(options =>
            {
                options.PageViewLocationFormats
                 .Add("/Views/shared/{0}.cshtml");
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.LoginPath = "/Login"; // Página de login
                options.AccessDeniedPath = "/Login/AcessoNegado"; // Página de acesso negado
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tempo de expiração do cookie de autenticação
            });
        }

        public static void UseWebAppConfig(this IApplicationBuilder app)
        {

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Home}/{id?}");

            });
        }
    }
}
