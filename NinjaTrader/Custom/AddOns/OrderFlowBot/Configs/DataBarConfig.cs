﻿namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Configs
{
    public class DataBarConfig : IDataBarConfig
    {
        private static readonly DataBarConfig _instance = new DataBarConfig();

        private DataBarConfig()
        {
        }

        public static DataBarConfig Instance => _instance;

        public double TickSize { get; set; }
        public int TicksPerLevel { get; set; }
        public int StackedImbalance { get; set; }
        public double ImbalanceRatio { get; set; }
        public long ImbalanceMinDelta { get; set; }
        public double ValueAreaPercentage { get; set; }
        public string TrainingDataDirectory { get; set; }
        public int Target { get; set; }
        public int Stop { get; set; }

    }
}
