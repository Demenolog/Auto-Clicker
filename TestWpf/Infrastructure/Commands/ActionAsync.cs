using System.Threading.Tasks;

namespace TestWpf.Infrastructure.Commands
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
}