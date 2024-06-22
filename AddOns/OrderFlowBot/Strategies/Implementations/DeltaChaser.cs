using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    // This strategy is designed to chase the move after it exceeds a certain delta.
    // Trade the structure with appropriate targets
    public class DeltaChaser : StrategyBase
    {
        public override string Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }

        public DeltaChaser(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, string name, List<TechnicalLevels> technicalLevels)
        : base(orderFlowBotState, dataBars, name, technicalLevels)
        {
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
            if (IsBarType() && IsValidSpotStrikePrice() && IsValidDelta())
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        public override void CheckShort()
        {
            if (IsBarType(false) && IsValidSpotStrikePrice(false) && IsValidDelta(false))
            {
                ValidStrategyDirection = Direction.Short;
            }
        }

        private bool IsBarType(bool longCheck = true)
        {
            if (longCheck)
            {
                return dataBars.Bar.BarType == BarType.Bullish;
            }

            return dataBars.Bar.BarType == BarType.Bearish;
        }

        private bool IsValidSpotStrikePrice(bool longCheck = true)
        {
            if (orderFlowBotState.TriggerStrikePrice == 0)
            {
                return true;
            }

            // Is above trigger strike price
            if (longCheck)
            {
                return dataBars.Bar.Prices.Close > orderFlowBotState.TriggerStrikePrice;
            }

            // Is below trigger strike price
            return dataBars.Bar.Prices.Close < orderFlowBotState.TriggerStrikePrice;
        }

        private bool IsValidDelta(bool longCheck = true)
        {
            if (longCheck)
            {
                return dataBars.Bar.Deltas.Delta > OrderFlowBotStrategiesProperties.DeltaChaserDelta;
            }

            return dataBars.Bar.Deltas.Delta < OrderFlowBotStrategiesProperties.DeltaChaserDelta;
        }
    }
}
