using System.Reflection.Metadata.Ecma335;
using BackEndMimimal.Models;
using BackEndMimimal.Models.DTO;
using Microsoft.AspNetCore.Mvc;
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
        Version="v1",                  
        Contact = new OpenApiContact
        {
            Name = "Ian Nascimento",
            Email = "ian.dev.1993@gmail.com",
            Url = new Uri("https://br.linkedin.com/in/ian-nascimento-3b7574140"),
        }         
    });
});

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddCors(opt=>opt.AddPolicy("corspolicy",build=> 
    {
        build.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    }
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {        
        config.SwaggerEndpoint("/swagger/v1/swagger.json","v1");
        config.InjectStylesheet("/swagger/custom.css");
        config.RoutePrefix = string.Empty;
    });
}


app.MapGet("/order", async (AppDbContext context) => 
    await context
    .Orders
    .AsNoTracking()
    .ToListAsync()
).Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName(endpointName: "GetOrders")
    .WithTags("Order");;

app.MapPost("/order", async (
    [FromBody] OrderDTO orderDto,
    [FromServices] AppDbContext context) =>
    {
        Order order = new Order()
        {
            Code=orderDto.Code,
            Status=orderDto.Status,
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();
      }
).Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("CreateOrder")
    .WithTags("Order");

app.MapDelete("/order", async (
    [FromBody] Guid id,
    [FromServices] AppDbContext context) =>
    {       
        Order? order = await context.Orders.FindAsync(id) ?? null;

        if(order is null)
        {
            return Results.NotFound();
        }            
        else
        {
            context.Orders.Remove(order);

            await context.SaveChangesAsync();
            
            return Results.Ok(order);
        }
      
    }
).Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .WithName("DeleteOrder")
    .WithTags("Order");







app.UseHttpsRedirection();
app.UseCors("corspolicy");
app.UseStaticFiles();
app.Run();

