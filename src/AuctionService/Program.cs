using AuctionService.Data;
using MassTransit;
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
builder.Services.AddMassTransit( x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

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
