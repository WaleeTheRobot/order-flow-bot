namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public interface IStrategy
    {
        IStrategyData StrategyData { get; set; }
        IStrategyData CheckStrategy();
        bool CheckLong();
        bool CheckShort();
    }
}
