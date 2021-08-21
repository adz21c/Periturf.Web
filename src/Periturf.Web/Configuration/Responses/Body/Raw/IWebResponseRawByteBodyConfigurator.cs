namespace Periturf.Web.Configuration.Responses.Body.Raw
{
    public interface IWebResponseRawByteBodyConfigurator
    {
        void ContentType(string contentType);

        void Body(byte[] content);
    }
}
