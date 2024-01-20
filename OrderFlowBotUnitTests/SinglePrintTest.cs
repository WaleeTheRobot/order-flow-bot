using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;
using OrderFlowBotTestFiles;
using OrderFlowBotUnitTests.Data;

namespace OrderFlowBotUnitTests
{
    public class SinglePrintTest
    {
        private readonly SinglePrintData _data;

        public SinglePrintTest()
        {
            var _ = CommonData.DefaultBidAskVolumeList;
            _data = new SinglePrintData();
        }

        [Fact(DisplayName = "Ask single print should be valid")]
        public void ValidAskSinglePrint()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetValidAskSinglePrint();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetSinglePrints();

            Assert.True(dataBar.Volumes.HasAskSinglePrint);
        }

        [Fact(DisplayName = "Bid single print should be valid")]
        public void ValidBidSinglePrint()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetValidBidSinglePrint();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetSinglePrints();

            Assert.True(dataBar.Volumes.HasBidSinglePrint);
        }

        [Fact(DisplayName = "Ask single print should be invalid")]
        public void InvalidAskSinglePrint()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.bidAskVolumeListDefault;
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetSinglePrints();

            Assert.False(dataBar.Volumes.HasAskSinglePrint);
        }

        [Fact(DisplayName = "Bid single print should be invalid")]
        public void InvalidBidSinglePrint()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.bidAskVolumeListDefault;
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetSinglePrints();

            Assert.False(dataBar.Volumes.HasBidSinglePrint);
        }
    }
}