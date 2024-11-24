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
    public class UserInterfaceEventsAndServiceTests
    {
        private readonly ServicesContainer _servicesContainer;
        private readonly TradingService _tradingService;
        private readonly TradingEvents _tradingEvents;
        private readonly UserInterfaceEvents _userInterfaceEvents;
        private readonly UserInterfaceService _userInterfaceService;

        public UserInterfaceEventsAndServiceTests()
        {
            EventsContainer eventsContainer = new EventsContainer();
            var backtestData = new BacktestConfigData();
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

        #region Trade Management
        [Fact]
        public void ShouldTriggerEnabledDisabledTriggered()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnEnabledDisabledTriggered += (isEnabled) => eventTriggered = true;

            _userInterfaceEvents.EnabledDisabledTriggered(true);
            Assert.True(
                eventTriggered,
                "Trading enabled. Expected EnabledDisabledTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().IsTradingEnabled,
                "Expected IsTradingEnabled event to be enabled."
            );

            // Trigger strategy true to mock a strategy triggering for testing disabling
            _tradingEvents.StrategyTriggered(new StrategyConfigData());
            Assert.True(
                _tradingEvents.GetTradingState().StrategyTriggered,
                "Expected StrategyTriggered to be true after triggering."
            );

            _userInterfaceEvents.EnabledDisabledTriggered(false);
            Assert.True(
                eventTriggered,
                "Trading disabled. Expected EnabledDisabledTriggered event to be triggered."
            );
            Assert.False(
                _tradingEvents.GetTradingState().IsTradingEnabled,
                "Expected IsTradingEnabled event to be disabled."
            );
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

        [Fact]
        public void ShouldTriggerAutoTradeTriggered()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnAutoTradeTriggered += (isEnabled) => eventTriggered = true;

            _userInterfaceEvents.AlertTriggered(true);
            _userInterfaceEvents.StandardTriggered(Direction.Inverse);
            Assert.True(
                _tradingEvents.GetTradingState().IsAlertEnabled,
                "Set initial test state for Alert. Expected IsAlertEnabled to be Inverse"
            );
            Assert.True(
                _tradingEvents.GetTradingState().StandardInverse == Direction.Inverse,
                "Set initial test state for StandardInverse. Expected StandardInverse to be Inverse"
            );

            _userInterfaceEvents.AutoTradeTriggered(true);
            Assert.True(
                eventTriggered,
                "AutoTrade Enabled. Expected AutoTradeTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().IsAutoTradeEnabled,
                "Expected TradingState IsAutoTradeEnabled to be enabled."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedTradeDirection == Direction.Any,
                "Long and Short allowed. Expected TradingState SelectedTradeDirection to be any."
            );

            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 0,
                "Long and Short allowed. Expected TradingState SelectedTradeDirection to be any."
            );

            _userInterfaceEvents.AutoTradeTriggered(false);
            Assert.True(
                eventTriggered,
                "AutoTrade Disabled. Expected AutoTradeTriggered event to be triggered."
            );
            Assert.False(
                _tradingEvents.GetTradingState().IsAutoTradeEnabled,
                "Expected TradingState IsAutoTradeEnabled to be disabled."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedTradeDirection == Direction.Flat,
                "Long and Short reset. Expected TradingState SelectedTradeDirection to be flat."
            );
            Assert.True(
                _tradingEvents.GetTradingState().IsAlertEnabled,
                "Alert not reset. Expected IsAlertEnabled to be Inverse"
            );
            Assert.True(
                _tradingEvents.GetTradingState().StandardInverse == Direction.Inverse,
                "StandardInverse not reset. Expected StandardInverse to be Inverse"
            );
        }

        [Fact]
        public void ShouldTriggerAlertTriggered()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnAlertTriggered += (isEnabled) => eventTriggered = true;

            _userInterfaceEvents.AlertTriggered(true);
            Assert.True(
                eventTriggered,
                "Expected AlertTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().IsAlertEnabled,
                "Alert enabled. Expected IsAlertEnabled to be enabled"
            );

            _userInterfaceEvents.AlertTriggered(false);
            Assert.True(
                eventTriggered,
                "Expected AlertTriggered event to be triggered."
            );
            Assert.False(
                _tradingEvents.GetTradingState().IsAlertEnabled,
                "Alert disabled. Expected IsAlertEnabled to be disabled"
            );
        }

        [Fact]
        public void ShouldTriggerCloseTriggered()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnCloseTriggered += () => eventTriggered = true;

            _userInterfaceEvents.CloseTriggered();
            Assert.True(
                eventTriggered,
                "Expected AlertTriggered event to be triggered."
            );
        }

        #endregion

        #region Trade Direction

        [Fact]
        public void ShouldTriggerStandardTriggered()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnStandardTriggered += (direction) => eventTriggered = true;

            _userInterfaceEvents.StandardTriggered(Direction.Standard);
            Assert.True(
                eventTriggered,
                "Expected StandardTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().StandardInverse == Direction.Standard,
                "Standard selected. Expected StandardInverse to be standard."
            );

            _userInterfaceEvents.StandardTriggered(Direction.Inverse);
            Assert.True(
                eventTriggered,
                "Expected StandardTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().StandardInverse == Direction.Inverse,
                "Inverse selected. Expected StandardInverse to be inverse."
            );

            _userInterfaceEvents.StandardTriggered(Direction.Any);
            Assert.True(
                eventTriggered,
                "Expected StandardTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().StandardInverse == Direction.Standard,
                "Standard/Inverse not used. Expected StandardInverse to be standard."
            );
        }

        [Fact]
        public void ShouldTriggerDirectionTriggered()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnDirectionTriggered += (direction) => eventTriggered = true;

            _userInterfaceEvents.DirectionTriggered(Direction.Long);
            Assert.True(
                eventTriggered,
                "Expected DirectionTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedTradeDirection == Direction.Long,
                "Long enabled. Expected SelectedTradeDirection event to be long."
            );
        }

        [Fact]
        public void ShouldTriggerTriggerStrikePriceTriggered()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnTriggerStrikePriceTriggered += (price) => eventTriggered = true;

            _userInterfaceEvents.TriggerStrikePriceTriggered(4000.00);
            Assert.True(
                eventTriggered,
                "Expected TriggerStrikePriceTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 4000.00,
                "Trigger Strike Price entered. Expected TriggerStrikePrice to be same as entry."
            );
        }

        [Fact]
        public void ShouldTriggerResetTriggerStrikePrice()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnResetTriggerStrikePrice += () => eventTriggered = true;

            _userInterfaceEvents.ResetTriggerStrikePrice();
            Assert.True(
                eventTriggered,
                "Expected ResetTriggerStrikePrice event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 0,
                "Trigger Strike Price reset. Expected TriggerStrikePrice to be reset."
            );
        }

        #endregion
    }
}
