using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
using TataApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TataDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Sqlite") ?? ""));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(opt =>
{
    opt.Cookie.Name = ".Tata.App";
    opt.Cookie.IsEssential = true;
});

builder.Services.Configure<AntiforgeryOptions>(options =>
{
    options.Cookie.Name = "APP-COOKIE";
    options.HeaderName = "APP-TOKEN";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (ctx, next) =>
{
    try
    {
        await next();
    }
    catch (Exception e)
    {
        app.Logger.LogError("Error {error} acour in {path}", e, ctx.Request.Path);

        throw;
    }

    if (ctx.Response.StatusCode == 404 )//&& !ctx.Response.HasStarted)
    {
        ctx.Request.Path = "/404";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
