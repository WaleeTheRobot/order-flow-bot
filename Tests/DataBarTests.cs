using Moq;
using Xunit;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using OrderFlowBot.Tests.Mocks;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests
{
    public class DataBarTests : IClassFixture<DataBarConfigFixture>
    {
        private readonly Mock<IDataBarConfig> _dataBarConfig;

        public DataBarTests(DataBarConfigFixture fixture)
        {
            _dataBarConfig = fixture.DataBarConfig;
        }

        [Fact]
        public void ShouldPopulateDataBarBasics()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(BarType.Bearish, dataBar.BarType);
            Assert.Equal(DataBarDataProviderData.Time, dataBar.Time);
            Assert.Equal(DataBarDataProviderData.CurrentBar, dataBar.BarNumber);
        }

        [Fact]
        public void ShouldPopulateDataBarPrices()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(DataBarDataProviderData.High, dataBar.Prices.High);
            Assert.Equal(DataBarDataProviderData.Low, dataBar.Prices.Low);
            Assert.Equal(DataBarDataProviderData.Open, dataBar.Prices.Open);
            Assert.Equal(DataBarDataProviderData.Close, dataBar.Prices.Close);
        }

        [Fact]
        public void ShouldPopulateDataBarRatios()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(1, dataBar.Ratios.BidRatio);
            Assert.Equal(2.33, dataBar.Ratios.AskRatio);
        }

        [Fact]
        public void ShouldPopulateDataBarVolumes()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(VolumetricBarData.PointOfControl, dataBar.Volumes.PointOfControl);
            Assert.Equal(VolumetricBarData.TotalVolume, dataBar.Volumes.Volume);
            Assert.Equal(VolumetricBarData.TotalBuyingVolume, dataBar.Volumes.BuyingVolume);
            Assert.Equal(VolumetricBarData.TotalSellingVolume, dataBar.Volumes.SellingVolume);
            Assert.Equal(0.70, dataBar.Volumes.ValueAreaPercentage);
            //Assert.Equal(VolumetricBarData.ValueAreaHighPrice, dataBar.Volumes.ValueAreaHighPrice);
            //Assert.Equal(VolumetricBarData.ValueAreaLowPrice, dataBar.Volumes.ValueAreaLowPrice);
            var expectedList = VolumetricBarData.GetTestBarA1BidAskVolume();
            var actualList = dataBar.Volumes.BidAskVolumes;

            Assert.Equal(expectedList.Count, actualList.Count);

            for (int i = 0; i < expectedList.Count; i++)
            {
                var expectedItem = expectedList[i];
                var actualItem = actualList[i];

                Assert.Equal(expectedItem.Price, actualItem.Price);
                Assert.Equal(expectedItem.BidVolume, actualItem.BidVolume);
                Assert.Equal(expectedItem.AskVolume, actualItem.AskVolume);
            }
        }

        [Fact]
        public void ShouldPopulateDataBarDeltas()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(VolumetricBarData.BarDelta, dataBar.Deltas.Delta);
            Assert.Equal(VolumetricBarData.MinSeenDelta, dataBar.Deltas.MinDelta);
            Assert.Equal(VolumetricBarData.MaxSeenDelta, dataBar.Deltas.MaxDelta);
            Assert.Equal(VolumetricBarData.CumulativeDelta, dataBar.Deltas.CumulativeDelta);
            Assert.Equal(VolumetricBarData.DeltaPercentage, dataBar.Deltas.DeltaPercentage);
            Assert.Equal(VolumetricBarData.DeltaChange, dataBar.Deltas.DeltaChange);
            Assert.Equal(VolumetricBarData.DeltaSl, dataBar.Deltas.DeltaSl);
            Assert.Equal(VolumetricBarData.DeltaSh, dataBar.Deltas.DeltaSh);
            Assert.Equal(50, dataBar.Deltas.MinMaxDeltaRatio);
            Assert.Equal(0.02, dataBar.Deltas.MaxMinDeltaRatio);
        }

        [Fact]
        public void ShouldPopulateDataBarImbalances()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(false, dataBar.Imbalances.HasBidStackedImbalances);
            Assert.Equal(false, dataBar.Imbalances.HasAskStackedImbalances);
            Assert.Equal(dataBarConfig.TickSize, dataBar.Imbalances.TickSize);
            Assert.Equal(dataBarConfig.ImbalanceRatio, dataBar.Imbalances.ImbalanceRatio);
            Assert.Equal(dataBarConfig.ImbalanceMinDelta, dataBar.Imbalances.ImbalanceMinDelta);
            Assert.Equal(dataBarConfig.StackedImbalance, dataBar.Imbalances.StackedImbalance);
            // Will test imbalances at other tick levels
        }

        [Fact]
        public void ShouldPopulateDataBarVolumesWithDifferentTicksPerLevel()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            var expectedList = VolumetricBarData.GetTestBarA3BidAskVolume();
            var actualList = dataBar.Volumes.BidAskVolumes;

            for (int i = 0; i < expectedList.Count; i++)
            {
                var expectedItem = expectedList[i];
                var actualItem = actualList[i];

                // Print for debugging
                Console.WriteLine($"Expected: {expectedItem.Price}, {expectedItem.BidVolume}, {expectedItem.AskVolume}");
                Console.WriteLine($"Actual: {actualItem.Price}, {actualItem.BidVolume}, {actualItem.AskVolume}");

                Assert.Equal(expectedItem.Price, actualItem.Price);
                Assert.Equal(expectedItem.BidVolume, actualItem.BidVolume);
                Assert.Equal(expectedItem.AskVolume, actualItem.AskVolume);
            }

        }

        /*[Fact]
        public void ShouldHaveBidImbalances()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProviderA3().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            var expectedList = VolumetricBarData.GetTestBarA3BidAskVolumeBidImbalances();
            var actualList = dataBar.Volumes.BidAskVolumes;

            Assert.Equal(expectedList.Count, actualList.Count);

            for (int i = 0; i < expectedList.Count; i++)
            {
                var expectedItem = expectedList[i];
                var actualItem = actualList[i];

                Assert.Equal(expectedItem.Price, actualItem.Price);
                Assert.Equal(expectedItem.BidVolume, actualItem.BidVolume);
                Assert.Equal(expectedItem.AskVolume, actualItem.AskVolume);
            }
        }

        [Fact]
        public void ShouldHaveAskImbalances()
        {
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProviderA3().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            var expectedList = VolumetricBarData.GetTestBarA3BidAskVolumeAskImbalances();
            var actualList = dataBar.Volumes.BidAskVolumes;

            Assert.Equal(expectedList.Count, actualList.Count);

            for (int i = 0; i < expectedList.Count; i++)
            {
                var expectedItem = expectedList[i];
                var actualItem = actualList[i];

                Assert.Equal(expectedItem.Price, actualItem.Price);
                Assert.Equal(expectedItem.BidVolume, actualItem.BidVolume);
                Assert.Equal(expectedItem.AskVolume, actualItem.AskVolume);
            }
        }*/
    }
}
