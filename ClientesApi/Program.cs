using ClientesApi.Controllers;
using ClientesApi.Data;
using ClientesApi.Interfaces;
using ClientesApi.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Configurar logs
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Logger(l => // Archivo de los logs de error
        l.Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Error)
        .WriteTo.File("Logs/Log-Error-.txt", rollingInterval: RollingInterval.Day)
    )
    .WriteTo.Logger(l => // Archivo de los logs normales
        l.Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Information)
        .WriteTo.File("Logs/Log-.txt", rollingInterval: RollingInterval.Day)
    )
    .CreateLogger(); // Crear logs

// Configuración del contexto de la base de datos
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped<ICuponesService, CuponesService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
