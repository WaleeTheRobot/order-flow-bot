using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class CustomVolumetricBarMock
    {
        public static Mock<ICustomVolumetricBar> CreateCustomVolumetricBar()
        {
            var mock = new Mock<ICustomVolumetricBar>();
            mock.SetupGet(x => x.TotalVolume).Returns(VolumetricBarData.TotalVolume);
            mock.SetupGet(x => x.TotalBuyingVolume).Returns(VolumetricBarData.TotalBuyingVolume);
            mock.SetupGet(x => x.TotalSellingVolume).Returns(VolumetricBarData.TotalSellingVolume);
            //mock.SetupGet(x => x.ValueAreaHighPrice).Returns(0);
            //mock.SetupGet(x => x.ValueAreaLowPrice).Returns(0);
            mock.SetupGet(x => x.PointOfControl).Returns(VolumetricBarData.PointOfControl);
            mock.SetupGet(x => x.BarDelta).Returns(VolumetricBarData.BarDelta);
            mock.SetupGet(x => x.MinSeenDelta).Returns(VolumetricBarData.MinSeenDelta);
            mock.SetupGet(x => x.MaxSeenDelta).Returns(VolumetricBarData.MaxSeenDelta);
            mock.SetupGet(x => x.DeltaSh).Returns(VolumetricBarData.DeltaSh);
            mock.SetupGet(x => x.DeltaSl).Returns(VolumetricBarData.DeltaSl);
            mock.SetupGet(x => x.CumulativeDelta).Returns(VolumetricBarData.CumulativeDelta);
            mock.SetupGet(x => x.DeltaPercentage).Returns(VolumetricBarData.DeltaPercentage);
            mock.SetupGet(x => x.DeltaChange).Returns(VolumetricBarData.DeltaChange);
            mock.SetupGet(x => x.BidAskVolumes).Returns(VolumetricBarData.GetTestBarBidAskVolume());

            return mock;
        }
    }
}
