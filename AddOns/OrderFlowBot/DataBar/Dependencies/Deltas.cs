namespace NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies
{
    public class Deltas
    {
        public long Delta { get; set; }
        public long MinDelta { get; set; }
        public long MaxDelta { get; set; }
        public long CumulativeDelta { get; set; }
        public double DeltaPercentage { get; set; }
        public double MinMaxDeltaRatio { get; set; }
        public double MaxMinDeltaRatio { get; set; }
        public double DeltaChange { get; set; }
    }
}
