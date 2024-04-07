namespace BackEndMimimal.Extensions;

public static class AppExtension
{
    public static void UseArchitectures( this WebApplication app)
    {
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

        app.UseHttpsRedirection();

        app.UseCors("corspolicy");

        app.UseStaticFiles();
    }
}