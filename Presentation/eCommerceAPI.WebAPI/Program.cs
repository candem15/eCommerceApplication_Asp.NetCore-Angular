using eCommerceAPI.Application;
using eCommerceAPI.Application.Validators.Products;
using eCommerceAPI.Domain.Entities.Identity;
using eCommerceAPI.Infrastructure;
using eCommerceAPI.Infrastructure.eCommerceAPI.Persistence;
using eCommerceAPI.Infrastructure.Filters;
using eCommerceAPI.Infrastructure.Services.Storage.Azure;
using eCommerceAPI.Infrastructure.Services.Storage.Local;
using eCommerceAPI.Persistence.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
//builder.Services.AddStorage<AzureStorage>();
builder.Services.AddStorage<LocalStorage>();

builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy =>
    policy.WithOrigins("https://localhost:4200", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // Olu�turulacak token de�erini hangi site/origin in kullanaca��n� belirliyoruz.
            ValidateIssuer = true, // Olu�turulacak token �n da��t�m�n� kimin yap�ca��n� belirliyoruz.
            ValidateIssuerSigningKey = true, // Olu�turulacak olan token �n kendi uygulumam�za ait oldu�unu belirten unique key.
            ValidateLifetime = true, // Token �n ge�erlilik s�resini belirler.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseStaticFiles(); // wwwroot i�in gerekli

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
