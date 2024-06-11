using Application.OutputPorts;
using Application.Services.Github;
using Repositories;

namespace Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddScoped<IGithubRepoRepository, GithubRepoRepository>();
            services.AddSingleton<IGithubService, GithubService>();
            return services;
        }
    }
}
