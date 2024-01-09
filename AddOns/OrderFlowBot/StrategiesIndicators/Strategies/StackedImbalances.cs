using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.StrategiesIndicators.Strategies
{
    public class StackedImbalances : StrategyBase
    {
        public override string Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }

        public StackedImbalances(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, string name)
        : base(orderFlowBotState, dataBars, name)
        {
            // This can be used to initialize other values.
        }

        public override void CheckStrategy()
        {
            if (IsValidLongDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckLong();
            }

            if (IsValidShortDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckShort();
            }
        }

        // Bar is bullish and has x ask stacked imbalances.
        public override void CheckLong()
        {
            if (IsBullishBar() && HasValidAskStackedImbalance())
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        // Bar is bearish and has x bid stacked imbalances.
        public override void CheckShort()
        {
            if (IsBearishBar() && HasValidBidStackedImbalance())
            {
                ValidStrategyDirection = Direction.Short;
            }
        }

        private bool IsBullishBar()
        {
            return dataBars.Bar.BarType == BarType.Bullish;
        }

        private bool IsBearishBar()
        {
            return dataBars.Bar.BarType == BarType.Bearish;
        }

        private bool HasValidAskStackedImbalance()
        {
            return dataBars.Bar.Imbalances.HasAskStackedImbalances;
        }

        private bool HasValidBidStackedImbalance()
        {
            return dataBars.Bar.Imbalances.HasBidStackedImbalances;
        }
    }
}