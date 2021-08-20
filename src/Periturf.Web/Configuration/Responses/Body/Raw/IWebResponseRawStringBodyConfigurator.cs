namespace Periturf.Web.Configuration.Responses.Body.Raw
{
    public interface IWebResponseRawStringBodyConfigurator
    {
        void ContentType(string contentType);

        void Body(string content);
    }
}
