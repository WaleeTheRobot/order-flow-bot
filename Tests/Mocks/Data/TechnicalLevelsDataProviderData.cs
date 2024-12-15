using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public class TechnicalLevelsDataProviderData
    {
        public int BarNumber { get; set; }
        public Ema Ema { get; set; }
        public Atr Atr { get; set; }

        public TechnicalLevelsDataProviderData()
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
