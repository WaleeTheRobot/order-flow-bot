using OrderFlowBotTestFiles;
using OrderFlowBotUnitTests.Data;

namespace OrderFlowBotUnitTests
{
    public class RatiosTest
    {
        private readonly RatiosData _data;

        public RatiosTest()
        {
            _data = new RatiosData();
        }

        [Fact(DisplayName = "Bid Ask Ratio should be correctly calculated")]
        public void BidAskRatio()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            dataBar.Volumes.BidAskVolumes = _data.GetBidAskRatio();
            dataBar.Ratios.SetRatios(_data.GetBidAskRatio(), true);

            Assert.Equal(40.2, dataBar.Ratios.AskRatio);
            Assert.Equal(15, dataBar.Ratios.BidRatio);
        }

        [Fact(DisplayName = "Bid Ask Ratio should be correctly calculated when high or low is zero")]
        public void BidAskRatioZero()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            dataBar.Volumes.BidAskVolumes = _data.GetBidAskRatioZero();
            dataBar.Ratios.SetRatios(_data.GetBidAskRatioZero(), true);

            Assert.Equal(201, dataBar.Ratios.AskRatio);
            Assert.Equal(150, dataBar.Ratios.BidRatio);
        }

        [Fact(DisplayName = "HasValid Ask/Bid Exhuastion Ratios should be valid")]
        public void HasBidAskRatioValid()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            dataBar.Volumes.BidAskVolumes = _data.GetValidBidAskRatio();
            dataBar.Ratios.SetRatios(_data.GetValidBidAskRatio(), true);

            Assert.True(dataBar.Ratios.HasValidAskExhaustionRatio);
            Assert.True(dataBar.Ratios.HasValidBidExhaustionRatio);
        }

        [Fact(DisplayName = "HasValid Ask/Bid Exhuastion Ratios should be invalid")]
        public void HasBidAskRatioInvalid()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            dataBar.Volumes.BidAskVolumes = _data.GetInValidBidAskRatio();
            dataBar.Ratios.SetRatios(_data.GetInValidBidAskRatio(), false);

            Assert.False(dataBar.Ratios.HasValidAskExhaustionRatio);
            Assert.False(dataBar.Ratios.HasValidBidExhaustionRatio);
        }
    }
}