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
        bool IsBacktestEnabled { get; set; }
        string BacktestStrategyName { get; set; }
        bool IsTradingEnabled { get; }
        bool IsAutoTradeEnabled { get; }
        bool IsAlertEnabled { get; }
        double TriggerStrikePrice { get; }
        List<string> SelectedStrategies { get; }
        int LastTradedBarNumber { get; set; }
        int CurrentBarNumber { get; set; }
        bool HasMarketPosition { get; set; }
    }
}
