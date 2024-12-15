namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Configs
{
    public interface ITechnicalLevelsPrintConfig
    {
        int BarsAgo { get; set; }
        bool ShowEma { get; set; }
        bool ShowAtr { get; set; }
    }
}
