using Microsoft.AspNetCore.Authentication.Cookies;
using WebNews.Services;
using WebNews.Data.Extensions;
using WebNews.Data.UnitOfWork;
using WebNews.Helpers.Auth;
using WebNews.Helpers.AutoMapper.MappingProfiles;
using WebNews.Helpers.Image;
using WebNews.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase();

builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddRazorRuntimeCompilation();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<NewsService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ImageHelper>();
builder.Services.AddScoped<Hasher<User>>();
builder.Services.AddAutoMapper(cfg => { }, typeof(NewsProfile), typeof(UserProfile));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthHelper>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();