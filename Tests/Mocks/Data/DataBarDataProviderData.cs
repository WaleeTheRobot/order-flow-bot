using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public static class DataBarDataProviderData
    {
        public static CustomVolumetricBar VolumetricBar { get; set; }
        public static int Time { get; set; }
        public static int CurrentBar { get; set; }
        public static int BarsAgo { get; set; }
        public static double High { get; set; }
        public static double Low { get; set; }
        public static double Open { get; set; }
        public static double Close { get; set; }
        public static CumulativeDeltaBar CumulativeDeltaBar { get; set; }

        static DataBarDataProviderData()
        {
            VolumetricBar = null;
            Time = 102524;
            CurrentBar = 47;
            BarsAgo = 0;
            High = 18697.25;
            Low = 18667.25;
            Open = 18692.50;
            Close = 18667.25;
            CumulativeDeltaBar = new CumulativeDeltaBar
            {
                High = 10,
                Low = 1,
                Open = 5,
                Close = 7
            };
        }
    }
}
