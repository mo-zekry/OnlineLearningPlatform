using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Mapping;
using OnlineLearningPlatform.Repositories;

namespace OnlineLearningPlatform;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Injecting the db context
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Identity Configurations
        builder
            .Services.AddIdentity<Student, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Repositories and Unit of Work (Scoped)
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

        // Auto Mapper Configurations
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // WebOptimizer javascript bundler configurations
        builder.Services.AddWebOptimizer(pipeline =>
        {
            pipeline.AddJavaScriptBundle("/js/bundle.js", "wwwroot/js/*.js").UseContentRoot();
            pipeline.MinifyJsFiles();
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseWebOptimizer();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id:?}",
            defaults: new { controller = "Home", action = "Index" }
        );

        app.Run();
    }
}
