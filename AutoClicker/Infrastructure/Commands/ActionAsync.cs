using System.Threading.Tasks;

namespace AutoClicker.Infrastructure.Commands
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
}