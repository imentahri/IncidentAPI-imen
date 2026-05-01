using IncidentAPI_imen.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ IMPORTANT : éviter conflit avec les tests
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<IncidentsDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("IncidentsConnection")
        ));
}

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// 🔥 OBLIGATOIRE POUR WebApplicationFactory
public partial class Program { }