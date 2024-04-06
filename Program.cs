using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
            Title = "Orders API",
            Version = "v1",
            Description = "An API to perform Orders operations",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Ian Nascimento",
                Email = "ian.dev.1993@gmail.com",
                Url = new Uri("https://br.linkedin.com/in/ian-nascimento-3b7574140"),
            },
            License = new OpenApiLicense
            {
                Name = "Orders API LICX",
                Url = new Uri("https://example.com/license"),
            }
     
    });
});

builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        
    });
}


app.MapGet("/orders", async (AppDbContext context) => 
    await context
            .Orders
            .Include(x=>x.Driver)
            .ToListAsync()
);


app.MapGet("/orders-drivers", async (AppDbContext context) => 
    await context
            .OrderDrivers
            .ToListAsync()
);

app.MapPost("/orders", async (Order order, AppDbContext context) =>
{
    context.Orders.Add(order);
    await context.SaveChangesAsync();

    return Results.Created($"/orders/{order.Id}", order);
});


app.UseHttpsRedirection();
app.UseStaticFiles();
app.Run();

