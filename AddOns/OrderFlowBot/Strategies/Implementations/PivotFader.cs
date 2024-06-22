using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    // Fade the pivot.
    // This is just an example strategy used to access the pivot points in teh technical levels class.
    public class PivotFader : StrategyBase
    {
        public override string Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }
        private OrderFlowBotDataBar _previousDataBar;
        private List<PivotPoint> _pivotPoints;
        private PivotPoint _currentPivot;
        private PivotPoint _currentPivot2;
        private readonly int _pivotCount;
        private readonly int _count;

        public PivotFader(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, string name, List<TechnicalLevels> technicalLevels)
        : base(orderFlowBotState, dataBars, name, technicalLevels)
        {
            _previousDataBar = new OrderFlowBotDataBar();
            _pivotPoints = new List<PivotPoint>();
            _currentPivot = new PivotPoint(dataBars.Bar.BarNumber, 0, 0, false);
            _currentPivot2 = new PivotPoint(dataBars.Bar.BarNumber, 0, 0, false);
            _pivotCount = 2;
            _count = 20;
        }

        public override void CheckStrategy()
        {
            // Make sure both data series have at least two pivots
            if (dataBars.Bars.Count < _count || (technicalLevels[0].Pivots.Count < _pivotCount && technicalLevels[1].Pivots.Count < _pivotCount))
            {
                return;
            }

            _previousDataBar = dataBars.Bars.Last();

            // This gets the last 2 pivot points from the list
            _pivotPoints = technicalLevels[0].Pivots.Skip(Math.Max(0, technicalLevels[0].Pivots.Count - _pivotCount)).Take(_pivotCount).ToList();
            // This is the current not complete pivot. This doesn't complete until after the next bar after it closes.
            _currentPivot = technicalLevels[0].CurrentPivot;
            // The current pivot from the second data series
            _currentPivot2 = technicalLevels[1].CurrentPivot;

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
            if (
                !IsLookingForHigh() &&
                IsLookingForHigh(1) &&
                IsValidBarType() &&
                IsValidBars() &&
                IsValidMaxDelta() &&
                IsValidMinDelta() &&
                IsValidPreviousLevel() &&
                IsValidCurrentPivot2()
                )
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        public override void CheckShort()
        {
            if (
                IsLookingForHigh() &&
                IsLookingForHigh(1) &&
                IsValidBarType(false) &&
                IsValidBars(false) &&
                IsValidMaxDelta(false) &&
                IsValidMinDelta(false) &&
                IsValidPreviousLevel(false) &&
                IsValidCurrentPivot2(false)
                )
            {
                ValidStrategyDirection = Direction.Short;
            }
        }

        private bool IsLookingForHigh(int dataSeries = 0)
        {
            // The IsLookingForHigh doesn't reset until the next bar closes
            // It should be looking for new high to fade short while price continues up
            // It should not be looking for new high to fade long while price continues down
            return technicalLevels[dataSeries].IsLookingForHigh;
        }

        private bool IsValidBarType(bool checkLong = true)
        {
            // The current bar should be bullish for long and bearish for short
            if (checkLong)
            {
                return dataBars.Bar.BarType == BarType.Bullish;
            }

            return dataBars.Bar.BarType == BarType.Bearish;
        }

        private bool IsValidBars(bool checkLong = true)
        {
            if (checkLong)
            {
                // The previous bar close - low is greater than 2 points. This shows potential rejection.
                // The current bar low is greater than the previous bar low. This shows potential reversal.
                // The current bar close is greater than the previous bar close + 1. This shows potential reversal continuation.
                bool isValidLongRejection = _previousDataBar.Prices.Close - _previousDataBar.Prices.Low > 2;
                bool isValidLongReversal = dataBars.Bar.Prices.Low > _previousDataBar.Prices.Low;
                bool isValidLongReversalContinuation = dataBars.Bar.Prices.Close > _previousDataBar.Prices.Close + 1;

                return isValidLongRejection && isValidLongReversal && isValidLongReversalContinuation;
            }

            // The previous bar high - close is greater than 2 points. This shows potential rejection.
            // The current bar high is less than the previous bar high. This shows potential reversal.
            // The current bar close is less than the previous bar close + 1. This shows potential reversal continuation.
            bool isValidShortRejection = _previousDataBar.Prices.High - _previousDataBar.Prices.Close > 2;
            bool isValidShortReversal = dataBars.Bar.Prices.High < _previousDataBar.Prices.High;
            bool isValidShortReversalContinuation = dataBars.Bar.Prices.Close < _previousDataBar.Prices.Close + 1;

            return isValidShortRejection && isValidShortReversal && isValidShortReversalContinuation;
        }

        private bool IsValidCurrentPivot2(bool checkLong = true)
        {
            if (checkLong)
            {
                // The current bar low is greater than the previous bar low. This shows potential reversal.
                // The current bar close is greater than the previous bar close + 1. This shows potential reversal continuation.
                bool isValidLongReversal = dataBars.Bar.Prices.Low > _previousDataBar.Prices.Low;
                bool isValidLongReversalContinuation = dataBars.Bar.Prices.Close > _previousDataBar.Prices.Close + 1;

                return isValidLongReversal && isValidLongReversalContinuation;
            }

            // The current bar high is less than the previous bar high. This shows potential reversal.
            // The current bar close is less than the previous bar close + 1. This shows potential reversal continuation.
            bool isValidShortReversal = dataBars.Bar.Prices.High < _previousDataBar.Prices.High;
            bool isValidShortReversalContinuation = dataBars.Bar.Prices.Close < _previousDataBar.Prices.Close + 1;

            return isValidShortReversal && isValidShortReversalContinuation;
        }

        private bool IsValidMaxDelta(bool checkLong = true)
        {
            if (checkLong)
            {
                return dataBars.Bar.Deltas.MaxDelta > 75;
            }

            return dataBars.Bar.Deltas.MaxDelta < 50;
        }

        private bool IsValidMinDelta(bool checkLong = true)
        {
            if (checkLong)
            {
                return dataBars.Bar.Deltas.MinDelta > -50;
            }

            return dataBars.Bar.Deltas.MinDelta < -75;
        }

        private bool IsValidPreviousLevel(bool checkLong = true)
        {
            if (checkLong)
            {
                // The previous bar low is greater than the last support level
                bool validPreviousLow = false;

                foreach (var pivot in _pivotPoints)
                {
                    if (!pivot.IsHigh)
                    {
                        validPreviousLow = _currentPivot.Price > pivot.Price;

                        // Example set for debugging
                        //technicalLevels[0].currentLow = _currentPivot.Price;
                        //technicalLevels[0].previousLow = pivot.Price;

                        break;
                    }
                }

                return validPreviousLow;
            }

            // The previous bar high is less than the last resistance level
            bool validPreviousHigh = false;

            foreach (var pivot in _pivotPoints)
            {
                if (pivot.IsHigh)
                {
                    validPreviousHigh = _currentPivot.Price < pivot.Price;

                    // Example set for debugging
                    //technicalLevels[0].currentHigh = _currentPivot.Price;
                    //technicalLevels[0].previousHigh = pivot.Price;

                    break;
                }
            }

            return validPreviousHigh;
        }
    }
}
