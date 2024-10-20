using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class DataBarDataProviderMock
    {
        public static Mock<IDataBarDataProvider> CreateDataBarDataProvider()
        {
            var mock = new Mock<IDataBarDataProvider>();

            mock.SetupProperty(x => x.Time, DataBarDataProviderData.Time);
            mock.SetupProperty(x => x.CurrentBar, DataBarDataProviderData.CurrentBar);
            mock.SetupProperty(x => x.BarsAgo, DataBarDataProviderData.BarsAgo);
            mock.SetupProperty(x => x.High, DataBarDataProviderData.High);
            mock.SetupProperty(x => x.Low, DataBarDataProviderData.Low);
            mock.SetupProperty(x => x.Open, DataBarDataProviderData.Open);
            mock.SetupProperty(x => x.Close, DataBarDataProviderData.Close);

            var mockVolumetricBar = CustomVolumetricBarMock.CreateCustomVolumetricBar().Object;
            mock.SetupProperty(x => x.VolumetricBar, mockVolumetricBar);

            return mock;
        }
    }
}
