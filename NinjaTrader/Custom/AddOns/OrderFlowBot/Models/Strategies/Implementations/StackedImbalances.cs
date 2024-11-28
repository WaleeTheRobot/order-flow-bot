using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies.Implementations
{
    // Example strategy showing how to use current, previous bar and technical levels
    public class StackedImbalances : StrategyBase
    {
        private readonly int _validBarCount;
        private readonly int _validVolume;
        private readonly int _validDelta;

        public StackedImbalances(EventsContainer eventsContainer) : base(eventsContainer)
        {
            StrategyData.Name = "Stacked Imbalances";

            _validBarCount = 2;
            _validVolume = 500;
            _validDelta = 100;
        }

        public override bool CheckLong()
        {
            return
                IsValidBarCount() &&
                IsValidVolume() &&
                IsValidDelta(true) &&
                IsValidCurrentBar(true) &&
                IsValidOneBarAgoBar(true) &&
                HasValidStackedImbalances(true) &&
                IsValidEma(true);
        }

        public override bool CheckShort()
        {
            return
                IsValidBarCount() &&
                IsValidVolume() &&
                IsValidDelta() &&
                IsValidCurrentBar() &&
                IsValidOneBarAgoBar() &&
                HasValidStackedImbalances() &&
                IsValidEma();
        }

        private bool IsValidBarCount()
        {
            return dataBars.Count > _validBarCount;
        }

        private bool IsValidVolume()
        {
            return currentDataBar.Volumes.Volume > _validVolume;
        }

        private bool IsValidDelta(bool isLong = false)
        {
            long currentDelta = currentDataBar.Deltas.Delta;

            return isLong ? currentDelta > _validDelta : currentDelta < (_validDelta * -1);
        }

        private bool IsValidCurrentBar(bool isLong = false)
        {
            BarType currentBarType = currentDataBar.BarType;

            return isLong ? currentBarType == BarType.Bullish : currentBarType == BarType.Bearish;
        }

        private bool IsValidOneBarAgoBar(bool isLong = false)
        {
            IReadOnlyDataBar oneBarAgo = dataBars[dataBars.Count - 1];
            BarType oneBarAgoBarType = oneBarAgo.BarType;

            return isLong ? oneBarAgoBarType == BarType.Bullish : oneBarAgoBarType == BarType.Bearish;
        }

        private bool HasValidStackedImbalances(bool isLong = false)
        {
            return isLong ? currentDataBar.Imbalances.HasAskStackedImbalances : currentDataBar.Imbalances.HasBidStackedImbalances;
        }

        private bool IsValidEma(bool isLong = false)
        {
            double currentPrice = currentDataBar.Prices.Close;
            double currentFastEma = currentTechnicalLevels.Ema.FastEma;

            return isLong ? currentPrice > currentFastEma : currentPrice < currentFastEma;
        }
    }
}
