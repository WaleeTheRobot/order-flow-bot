using Moq;
using Xunit;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests
{
    public class TradingEventsAndServiceTests
    {
        private readonly EventManager _eventManager;
        private readonly TradingEvents _tradingEvents;

        public TradingEventsAndServiceTests()
        {
            _eventManager = new EventManager();
            _tradingEvents = new TradingEvents(_eventManager);
            var backtestData = new BacktestConfigData();

            var eventsContainer = new EventsContainer
            {
                EventManager = _eventManager,
                TradingEvents = _tradingEvents
            };

            new TradingService(eventsContainer, backtestData);
        }

        [Fact]
        public void ShouldTriggerGetTradingStateEvent()
        {
            var eventTriggered = false;
            var tradingStateMock = new Mock<IReadOnlyTradingState>();

            tradingStateMock.Setup(state => state.TriggeredName).Returns("Stacked Imbalances");

            _tradingEvents.OnGetTradingState += () =>
            {
                eventTriggered = true;
                return tradingStateMock.Object;
            };

            var result = _tradingEvents.GetTradingState();

            Assert.True(eventTriggered, "Expected the OnGetTradingState event to be triggered.");
            Assert.NotNull(result);
            Assert.Equal(tradingStateMock.Object.TriggeredName, result.TriggeredName);
            Assert.Equal(tradingStateMock.Object.StrategyTriggered, result.StrategyTriggered);
            Assert.Equal(tradingStateMock.Object.TriggeredDirection, result.TriggeredDirection);
            Assert.Equal(tradingStateMock.Object.SelectedTradeDirection, result.SelectedTradeDirection);
        }

        [Fact]
        public void ShouldTriggerStrategyTriggeredEvent()
        {
            var eventTriggered = false;
            var strategyDataMock = new Mock<StrategyConfigData>();

            _tradingEvents.OnStrategyTriggered += (strategyData) =>
            {
                eventTriggered = true;

                Assert.NotNull(strategyData);
                Assert.Equal(strategyDataMock.Object.Name, strategyData.Name);
                Assert.Equal(strategyDataMock.Object.StrategyTriggered, strategyData.StrategyTriggered);
                Assert.Equal(strategyDataMock.Object.TriggeredDirection, strategyData.TriggeredDirection);
            };

            _tradingEvents.StrategyTriggered(strategyDataMock.Object);

            Assert.True(eventTriggered, "Expected the OnStrategyTriggered event to be triggered.");
        }

        [Fact]
        public void ShouldTriggerStrategyTriggeredProcessedEvent()
        {
            var eventTriggered = false;

            _tradingEvents.OnStrategyTriggeredProcessed += () => eventTriggered = true;
            _tradingEvents.StrategyTriggeredProcessed();

            Assert.True(eventTriggered, "Expected the OnStrategyTriggeredProcessed event to be triggered.");
        }

        [Fact]
        public void ShouldTriggerResetTradingStateEvent()
        {
            var eventTriggered = false;

            _tradingEvents.OnResetTriggeredTradingState += () => eventTriggered = true;
            _tradingEvents.ResetTriggeredTradingState();

            Assert.True(eventTriggered, "Expected the OnResetTradingState event to be triggered.");
        }
    }
}
