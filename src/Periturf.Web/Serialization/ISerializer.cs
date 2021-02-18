using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Serialization
{
    public interface ISerializer
    {
        ValueTask<T> Deserialize<T>(Stream body, CancellationToken ct);
    }
}
