using Microsoft.EntityFrameworkCore;
using Serilog;
using CuponesApi.Data;
using CuponesApi.Interfaces;
using CuponesApi.Services;
using CuponesApi.Controllers;


var builder = WebApplication.CreateBuilder(args); // Crear build

// Configuración del contexto de la base de datos
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

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

// Añadir servicios
builder.Services.AddScoped<ICuponesService, CuponesService>();
builder.Services.AddScoped<ISendEmailService, SendEmailService>();

builder.Services.AddHttpClient();




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build(); // Iniciar build

// Configuración de la página
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();






