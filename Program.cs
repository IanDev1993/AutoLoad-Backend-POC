using System.Reflection.Metadata.Ecma335;
using BackEndMimimal.Models;
using BackEndMimimal.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;

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

app.UseHttpsRedirection();
app.UseCors("corspolicy");
app.UseStaticFiles();
app.Run();

