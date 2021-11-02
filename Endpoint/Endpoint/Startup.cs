using Core.DTO;
using Endpoint.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Endpoint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private TokenModel tokenSettings { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            string conection = Configuration["Conection"];

            tokenSettings = Configuration.GetSection("JWtConfig").Get<TokenModel>();

            services.AddJwtAuthorization(tokenSettings);

            services.AddSqlconfigs(conection);

            services.AddYSServices();

            services.AddControllers();

            services.AddSwaggerService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwaggerService(env);
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
