namespace SauceConnect.Service.Configuration
{
    public interface ISauceConnectConfiguration
    {
        string Username { get; }
        string AccessKey { get; }
        string TunnelIdentifier { get; }
        double TunnelPollInterval { get; }
    }
}