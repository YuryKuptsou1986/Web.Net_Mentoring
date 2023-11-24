using DAL;
using BLL;
using Homework.Entities.Configuration;
using Homework.Filters;
using Homework.MIddleware;
using Homework.Services.Implementation;
using Homework.Services.Interfaces;
using Microsoft.AspNetCore.Localization;
using Serilog;
using System.Globalization;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Homework.Data;
using Homework.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;

namespace Homework
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            // setting for smpt server
            ConfigureSmtp(builder);

            // ASP.Net Core Identity
            ConfigureIndentity(builder);

            // configure dependency from other libraries
            builder.Services.AddDalDependencies(builder.Configuration);
            builder.Services.AddBllDependencies();

            // Add services to the container.
            var mvcBuilder = builder.Services.AddControllersWithViews(options => {
                options.Filters.Add<ActionLoggingFilter>();
            });
            if (builder.Environment.IsDevelopment()) {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            // Web optimizer
            builder.Services.AddWebOptimizer(pipeline => {
                pipeline.MinifyCssFiles("/css/**/*.css");
                pipeline.MinifyJsFiles("/js/**/*.js");
            });

            // Localization
            ConfigureLocalization(builder);

            // Services
            ConfigureServices(builder);

            // settings
            ConfigureSettings(builder);

            // mapper
            builder.Services.AddAutoMapper(typeof(Homework.Mapper.AppMappingProfile));

            // logs
            builder.Host.UseSerilog((ctx, sp) => {
                sp.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration);
            });

            // swagger
            builder.Services.AddSwaggerGen(
            c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Northwind Service" });
            });
            
            var app = builder.Build();

            // swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "version v1");
            });
            
            // log start up info
            LogStartUpInfo(app);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                //app.UseExceptionHandler("/Home/Error");
                app.UseMiddleware<ExceptionMiddleware>();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseMiddleware<ImageCacheMiddleware>();

            app.UseHttpsRedirection();
            app.UseWebOptimizer();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "images",
                pattern: "images/{image_id}",
                defaults: new { controller = "Image", action = "Index" }
                );

            app.MapRazorPages();

            var scope = app.Services.CreateScope();
            
            var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
            var roles = (IOptions<RoleSettings>)scope.ServiceProvider.GetService(typeof(IOptions<RoleSettings>));
            var userManager = (UserManager<UserIdentity>)scope.ServiceProvider.GetService(typeof(UserManager<UserIdentity>));

            await CreateRoles(roleManager, roles);
            await CreateAdmin(userManager, roles);

            app.Run();
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager, IOptions<RoleSettings> roles)
        {
            foreach (string rol in roles.Value.Roles) {
                if (!await roleManager.RoleExistsAsync(rol)) {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }
        }

        private static async Task CreateAdmin(UserManager<UserIdentity> userManager, IOptions<RoleSettings> roles)
        {
            var admin = await userManager.FindByEmailAsync(roles.Value.AdminEmail);

            if(admin == null) {
                admin = new UserIdentity {
                    Email = roles.Value.AdminEmail,
                    EmailConfirmed = false,
                    UserName = roles.Value.AdminEmail,
                    NormalizedUserName = roles.Value.AdminRole.ToUpperInvariant(),
                    NormalizedEmail = roles.Value.AdminEmail.ToUpperInvariant(),
                };

                // generate random password
                string password = Convert.ToBase64String(RandomNumberGenerator.GetBytes(20));

                // Set random password. Admin will change it when login at first time
                // 1. Resend confirmation. Will send link to email.
                // 2. Confirm email.
                // 3. Reset password. Will send link to email.
                // 4. Enter new password and login.
                await userManager.CreateAsync(admin, password);
                // add user to role
                await userManager.AddToRoleAsync(admin, roles.Value.AdminRole);
            }
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IFormFileToStreamConverter, FormFileToStreamConverter>();
        }

        private static void ConfigureLocalization(WebApplicationBuilder builder)
        {
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options => {
                var supportedCultures = new[]
                {
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        private static void ConfigureIndentity(WebApplicationBuilder builder)
        {
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // azure AD
            builder.Services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => { 
                    builder.Configuration.Bind("AzureAd", options);
                    options.CookieSchemeName = IdentityConstants.ExternalScheme;
                });
            builder.Services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options => {
                options.Authority = options.Authority + "/v2.0/";
                options.TokenValidationParameters.ValidateIssuer = true;
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<UserIdentityContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<UserIdentity>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<UserIdentityContext>();

            builder.Services.AddAuthorization();

            // roles
            builder.Services.AddAuthorization(options => {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });
        }

        private static void ConfigureSmtp(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    builder.Configuration["EmailSender:Host"],
                    builder.Configuration.GetValue<int>("EmailSender:Port"),
                    builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    builder.Configuration["EmailSender:UserName"],
                    builder.Configuration["EmailSender:Password"]
                )
            );
        }

        private static void ConfigureSettings(WebApplicationBuilder builder)
        {
            builder.Services.Configure<ProductSettings>(
                builder.Configuration.GetSection(
                key: nameof(ProductSettings)));
            builder.Services.Configure<ImageCacheSettings>(
                builder.Configuration.GetSection(
                key: nameof(ImageCacheSettings)));
            builder.Services.Configure<FilterLoggingSettings>(
                builder.Configuration.GetSection(
                key: nameof(FilterLoggingSettings)));
            builder.Services.Configure<RoleSettings>(
                builder.Configuration.GetSection(
                key: nameof(RoleSettings)));
        }

        private static void LogStartUpInfo(WebApplication app)
        {
            IWebHostEnvironment environment = app.Environment;
            app.Logger.LogInformation(environment.ContentRootPath);
            app.Logger.LogInformation(environment.WebRootPath);
            app.Logger.LogInformation(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            app.Logger.LogInformation(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            app.Logger.LogInformation(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            app.Logger.LogInformation(Directory.GetCurrentDirectory());
            app.Logger.LogInformation(AppDomain.CurrentDomain.BaseDirectory);
            app.Logger.LogInformation(AppContext.BaseDirectory);
        }
    }
}