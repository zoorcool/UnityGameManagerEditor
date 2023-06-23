using NSmirnov.Core.Foundation;

namespace NSmirnov.Core
{
    public interface ILoaderManager<M, C, P>
        where M : IGameManager<C, P>, new()
        where C : IGameConfiguration<P>, new()
        where P : IGameProperties, new()
    {
        M gameManager { get; }
    }
}