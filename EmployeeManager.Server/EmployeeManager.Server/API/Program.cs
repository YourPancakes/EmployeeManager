using EmployeeManager.Server.API.Middleware;
using EmployeeManager.Server.Application.Mapping;
using EmployeeManager.Server.Application.Services.Implementations;
using EmployeeManager.Server.Application.Services.Interfaces;
using EmployeeManager.Server.Application.Validators;
using EmployeeManager.Server.Infrastructure.Persistence;
using EmployeeManager.Server.Infrastructure.Repositories.Implementations;
using EmployeeManager.Server.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Manager API", Version = "v1" });
    opts.AddServer(new OpenApiServer { Url = "http://localhost:5000", Description = "Development server" });
});
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    o.AddPolicy("AllowSpecific", p => p
        .WithOrigins("http://localhost:3000", "http://localhost:4200", "https://employeemanager.com")
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});
builder.Services.AddApiVersioning(opts =>
{
    opts.DefaultApiVersion = new ApiVersion(1, 0);
    opts.AssumeDefaultVersionWhenUnspecified = true;
    opts.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});
builder.Services.Configure<JsonOptions>(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    opts.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddDbContext<EmployeeManagerDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeCreateDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<EmployeeSalaryUpdateValidator>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICompanyStatisticsService, CompanyStatisticsService>();

builder.Services.AddHostedService<DatabaseInitializer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Manager API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowSpecific");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();