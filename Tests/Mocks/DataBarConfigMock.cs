using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class DataBarConfigMock
    {
        public static Mock<IDataBarConfig> CreateDataBarConfig()
        {
            var dataBarConfigData = new DataBarConfigData();

            var mock = new Mock<IDataBarConfig>();
            mock.SetupGet(x => x.TickSize).Returns(dataBarConfigData.TickSize);
            mock.SetupGet(x => x.TicksPerLevel).Returns(dataBarConfigData.TicksPerLevel);
            mock.SetupGet(x => x.StackedImbalance).Returns(dataBarConfigData.StackedImbalance);
            mock.SetupGet(x => x.ImbalanceRatio).Returns(dataBarConfigData.ImbalanceRatio);
            mock.SetupGet(x => x.ImbalanceMinDelta).Returns(dataBarConfigData.ImbalanceMinDelta);
            mock.SetupGet(x => x.ValueAreaPercentage).Returns(dataBarConfigData.ValueAreaPercentage);

            return mock;
        }
    }
}
