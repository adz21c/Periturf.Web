using System;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration.Requests.Responses
{
    public interface IWebWriterSpecification
    {
        Func<IWebResponse, object?, CancellationToken, ValueTask> Build();
    }
}
