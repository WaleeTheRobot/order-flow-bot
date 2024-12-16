using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Utils;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TradeAnalysis.StackedImbalances
{
    public class SignalBar
    {
        public int BarTypeTraits { get; set; }
        public double OpenTraits { get; set; }
        public double CloseTraits { get; set; }
        public double Atr { get; set; }

        public SignalBar()
        {
            BarTypeTraits = (int)BarType.Flat;
            OpenTraits = 0;
            CloseTraits = 0;
            Atr = 0;
        }

        public void Update(IReadOnlyDataBar dataBar, IReadOnlyTechnicalLevels technicalLevels, SignalBarType signalBarType)
        {
            double high = signalBarType == SignalBarType.DataBar ? dataBar.Prices.High : dataBar.CumulativeDeltaBar.High;
            double low = signalBarType == SignalBarType.DataBar ? dataBar.Prices.Low : dataBar.CumulativeDeltaBar.Low;
            double close = signalBarType == SignalBarType.DataBar ? dataBar.Prices.Close : dataBar.CumulativeDeltaBar.Close;
            double open = signalBarType == SignalBarType.DataBar ? dataBar.Prices.Open : dataBar.CumulativeDeltaBar.Open;

            BarTypeTraits = (int)dataBar.BarType;

            // Measure how open is relative to bar range
            OpenTraits = AnalysisUtils.Normalize(open, low, high);
            // Measure how close is relative to bar range
            CloseTraits = AnalysisUtils.Normalize(close, low, high);
            Atr = technicalLevels.Atr.Value;
        }
    }
}
