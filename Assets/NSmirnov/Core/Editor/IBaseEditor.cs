using NSmirnov.Core.Foundation;

namespace NSmirnov.Core.Editor
{
    public interface IBaseEditor<C, P> where P : IGameProperties, new()
    {
        string Title { get; }
        void Draw();
        void ClearSelection();
        IBaseEditor<C, P> Init(C gameConfig);
    }
}