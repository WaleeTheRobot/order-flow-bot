using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public class ImbalanceRatioVolumeProfile : StrategyBase
    {
        public ImbalanceRatioVolumeProfile(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars)
        : base(orderFlowBotState, dataBars)
        {
        }

        public override void CheckStrategy()
        {
            if (IsValidLongDirection())
            {
                CheckLong();
            }

            if (IsValidShortDirection())
            {
                CheckShort();
            }
        }

        public override void CheckLong()
        {
            if (IsLowBelowVAL() && IsValidBidRatio() && IsBullishBar())
            {
                orderFlowBotState.ValidStrategy = OrderFlowBotValidStrategy.ImbalanceRatioVolumeProfileLong;
            }
        }

        public override void CheckShort()
        {
            if (IsHighAboveVAH() && IsValidAskRatio() && IsBearishBar())
            {
                orderFlowBotState.ValidStrategy = OrderFlowBotValidStrategy.ImbalanceRatioVolumeProfileShort;
            }
        }

        private bool IsLowBelowVAL()
        {
            return dataBars.Bar.Prices.Low < dataBars.Bar.AutoVolumeProfile.ValueAreaLow;
        }

        private bool IsHighAboveVAH()
        {
            return dataBars.Bar.Prices.High > dataBars.Bar.AutoVolumeProfile.ValueAreaHigh;
        }

        private bool IsValidAskRatio()
        {
            return dataBars.Bar.Ratios.HasValidAskRatio;
        }

        private bool IsValidBidRatio()
        {
            return dataBars.Bar.Ratios.HasValidBidRatio;
        }

        private bool IsBullishBar()
        {
            return dataBars.Bar.BarType == BarType.Bullish;
        }

        private bool IsBearishBar()
        {
            return dataBars.Bar.BarType == BarType.Bearish;
        }
    }
}
