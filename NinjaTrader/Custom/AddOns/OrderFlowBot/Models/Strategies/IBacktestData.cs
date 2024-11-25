namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public interface IBacktestData
    {
        string Name { get; set; }
        bool IsBacktestEnabled { get; set; }
    }
}
