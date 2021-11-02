using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Configs
{
    public static class AddSQL
    {
        public static void AddSqlconfigs(this IServiceCollection services, string Connection)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<DatabaseContext>(o => o.UseSqlServer(Connection));
        }
    }
}
