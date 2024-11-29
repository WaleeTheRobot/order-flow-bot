using Moq;
using System;
using Xunit;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Services;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using System.Diagnostics.CodeAnalysis;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests
{
    [SuppressMessage("SonarLint", "S1244", Justification = "Double value is not calculated")]
    [SuppressMessage("SonarLint", "S4487", Justification = "Instantiated for event handling")]
    public class OrderFlowBotStrategyManagerTests
    {
        private readonly ServicesContainer _servicesContainer;
        private readonly TradingService _tradingService;
        private readonly TradingEvents _tradingEvents;
        private readonly UserInterfaceEvents _userInterfaceEvents;
        private readonly UserInterfaceService _userInterfaceService;

        public OrderFlowBotStrategyManagerTests()
        {
            var eventsContainer = new EventsContainer();
            var backtestData = new BacktestConfigData();
            backtestData.SetNoBacktest();
            _servicesContainer = new ServicesContainer(eventsContainer, backtestData);
            _tradingService = _servicesContainer.TradingService;
            _tradingEvents = eventsContainer.TradingEvents;

            _userInterfaceEvents = new UserInterfaceEvents(eventsContainer.EventManager);
            _userInterfaceService =
                new UserInterfaceService(
                    _servicesContainer,
                    _userInterfaceEvents,
                    null,
                    null,
                    null
                );
        }

        private void SimulateStrategyTriggered()
        {
            _tradingEvents.StrategyTriggered(new StrategyConfigData());
            Assert.True(
                _tradingEvents.GetTradingState().StrategyTriggered,
                "Expected StrategyTriggered to be true after triggering."
            );
        }

        private void SimulateStrategyTriggeredReset()
        {
            _tradingEvents.ResetTriggeredTradingState();
            Assert.False(
                _tradingEvents.GetTradingState().StrategyTriggered,
                "Expected StrategyTriggered to be false after reset."
            );
        }

        private void VerifyStrategyTriggeredReset()
        {
            Assert.True(
                _tradingEvents.GetTradingState().TriggeredName == "None",
                "Expected initial triggered state Name to be None."
            );
            Assert.False(
                _tradingEvents.GetTradingState().StrategyTriggered,
                "Expected initial triggered state StrategyTriggered to be false."
            );
            Assert.True(
                _tradingEvents.GetTradingState().TriggeredDirection == Direction.Flat,
                "Expected initial triggered state TriggeredDirection to be Flat."
            );
        }

        private void VerifyTradeDirectionTriggeredReset()
        {
            Assert.True(
                _tradingEvents.GetTradingState().SelectedTradeDirection == Direction.Flat,
                "Expected SelectedTradeDirection to be flat."
            );
            Assert.True(
                _tradingEvents.GetTradingState().StandardInverse == Direction.Standard,
                "Expected StandardInverse to be standard."
            );
            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 0,
                "Expected TriggerStrikePrice to be zero."
            );
        }

        [Fact]
        public void PositionClosedAutoTradeDisabled()
        {
            // User actions
            _tradingService.HandleTriggerStrikePriceTriggered(1000);
            // Strategy triggered before closing
            SimulateStrategyTriggered();

            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 1000,
                "Expected TriggerStrikePrice to be 1000."
            );

            var eventTriggered = false;
            _userInterfaceEvents.OnCloseTriggered += () => eventTriggered = true;

            _userInterfaceEvents.CloseTriggered();
            Assert.True(
                eventTriggered,
                "Expected CloseTriggered event to be triggered."
            );

            // OrderFlowBot.StrategyManager.HandleCloseAtmPosition
            SimulateStrategyTriggeredReset();
            _tradingEvents.ResetTriggerStrikePrice();
            _tradingEvents.ResetSelectedTradeDirection();
            // Strategy triggered closed from disabling
            VerifyStrategyTriggeredReset();
            VerifyTradeDirectionTriggeredReset();

            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 0,
                "Expected TriggerStrikePrice to be zero."
            );
        }
    }
}
