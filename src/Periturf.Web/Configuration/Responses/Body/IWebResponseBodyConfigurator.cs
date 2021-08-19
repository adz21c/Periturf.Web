using Periturf.Web.BodyWriters;

namespace Periturf.Web.Configuration.Responses
{
    public interface IWebResponseBodyConfigurator : IWebBodyWritableConfigurator
    {
        void Content(object content);
    }
}
