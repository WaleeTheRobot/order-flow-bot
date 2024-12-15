using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class TechnicalLevelsDataProviderMock
    {
        public static Mock<TechnicalLevelsDataProviderData> CreateTechnicalLevelsDataProvider()
        {
            var config = new TechnicalLevelsDataProviderData();
            var mock = new Mock<TechnicalLevelsDataProviderData>();

            mock.SetupGet(x => x.BarNumber).Returns(config.BarNumber);
            mock.SetupGet(x => x.Ema).Returns(config.Ema);
            mock.SetupGet(x => x.Atr).Returns(config.Atr);

            return mock;
        }
    }
}
