using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel
{
    public class TechnicalLevels : IReadOnlyTechnicalLevels
    {
        public ITechnicalLevelsDataProvider TechnicalLevelsDataProvider { get; private set; }
        public Ema Ema { get; set; }
        public Atr Atr { get; set; }

        public TechnicalLevels()
        {
            TechnicalLevelsDataProvider = new TechnicalLevelsDataProvider();
            Ema = new Ema();
            Atr = new Atr();
        }

        public void SetCurrentTechnicalIndicators(ITechnicalLevelsDataProvider technicalLevelsDataProvider)
        {
            TechnicalLevelsDataProvider = technicalLevelsDataProvider;

            PopulateEma();
            PopulateAtr();
        }

        private void PopulateEma()
        {
            Ema.FastEma = TechnicalLevelsDataProvider.Ema.FastEma;
            Ema.SlowEma = TechnicalLevelsDataProvider.Ema.SlowEma;
        }

        private void PopulateAtr()
        {
            Atr.Value = TechnicalLevelsDataProvider.Atr.Value;
        }
    }
}
