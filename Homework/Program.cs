﻿using DAL;
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

namespace Homework
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // configure dependency from other libraries
            builder.Services.AddDalDependencies(builder.Configuration);
            builder.Services.AddBllDependencies();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            // Localization
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

            // Services
            builder.Services.AddSingleton<INorthwindImageConverterService, NorthwindImageConverterService>();

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
            builder.Services.AddAutoMapper(typeof(Homework.Mapper.AppMappingProfile));

            // logs
            builder.Host.UseSerilog((ctx, sp) => {
                sp.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration);
            });

            // filters
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ActionLoggingFilter>();
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