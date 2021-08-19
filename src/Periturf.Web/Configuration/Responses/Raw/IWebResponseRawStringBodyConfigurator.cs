namespace Periturf.Web.Configuration.Responses
{
    public interface IWebResponseRawStringBodyConfigurator
    {
        void ContentType(string contentType);

        void Body(string content);
    }
}
