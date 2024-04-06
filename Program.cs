using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.DocumentTitle = "Orders Api";     
    });
}


app.MapGet("/orders-itens", async (AppDbContext context) => await context.Orders.ToListAsync());
app.MapGet("/orders-drivers-itens", async (AppDbContext context) => await context.OrderDrivers.ToListAsync());

app.MapPost("/orders-item", async (Order order, AppDbContext context) =>
{
    context.Orders.Add(order);
    await context.SaveChangesAsync();

    return Results.Created($"/orders-item/{order.Id}", order);
});


app.UseHttpsRedirection();
app.Run();

