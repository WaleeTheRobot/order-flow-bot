using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class TechnicalLevelsDataProviderMock
    {
        public static Mock<ITechnicalLevelsDataProvider> CreateTechnicalLevelsDataProvider()
        {
            var config = new TechnicalLevelsDataProviderData();

            var mock = new Mock<ITechnicalLevelsDataProvider>();
            mock.Setup(x => x.Ema).Returns(config.Ema);
            mock.Setup(x => x.Atr).Returns(config.Atr);

            return mock;
        }
    }
}
