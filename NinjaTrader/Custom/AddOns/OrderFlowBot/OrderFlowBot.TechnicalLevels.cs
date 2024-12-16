using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;
using System;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private TechnicalLevelsDataProvider _technicalLevelsDataProvider;

        private void InitializeTechnicalLevels()
        {
            _technicalLevelsDataProvider = new TechnicalLevelsDataProvider();
        }

        private ITechnicalLevelsDataProvider GetTechnicalLevelsDataProvider(int barsAgo = 0)
        {
            _technicalLevelsDataProvider.Ema = GetEma(barsAgo);
            _technicalLevelsDataProvider.Atr = GetAtr(barsAgo);

            return _technicalLevelsDataProvider;
        }

        private Ema GetEma(int barsAgo)
        {
            var ema = new Ema
            {
                FastEma = Math.Round(EMA(9)[barsAgo], 2),
                SlowEma = Math.Round(EMA(20)[barsAgo], 2)
            };

            return ema;
        }

        private Atr GetAtr(int barsAgo)
        {
            var atr = new Atr
            {
                Value = Math.Round(ATR(BarsArray[_dataSeriesIndexMap["Atr"]], 9)[barsAgo], 2)
            };

            return atr;
        }
    }
}
