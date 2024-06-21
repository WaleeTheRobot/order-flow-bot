using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot
{
    public class PivotPoint
    {
        public int BarNumber { get; set; }
        public double Price { get; set; }
        public double Close { get; set; }
        public bool IsHigh { get; set; }
        public bool DisplayLevel { get; set; }
        public bool IsLevelTested { get; set; }
        public bool IsLevelBroken { get; set; }

        public PivotPoint(int barNumber, double price, double close, bool isHigh)
        {
            BarNumber = barNumber;
            Price = price;
            Close = close;
            IsHigh = isHigh;
            DisplayLevel = true;
            IsLevelTested = false;
            IsLevelBroken = false;
        }
    }

    public class TechnicalLevels
    {
        public List<PivotPoint> Pivots;
        public PivotPoint CurrentPivot;
        public bool IsLookingForHigh;
        public bool HasFirstPivot;
        public double RequiredTicksForBroken;

        // You can set these in your strategies to print in the main program for debugging.
        public double currentLow;
        public double previousLow;
        public double previousHigh;
        public double currentHigh;

        public TechnicalLevels(int currentBar, double requiredTicksForBroken)
        {
            Pivots = new List<PivotPoint>();
            CurrentPivot = new PivotPoint(currentBar, 0, 0, false);
            IsLookingForHigh = true;
            HasFirstPivot = false;
            RequiredTicksForBroken = requiredTicksForBroken;

            currentLow = 0;
            previousLow = 0;
            previousHigh = 0;
            currentHigh = 0;
        }
    }
}
