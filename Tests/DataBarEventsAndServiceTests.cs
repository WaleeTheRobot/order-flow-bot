using Moq;
using System;
using Xunit;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using System.Collections.Generic;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace OrderFlowBot.Tests
{
    public class DataBarServiceTests
    {
        private readonly Mock<IDataBarDataProvider> _dataBarDataProviderMock;
        private readonly EventManager _eventManager;
        private readonly DataBarEvents _dataBarEvents;

        public DataBarServiceTests()
        {
            _dataBarDataProviderMock = new Mock<IDataBarDataProvider>();
            _eventManager = new EventManager();
            _dataBarEvents = new DataBarEvents(_eventManager);
        }

        [Fact]
        public void ShouldTriggerUpdateCurrentDataBarEvent()
        {
            var eventTriggered = false;
            _dataBarEvents.OnUpdateCurrentDataBar += (dataBarDataProvider) => eventTriggered = true;
            _dataBarEvents.UpdateCurrentDataBar(_dataBarDataProviderMock.Object);

            Assert.True(eventTriggered, "Expected the OnUpdateCurrentDataBar event to be triggered.");
        }

        [Fact]
        public void ShouldTriggerUpdateCurrentDataBarListEvent()
        {
            var eventTriggered = false;
            _dataBarEvents.OnUpdateCurrentDataBarList += () => eventTriggered = true;
            _dataBarEvents.UpdateCurrentDataBarList();

            Assert.True(eventTriggered, "Expected the OnUpdateCurrentDataBarList event to be triggered.");
        }

        [Fact]
        public void ShouldTriggerUpdatedCurrentDataBarEvent()
        {
            Mock<IDataBarConfig> dataBarConfigMock = new Mock<IDataBarConfig>();
            DataBar dataBarMock = new DataBar(dataBarConfigMock.Object);
            List<DataBar> dataBarsMock = new List<DataBar>();
            dataBarsMock.Add(dataBarMock);

            var eventTriggered = false;
            _dataBarEvents.OnUpdatedCurrentDataBar += (dataBar, dataBars) => eventTriggered = true;
            _dataBarEvents.UpdatedCurrentDataBar(dataBarMock, dataBarsMock);

            Assert.True(eventTriggered, "Expected the OnUpdatedCurrentDataBar event to be triggered.");
        }

        [Fact]
        public void ShouldTriggerPrintDataBarEvent()
        {
            Mock<IDataBarPrintConfig> dataBarPrintConfigMock = new Mock<IDataBarPrintConfig>();

            var eventTriggered = false;
            _dataBarEvents.OnPrintDataBar += (dataBarConfig) => eventTriggered = true;
            _dataBarEvents.PrintDataBar(dataBarPrintConfigMock.Object);

            Assert.True(eventTriggered, "Expected the OnPrintDataBar event to be triggered.");
        }

        [Fact]
        public void ShouldVerifyUpdateCurrentDataBar()
        {
            _dataBarEvents.UpdateCurrentDataBar(_dataBarDataProviderMock.Object);

            var currentDataBar = _dataBarEvents.GetCurrentDataBar();

            Assert.NotNull(currentDataBar);
            Assert.Equal(_dataBarDataProviderMock.Object.Time, currentDataBar.Time);
            Assert.Equal(_dataBarDataProviderMock.Object.High, currentDataBar.Prices.High);
            Assert.Equal(_dataBarDataProviderMock.Object.Low, currentDataBar.Prices.Low);
            Assert.Equal(_dataBarDataProviderMock.Object.Open, currentDataBar.Prices.Open);
            Assert.Equal(_dataBarDataProviderMock.Object.Close, currentDataBar.Prices.Close);
        }

        [Fact]
        public void ShouldVerifyMultipleDataBarsInList()
        {
            _dataBarEvents.UpdateCurrentDataBarList();
            _dataBarEvents.UpdateCurrentDataBarList();

            var dataBars = _dataBarEvents.GetDataBars();
            Assert.NotNull(dataBars);
            Assert.True(dataBars.Count > 1, "Expected multiple DataBars in the list.");
        }

        [Fact]
        public void ShouldReturnEmptyDataBarsListWhenNoneAreAdded()
        {
            var dataBars = _dataBarEvents.GetDataBars();

            Assert.NotNull(dataBars);
            Assert.Empty(dataBars);
        }
    }
}
