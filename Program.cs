using ges_commande.Models.Fixture;
using GesCommande.core;
using Gesdette.Service;
using GesDetteWeb.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("MysqlConnection");
builder.Services.AddDbContext<Context>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 2, 0))));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IPersonneService, PersonneService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<Fixtures>(); // Enregistrement du service Fixtures

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Securite/Login"; // Chemin pour accéder à la page de connexion
        options.LogoutPath = "/Securite/Logout"; // Chemin pour se déconnecter
    });

var app = builder.Build();

app.UseAuthentication(); // Ajoute ce middleware avant UseAuthorization
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var context = serviceProvider.GetRequiredService<Context>();
    var seeder = serviceProvider.GetRequiredService<Fixtures>();
    seeder.Initialize(serviceProvider, context);
}
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Securite}/{action=Login}");

app.Run();
