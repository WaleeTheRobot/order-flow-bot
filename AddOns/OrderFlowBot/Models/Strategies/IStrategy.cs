namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public interface IStrategy
    {
        string Name { get; set; }
        bool StrategyTriggered { get; set; }

        void CheckStrategy();
        void CheckLong();
        void CheckShort();
    }
}
