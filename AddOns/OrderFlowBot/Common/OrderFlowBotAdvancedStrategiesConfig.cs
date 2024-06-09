namespace NinjaTrader.Custom.AddOns.OrderFlowBot
{
    public struct OrderFlowBotAdvancedStrategiesConfigValues
    {
        // Delta Chaser
        public int DeltaChaserDelta { get; set; }
        public int DeltaChaserMinMaxDifferenceDelta { get; set; }
        public double DeltaChaserMinMaxDifferenceMultiplier { get; set; }
        public int DeltaChaserValidEntryTicks { get; set; }

        // Range Rebound
        public int RangeReboundMinMaxDelta { get; set; }
        public int RangeReboundValidEntryTicks { get; set; }
    }

    public static class OrderFlowBotAdvancedStrategiesConfig
    {
        // Delta Chaser
        public static int DeltaChaserDelta { get; set; }
        public static int DeltaChaserMinMaxDifferenceDelta { get; set; }
        public static double DeltaChaserMinMaxDifferenceMultiplier { get; set; }
        public static int DeltaChaserValidEntryTicks { get; set; }

        // Range Rebound
        public static int RangeReboundMinMaxDelta { get; set; }
        public static int RangeReboundValidEntryTicks { get; set; }

        public static void Initialize(OrderFlowBotAdvancedStrategiesConfigValues config)
        {
            DeltaChaserDelta = config.DeltaChaserDelta;
            DeltaChaserMinMaxDifferenceDelta = config.DeltaChaserMinMaxDifferenceDelta;
            DeltaChaserMinMaxDifferenceMultiplier = config.DeltaChaserMinMaxDifferenceMultiplier;
            DeltaChaserValidEntryTicks = config.DeltaChaserValidEntryTicks;
            RangeReboundMinMaxDelta = config.RangeReboundMinMaxDelta;
            RangeReboundValidEntryTicks = config.RangeReboundValidEntryTicks;
        }
    }
}
