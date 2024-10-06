using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;

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

        static DataBarDataProviderData()
        {
            VolumetricBar = null;
            Time = 113330;
            CurrentBar = 48;
            BarsAgo = 0;
            High = 18549.75;
            Low = 18533;
            Open = 18549.75;
            Close = 18546.75;
        }
    }
}
