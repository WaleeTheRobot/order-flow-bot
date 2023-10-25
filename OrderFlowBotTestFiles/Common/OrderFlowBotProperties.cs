namespace OrderFlowBotTestFiles.Common
{
    public struct OrderFlowBotPropertiesConfig
    {
        // DataBar
        public double ImbalanceRatio { get; set; }
        public int StackedImbalance { get; set; }
        public long ValidBidVolume { get; set; }
        public long ValidAskVolume { get; set; }
        public int ValidRatio { get; set; }

        // AutoVolumeProfile
        public int AutoVolumeProfileLookBackBars { get; set; }
    }

    public static class OrderFlowBotProperties
    {
        // DataBar
        public static double ImbalanceRatio { get; private set; }
        public static int StackedImbalance { get; private set; }
        public static long ValidBidVolume { get; private set; }
        public static long ValidAskVolume { get; private set; }
        public static int ValidRatio { get; private set; }

        // AutoVolumeProfile
        public static int AutoVolumeProfileLookBackBars { get; private set; }

        public static void Initialize(OrderFlowBotPropertiesConfig config)
        {
            ImbalanceRatio = config.ImbalanceRatio;
            StackedImbalance = config.StackedImbalance;
            ValidBidVolume = config.ValidBidVolume;
            ValidAskVolume = config.ValidAskVolume;
            ValidRatio = config.ValidRatio;

            AutoVolumeProfileLookBackBars = config.AutoVolumeProfileLookBackBars;
        }
    }
}
