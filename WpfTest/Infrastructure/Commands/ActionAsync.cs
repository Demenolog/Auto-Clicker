using System.Threading.Tasks;

namespace WpfTest.Infrastructure.Commands
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
}