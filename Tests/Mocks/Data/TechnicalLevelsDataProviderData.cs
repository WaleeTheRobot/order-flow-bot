using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public class TechnicalLevelsDataProviderData : ITechnicalLevelsDataProvider
    {
        public Ema Ema { get; set; }
        public Atr Atr { get; set; }

        public TechnicalLevelsDataProviderData()
        {
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
