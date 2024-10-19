namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public interface IStrategy
    {
        StrategyData StrategyData { get; set; }
        StrategyData CheckStrategy();
        bool CheckLong();
        bool CheckShort();
    }
}
