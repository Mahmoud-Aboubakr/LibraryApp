using Application.Interfaces;
using Application.Validators;
using Infrastructure.AppServices;
using Infrastructure.AppServicesContracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Data;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LibraryDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<ISearchBookDataWithDetailService, SearchBookDataWithDetailService>();
builder.Services.AddScoped<INumbersValidator, NumbersValidator>();
builder.Services.AddScoped<IPhoneNumberValidator, PhoneNumberValidator>();
builder.Services.AddTransient<ISearchAuthorDataService, SearchAuthorDataService>();
builder.Services.AddTransient<ISearchCustomerService, SearchCustomerService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
