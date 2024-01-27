using OrderFlowBotTestFiles;
using OrderFlowBotUnitTests.Data;

namespace OrderFlowBotUnitTests
{
    public class BidAskVolumeTest
    {
        public BidAskVolumeTest()
        {
        }

        [Fact(DisplayName = "Volume and VolumeDelta should be valid")]
        public void ValidBidAskVolume()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            dataBar.Volumes.BidAskVolumes = CommonData.DefaultBidAskVolumeList;
            dataBar.Volumes.SetBidAskPriceVolumeAndVolumeDelta();


            Assert.Equal(26, dataBar.Volumes.BidAskVolumes[0].Volume);
            Assert.Equal(-16, dataBar.Volumes.BidAskVolumes[0].VolumeDelta);
            Assert.Equal(278, dataBar.Volumes.BidAskVolumes[1].Volume);
            Assert.Equal(124, dataBar.Volumes.BidAskVolumes[1].VolumeDelta);
            Assert.Equal(322, dataBar.Volumes.BidAskVolumes[2].Volume);
            Assert.Equal(-30, dataBar.Volumes.BidAskVolumes[2].VolumeDelta);
            Assert.Equal(267, dataBar.Volumes.BidAskVolumes[3].Volume);
            Assert.Equal(7, dataBar.Volumes.BidAskVolumes[3].VolumeDelta);
            Assert.Equal(103, dataBar.Volumes.BidAskVolumes[4].Volume);
            Assert.Equal(-13, dataBar.Volumes.BidAskVolumes[4].VolumeDelta);
            Assert.Equal(175, dataBar.Volumes.BidAskVolumes[5].Volume);
            Assert.Equal(-125, dataBar.Volumes.BidAskVolumes[5].VolumeDelta);
            Assert.Equal(5, dataBar.Volumes.BidAskVolumes[6].Volume);
            Assert.Equal(-5, dataBar.Volumes.BidAskVolumes[6].VolumeDelta);
        }
    }
}