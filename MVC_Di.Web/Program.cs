using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MVC_Di.Data;
using MVC_Di.Middleware;
using MVC_Di.Services;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().LogFactory.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var isEfDesignTime = AppDomain.CurrentDomain
        .GetAssemblies()
        .Any(assembly => assembly.GetName().Name == "Microsoft.EntityFrameworkCore.Design");

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<AccountingDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IRecordService, RecordService>();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/Login";
        });

    var app = builder.Build();

    if (!isEfDesignTime)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AccountingDbContext>();
            await SeedData.InitializeAsync(dbContext);
        }
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseMiddleware<RequestTimingMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    if (!isEfDesignTime)
    {
        app.Run();
    }
}
catch (Exception exception)
{
    logger.Error(exception, "Application stopped because of an exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
