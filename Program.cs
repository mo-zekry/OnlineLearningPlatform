using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Handlers;
using OnlineLearningPlatform.Mapping;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.Requirements;
using Stripe;

namespace OnlineLearningPlatform;

public static class Program {
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Identity Password Configurations
        builder.Services.Configure<IdentityOptions>(options => {
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
        builder.Services.AddDbContext<ApplicationDbContext>(options => {
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

        // stripe payment configurations
        StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

        // Configure Authorization
        builder.Services.AddAuthorization(options => {
            options.AddPolicy(
                "EnrolledInCourse",
                policy => policy.Requirements.Add(new EnrolledInCourseRequirement())
            );
        });

        // Register the authorization handler as a scoped service
        builder.Services.AddScoped<IAuthorizationHandler, EnrolledInCourseHandler>();

        var app = builder.Build();

        // Data Seeding
        using (var scope = app.Services.CreateScope()) {
            var services = scope.ServiceProvider;
            await SeedData.Initialize(services);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // app.UseWebOptimizer();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id:?}",
            new { controller = "Home", action = "Index" }
        );

        app.Run();
    }
}