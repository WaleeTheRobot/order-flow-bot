namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Configs
{
    public class TechnicalLevelsPrintConfig : ITechnicalLevelsPrintConfig
    {
        public int BarsAgo { get; set; }
        public bool ShowEma { get; set; }

        public TechnicalLevelsPrintConfig()
        {
            BarsAgo = 0;
            ShowEma = true;
        }
    }
}
