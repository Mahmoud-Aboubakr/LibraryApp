using API.Extensions;
using Application.Handlers;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Data;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers(o => o.Filters.Add(new CustomApiExceptions()))
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

builder.Services.AppServices(builder.Configuration);
//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware(typeof(ExceptionHandlerMiddleware));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
    
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<LibraryDbContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    var path = Path.Combine(Directory.GetCurrentDirectory(), "Utilities\\Logs");
    var tracePath = Path.Join(path, $"Log_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}.txt");
    var loggerFactory = app.Services.GetService<ILoggerFactory>();
    loggerFactory.AddFile(tracePath);
    await context.Database.MigrateAsync();
    await LibraryDbContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
