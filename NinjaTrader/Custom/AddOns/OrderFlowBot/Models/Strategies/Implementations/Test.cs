using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies.Implementations
{
    public class Test : StrategyBase
    {
        public Test(EventsContainer eventsContainer) : base(eventsContainer)
        {
            StrategyData.Name = "Test";
        }

        public override bool CheckLong()
        {
            return IsBullishBar() && IsValidDelta();
        }

        public override bool CheckShort()
        {
            return IsBearishBar() && IsValidDelta(false);
        }

        private bool IsBullishBar()
        {
            return currentDataBar.BarType == BarType.Bullish;
        }

        private bool IsBearishBar()
        {
            return currentDataBar.BarType == BarType.Bearish;
        }

        private bool IsValidDelta(bool longCheck = true)
        {
            if (longCheck)
            {
                return currentDataBar.Deltas.Delta > 100;
            }

            return currentDataBar.Deltas.Delta < -100;
        }
    }
}
