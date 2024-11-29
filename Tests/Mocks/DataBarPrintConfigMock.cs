using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class DataBarPrintConfigMock
    {
        public static Mock<IDataBarPrintConfig> CreateDataBarPrintConfig()
        {
            var configData = new DataBarPrintConfigData();

            var mock = new Mock<IDataBarPrintConfig>();
            mock.SetupProperty(x => x.BarsAgo, configData.BarsAgo);
            mock.SetupProperty(x => x.ShowBasic, configData.ShowBasic);
            mock.SetupProperty(x => x.ShowDeltas, configData.ShowDeltas);
            mock.SetupProperty(x => x.ShowImbalances, configData.ShowImbalances);
            mock.SetupProperty(x => x.ShowPrices, configData.ShowPrices);
            mock.SetupProperty(x => x.ShowRatios, configData.ShowRatios);
            mock.SetupProperty(x => x.ShowVolumes, configData.ShowVolumes);
            mock.SetupProperty(x => x.ShowBidAskVolumePerBar, configData.ShowBidAskVolumePerBar);
            mock.SetupProperty(x => x.ShowCumulativeDeltaBar, configData.ShowCumulativeDeltaBar);

            return mock;
        }
    }
}
