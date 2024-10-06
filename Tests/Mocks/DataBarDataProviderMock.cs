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

            mock.SetupProperty(dp => dp.Time, DataBarDataProviderData.Time);
            mock.SetupProperty(dp => dp.CurrentBar, DataBarDataProviderData.CurrentBar);
            mock.SetupProperty(dp => dp.BarsAgo, DataBarDataProviderData.BarsAgo);
            mock.SetupProperty(dp => dp.High, DataBarDataProviderData.High);
            mock.SetupProperty(dp => dp.Low, DataBarDataProviderData.Low);
            mock.SetupProperty(dp => dp.Open, DataBarDataProviderData.Open);
            mock.SetupProperty(dp => dp.Close, DataBarDataProviderData.Close);

            var mockVolumetricBar = CustomVolumetricBarMock.CreateCustomVolumetricBar().Object;
            mock.SetupProperty(dp => dp.VolumetricBar, mockVolumetricBar);

            return mock;
        }
    }
}
