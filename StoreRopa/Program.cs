using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreRopa.Data;
using StoreRopa.Data.Extensions;
using StoreRopa.Data.utils;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext(configuration).AddServices();
//DbComntext para Identity
builder.Services.AddDbContext<StoreIdentityDbContext> (options =>
        options.UseSqlServer(configuration.GetConnectionString("StringConnection")));

// Configuración de ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configuración de opciones de seguridad de contraseñas
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;

    // Configuración de bloqueo de cuenta por intentos fallidos
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    // Configuración de opciones de usuario
    options.User.RequireUniqueEmail = false;

    // Configuración de cookies
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<StoreIdentityDbContext>() // Configuración de almacenamiento de datos
.AddDefaultTokenProviders(); // Proveedores de tokens por defecto

//Configuración de política de cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.LoginPath = "/Auth";
    options.AccessDeniedPath = "/Auth/Denied";
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddAuthentication();

builder.Services.AddAuthorization(
////options =>
////{
////    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
////    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
////    // Puedes agregar más políticas según sea necesario
////}
);



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile($@"C:\logs\log.txt");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}");

app.Run();
