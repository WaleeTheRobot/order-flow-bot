namespace OrderFlowBot.Tests.Mocks.Data
{
    public class DataBarPrintConfigData
    {
        public int BarsAgo { get; set; }
        public bool ShowBasic { get; set; }
        public bool ShowDeltas { get; set; }
        public bool ShowImbalances { get; set; }
        public bool ShowPrices { get; set; }
        public bool ShowRatios { get; set; }
        public bool ShowVolumes { get; set; }
        public bool ShowBidAskVolumePerBar { get; set; }

        public DataBarPrintConfigData()
        {
            BarsAgo = 0;
            ShowBasic = true;
            ShowDeltas = true;
            ShowImbalances = true;
            ShowPrices = true;
            ShowRatios = true;
            ShowVolumes = true;
            ShowBidAskVolumePerBar = true;
        }
    }
}
