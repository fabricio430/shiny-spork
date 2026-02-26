using DevFreela.API.Database;
using DevFreela.API.ExceptionHandler;
using DevFreela.API.Models.Config;
using DevFreela.API.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FreelanceTotalCostConfig>(
    builder.Configuration.GetSection("FreelanceTotalCostConfig")
);

// Configure to use in-memory database
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("MyDatabase"));

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ISkillsService, SkillsService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

app.UseExceptionHandler();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
