using PivotPoint = NinjaTrader.Custom.AddOns.OrderFlowBot.PivotPoint;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private void UpdateSupportResistanceLevels()
        {
            if (CurrentBar < BarsRequiredToPlot) return;

            if (!IsFirstTickOfBar) return;

            int previousBarNumber = CurrentBar - 1;
            double previousOpen = Open[1];
            double previousClose = Close[1];
            double previousHigh = High[1];
            double previousLow = Low[1];

            if (!_technicalLevels.HasFirstPivot)
            {
                if (previousOpen <= previousClose)
                {
                    // Set first pivot as low
                    _technicalLevels.CurrentPivot = new PivotPoint(previousBarNumber, previousLow, previousClose, false);
                    _technicalLevels.Pivots.Add(_technicalLevels.CurrentPivot);
                }
                else
                {
                    // Set first pivot as high
                    _technicalLevels.CurrentPivot = new PivotPoint(previousBarNumber, previousHigh, previousClose, true);
                    _technicalLevels.Pivots.Add(_technicalLevels.CurrentPivot);
                    _technicalLevels.IsLookingForHigh = false;
                }

                _technicalLevels.HasFirstPivot = true;
                return;
            }

            // Check level tested
            foreach (var pivot in _technicalLevels.Pivots)
            {
                if (!pivot.DisplayLevel)
                {
                    continue;
                }

                // The price is within the threshold and considered as tested
                // Check for resistance
                if (pivot.IsHigh && previousHigh >= pivot.Price && previousHigh <= pivot.Price + _technicalLevels.RequiredTicksForBroken)
                {
                    pivot.IsLevelTested = true;
                }
                // Check for support
                else if (!pivot.IsHigh && previousLow <= pivot.Price && previousLow >= pivot.Price - _technicalLevels.RequiredTicksForBroken)
                {
                    pivot.IsLevelTested = true;
                }
                // The price is outside of the threshold and considered as broken
                // Check for resistance
                else if (pivot.IsHigh && previousHigh > pivot.Price + _technicalLevels.RequiredTicksForBroken)
                {
                    pivot.IsLevelBroken = true;
                    pivot.DisplayLevel = false;
                }
                // Check for support
                else if (!pivot.IsHigh && previousLow < pivot.Price - _technicalLevels.RequiredTicksForBroken)
                {
                    pivot.IsLevelBroken = true;
                    pivot.DisplayLevel = false;
                }
            }

            if (_technicalLevels.IsLookingForHigh)
            {
                // Update the high pivot until a lower high is found
                if (previousHigh > _technicalLevels.CurrentPivot.Price)
                {
                    _technicalLevels.CurrentPivot.BarNumber = previousBarNumber;
                    _technicalLevels.CurrentPivot.Price = previousHigh;
                    _technicalLevels.CurrentPivot.Close = previousClose;
                }
                else if (previousHigh < _technicalLevels.CurrentPivot.Price)
                {
                    // High pivot found and switch to looking for a low pivot
                    _technicalLevels.Pivots.Add(new PivotPoint(_technicalLevels.CurrentPivot.BarNumber, _technicalLevels.CurrentPivot.Price, _technicalLevels.CurrentPivot.Close, true));
                    _technicalLevels.CurrentPivot = new PivotPoint(previousBarNumber, previousLow, previousClose, false);
                    _technicalLevels.IsLookingForHigh = false;
                }
            }
            else
            {
                // Update the low pivot until a higher low is found
                if (previousLow < _technicalLevels.CurrentPivot.Price)
                {
                    _technicalLevels.CurrentPivot.BarNumber = previousBarNumber;
                    _technicalLevels.CurrentPivot.Price = previousLow;
                    _technicalLevels.CurrentPivot.Close = previousClose;
                }
                else if (previousLow > _technicalLevels.CurrentPivot.Price)
                {
                    // Low pivot found and switch to looking for a high pivot
                    _technicalLevels.Pivots.Add(new PivotPoint(_technicalLevels.CurrentPivot.BarNumber, _technicalLevels.CurrentPivot.Price, _technicalLevels.CurrentPivot.Close, false));
                    _technicalLevels.CurrentPivot = new PivotPoint(previousBarNumber, previousHigh, previousClose, true);
                    _technicalLevels.IsLookingForHigh = true;
                }
            }
        }
    }
}
