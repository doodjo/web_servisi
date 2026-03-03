using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// uvek nakon kreiranja kontrolera ide builder za dbccontext
builder.Services.AddDbContext<AuctionDbContext>( opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.




app.UseAuthorization();

app.MapControllers();

try
{
    DbInitializer.IntDb(app);
}
catch (Exception e)
{
    Console.WriteLine("Greska prilikom povlacenja podataka");
    Console.WriteLine(e);
}

app.Run();
