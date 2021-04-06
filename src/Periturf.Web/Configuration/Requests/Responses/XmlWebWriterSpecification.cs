using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Periturf.Web.Configuration.Requests.Responses
{
    class XmlWebWriterSpecification : IWebWriterSpecification, IXmlWebWriterConfigurator
    {
        public Func<IWebResponse, object?, CancellationToken, ValueTask> Build()
        {
            return (response, obj, ct) =>
            {
                response.ContentType = "application/xml";
        
                if (obj == null)
                    return new ValueTask();

                var serializer = new XmlSerializer(obj.GetType());
                
                serializer.Serialize(response.BodyStream, obj);

                return new ValueTask();
            };
        }
    }
}
