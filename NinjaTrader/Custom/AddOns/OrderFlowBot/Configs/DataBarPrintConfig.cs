namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Configs
{
    public class DataBarPrintConfig : IDataBarPrintConfig
    {
        public int BarsAgo { get; set; }
        public bool ShowBasic { get; set; }
        public bool ShowDeltas { get; set; }
        public bool ShowImbalances { get; set; }
        public bool ShowPrices { get; set; }
        public bool ShowRatios { get; set; }
        public bool ShowVolumes { get; set; }
        public bool ShowBidAskVolumePerBar { get; set; }
        public bool ShowCumulativeDeltaBar { get; set; }

        public DataBarPrintConfig()
        {
            BarsAgo = 0;
            ShowBasic = true;
            ShowDeltas = true;
            ShowImbalances = true;
            ShowPrices = true;
            ShowRatios = true;
            ShowVolumes = true;
            ShowBidAskVolumePerBar = true;
            ShowCumulativeDeltaBar = true;
        }
    }
}
