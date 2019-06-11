using Microsoft.AspNetCore.Hosting;
using Periturf.Components;

namespace Periturf
{
    public interface IPeriturfWebHostBuilder : IWebHostBuilder
    {
        void AddComponent(string name, IComponent component);
    }
}
