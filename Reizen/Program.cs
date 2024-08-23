using Microsoft.EntityFrameworkCore;
using Reizen.Models;
using Reizen.Models.Repositories;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ReizenContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ReizenConnection")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IWerelddeelRepository, SQLWerelddeelRepository>();
builder.Services.AddTransient<ILandRepository, SQLLandenRepository>();
builder.Services.AddTransient<IBestemmingRepository, SQLBestemmingRepository>();
builder.Services.AddTransient<IReisRepository, SQLReisRepository>();
builder.Services.AddTransient<IKlantRepository, SQLKlantRepository>();
builder.Services.AddTransient<IBoekingRepository, SQLBoekingRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
