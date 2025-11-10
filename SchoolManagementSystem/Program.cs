using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Repositories;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Register MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();   // ✅ Required for app.MapRazorPages()

// Configure EF Core with SQL Server + Lazy Loading
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolManagementSystem"))
           .UseLazyLoadingProxies());

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register custom services
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Ensure database is migrated and seed data is added on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        db.Database.Migrate();
        await DataSeeder.SeedRolesAsync(scope.ServiceProvider);
        await DataSeeder.SeedAdminUserAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Seeding failed: {ex.Message}");
    }
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
