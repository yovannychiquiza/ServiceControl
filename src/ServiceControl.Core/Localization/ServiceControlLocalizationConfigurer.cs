using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ServiceControl.Localization
{
    public static class ServiceControlLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(ServiceControlConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ServiceControlLocalizationConfigurer).GetAssembly(),
                        "ServiceControl.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
