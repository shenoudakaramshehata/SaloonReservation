using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using SaloonReservation.Data;
using System.Globalization;
using System.Reflection;
using NToastNotify;
using Email;
using SaloonReservation.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using SaloonReservation.Services;
using DevExpress.AspNetCore.Reporting;
using DevExpress.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<SalonContext>(options => options.UseSqlServer(connectionString));
#region Toast
builder.Services.AddRazorPages().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    PreventDuplicates = true,
    CloseButton = true
});
#endregion
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#region "Identity Configuration"
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;


    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});
#endregion

#region "Localization"
var supportedCultures = new[]
{
    new CultureInfo("ar-EG"),
    new CultureInfo("en-US"),
};

// Configure localization options
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("ar-EG");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddRazorPages()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
    .AddDataAnnotationsLocalization(options =>
    {
        var type = typeof(SaloonReservation.SharedResource);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
        var localizer = factory.Create("SharedResource", assemblyName.Name);
        options.DataAnnotationLocalizerProvider = (t, f) => localizer;
    });

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        var type = typeof(SaloonReservation.SharedResource);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
        var localizer = factory.Create("SharedResource", assemblyName.Name);
        options.DataAnnotationLocalizerProvider = (t, f) => localizer;
    });

builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization(options =>
    {
        var type = typeof(SaloonReservation.SharedResource);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
        var localizer = factory.Create("SharedResource", assemblyName.Name);
        options.DataAnnotationLocalizerProvider = (t, f) => localizer;
    });
#endregion
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IRazorPartialToStringRenderer, RazorPartialToStringRenderer>();
builder.Services.Configure<RazorViewEngineOptions>(o =>
{
    o.ViewLocationFormats.Add("/Shared/Components/InputForm/{0}" + RazorViewEngine.ViewExtension);
    o.ViewLocationFormats.Add("/Pages/Shared/Components/InputForm/{0}" + RazorViewEngine.ViewExtension);
});
builder.Services.AddDevExpressControls();
builder.Services.AddMvc();
builder.Services.ConfigureReportingServices(configurator => {
    configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
        viewerConfigurator.UseCachedReportSourceBuilder();
    });
});
builder.Services.AddRazorPages();

var app = builder.Build();
app.UseDevExpressControls();
System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ar-EG"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseRouting();
app.UseNToastNotify();
app.UseAuthentication(); // Add this to configure Identity's authentication.
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

    endpoints.MapRazorPages();
    endpoints.MapControllers();
});


app.Run();
