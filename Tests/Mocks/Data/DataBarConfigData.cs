namespace OrderFlowBot.Tests.Mocks.Data
{
    public class DataBarConfigData
    {
        public double TickSize { get; set; }
        public int TicksPerLevel { get; set; }
        public int StackedImbalance { get; set; }
        public double ImbalanceRatio { get; set; }
        public long ImbalanceMinDelta { get; set; }
        public double ValueAreaPercentage { get; set; }

        public DataBarConfigData()
        {
            TickSize = 0.25;
            TicksPerLevel = 1;
            StackedImbalance = 3;
            ImbalanceRatio = 1.5;
            ImbalanceMinDelta = 10;
            ValueAreaPercentage = 70;
        }
    }
}
