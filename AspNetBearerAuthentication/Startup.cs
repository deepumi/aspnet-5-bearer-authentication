using AspNetBearerAuthentication.Auth;
using AspNetBearerAuthentication.Repository;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetBearerAuthentication
{
    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserMocRepository>();

            services.AddSingleton<ICalimsService, ClaimsService>();
            
            services.AddSingleton<IPolicyEvaluator, BearerPolicyEvaluator>();

            services.AddControllers(o => o.Filters.Add(new BearerAuthorizeFilter()));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}