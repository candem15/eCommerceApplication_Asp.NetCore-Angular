using eCommerceAPI.Application;
using eCommerceAPI.Application.Validators.Products;
using eCommerceAPI.Domain.Entities.Identity;
using eCommerceAPI.Infrastructure;
using eCommerceAPI.Infrastructure.eCommerceAPI.Persistence;
using eCommerceAPI.Infrastructure.Filters;
using eCommerceAPI.Infrastructure.Services.Storage.Azure;
using eCommerceAPI.Infrastructure.Services.Storage.Local;
using eCommerceAPI.Persistence.Contexts;
using eCommerceAPI.SignalR;
using eCommerceAPI.WebAPI.Configurations.Serilog;
using eCommerceAPI.WebAPI.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();//Client'tan gelen request neticesinde olusturulan HttpContext nesnesine katmanlardaki class'lar üzerinden(busineess logic) erisebilmemizi saglayan bir servistir.

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddSignalRServices();
//builder.Services.AddStorage<AzureStorage>();
builder.Services.AddStorage<LocalStorage>();

builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy =>
    policy.WithOrigins("https://localhost:4200", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>());

// Serilog 
Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.PostgreSQL(
    builder.Configuration.GetConnectionString("DockerPostgreSQL"),
    "Logs",
    needAutoCreateTable: true,
    columnOptions: new Dictionary<string, ColumnWriterBase>
    {
        { "message", new RenderedMessageColumnWriter() },
        { "message_template", new MessageTemplateColumnWriter() },
        { "level", new LevelColumnWriter() },
        { "time_stampt", new TimestampColumnWriter() },
        { "exception", new ExceptionColumnWriter() },
        { "log_event", new LogEventSerializedColumnWriter() },
        { "username", new UsernameColumnWriter() }
    })
    .WriteTo.Seq(builder.Configuration["Logging:Seq:ServerUrl"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);

// HttpLogging.AspNetCore
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // Oluþturulacak token deðerini hangi site/origin in kullanacaðýný belirliyoruz.
            ValidateIssuer = true, // Oluþturulacak token ýn daðýtýmýný kimin yapýcaðýný belirliyoruz.
            ValidateIssuerSigningKey = true, // Oluþturulacak olan token ýn kendi uygulumamýza ait olduðunu belirten unique key.
            ValidateLifetime = true, // Token ýn geçerlilik süresini belirler.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
            NameClaimType = ClaimTypes.Name
        };
    });

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.UseCors();

app.UseStaticFiles(); // wwwroot için gerekli

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("username", username);
    await next();
});

app.MapControllers();
app.MapHubs();

app.Run();
