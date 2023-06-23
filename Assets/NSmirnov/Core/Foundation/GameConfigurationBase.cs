namespace NSmirnov.Core.Foundation
{
    public abstract class GameConfigurationBase<P> : IGameConfiguration<P> where P : IGameProperties, new()
    {
        protected P properties;
        public P Properties
        {
            get 
            {
                if (properties == null)
                    properties = new P();

                return properties;
            }
        }
        public abstract void SaveGameConfiguration(GameConfigurationProfile.Item profile);
        public abstract void LoadGameConfiguration(GameConfigurationProfile.Item profile);
    }
}