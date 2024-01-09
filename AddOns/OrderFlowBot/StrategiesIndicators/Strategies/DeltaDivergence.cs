using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.StrategiesIndicators.Strategies
{
    public class DeltaDivergence : StrategyBase
    {
        public override string Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }

        public DeltaDivergence(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, string name)
        : base(orderFlowBotState, dataBars, name)
        {
            // This can be used to initialize other values.
        }

        public override void CheckStrategy()
        {
            if (dataBars.Bars.Count < OrderFlowBotProperties.LookBackBars)
            {
                return;
            }

            if (IsValidLongDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckLong();
            }

            if (IsValidShortDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckShort();
            }
        }

        // Price makes a new low based on the last x bars, delta is positive and the bar is bullish.
        public override void CheckLong()
        {
            if (IsBullishBar() && DeltaIsPositive() && IsValidLastBarsForBullishInverse())
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        // Price makes a new high based on the last x bars, delta is negative and the bar is bearish.
        public override void CheckShort()
        {
            if (IsBearishBar() && DeltaIsNegative() && IsValidLastBarsForBearishInverse())
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

        private bool DeltaIsPositive()
        {
            return dataBars.Bar.Deltas.Delta > 1;
        }

        private bool DeltaIsNegative()
        {
            return dataBars.Bar.Deltas.Delta < -1;
        }

        private bool IsValidLastBarsForBullishInverse()
        {
            double currentBarLowPrice = dataBars.Bar.Prices.Low;

            // Check if the current bar's low price is lower than each of the x numbers of preceding bars
            for (int i = dataBars.Bars.Count - 1; i > dataBars.Bars.Count - 1 - OrderFlowBotProperties.LookBackBars; i--)
            {
                if (currentBarLowPrice >= dataBars.Bars[i].Prices.Low)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidLastBarsForBearishInverse()
        {
            double currentBarHighPrice = dataBars.Bar.Prices.High;

            // Check if the current bar's high price is higher than each of the x numbers of preceding bars
            for (int i = dataBars.Bars.Count - 1; i > dataBars.Bars.Count - 1 - OrderFlowBotProperties.LookBackBars; i--)
            {
                if (currentBarHighPrice <= dataBars.Bars[i].Prices.High)
                {
                    return false;
                }
            }

            return true;
        }
    }
}