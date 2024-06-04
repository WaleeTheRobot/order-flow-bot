namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
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
