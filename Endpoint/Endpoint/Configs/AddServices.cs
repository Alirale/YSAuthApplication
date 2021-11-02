using Applcation.Services;
using Core.RepositoryInterface;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Configs
{
    public static class AddServices
    {
        public static void AddYSServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
