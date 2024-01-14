using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;
using OrderFlowBotTestFiles;
using OrderFlowBotTestFiles.Common;
using OrderFlowBotUnitTests.Data;

namespace OrderFlowBotUnitTests
{
    public class VolumeSequencingTest
    {
        private readonly VolumeData _data;

        public VolumeSequencingTest()
        {
            var _ = CommonData.DefaultBidAskVolumeList;
            _data = new VolumeData();
        }

        [Fact(DisplayName = "Ask volume sequencing should be valid")]
        public void ValidAskVolumeSequencing()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetValidAskVolumeSequencing();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetVolumeSequencing(bidAskVolumeList, BarType.Bullish, 500);

            Assert.True(dataBar.Volumes.HasAskVolumeSequencing);
        }

        [Fact(DisplayName = "Bid volume sequencing should be valid")]
        public void ValidBidVolumeSequencing()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetValidBidVolumeSequencing();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetVolumeSequencing(bidAskVolumeList, BarType.Bearish, 500);

            Assert.True(dataBar.Volumes.HasBidVolumeSequencing);
        }

        [Fact(DisplayName = "Ask volume sequencing should be invalid")]
        public void InValidAskVolumeSequencing()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.bidAskVolumeListDefault;
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetVolumeSequencing(bidAskVolumeList, BarType.Bullish, 500);

            Assert.False(dataBar.Volumes.HasAskVolumeSequencing);
        }

        [Fact(DisplayName = "Bid volume sequencing should be invalid")]
        public void InValidBidVolumeSequencing()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.bidAskVolumeListDefault;
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetVolumeSequencing(bidAskVolumeList, BarType.Bearish, 500);

            Assert.False(dataBar.Volumes.HasBidVolumeSequencing);
        }

        [Fact(DisplayName = "Bid volume sequencing should be invalid due to lack of volume")]
        public void InValidBidVolumeSequencingMinVolume()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.bidAskVolumeListDefault;
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetVolumeSequencing(bidAskVolumeList, BarType.Bearish, 499);

            Assert.False(dataBar.Volumes.HasBidVolumeSequencing);
            Assert.False(dataBar.Volumes.HasAskVolumeSequencing);
        }

        [Fact(DisplayName = "Bid ask volume list needs to be more than ValidVolumeSequencing number")]
        public void ListMoreThanValidVolumeSequencingNumber()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            List<BidAskVolume> bidAskVolumeList = _data.GetInValidBidAskTotalList();
            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetVolumeSequencing(bidAskVolumeList, BarType.Bearish, 600);

            Assert.False(dataBar.Volumes.HasBidVolumeSequencing);
            Assert.False(dataBar.Volumes.HasAskVolumeSequencing);
        }
    }
}