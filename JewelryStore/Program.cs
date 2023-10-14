using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JewelryStore.Data;

using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JewelryStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JewelryStoreContext") ?? throw new InvalidOperationException("Connection string 'JewelryStoreContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => {
        option.LoginPath = "/Access/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

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

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
