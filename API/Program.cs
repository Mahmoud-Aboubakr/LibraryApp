using API.Extensions;
using API.Middlewares;
using Application.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);


builder.Services.AppServices(builder.Configuration);
builder.Services.IdentityService(builder.Configuration);
//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware(typeof(ExceptionHandlerMiddleware));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
    
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<LibraryDbContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
try
{
    var path = Path.Combine(Directory.GetCurrentDirectory(), "Utilities\\Logs");
    var tracePath = Path.Join(path, $"Log_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}.txt");
    var loggerFactory = app.Services.GetService<ILoggerFactory>();
    loggerFactory.AddFile(tracePath);
    await context.Database.MigrateAsync();
    await LibraryDbContextSeed.SeedAsync(context);
    await LibraryDbContextSeed.SeedDemoUserAndRoles(context, userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
