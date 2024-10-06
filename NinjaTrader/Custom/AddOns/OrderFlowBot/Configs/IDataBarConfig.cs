namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Configs
{
    public interface IDataBarConfig
    {
        double TickSize { get; set; }
        int TicksPerLevel { get; set; }
        int StackedImbalance { get; set; }
        double ImbalanceRatio { get; set; }
        long ImbalanceMinDelta { get; set; }
        double ValueAreaPercentage { get; set; }
    }
}
