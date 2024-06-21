namespace NinjaTrader.Custom.AddOns.OrderFlowBot
{
    public struct OrderFlowBotStrategiesPropertiesValues
    {
        // Delta Chaser
        public int DeltaChaserDelta { get; set; }

        // Stacked Imbalances
        public bool StackedImbalanceValidOpenTSP { get; set; }

        // Volume Sequencing
        public bool VolumeSequencingValidOpenTSP { get; set; }
    }

    public static class OrderFlowBotStrategiesProperties
    {
        // Delta Chaser
        public static int DeltaChaserDelta { get; set; }

        // Stacked Imbalances
        public static bool StackedImbalanceValidOpenTSP { get; set; }

        // Volume Sequencing
        public static bool VolumeSequencingValidOpenTSP { get; set; }

        public static void Initialize(OrderFlowBotStrategiesPropertiesValues config)
        {
            DeltaChaserDelta = config.DeltaChaserDelta;
            StackedImbalanceValidOpenTSP = config.StackedImbalanceValidOpenTSP;
            VolumeSequencingValidOpenTSP = config.VolumeSequencingValidOpenTSP;
        }
    }
}
