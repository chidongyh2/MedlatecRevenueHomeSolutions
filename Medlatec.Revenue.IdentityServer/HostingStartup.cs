using Medlatec.Revenue.IdentityServer.IdentityConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Medlatec.Revenue.IdentityServer
{
    public class HostingStartup: IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers();
                services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
              .AddDeveloperSigningCredential()
              .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
              .AddInMemoryApiResources(IdentityConfiguration.GetApis())
              .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
              .AddInMemoryClients(IdentityConfiguration.GetClients())
              .AddJwtBearerClientAuthentication();
            });
            
        }
    }
}
