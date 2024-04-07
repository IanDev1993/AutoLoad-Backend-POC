using BackEndMimimal.Extensions;
using BackEndMimimal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
builder.AddArchitectures();

var app = builder.Build();
app.UseArchitectures();


app.MapGet("/order", async (AppDbContext context) => 
    {
        var orders = await context.Orders.AsNoTracking().ToListAsync();
        
        return orders.Count>0 ? Results.Ok(orders) : Results.NoContent();
    }   
 ).Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status204NoContent).WithName(endpointName: "GetOrders").WithTags("Order");

app.MapPost("/order", async (
    [FromQuery] string code,
    [FromQuery] bool status,
    [FromServices] AppDbContext context) =>
    {
        Order order = new Order()
        {
            Code = code,
            Status = status,
        };

       context.Orders.Add(order);
       var result = await context.SaveChangesAsync();

       return result > 0 ? Results.CreatedAtRoute("GetOrders",order) : Results.BadRequest();
      }
).Produces(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest).WithName("CreateOrder").WithTags("Order");

app.MapPut("/order", async (
    [FromQuery] Guid id,
    [FromQuery] bool status,
    [FromServices] AppDbContext context) =>
    {
        Order? order = await context.Orders.FindAsync(id) ?? null;

        if(order is null)
        {
            return Results.NotFound();
        }            
        else
        {        
            order.Status = status;
        
            context.Orders.Update(order);

            var result = await context.SaveChangesAsync();
            
            return result > 0 ? Results.Ok(order) : Results.BadRequest();
        }  
}
).Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status400BadRequest).WithName("UpdateOrder").WithTags("Order");


app.MapDelete("/order", async (
    [FromQuery] Guid id,
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

            var result = await context.SaveChangesAsync();
            
            return result > 0 ? Results.Ok(order) : Results.BadRequest();
        }      
    }
).Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status400BadRequest).WithName("DeleteOrder").WithTags("Order");


app.Run();

