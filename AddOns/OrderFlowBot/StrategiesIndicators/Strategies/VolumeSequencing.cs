using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.StrategiesIndicators.Strategies
{
    public class VolumeSequencing : StrategyBase
    {
        public override string Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }

        public VolumeSequencing(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, string name)
        : base(orderFlowBotState, dataBars, name)
        {
            // This can be used to initialize other values.
        }

        public override void CheckStrategy()
        {
            if (IsValidLongDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckLong();
            }

            if (IsValidShortDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckShort();
            }
        }

        // Has sequential ask volume from last ValidVolumeSequencing starting from the bottom.
        public override void CheckLong()
        {
            if (IsBullishBar() && HasAskVolumeSequencing())
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        // Has sequential bid volume from first ValidVolumeSequencing starting from the top.
        public override void CheckShort()
        {
            if (IsBearishBar() && HasBidVolumeSequencing())
            {
                ValidStrategyDirection = Direction.Short;
            }
        }

        private bool IsBullishBar()
        {
            return dataBars.Bar.BarType == BarType.Bullish;
        }

        private bool IsBearishBar()
        {
            return dataBars.Bar.BarType == BarType.Bearish;
        }

        private bool HasAskVolumeSequencing()
        {
            return dataBars.Bar.Volumes.HasAskVolumeSequencing;
        }

        private bool HasBidVolumeSequencing()
        {
            return dataBars.Bar.Volumes.HasBidVolumeSequencing;
        }
    }
}