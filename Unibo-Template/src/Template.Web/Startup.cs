using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Template.Services;
using Template.Web.Infrastructure;
using Template.Web.SignalR.Hubs;

namespace Template.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Env = env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<TemplateDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "Template");
            });

            // Authentication
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login";
                    options.LogoutPath = "/Login/Logout";
                });

            var builder = services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider =
                        (type, factory) => factory.Create(typeof(SharedResource));
                });

#if DEBUG
            builder.AddRazorRuntimeCompilation();
#endif

            // VIEW LOCATION FIX
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Features/Views/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Features/Views/Shared/{0}.cshtml");
                options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            services.AddSignalR();

            // CUSTOM SERVICES
            Container.RegisterTypes(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRequestLocalization(SupportedCultures.CultureNames);

            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            var node_modules = new CompositePhysicalFileProvider(Directory.GetCurrentDirectory(), "node_modules");
            var areas = new CompositePhysicalFileProvider(Directory.GetCurrentDirectory(), "Areas");
            var compositeFp = new CustomCompositeFileProvider(env.WebRootFileProvider, node_modules, areas);
            env.WebRootFileProvider = compositeFp;

            app.UseStaticFiles();

            // ========================================
            // GESTIONE PAGINE DI ERRORE PERSONALIZZATE
            // ========================================
            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                var statusCode = response.StatusCode;
                response.ContentType = "text/html; charset=utf-8";

                try
                {
                    string htmlFilePath = statusCode switch
                    {
                        404 => Path.Combine(env.WebRootPath, "404.html"),
                        403 => Path.Combine(env.WebRootPath, "403.html"),
                        500 => Path.Combine(env.WebRootPath, "500.html"),
                        _ => null
                    };

                    if (htmlFilePath != null && File.Exists(htmlFilePath))
                    {
                        var htmlContent = await File.ReadAllTextAsync(htmlFilePath);
                        await response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(htmlContent));
                    }
                    else
                    {
                        // Fallback generico per altri status code
                        var html = $@"
                            <!DOCTYPE html>
                            <html lang='it'>
                            <head>
                                <meta charset='UTF-8'>
                                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                <title>Errore {statusCode}</title>
                                <style>
                                    body {{
                                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                                        min-height: 100vh;
                                        display: flex;
                                        align-items: center;
                                        justify-content: center;
                                        margin: 0;
                                        color: white;
                                        text-align: center;
                                    }}
                                    .container {{
                                        background: rgba(255, 255, 255, 0.1);
                                        backdrop-filter: blur(10px);
                                        border-radius: 20px;
                                        padding: 50px;
                                        box-shadow: 0 8px 32px rgba(0,0,0,0.3);
                                    }}
                                    h1 {{
                                        font-size: 120px;
                                        margin: 0;
                                        text-shadow: 0 5px 20px rgba(0,0,0,0.3);
                                    }}
                                    p {{
                                        font-size: 24px;
                                        margin: 20px 0;
                                    }}
                                    a {{
                                        display: inline-block;
                                        margin-top: 30px;
                                        padding: 15px 40px;
                                        background: white;
                                        color: #667eea;
                                        text-decoration: none;
                                        border-radius: 50px;
                                        font-weight: 600;
                                        transition: transform 0.3s;
                                    }}
                                    a:hover {{
                                        transform: translateY(-3px);
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <h1>{statusCode}</h1>
                                    <p>Si è verificato un errore</p>
                                    <a href='/'>🏠 Torna alla Dashboard</a>
                                </div>
                            </body>
                            </html>";
                        await response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(html));
                    }
                }
                catch (Exception ex)
                {
                    // Fallback minimale in caso di errore nella gestione errori
                    var fallbackHtml = $@"
                        <!DOCTYPE html>
                        <html>
                        <head><title>Errore {statusCode}</title></head>
                        <body style='text-align:center; padding:50px; background:#c92a2a; color:white;'>
                            <h1>{statusCode}</h1>
                            <p>Errore nel caricamento della pagina di errore</p>
                            <p style='font-size:12px;'>{ex.Message}</p>
                            <a href='/' style='color:white; text-decoration:underline;'>Torna alla Home</a>
                        </body>
                        </html>";
                    await response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(fallbackHtml));
                }
            });

            app.UseEndpoints(endpoints =>
            {
                // HUB
                endpoints.MapHub<TemplateHub>("/templateHub");

                // AREA ROUTING (IMPORTANTE!)
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                // Existing Example routes
                endpoints.MapAreaControllerRoute(
                    "Example",
                    "Example",
                    "Example/{controller=Users}/{action=Index}/{id?}");

                // DEFAULT → LOGIN
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Login}/{action=Login}/{id?}");
            });
        }
    }

    public static class SupportedCultures
    {
        public readonly static string[] CultureNames;
        public readonly static CultureInfo[] Cultures;

        static SupportedCultures()
        {
            CultureNames = new[] { "it-it" };
            Cultures = CultureNames.Select(c => new CultureInfo(c)).ToArray();
        }
    }
}