using Microsoft.AspNetCore.Builder;

namespace Infrastructure.API.Configuration.Application;

public static class BaseApplicationConfiguration
{
    public static void BaseConfiguration(
        this WebApplication app,
        bool useHttps,
        bool isPrivateHosted)
    {
        app.MapControllers();
        
        // if (app.Environment.IsDevelopment()) TODO
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (useHttps)
            app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        if (isPrivateHosted)
            app.UseCors(opt => opt.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()); // TODO
    }
}