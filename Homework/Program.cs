using Homework.DBContext;
using Homework.DBContext.Repositories.Implementaion;
using Homework.DBContext.Repositories.Interfaces;
using Homework.Entities.Configuration;
using Homework.Filters;
using Homework.Mapper;
using Homework.MIddleware;
using Homework.Services.Implementation;
using Homework.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Reflection;

namespace Homework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<NorthwindContext>(options =>
                options.UseSqlServer(connectionString));

            // Repositories
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

            // Services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<INorthwindImageConverterService, NorthwindImageConverterService>();

            // settings
            builder.Services.Configure<ProductSettings>(
                builder.Configuration.GetSection(
                key: nameof(ProductSettings)));
            builder.Services.Configure<ImageCacheSettings>(
                builder.Configuration.GetSection(
                key: nameof(ImageCacheSettings)));
            builder.Services.Configure<FilterLoggingSettings>(
                builder.Configuration.GetSection(
                key: nameof(FilterLoggingSettings)));

            // mapper
            builder.Services.AddAutoMapper(typeof(AppMapperConfiguration));

            builder.Host.UseSerilog((ctx, sp) => {
                sp.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration);
            });

            // filters
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ActionLoggingFilter>();
            });

            var app = builder.Build();

            // log start up info
            IWebHostEnvironment environment = app.Environment;
            app.Logger.LogInformation(environment.ContentRootPath);
            app.Logger.LogInformation(environment.WebRootPath);
            app.Logger.LogInformation(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            app.Logger.LogInformation(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            app.Logger.LogInformation(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            app.Logger.LogInformation(Directory.GetCurrentDirectory());
            app.Logger.LogInformation(AppDomain.CurrentDomain.BaseDirectory);
            app.Logger.LogInformation(AppContext.BaseDirectory);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                //app.UseExceptionHandler("/Home/Error");
                app.UseMiddleware<ExceptionMiddleware>();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ImageCacheMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "images",
                pattern: "images/{image_id}",
                defaults: new { controller = "Image", action = "Index" }
                );
            app.Run();
        }
    }
}