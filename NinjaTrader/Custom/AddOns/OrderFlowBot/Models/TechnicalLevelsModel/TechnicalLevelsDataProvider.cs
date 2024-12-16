using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel
{
    public class TechnicalLevelsDataProvider : ITechnicalLevelsDataProvider
    {
        public Ema Ema { get; set; }
        public Atr Atr { get; set; }

        public TechnicalLevelsDataProvider()
        {
            Ema = null;
            Atr = null;
        }
    }
}
