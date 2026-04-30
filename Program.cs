using Azure.Identity;
using LoginApp.Data;
using LoginApp.Models;
using LoginApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

// FOR NON-DEVELOPMENT ENVIRONMENTS (Production)
if (!builder.Environment.IsDevelopment())
{
    // Load App Configuration first
    var appConfigUri = builder.Configuration["AppConfiguration:Uri"];
    if (!string.IsNullOrEmpty(appConfigUri))
    {
        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            options
                .Connect(new Uri(appConfigUri), new DefaultAzureCredential())
                .Select(KeyFilter.Any, LabelFilter.Null)
                .Select(KeyFilter.Any, builder.Environment.EnvironmentName);
        });
    }

    // Then load Key Vault (overrides App Config if keys exist in both)
    var keyVaultUri = builder.Configuration["KeyVault:Uri"];
    if (!string.IsNullOrEmpty(keyVaultUri))
    {
        builder.Configuration.AddAzureKeyVault(
            new Uri(keyVaultUri),
            new DefaultAzureCredential());
    }
}
else
{
    // For local development: Use connection string from appsettings.Development.json
    var appConfigConnectionString = builder.Configuration["AppConfiguration:ConnectionString"];
    if (!string.IsNullOrEmpty(appConfigConnectionString))
    {
        builder.Configuration.AddAzureAppConfiguration(appConfigConnectionString);
    }
}

// Added Azure services
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSingleton<AzureKeyVaultService>();
// builder.Services.AddApplicationInsightsTelemetry(options =>
// {
//     options.EnableAdaptiveSampling = true;
//     options.DependencyCollectionOptions.EnableSqlCommandTextInstrumentation = true;
// });

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true; 
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();  

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
