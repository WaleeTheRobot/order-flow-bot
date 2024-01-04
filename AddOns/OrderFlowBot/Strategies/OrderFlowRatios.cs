using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System.Linq;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public class OrderFlowRatios : StrategyBase
    {
        private OrderFlowBotDataBar _previousDataBar;
        public override OrderFlowBotStrategy Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }

        public OrderFlowRatios(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, OrderFlowBotStrategy name)
        : base(orderFlowBotState, dataBars, name)
        {
            _previousDataBar = new OrderFlowBotDataBar();
        }

        public override void CheckStrategy()
        {
            if (dataBars.Bars.Count > OrderFlowBotProperties.LookBackBars)
            {
                _previousDataBar = dataBars.Bars.Last();
            }
            else
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

        public override void CheckLong()
        {
            if (PreviousBarHasValidBidRatio() && IsBullishBar())
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        public override void CheckShort()
        {
            if (PreviousBarHasValidAskRatio() && IsBearishBar())
            {
                ValidStrategyDirection = Direction.Short;
            }
        }

        private bool PreviousBarHasValidBidRatio()
        {
            return _previousDataBar.BarType == BarType.Bullish && (_previousDataBar.Ratios.HasValidBidExhaustionRatio || _previousDataBar.Ratios.HasValidBidAbsorptionRatio);
        }

        private bool PreviousBarHasValidAskRatio()
        {
            return _previousDataBar.BarType == BarType.Bearish && (_previousDataBar.Ratios.HasValidAskExhaustionRatio || _previousDataBar.Ratios.HasValidAskAbsorptionRatio);
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