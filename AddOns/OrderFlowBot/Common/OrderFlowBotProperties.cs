namespace NinjaTrader.Custom.AddOns.OrderFlowBot
{
    public struct OrderFlowBotPropertiesConfig
    {
        // DataBar
        public int LookBackBars { get; set; }
        public double ImbalanceRatio { get; set; }
        public int StackedImbalance { get; set; }
        public long ValidBidVolume { get; set; }
        public long ValidAskVolume { get; set; }
        public double ValidExhaustionRatio { get; set; }
        public double ValidAbsorptionRatio { get; set; }

        // AutoVolumeProfile
        public int AutoVolumeProfileLookBackBars { get; set; }
    }

    public static class OrderFlowBotProperties
    {
        // DataBar
        public static int LookBackBars { get; private set; }
        public static double ImbalanceRatio { get; private set; }
        public static int StackedImbalance { get; private set; }
        public static long ValidBidVolume { get; private set; }
        public static long ValidAskVolume { get; private set; }
        public static double ValidExhaustionRatio { get; private set; }
        public static double ValidAbsorptionRatio { get; private set; }

        // AutoVolumeProfile
        public static int AutoVolumeProfileLookBackBars { get; private set; }

        public static void Initialize(OrderFlowBotPropertiesConfig config)
        {
            LookBackBars = config.LookBackBars;
            ImbalanceRatio = config.ImbalanceRatio;
            StackedImbalance = config.StackedImbalance;
            ValidBidVolume = config.ValidBidVolume;
            ValidAskVolume = config.ValidAskVolume;
            ValidExhaustionRatio = config.ValidExhaustionRatio;
            ValidAbsorptionRatio = config.ValidAbsorptionRatio;

            AutoVolumeProfileLookBackBars = config.AutoVolumeProfileLookBackBars;
        }
    }
}
