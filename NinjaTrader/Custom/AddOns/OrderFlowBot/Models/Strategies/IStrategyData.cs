using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public interface IStrategyData
    {
        string Name { get; set; }
        Direction TriggeredDirection { get; set; }
        bool StrategyTriggered { get; set; }
        void UpdateTriggeredDataProvider(Direction triggeredDirection, bool strategyTriggered);
    }
}
