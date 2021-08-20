using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Periturf.Web.Configuration.Responses
{
    public interface IWebResponseConfigurator<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        void StatusCode(int code);

        void AddHeader(string name, StringValues value);

        void AddCookie(string name, string value, CookieOptions? options = null);

        void AddResponseBodySpecification(IWebResponseBodySpecification<TWebRequestEvent> spec);
    }
}
