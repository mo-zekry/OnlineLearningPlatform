using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Mapping;
using OnlineLearningPlatform.Repositories;

namespace OnlineLearningPlatform;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Identity Options Configurations
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 6;
        });

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Injecting the db context
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Identity Configurations
        builder
            .Services.AddIdentity<ApplicationUser, IdentityRole>()
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

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await SeedData.Initialize(services);
        }

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
