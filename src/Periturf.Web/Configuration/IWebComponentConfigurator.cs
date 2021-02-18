using Periturf.Web.Configuration.Requests;
using System;

namespace Periturf.Web.Configuration
{
    public interface IWebComponentConfigurator
    {
        void OnRequest(Action<IWebRequestEventConfigurator<IWebRequestEvent>> config);

        void OnRequest<TBody>(Action<IWebRequestEventConfigurator<IWebRequestEvent<TBody>>> config);
    }
}
