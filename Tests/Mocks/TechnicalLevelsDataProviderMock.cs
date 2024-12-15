using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class TechnicalLevelsDataProviderMock
    {
        public static Mock<ITechnicalLevelsDataProvider> CreateTechnicalLevelsDataProvider()
        {
            var mock = new Mock<ITechnicalLevelsDataProvider>();

            mock.SetupProperty(x => x.BarNumber, TechnicalLevelsDataProviderData.BarNumber);
            mock.SetupProperty(x => x.Ema, TechnicalLevelsDataProviderData.Ema);
            mock.SetupProperty(x => x.Atr, TechnicalLevelsDataProviderData.Atr);

            return mock;
        }
    }
}
