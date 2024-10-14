namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Configs
{
    public interface IDataBarPrintConfig
    {
        public int BarsAgo { get; set; }
        public bool ShowBasic { get; set; }
        public bool ShowDeltas { get; set; }
        public bool ShowImbalances { get; set; }
        public bool ShowPrices { get; set; }
        public bool ShowRatios { get; set; }
        public bool ShowVolumes { get; set; }
        public bool ShowBidAskVolumePerBar { get; set; }
    }
}
