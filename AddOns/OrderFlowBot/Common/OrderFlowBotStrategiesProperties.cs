namespace NinjaTrader.Custom.AddOns.OrderFlowBot
{
    public struct OrderFlowBotStrategiesPropertiesValues
    {
        // Delta Chaser
        public int DeltaChaserDelta { get; set; }
        public int DeltaChaserMinMaxDifferenceDelta { get; set; }
        public double DeltaChaserMinMaxDifferenceMultiplier { get; set; }
        public int DeltaChaserValidEntryTicks { get; set; }
        public bool DeltaChaserRatiosEnabled { get; set; }

        // Range Rebound
        public int RangeReboundMinMaxDelta { get; set; }
        public int RangeReboundValidEntryTicks { get; set; }
        public bool RangeReboundRatiosEnabled { get; set; }

        // Stacked Imbalances
        public bool StackedImbalanceValidOpenTSP { get; set; }
        public bool StackedImbalancesRatiosEnabled { get; set; }

        // Volume Sequencing
        public bool VolumeSequencingValidOpenTSP { get; set; }
        public bool VolumeSequencingRatiosEnabled { get; set; }
    }

    public static class OrderFlowBotStrategiesProperties
    {
        // Delta Chaser
        public static int DeltaChaserDelta { get; set; }
        public static int DeltaChaserMinMaxDifferenceDelta { get; set; }
        public static double DeltaChaserMinMaxDifferenceMultiplier { get; set; }
        public static int DeltaChaserValidEntryTicks { get; set; }
        public static bool DeltaChaserRatiosEnabled { get; set; }

        // Range Rebound
        public static int RangeReboundMinMaxDelta { get; set; }
        public static int RangeReboundValidEntryTicks { get; set; }
        public static bool RangeReboundRatiosEnabled { get; set; }

        // Stacked Imbalances
        public static bool StackedImbalanceValidOpenTSP { get; set; }
        public static bool StackedImbalancesRatiosEnabled { get; set; }

        // Volume Sequencing
        public static bool VolumeSequencingValidOpenTSP { get; set; }
        public static bool VolumeSequencingRatiosEnabled { get; set; }

        public static void Initialize(OrderFlowBotStrategiesPropertiesValues config)
        {
            DeltaChaserDelta = config.DeltaChaserDelta;
            DeltaChaserMinMaxDifferenceDelta = config.DeltaChaserMinMaxDifferenceDelta;
            DeltaChaserMinMaxDifferenceMultiplier = config.DeltaChaserMinMaxDifferenceMultiplier;
            DeltaChaserValidEntryTicks = config.DeltaChaserValidEntryTicks;
            DeltaChaserRatiosEnabled = config.DeltaChaserRatiosEnabled;
            RangeReboundMinMaxDelta = config.RangeReboundMinMaxDelta;
            RangeReboundValidEntryTicks = config.RangeReboundValidEntryTicks;
            RangeReboundRatiosEnabled = config.RangeReboundRatiosEnabled;
            StackedImbalanceValidOpenTSP = config.StackedImbalanceValidOpenTSP;
            StackedImbalancesRatiosEnabled = config.StackedImbalancesRatiosEnabled;
            VolumeSequencingValidOpenTSP = config.VolumeSequencingValidOpenTSP;
            VolumeSequencingRatiosEnabled = config.VolumeSequencingRatiosEnabled;
        }
    }
}
