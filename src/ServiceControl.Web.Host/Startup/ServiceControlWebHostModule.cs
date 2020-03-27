using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ServiceControl.Configuration;

namespace ServiceControl.Web.Host.Startup
{
    [DependsOn(
       typeof(ServiceControlWebCoreModule))]
    public class ServiceControlWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ServiceControlWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ServiceControlWebHostModule).GetAssembly());
        }
    }
}
