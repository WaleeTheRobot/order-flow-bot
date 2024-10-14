using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class DataBarPrintConfigMock
    {
        public static Mock<IDataBarPrintConfig> CreateDataBarPrintConfig()
        {
            var dataBarPrintConfigData = new DataBarPrintConfigData();

            var mock = new Mock<IDataBarPrintConfig>();
            mock.SetupProperty(m => m.BarsAgo, dataBarPrintConfigData.BarsAgo);
            mock.SetupProperty(m => m.ShowBasic, dataBarPrintConfigData.ShowBasic);
            mock.SetupProperty(m => m.ShowDeltas, dataBarPrintConfigData.ShowDeltas);
            mock.SetupProperty(m => m.ShowImbalances, dataBarPrintConfigData.ShowImbalances);
            mock.SetupProperty(m => m.ShowPrices, dataBarPrintConfigData.ShowPrices);
            mock.SetupProperty(m => m.ShowRatios, dataBarPrintConfigData.ShowRatios);
            mock.SetupProperty(m => m.ShowVolumes, dataBarPrintConfigData.ShowVolumes);
            mock.SetupProperty(m => m.ShowBidAskVolumePerBar, dataBarPrintConfigData.ShowBidAskVolumePerBar);

            return mock;
        }
    }
}
