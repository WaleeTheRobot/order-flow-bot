using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.States
{
    public interface IReadOnlyTradingState
    {
        string TriggeredName { get; }
        bool StrategyTriggered { get; }
        Direction TriggeredDirection { get; }
        Direction SelectedTradeDirection { get; }
        bool IsTradingEnabled { get; }
        bool IsAutoTradeEnabled { get; }
        bool IsAlertEnabled { get; }
    }
}
