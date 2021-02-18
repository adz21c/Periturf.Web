using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Interpretation
{
    public interface IBodyInterprettor<TBody>
    {
        ValueTask<TBody> InterpretAsync(IWebRequest request, Stream body, CancellationToken ct);
    }
}