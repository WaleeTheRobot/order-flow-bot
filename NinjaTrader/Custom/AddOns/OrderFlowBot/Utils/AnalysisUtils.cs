using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Utils
{
    public static class AnalysisUtils
    {
        // Standardized measure of value's absolute position within a range, centered around zero.
        // Zero is the mid point of the range.
        // Normalizes -1 to 1
        public static double Normalize(double value, double min, double max, double tolerance = 1e-10)
        {
            double range = max - min;

            if (Math.Abs(range) < tolerance)
            {
                return 0;
            }

            double midpoint = (max + min) / 2.0;
            double halfRange = range / 2.0;

            return (value - midpoint) / halfRange;
        }

        // Relative measure of how far one specific price is from another reference price, scaled by the entire range.
        // Returns relative position 0 to 1, where 0 means price is very close to reference
        public static double GetRelativePosition(double price, double reference, double low, double high, double tolerance = 1e-10)
        {
            double range = high - low;
            if (range <= tolerance)
            {
                return 0;
            }

            // Just return the normalized distance
            return Math.Abs(price - reference) / range;
        }
    }
}
