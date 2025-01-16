using Jobify.BL.DALModels;
using Jobify.Api.Service;
using Jobify.Client.Pages;
using Jobify.Components;
using Jobify.Components.Account;
using Jobify.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Jobify.BL.Interfaces;
using Jobify.BL.Database;
using Jobify.BL.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
//builder.Services.AddScoped<IQueries, Queries>();


builder.Services.AddHttpClient("Jobify.Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Jobify:ApiBaseAddress"]);
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IAccountApiService, AccountApiService>();

//builder.Services.AddDbContext<JobifyContext>(options =>
//{
//    options.UseNpgsql("name=ConnectionStrings:AppConnStr");
//});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();
var connectionString = builder.Configuration.GetConnectionString("Tembo") ?? throw new InvalidOperationException("Connection string 'Tembo' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<JobifyContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
Console.WriteLine(builder.Configuration.GetConnectionString("Tembo"));

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddSingleton<IQueries>(q => Queries.GetInstance(q.GetRequiredService<JobifyContext>()));

//builder.Services
//    .AddBlazorise(options =>
//{
//    options.Immediate = true;
//})
//    .AddBootstrap5Providers();
var serviceProvider = builder.Services.BuildServiceProvider();
DbQueryProvider.Init(serviceProvider.GetRequiredService<IQueries>());
builder.Services.AddBlazorBootstrap();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Jobify.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
