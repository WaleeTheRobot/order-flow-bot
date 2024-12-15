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
            var messagingConfigData = new MessagingConfigData();
            MessagingConfig.Instance.MarketEnvironment = messagingConfigData.MarketEnvironment;
            MessagingConfig.Instance.ExternalAnalysisService = messagingConfigData.ExternalAnalysisService;
            MessagingConfig.Instance.ExternalAnalysisServiceEnabled = messagingConfigData.ExternalAnalysisServiceEnabled;

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

        #region Trade Management
        [Fact]
        public void EnabledDisabledTriggered()
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

            // Strategy triggered while enabled
            SimulateStrategyTriggered();

            _userInterfaceEvents.EnabledDisabledTriggered(false);
            Assert.True(
                eventTriggered,
                "Trading disabled. Expected EnabledDisabledTriggered event to be triggered."
            );
            Assert.False(
                _tradingEvents.GetTradingState().IsTradingEnabled,
                "Expected IsTradingEnabled event to be disabled."
            );

            // Strategy triggered closed from disabling
            VerifyStrategyTriggeredReset();
        }

        [Fact]
        public void AutoTradeTriggered()
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
        public void AlertTriggered()
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

            SimulateStrategyTriggered();
            // Simulate OrderFlowBot.StrategyManager.TradeAlert triangle painted
            // Simulate OrderFlowBot.StrategyManager.ResetAtm
            SimulateStrategyTriggeredReset();

            _userInterfaceEvents.AlertTriggered(false);
            Assert.True(
                eventTriggered,
                "Expected AlertTriggered event to be triggered."
            );
            Assert.False(
                _tradingEvents.GetTradingState().IsAlertEnabled,
                "Alert disabled. Expected IsAlertEnabled to be disabled"
            );

            VerifyStrategyTriggeredReset();
        }

        [Fact]
        public void CloseTriggered()
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
            // Strategy triggered closed from disabling
            VerifyStrategyTriggeredReset();

            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 0,
                "Expected TriggerStrikePrice to be zero."
            );
        }

        [Fact]
        public void ResetDirectionTriggered()
        {
            VerifyTradeDirectionTriggeredReset();

            // User actions
            _userInterfaceEvents.TriggerStrikePriceTriggered(1000);
            _userInterfaceEvents.DirectionTriggered(Direction.Any);
            _userInterfaceEvents.StandardTriggered(Direction.Inverse);

            Assert.True(
                _tradingEvents.GetTradingState().SelectedTradeDirection == Direction.Any,
                "Expected SelectedTradeDirection to be any."
            );
            Assert.True(
                _tradingEvents.GetTradingState().StandardInverse == Direction.Inverse,
                "Expected StandardInverse to be inverse."
            );
            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice != 0,
                "Expected TriggerStrikePrice to not be zero."
            );

            var eventTriggered = false;
            _userInterfaceEvents.OnResetDirectionTriggered += () => eventTriggered = true;
            _userInterfaceEvents.ResetDirectionTriggered();

            Assert.True(
                eventTriggered,
                "Expected ResetDirectionTriggered event to be triggered."
            );

            VerifyTradeDirectionTriggeredReset();
        }

        [Fact]
        public void ResetStrategiesTriggered()
        {
            var strategy1 = "Stacked Imbalances";
            var strategy2 = "Test";

            // User actions
            _userInterfaceEvents.AddSelectedStrategyTriggered(strategy1);
            _userInterfaceEvents.AddSelectedStrategyTriggered(strategy2);

            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Count == 2,
                "Expected SelectedStrategies to be 2."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy1),
                $"Expected SelectedStrategies to have {strategy1}."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy2),
                $"Expected SelectedStrategies to have {strategy2}."
            );

            var eventTriggered = false;
            _userInterfaceEvents.OnResetStrategiesTriggered += () => eventTriggered = true;
            _userInterfaceEvents.ResetStrategiesTriggered();

            Assert.True(
                eventTriggered,
                "Expected ResetDirectionTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Count == 0,
                "Expected SelectedStrategies to be zero."
            );
        }

        #endregion

        #region Trade Direction

        [Fact]
        public void StandardTriggered()
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

            // Purposely incorrect
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
        public void DirectionTriggered()
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

            _userInterfaceEvents.DirectionTriggered(Direction.Short);
            Assert.True(
                eventTriggered,
                "Expected DirectionTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedTradeDirection == Direction.Short,
                "Long enabled. Expected SelectedTradeDirection event to be short."
            );

            _userInterfaceEvents.DirectionTriggered(Direction.Any);
            Assert.True(
                eventTriggered,
                "Expected DirectionTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedTradeDirection == Direction.Any,
                "Long enabled. Expected SelectedTradeDirection event to be any."
            );

            _userInterfaceEvents.DirectionTriggered(Direction.Flat);
            Assert.True(
                eventTriggered,
                "Expected DirectionTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedTradeDirection == Direction.Flat,
                "Long enabled. Expected SelectedTradeDirection event to be flat."
            );
        }

        [Fact]
        public void TriggerStrikePriceTriggered()
        {
            var eventTriggered = false;
            _userInterfaceEvents.OnTriggerStrikePriceTriggered += (price) => eventTriggered = true;

            _userInterfaceEvents.TriggerStrikePriceTriggered(1000.00);
            Assert.True(
                eventTriggered,
                "Expected TriggerStrikePriceTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 1000.00,
                "Trigger Strike Price entered. Expected TriggerStrikePrice to be 1000."
            );

            _userInterfaceEvents.TriggerStrikePriceTriggered(0);
            Assert.True(
                eventTriggered,
                "Expected TriggerStrikePriceTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().TriggerStrikePrice == 0,
                "Trigger Strike Price entered. Expected TriggerStrikePrice to be zero."
            );
        }

        [Fact]
        public void ResetTriggerStrikePrice()
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

        #region Strategies

        [Fact]
        public void AddSelectedStrategyTriggered()
        {
            var strategy1 = "Stacked Imbalances";
            var strategy2 = "Test";
            var eventTriggered = false;

            _userInterfaceEvents.OnAddSelectedStrategyTriggered += (strategy) => eventTriggered = true;
            _userInterfaceEvents.AddSelectedStrategyTriggered(strategy1);

            Assert.True(eventTriggered, "Expected AddSelectedStrategyTriggered event to be triggered.");
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Count == 1,
                "Expected SelectedStrategies to be 1."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy1),
                $"Expected SelectedStrategies to have {strategy1}."
            );

            _userInterfaceEvents.AddSelectedStrategyTriggered(strategy2);

            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Count == 2,
                "Expected SelectedStrategies to be 2."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy2),
                $"Expected SelectedStrategies to have {strategy2}."
            );
        }

        [Fact]
        public void RemoveSelectedStrategyTriggered()
        {
            var strategy1 = "Stacked Imbalances";
            var strategy2 = "Test";

            var eventTriggered = false;
            _userInterfaceEvents.OnAddSelectedStrategyTriggered += (strategy) => eventTriggered = true;
            _userInterfaceEvents.AddSelectedStrategyTriggered(strategy1);
            _userInterfaceEvents.AddSelectedStrategyTriggered(strategy2);

            Assert.True(
                eventTriggered,
                "Expected AddSelectedStrategyTriggered event to be triggered."
            );

            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Count == 2,
                "Expected SelectedStrategies to be 2."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy1),
                $"Expected SelectedStrategies to have {strategy1}."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy2),
                $"Expected SelectedStrategies to have {strategy2}."
            );

            var removeEventTriggered = false;
            _userInterfaceEvents.OnRemoveSelectedStrategyTriggered += (strategy) => removeEventTriggered = true;
            _userInterfaceEvents.RemoveSelectedStrategyTriggered(strategy1);

            Assert.True(
                removeEventTriggered,
                "Expected RemoveSelectedStrategyTriggered event to be triggered."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Count == 1,
                "Expected SelectedStrategies to be 1."
            );
            Assert.False(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy1),
                $"Expected SelectedStrategies to not have {strategy1}."
            );
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy2),
                $"Expected SelectedStrategies to have {strategy2}."
            );

            _userInterfaceEvents.RemoveSelectedStrategyTriggered(strategy2);
            Assert.True(
                _tradingEvents.GetTradingState().SelectedStrategies.Count == 0,
                "Expected SelectedStrategies to be zero."
            );
            Assert.False(
                _tradingEvents.GetTradingState().SelectedStrategies.Contains(strategy2),
                $"Expected SelectedStrategies to not have {strategy2}."
            );
        }

        #endregion
    }
}
