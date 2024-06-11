using Microsoft.OpenApi.Models;

namespace Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Github Repositories", 
                    Description = "API to show most starred Github repositories of C#, Java, JavaScript, Python, and Ruby",
                    Contact= new OpenApiContact
                    {
                        Name = "Arthur da Rosa Almeida",
                        Email = "arthurdralmeida@gmail.com"
                    },
                    Version = "v1" 
                });
            });

            return services;
        }
    }
}
