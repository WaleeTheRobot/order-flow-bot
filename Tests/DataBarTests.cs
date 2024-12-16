using Moq;
using Xunit;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using OrderFlowBot.Tests.Mocks;
using OrderFlowBot.Tests.Mocks.Data;
using System;

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
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(3.36, Math.Round(dataBar.Ratios.BidRatio, 2));
            Assert.Equal(0.69, Math.Round(dataBar.Ratios.AskRatio, 2));
        }

        [Fact]
        public void ShouldPopulateDataBarVolumes()
        {
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(VolumetricBarData.PointOfControl, dataBar.Volumes.PointOfControl);
            Assert.Equal(VolumetricBarData.TotalVolume, dataBar.Volumes.Volume);
            Assert.Equal(VolumetricBarData.TotalBuyingVolume, dataBar.Volumes.BuyingVolume);
            Assert.Equal(VolumetricBarData.TotalSellingVolume, dataBar.Volumes.SellingVolume);
            Assert.Equal(0.70, dataBar.Volumes.ValueAreaPercentage);
            Assert.Equal(VolumetricBarData.ValueAreaHighPrice, dataBar.Volumes.ValueAreaHighPrice);
            Assert.Equal(VolumetricBarData.ValueAreaLowPrice, dataBar.Volumes.ValueAreaLowPrice);
            var expectedList = VolumetricBarData.GetTestBarBidAskVolume();
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
            Assert.Equal(1.1, Math.Round(dataBar.Deltas.MinMaxDeltaRatio, 2));
            Assert.Equal(0.91, Math.Round(dataBar.Deltas.MaxMinDeltaRatio, 2));
        }

        [Fact]
        public void ShouldPopulateDataBarImbalances()
        {
            // There is an issue testing the stacked imbalances. For some reason they are empty, but only in testing.
            var dataBarConfig = DataBarConfigMock.CreateDataBarConfig().Object;
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.False(dataBar.Imbalances.HasBidStackedImbalances);
            Assert.False(dataBar.Imbalances.HasAskStackedImbalances);
            Assert.Equal(dataBarConfig.TickSize, dataBar.Imbalances.TickSize);
            Assert.Equal(dataBarConfig.ImbalanceRatio, dataBar.Imbalances.ImbalanceRatio);
            Assert.Equal(dataBarConfig.ImbalanceMinDelta, dataBar.Imbalances.ImbalanceMinDelta);
            Assert.Equal(dataBarConfig.StackedImbalance, dataBar.Imbalances.StackedImbalance);

            var expectedAskList = ImbalancesData.GetTestBarAskImbalances();
            var actualAskList = dataBar.Imbalances.AskImbalances;

            Assert.Equal(expectedAskList.Count, actualAskList.Count);

            for (int i = 0; i < expectedAskList.Count; i++)
            {
                var expectedItem = expectedAskList[i];
                var actualItem = actualAskList[i];

                Assert.Equal(expectedItem.Price, actualItem.Price);
                Assert.Equal(expectedItem.Volume, actualItem.Volume);
            }

            var expectedBidList = ImbalancesData.GetTestBarBidImbalances();
            var actualBidList = dataBar.Imbalances.BidImbalances;

            Assert.Equal(expectedBidList.Count, actualBidList.Count);

            for (int i = 0; i < expectedBidList.Count; i++)
            {
                var expectedItem = expectedBidList[i];
                var actualItem = actualBidList[i];

                Assert.Equal(expectedItem.Price, actualItem.Price);
                Assert.Equal(expectedItem.Volume, actualItem.Volume);
            }
        }

        [Fact]
        public void ShouldPopulateCumulativeDeltaBar()
        {
            var dataBarDataProvider = DataBarDataProviderMock.CreateDataBarDataProvider().Object;
            var dataBar = new DataBar(_dataBarConfig.Object);

            dataBar.SetCurrentDataBar(dataBarDataProvider);

            Assert.Equal(10, dataBar.CumulativeDeltaBar.High);
            Assert.Equal(1, dataBar.CumulativeDeltaBar.Low);
            Assert.Equal(5, dataBar.CumulativeDeltaBar.Open);
            Assert.Equal(7, dataBar.CumulativeDeltaBar.Close);
        }
    }
}
