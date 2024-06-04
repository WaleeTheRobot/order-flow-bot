using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public class DeltaChaser : StrategyBase
    {
        public override string Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }
        private double _deltaDifferenceMultiplier;

        public DeltaChaser(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, string name)
        : base(orderFlowBotState, dataBars, name)
        {
            _deltaDifferenceMultiplier = 2.5;
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

        public override void CheckLong()
        {
            if (IsBullishBar() && IsOpenAboveTriggerStrikePrice() && IsBullishMinMaxDifference())
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        public override void CheckShort()
        {
            if (IsBearishBar() && IsOpenBelowTriggerStrikePrice() && IsBearishMinMaxDifference())
            {
                ValidStrategyDirection = Direction.Short;
            }
        }

        private bool IsOpenAboveTriggerStrikePrice()
        {
            if (orderFlowBotState.TriggerStrikePrice == 0)
            {
                return true;
            }

            return dataBars.Bar.Prices.Open > orderFlowBotState.TriggerStrikePrice;
        }

        private bool IsOpenBelowTriggerStrikePrice()
        {
            if (orderFlowBotState.TriggerStrikePrice == 0)
            {
                return true;
            }

            return dataBars.Bar.Prices.Open < orderFlowBotState.TriggerStrikePrice;
        }

        private bool IsBullishMinMaxDifference()
        {
            var maxDelta = Math.Abs(dataBars.Bar.Deltas.MaxDelta);
            var minDelta = Math.Abs(dataBars.Bar.Deltas.MinDelta);

            return maxDelta >= 3 * minDelta && dataBars.Bar.Deltas.Delta > 150;
        }

        private bool IsBearishMinMaxDifference()
        {
            var maxDelta = Math.Abs(dataBars.Bar.Deltas.MaxDelta);
            var minDelta = Math.Abs(dataBars.Bar.Deltas.MinDelta);

            return minDelta >= 3 * maxDelta && dataBars.Bar.Deltas.Delta < -150;
        }

        private bool IsBullishBar()
        {
            return dataBars.Bar.BarType == BarType.Bullish;
        }

        private bool IsBearishBar()
        {
            return dataBars.Bar.BarType == BarType.Bearish;
        }
    }
}
