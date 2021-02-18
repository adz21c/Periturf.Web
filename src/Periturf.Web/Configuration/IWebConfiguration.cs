using System.Threading.Tasks;

namespace Periturf.Web.Configuration
{
    interface IWebConfiguration
    {
        Task ExecuteHandlers(IWebRequestEvent request);
        bool Matches(IWebRequestEvent request);
        Task WriteResponse(IWebResponse response);
    }
}