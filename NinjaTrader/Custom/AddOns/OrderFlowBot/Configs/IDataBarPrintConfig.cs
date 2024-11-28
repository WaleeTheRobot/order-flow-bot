namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Configs
{
    public interface IDataBarPrintConfig
    {
        int BarsAgo { get; set; }
        bool ShowBasic { get; set; }
        bool ShowDeltas { get; set; }
        bool ShowImbalances { get; set; }
        bool ShowPrices { get; set; }
        bool ShowRatios { get; set; }
        bool ShowVolumes { get; set; }
        bool ShowBidAskVolumePerBar { get; set; }
    }
}
