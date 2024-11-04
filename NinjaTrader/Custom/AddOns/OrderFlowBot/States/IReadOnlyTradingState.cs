using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.States
{
    public interface IReadOnlyTradingState
    {
        string TriggeredName { get; }
        bool StrategyTriggered { get; }
        Direction TriggeredDirection { get; }
        Direction SelectedTradeDirection { get; }
        Direction StandardInverse { get; }
        bool IsTradingEnabled { get; }
        bool IsAutoTradeEnabled { get; }
        bool IsAlertEnabled { get; }
        double TriggerStrikePrice { get; }
        public List<string> SelectedStrategies { get; }
    }
}
