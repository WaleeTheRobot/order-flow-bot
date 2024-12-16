using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Utils
{
    public static class BarUtils
    {
        public static double CalculateRatio(double numerator, double denominator)
        {
            if (numerator == 0 && denominator == 0)
            {
                return 0;
            }
            if (denominator == 0)
            {
                return Math.Round(numerator, 2);
            }

            return numerator / denominator;
        }
    }
}
