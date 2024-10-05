using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using OrderFlowBot.Tests.Mocks;

public class DataBarConfigFixture
{
    public Mock<IDataBarConfig> DataBarConfig { get; private set; }

    public DataBarConfigFixture()
    {
        DataBarConfig = DataBarConfigMock.CreateDataBarConfig();
    }
}
