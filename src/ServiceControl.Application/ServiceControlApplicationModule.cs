using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ServiceControl.Authorization;

namespace ServiceControl
{
    [DependsOn(
        typeof(ServiceControlCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ServiceControlApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ServiceControlAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ServiceControlApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
