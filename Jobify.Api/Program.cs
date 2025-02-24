using Jobify.Api.Mapping;
using Jobify.BL.DALModels;
using Jobify.BL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Microsoft.AspNetCore.Identity;
using Jobify.BL.Repositories;
using Jobify.BL.Security;
using Jobify.Api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWebDriver>(sp =>
{
    return new ChromeDriver();
});

builder.Services.AddDbContext<JobifyContext>(options =>
{
    options.UseNpgsql("name=ConnectionStrings:Tembo");
});
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Get connection string from configuration
//var connectionString = builder.Configuration.GetConnectionString("Tembo")
//                      ?? throw new InvalidOperationException("Connection string 'Tembo' not found.");

//// Register the DbContext with DI
//builder.Services.AddDbContext<JobifyContext>(options =>
//    options.UseNpgsql(connectionString));

//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddScoped<UserRepository, UserRepository>();
builder.Services.AddScoped<IRepository<Employer>, EmployerRepository>();
builder.Services.AddScoped<IRepository<Student>, StudentRepository>();
builder.Services.AddScoped<IRepository<Firm>, FirmRepository>();
builder.Services.AddScoped<IRepository<UserType>, UserTypeRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<OIBValidationService>();

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IUserTypeAdapter, UserTypeAdapter>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// configure JWT security services
var secureKey = builder.Configuration["JwtSettings:Secret"];
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => {
        var Key = Encoding.UTF8.GetBytes(secureKey);
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Key)
        };
    });
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
