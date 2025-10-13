using WebNews.Services;
using WebNews.Data.Extensions;
using WebNews.Data.UnitOfWork;
using WebNews.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddRazorRuntimeCompilation();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<NewsService>();
builder.Services.AddScoped<NewsService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UploadFileToFolder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();