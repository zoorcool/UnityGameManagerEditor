namespace NSmirnov.Core.Foundation
{
    public interface IGameManager<C, P> where C : IGameConfiguration<P> where P : IGameProperties, new()
    {
        C Config { get; }
    }
}