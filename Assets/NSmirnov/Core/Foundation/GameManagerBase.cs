namespace NSmirnov.Core.Foundation
{
    public abstract class GameManagerBase<M, C, P> : IGameManager<C, P>
        where M : new()
        where C : IGameConfiguration<P>, new()
        where P : IGameProperties, new()
    {
        public C Config { get; }

        private static readonly M instance = new M();
        public static M Instance
        {
            get { return instance; }
        }

        protected GameManagerBase()
        {
            Config = new C();
        }
    }
}