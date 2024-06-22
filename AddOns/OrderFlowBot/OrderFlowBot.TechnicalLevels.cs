using PivotPoint = NinjaTrader.Custom.AddOns.OrderFlowBot.PivotPoint;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private void UpdateSupportResistanceLevels(int dataSeries = 0)
        {
            if (CurrentBars[dataSeries] < BarsRequiredToPlot) return;

            int previousBarNumber = CurrentBars[dataSeries] - 1;
            double previousOpen = Opens[dataSeries][1];
            double previousClose = Closes[dataSeries][1];
            double previousHigh = Highs[dataSeries][1];
            double previousLow = Lows[dataSeries][1];

            if (!_technicalLevels[dataSeries].HasFirstPivot)
            {
                if (previousOpen <= previousClose)
                {
                    // Set first pivot as low
                    _technicalLevels[dataSeries].CurrentPivot = new PivotPoint(previousBarNumber, previousLow, previousClose, false);
                    _technicalLevels[dataSeries].Pivots.Add(_technicalLevels[dataSeries].CurrentPivot);
                }
                else
                {
                    // Set first pivot as high
                    _technicalLevels[dataSeries].CurrentPivot = new PivotPoint(previousBarNumber, previousHigh, previousClose, true);
                    _technicalLevels[dataSeries].Pivots.Add(_technicalLevels[dataSeries].CurrentPivot);
                    _technicalLevels[dataSeries].IsLookingForHigh = false;
                }

                _technicalLevels[dataSeries].HasFirstPivot = true;
                return;
            }

            // Check level tested
            foreach (var pivot in _technicalLevels[dataSeries].Pivots)
            {
                if (!pivot.DisplayLevel)
                {
                    continue;
                }

                // The price is within the threshold and considered as tested
                // Check for resistance
                if (pivot.IsHigh && previousHigh >= pivot.Price && previousHigh <= pivot.Price + _technicalLevels[dataSeries].RequiredTicksForBroken)
                {
                    pivot.IsLevelTested = true;
                }
                // Check for support
                else if (!pivot.IsHigh && previousLow <= pivot.Price && previousLow >= pivot.Price - _technicalLevels[dataSeries].RequiredTicksForBroken)
                {
                    pivot.IsLevelTested = true;
                }
                // The price is outside of the threshold and considered as broken
                // Check for resistance
                else if (pivot.IsHigh && previousHigh > pivot.Price + _technicalLevels[dataSeries].RequiredTicksForBroken)
                {
                    pivot.IsLevelBroken = true;
                    pivot.DisplayLevel = false;
                }
                // Check for support
                else if (!pivot.IsHigh && previousLow < pivot.Price - _technicalLevels[dataSeries].RequiredTicksForBroken)
                {
                    pivot.IsLevelBroken = true;
                    pivot.DisplayLevel = false;
                }
            }

            if (_technicalLevels[dataSeries].IsLookingForHigh)
            {
                // Update the high pivot until a lower high is found
                if (previousHigh > _technicalLevels[dataSeries].CurrentPivot.Price)
                {
                    _technicalLevels[dataSeries].CurrentPivot.BarNumber = previousBarNumber;
                    _technicalLevels[dataSeries].CurrentPivot.Price = previousHigh;
                    _technicalLevels[dataSeries].CurrentPivot.Close = previousClose;
                }
                else if (previousHigh < _technicalLevels[dataSeries].CurrentPivot.Price)
                {
                    // High pivot found and switch to looking for a low pivot
                    _technicalLevels[dataSeries].Pivots.Add(new PivotPoint(_technicalLevels[dataSeries].CurrentPivot.BarNumber, _technicalLevels[dataSeries].CurrentPivot.Price, _technicalLevels[dataSeries].CurrentPivot.Close, true));
                    _technicalLevels[dataSeries].CurrentPivot = new PivotPoint(previousBarNumber, previousLow, previousClose, false);
                    _technicalLevels[dataSeries].IsLookingForHigh = false;
                }
            }
            else
            {
                // Update the low pivot until a higher low is found
                if (previousLow < _technicalLevels[dataSeries].CurrentPivot.Price)
                {
                    _technicalLevels[dataSeries].CurrentPivot.BarNumber = previousBarNumber;
                    _technicalLevels[dataSeries].CurrentPivot.Price = previousLow;
                    _technicalLevels[dataSeries].CurrentPivot.Close = previousClose;
                }
                else if (previousLow > _technicalLevels[dataSeries].CurrentPivot.Price)
                {
                    // Low pivot found and switch to looking for a high pivot
                    _technicalLevels[dataSeries].Pivots.Add(new PivotPoint(_technicalLevels[dataSeries].CurrentPivot.BarNumber, _technicalLevels[dataSeries].CurrentPivot.Price, _technicalLevels[dataSeries].CurrentPivot.Close, false));
                    _technicalLevels[dataSeries].CurrentPivot = new PivotPoint(previousBarNumber, previousHigh, previousClose, true);
                    _technicalLevels[dataSeries].IsLookingForHigh = true;
                }
            }
        }
    }
}
