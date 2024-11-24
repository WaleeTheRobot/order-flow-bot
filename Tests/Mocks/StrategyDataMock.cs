using Moq;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class StrategyDataMock
    {
        public static Mock<StrategyConfigData> CreateDataBarConfig()
        {
            var config = new StrategyConfigData();

            var mock = new Mock<StrategyConfigData>();
            mock.SetupGet(x => x.Name).Returns(config.Name);
            mock.SetupGet(x => x.StrategyTriggered).Returns(config.StrategyTriggered);
            mock.SetupGet(x => x.TriggeredDirection).Returns(config.TriggeredDirection);

            return mock;
        }
    }
}
