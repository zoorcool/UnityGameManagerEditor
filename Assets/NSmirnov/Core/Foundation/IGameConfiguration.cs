namespace NSmirnov.Core.Foundation
{
    public interface IGameConfiguration<P> where P : IGameProperties, new()
    {
        P Properties { get; }
        void SaveGameConfiguration(GameConfigurationProfile.Item profile);
        void LoadGameConfiguration(GameConfigurationProfile.Item profile);
    }
}