using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebV.Data;
using WebV.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebV") ?? throw new InvalidOperationException("Connection string 'WebVContext' not found.")));

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("WebV") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
//await DbSeeder.SeendRolesAndAdminAsync(app.Services);

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeendRolesAndAdminAsync(scope.ServiceProvider);
}
app.Run();
