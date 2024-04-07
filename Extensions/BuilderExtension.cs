using Microsoft.OpenApi.Models;

namespace BackEndMimimal.Extensions;

public static class BuilderExtension
{

    public static void AddArchitectures ( this WebApplicationBuilder builder)
    {           
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDbContext<AppDbContext>();

        builder.Services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Order API",
                Version="v1",                  
                Contact = new OpenApiContact
                {
                    Name = "Ian Nascimento",
                    Email = "ian.dev.1993@gmail.com",
                    Url = new Uri("https://br.linkedin.com/in/ian-nascimento-3b7574140"),
                }         
            });
        });

   
        builder.Services.AddCors(s=>
            s.AddPolicy("corspolicy",c=> 
            {
                c.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
            }
        ));
    } 
}