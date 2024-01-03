using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public class OrderFlowRatios : StrategyBase
    {
        private OrderFlowBotDataBar _previousDataBar;
        private long _totalPreviousMaxDeltas;
        private long _totalPreviousMinDeltas;
        public override OrderFlowBotStrategy Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }

        public OrderFlowRatios(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, OrderFlowBotStrategy name)
        : base(orderFlowBotState, dataBars, name)
        {
            _previousDataBar = new OrderFlowBotDataBar();
        }

        public override void CheckStrategy()
        {
            _previousDataBar = dataBars.Bars.Last();

            List<OrderFlowBotDataBar> previousBars = new List<OrderFlowBotDataBar>();
            previousBars = GetLastNBars(3);

            _totalPreviousMaxDeltas = 0;
            _totalPreviousMinDeltas = 0;

            // Trying to avoid entering at high deltas
            foreach (OrderFlowBotDataBar bar in previousBars)
            {
                _totalPreviousMaxDeltas += bar.Deltas.MaxDelta;
                _totalPreviousMinDeltas += bar.Deltas.MinDelta;
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

            /*if (IsOutOfValueArea() && IsValidMaxMinDeltaRatio() && IsValidBidRatio() && IsBullishBar())
            {
                ValidStrategyDirection = Direction.Long;
            }*/

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

            /*if (IsOutOfValueArea() && IsValidMinMaxDeltaRatio() && IsValidAskRatio() && IsBearishBar())
            {
                ValidStrategyDirection = Direction.Short;
            }*/
        }

        private bool PreviousBarHasValidBidRatio()
        {
            return _previousDataBar.BarType == BarType.Bullish && (_previousDataBar.Ratios.HasValidBidExhaustionRatio || _previousDataBar.Ratios.HasValidBidAbsorptionRatio);
        }

        private bool PreviousBarHasValidAskRatio()
        {
            return _previousDataBar.BarType == BarType.Bearish && (_previousDataBar.Ratios.HasValidAskExhaustionRatio || _previousDataBar.Ratios.HasValidAskAbsorptionRatio);
        }

        private bool IsOutOfValueArea()
        {
            double close = dataBars.Bar.Prices.Close;

            return close > _previousDataBar.AutoVolumeProfile.ValueAreaHigh || close < _previousDataBar.AutoVolumeProfile.ValueAreaLow;
        }

        private bool HasValidVolumeProfile()
        {
            return (_previousDataBar.AutoVolumeProfile.ValueAreaHigh - _previousDataBar.AutoVolumeProfile.ValueAreaLow) > 4;
        }

        private bool IsValidMinMaxDeltaRatio()
        {
            double ratio = dataBars.Bar.Deltas.MinMaxDeltaRatio;
            double minDelta = dataBars.Bar.Deltas.MinDelta;
            double maxDelta = dataBars.Bar.Deltas.MaxDelta;
            double deltaPercentage = dataBars.Bar.Deltas.DeltaPercentage;
            double previousMinDelta = _previousDataBar.Deltas.MinDelta;

            bool validMinDelta = minDelta < _totalPreviousMinDeltas + (_totalPreviousMinDeltas * .25);

            //return ratio > 5 && minDelta < previousMinDelta && deltaPercentage < -10;
            // 2
            return ratio > 5 && deltaPercentage < -10 && validMinDelta;
        }

        private bool IsValidMaxMinDeltaRatio()
        {
            double ratio = dataBars.Bar.Deltas.MaxMinDeltaRatio;
            double minDelta = dataBars.Bar.Deltas.MinDelta;
            double maxDelta = dataBars.Bar.Deltas.MaxDelta;
            double deltaPercentage = dataBars.Bar.Deltas.DeltaPercentage;
            double previousMaxDelta = _previousDataBar.Deltas.MaxDelta;

            bool validMaxDelta = maxDelta < _totalPreviousMaxDeltas + (_totalPreviousMaxDeltas * .25);

            //return ratio > 5 && maxDelta > previousMaxDelta && deltaPercentage > 10;
            // 2
            return ratio > 5 && deltaPercentage > 10 && validMaxDelta;
        }

        private bool IsValidAskRatio()
        {
            return dataBars.Bar.Ratios.HasValidAskExhaustionRatio || dataBars.Bar.Ratios.HasValidAskAbsorptionRatio;
        }

        private bool IsValidBidRatio()
        {
            return dataBars.Bar.Ratios.HasValidBidExhaustionRatio || dataBars.Bar.Ratios.HasValidBidAbsorptionRatio;
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
