using Jobify.Api.Mapping;
using Jobify.BL.DALModels;
using Jobify.BL.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Microsoft.AspNetCore.Identity;
using Jobify.BL.Repositories;

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
    options.UseNpgsql("name=ConnectionStrings:AppConnStr");
});

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<OIBValidationService>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Firm>, FirmRepository>();
builder.Services.AddScoped<IRepository<UserType>, UserTypeRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
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

app.MapControllers();

app.Run();
