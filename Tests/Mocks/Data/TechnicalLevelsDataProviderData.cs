using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public static class TechnicalLevelsDataProviderData
    {
        public static Ema Ema { get; set; }

        static TechnicalLevelsDataProviderData()
        {
            Ema = new Ema
            {
                FastEma = 9,
                SlowEma = 20,
            };
        }
    }
}
