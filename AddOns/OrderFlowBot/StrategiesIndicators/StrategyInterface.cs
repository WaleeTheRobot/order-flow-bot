namespace NinjaTrader.Custom.AddOns.OrderFlowBot.StrategiesIndicators.Strategies
{
    public interface IStrategyInterface
    {
        string Name { get; set; }
        Direction ValidStrategyDirection { get; set; }

        void CheckStrategy();
        void CheckLong();
        void CheckShort();
    }
}
