using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel
{
    public class TechnicalLevels : IReadOnlyTechnicalLevels
    {
        public ITechnicalLevelsDataProvider TechnicalLevelsDataProvider { get; private set; }
        public int BarNumber { get; set; }
        public Ema Ema { get; set; }

        public TechnicalLevels()
        {
            TechnicalLevelsDataProvider = new TechnicalLevelsDataProvider();
            Ema = new Ema();
        }

        public void SetCurrentTechnicalIndicators(ITechnicalLevelsDataProvider technicalLevelsDataProvider)
        {
            TechnicalLevelsDataProvider = technicalLevelsDataProvider;

            PopulateEma();
        }

        private void PopulateEma()
        {
            Ema.FastEma = TechnicalLevelsDataProvider.Ema.FastEma;
            Ema.SlowEma = TechnicalLevelsDataProvider.Ema.SlowEma;
        }
    }
}
