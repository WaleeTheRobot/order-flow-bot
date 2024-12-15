using Moq;
using System;
using Xunit;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using OrderFlowBot.Tests.Mocks;
using System.Diagnostics.CodeAnalysis;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace OrderFlowBot.Tests
{
    [SuppressMessage("SonarLint", "S1244", Justification = "Double value is not calculated")]
    public class TechnicalLevelsEventsAndServiceTests
    {
        private readonly EventManager _eventManager;
        private readonly TechnicalLevelsEvents _technicalLevelsEvents;

        public TechnicalLevelsEventsAndServiceTests()
        {

            _eventManager = new EventManager();
            _technicalLevelsEvents = new TechnicalLevelsEvents(_eventManager);

            var eventsContainer = new EventsContainer
            {
                EventManager = _eventManager,
                TechnicalLevelsEvents = _technicalLevelsEvents
            };

            new TechnicalLevelsService(eventsContainer);
        }

        [Fact]
        public void ShouldTriggerUpdateCurrentTechnicalLevelsEvent()
        {
            var technicalLevelsDataProvider = TechnicalLevelsDataProviderMock.CreateTechnicalLevelsDataProvider().Object;

            var eventTriggered = false;
            _technicalLevelsEvents.OnUpdateCurrentTechnicalLevels += (dataProvider) => eventTriggered = true;
            _technicalLevelsEvents.UpdateCurrentTechnicalLevels(technicalLevelsDataProvider);

            var currentEma = _technicalLevelsEvents.GetCurrentTechnicalLevels();

            Assert.True(eventTriggered, "Expected the OnUpdateCurrentTechnicalLevels event to be triggered.");
            Assert.True(currentEma.Ema.FastEma == 9, "Expected FastEma to be 9.");
            Assert.True(currentEma.Ema.SlowEma == 20, "Expected SlowEma to be 20.");
            Assert.True(currentEma.Atr.Value == 16, "Expected ATR to be 16.");
            Assert.True(currentEma.BarNumber == 99, "Expected BarNumber to be 99.");
        }

        [Fact]
        public void ShouldTriggerUpdateTechnicalLevelsListEvent()
        {
            var technicalLevelsDataProvider = TechnicalLevelsDataProviderMock.CreateTechnicalLevelsDataProvider().Object;
            var eventTriggered = false;
            _technicalLevelsEvents.OnUpdateTechnicalLevelsList += () => eventTriggered = true;
            _technicalLevelsEvents.UpdateCurrentTechnicalLevels(technicalLevelsDataProvider);
            _technicalLevelsEvents.UpdateTechnicalLevelsList();

            var technicalLevelsList = _technicalLevelsEvents.GetTechnicalLevelsList();

            Assert.True(eventTriggered, "Expected the OnUpdateTechnicalLevelsList event to be triggered.");
            Assert.True(technicalLevelsList.Count == 1, "Expected TechnicalLevelsList count to be 1.");
        }

        [Fact]
        public void ShouldTriggerPrintDataBarEvent()
        {
            var technicalLevelsPrintConfigMock = new Mock<ITechnicalLevelsPrintConfig>();

            var eventTriggered = false;
            _technicalLevelsEvents.OnPrintTechnicalLevels += (dataBarConfig) => eventTriggered = true;
            _technicalLevelsEvents.PrintTechnicalLevels(technicalLevelsPrintConfigMock.Object);

            Assert.True(eventTriggered, "Expected the OnPrintTechnicalLevels event to be triggered.");
        }
    }
}
