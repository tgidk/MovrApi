using Calculations;
using Controllers.Validation;
using Core;
using Mappings;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Scalar.AspNetCore;
using Serilog;
using Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.WriteTo.Console()
			.CreateLogger();

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IHaversineDistance, HaversineDistance>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
builder.Services.AddScoped<IRideRepo, RideRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IVehicleRepo, VehicleRepo>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ValidateModelAttribute>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//Use tools like Azure Key Vault, AWS Secrets Manager, or Vault by HashiCorp for better secret management in production environments.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<MovrContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseCors(builder => builder
		 .AllowAnyHeader()
		 .AllowAnyMethod()
		 .AllowAnyOrigin()
	);

	app.MapOpenApi();
	app.MapScalarApiReference();
}

if (app.Environment.IsProduction())
{
	app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
