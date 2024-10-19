using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars
{
    public interface IReadOnlyDataBar
    {
        public BarType BarType { get; }
        public int Time { get; }
        public int BarNumber { get; }

        public Prices Prices { get; }
        public Ratios Ratios { get; }
        public Volumes Volumes { get; }
        public Deltas Deltas { get; }
        public Imbalances Imbalances { get; }
    }
}
