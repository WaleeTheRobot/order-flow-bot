using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;
using OrderFlowBotTestFiles;
using OrderFlowBotTestFiles.Common;
using OrderFlowBotUnitTests.Data;

namespace OrderFlowBotUnitTests
{
    public class RatiosTest
    {
        private readonly RatiosData _data;

        public RatiosTest()
        {
            var _ = CommonData.DefaultBidAskVolumeList;
            _data = new RatiosData();
        }

        [Fact(DisplayName = "Bid Ask Ratio should be correctly calculated")]
        public void BidAskRatio()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetBidAskRatio();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Ratios.SetRatios(bidAskVolumeList, true, BarType.Bearish);

            Assert.Equal(40.2, dataBar.Ratios.AskRatio);
            Assert.Equal(15, dataBar.Ratios.BidRatio);
        }

        [Fact(DisplayName = "Bid Ask Ratio should be correctly calculated when high or low is zero")]
        public void BidAskRatioZero()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetBidAskRatioZero();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Ratios.SetRatios(bidAskVolumeList, true, BarType.Bearish);

            Assert.Equal(201, dataBar.Ratios.AskRatio);
            Assert.Equal(150, dataBar.Ratios.BidRatio);
        }

        [Fact(DisplayName = "Has Valid Bid Exhaustion Ratio should be valid")]
        public void HasValidBidExhaustionRatio()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetValidBidAskExhaustion();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Ratios.SetRatios(bidAskVolumeList, true, BarType.Bullish);

            Assert.True(dataBar.Ratios.HasValidBidExhaustionRatio);
        }

        [Fact(DisplayName = "Has Valid Ask Exhaustion Ratio should be valid")]
        public void HasValidAskExhaustionRatio()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetValidBidAskExhaustion();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Ratios.SetRatios(bidAskVolumeList, true, BarType.Bearish);

            Assert.True(dataBar.Ratios.HasValidAskExhaustionRatio);
        }

        [Fact(DisplayName = "Has Valid Bid Absorption Ratio should be valid")]
        public void HasValidBidAborptionRatio()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetValidBidAskAbsorption();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Ratios.SetRatios(bidAskVolumeList, true, BarType.Bullish);

            Assert.True(dataBar.Ratios.HasValidBidAbsorptionRatio);
        }

        [Fact(DisplayName = "Has Valid Ask Absorption Ratio should be valid")]
        public void HasValidAskAborptionRatio()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetValidBidAskAbsorption();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Ratios.SetRatios(bidAskVolumeList, true, BarType.Bearish);

            Assert.True(dataBar.Ratios.HasValidAskAbsorptionRatio);
        }
    }
}