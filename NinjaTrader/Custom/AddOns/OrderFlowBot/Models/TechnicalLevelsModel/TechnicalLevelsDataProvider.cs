using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel
{
    public class TechnicalLevelsDataProvider : ITechnicalLevelsDataProvider
    {
        public int BarNumber { get; set; }
        public Ema Ema { get; set; }
        public Atr Atr { get; set; }

        public TechnicalLevelsDataProvider()
        {
            BarNumber = 0;
            Ema = null;
            Atr = null;
        }
    }
}
