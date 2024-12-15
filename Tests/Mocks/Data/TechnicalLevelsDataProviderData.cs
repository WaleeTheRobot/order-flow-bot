using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public static class TechnicalLevelsDataProviderData
    {
        public static int BarNumber { get; set; }
        public static Ema Ema { get; set; }
        public static Atr Atr { get; set; }

        static TechnicalLevelsDataProviderData()
        {
            BarNumber = 99;
            Ema = new Ema
            {
                FastEma = 9,
                SlowEma = 20,
            };
            Atr = new Atr
            {
                Value = 16,
            };
        }
    }
}
