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
            _previousDataBar = dataBars.Bars.Last();

            if (IsValidMaxMinDeltaRatio() && IsValidBidRatio() && IsBullishBar())
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        public override void CheckShort()
        {
            _previousDataBar = dataBars.Bars.Last();

            if (IsValidMinMaxDeltaRatio() && IsValidAskRatio() && IsBearishBar())
            {
                ValidStrategyDirection = Direction.Short;
            }
        }

        private bool IsBelowAutoVolumeProfileVAH()
        {
            return dataBars.Bar.Prices.Close > _previousDataBar.AutoVolumeProfile.ValueAreaHigh;
        }

        private bool IsBelowAutoVolumeProfileVAL()
        {
            return dataBars.Bar.Prices.Close < _previousDataBar.AutoVolumeProfile.ValueAreaLow;
        }

        private bool IsValidMinMaxDeltaRatio()
        {
            double ratio = dataBars.Bar.Deltas.MinMaxDeltaRatio;
            double minDelta = dataBars.Bar.Deltas.MinDelta;
            double maxDelta = dataBars.Bar.Deltas.MaxDelta;
            double deltaPercentage = dataBars.Bar.Deltas.DeltaPercentage;
            double previousMinDelta = _previousDataBar.Deltas.MinDelta;

            bool validMinDelta = minDelta < _totalPreviousMinDeltas + (_totalPreviousMinDeltas * .25);

            // 1 return ratio > 5 && minDelta < previousMinDelta && deltaPercentage < -10;
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

            // 1 return ratio > 5 && maxDelta > previousMaxDelta && deltaPercentage > 10;
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
