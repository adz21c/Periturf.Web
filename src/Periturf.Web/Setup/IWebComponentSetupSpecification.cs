using Microsoft.AspNetCore.Http;

namespace Periturf.Web.Setup
{
    public interface IWebComponentSetupSpecification
    {
        string Name { get; }

        PathString Path { get; }

        ConfigureWebAppDto Configure();
    }
}
