using WebNews.AutoMapper.Profiles;
using WebNews.Services;
using WebNews.Data.UnitOfWork;
using WebNews.Extensions;
using WebNews.Helpers.AutoMapper.MappingProfiles;
using WebNews.Helpers.Image;
using WebNews.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentityServices();

builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddRazorRuntimeCompilation();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ImageHelper>();
builder.Services.AddAutoMapper(cfg => { }, typeof(NewsProfile), typeof(UserProfile));

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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