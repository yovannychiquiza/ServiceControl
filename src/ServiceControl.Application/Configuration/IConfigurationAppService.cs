using System.Threading.Tasks;
using ServiceControl.Configuration.Dto;

namespace ServiceControl.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
