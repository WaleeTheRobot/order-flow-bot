namespace NinjaTrader.Custom.AddOns.OrderFlowBot.DataBarConfigs
{
    public class DataBarConfig
    {
        private static readonly DataBarConfig _instance = new DataBarConfig();

        private DataBarConfig()
        {
        }

        public static DataBarConfig Instance
        {
            get
            {
                return _instance;
            }
        }

        public double TickSize { get; set; }
        public int TicksPerLevel { get; set; }
        public int StackedImbalance { get; set; }
        public double ImbalanceRatio { get; set; }
        public long ValidImbalanceVolume { get; set; }
    }
}
