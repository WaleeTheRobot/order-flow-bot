namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Configs
{
    public class MessagingConfig
    {
        private static readonly MessagingConfig _instance = new MessagingConfig();

        private MessagingConfig()
        {
        }

        public static MessagingConfig Instance => _instance;

        public EnvironmentType MarketEnvironment { get; set; }
        public string ExternalAnalysisService { get; set; }
        public bool ExternalAnalysisServiceEnabled { get; set; }
    }
}
