using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars
{
    public interface IReadOnlyDataBar
    {
        BarType BarType { get; }
        int Time { get; }
        int Day { get; }
        int BarNumber { get; }

        Prices Prices { get; }
        Ratios Ratios { get; }
        Volumes Volumes { get; }
        Deltas Deltas { get; }
        Imbalances Imbalances { get; }
        CumulativeDeltaBar CumulativeDeltaBar { get; }
    }
}
