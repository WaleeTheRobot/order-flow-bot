using Moq;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class StrategyDataMock
    {
        public static Mock<StrategyData> CreateDataBarConfig()
        {
            var config = new StrategyData();

            var mock = new Mock<StrategyData>();
            mock.SetupGet(x => x.Name).Returns(config.Name);
            mock.SetupGet(x => x.StrategyTriggered).Returns(config.StrategyTriggered);
            mock.SetupGet(x => x.TriggeredDirection).Returns(config.TriggeredDirection);

            return mock;
        }
    }
}
