﻿using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using Direction = NinjaTrader.Custom.AddOns.OrderFlowBot.Configs.Direction;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    // Handles the TradingState
    public class TradingService
    {
        private readonly EventManager _eventManager;
        private readonly TradingEvents _tradingEvents;
        private readonly TradingState _tradingState;

        public TradingService(EventsContainer eventsContainer, IBacktestData backtestData)
        {
            _eventManager = eventsContainer.EventManager;
            _tradingState = new TradingState(backtestData);

            _tradingEvents = eventsContainer.TradingEvents;
            _tradingEvents.OnGetTradingState += HandleGetTradingState;
            _tradingEvents.OnStrategyTriggered += HandleStrategyTriggered;
            _tradingEvents.OnResetTriggeredTradingState += HandleResetTriggeredTradingState;
            _tradingEvents.OnLastTradedBarNumberTriggered += HandleLastTradedBarNumberTriggered;
            _tradingEvents.OnCurrentBarNumberTriggered += HandleCurrentBarNumberTriggered;
        }

        private IReadOnlyTradingState HandleGetTradingState()
        {
            return _tradingState;
        }

        private void HandleStrategyTriggered(IStrategyData strategyTriggeredData)
        {
            _tradingState.SetTriggeredTradingState(
                strategyTriggeredData.Name,
                strategyTriggeredData.StrategyTriggered,
                strategyTriggeredData.TriggeredDirection
            );

            _tradingEvents.StrategyTriggeredProcessed();
        }

        private void HandleResetTriggeredTradingState()
        {
            _tradingState.SetInitialTriggeredState();
        }

        private void HandleLastTradedBarNumberTriggered(int barNumber)
        {
            _tradingState.LastTradedBarNumber = barNumber;
        }

        private void HandleCurrentBarNumberTriggered(int barNumber)
        {
            _tradingState.CurrentBarNumber = barNumber;
        }

        #region User Interface

        public void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            _tradingState.IsTradingEnabled = isEnabled;
            _tradingState.SetInitialTriggeredState();
            HandleCloseTriggered();

            _eventManager.PrintMessage($"IsTradingEnabled: {_tradingState.IsTradingEnabled}");
        }

        public void UpdateIsAutoTradeEnabled(bool isEnabled)
        {
            _tradingState.IsAutoTradeEnabled = isEnabled;

            _eventManager.PrintMessage($"IsAutoTradeEnabled: {_tradingState.IsAutoTradeEnabled}");
        }

        public void UpdateIsAlertEnabled(bool isEnabled)
        {
            _tradingState.IsAlertEnabled = isEnabled;

            _eventManager.PrintMessage($"IsAlertEnabled: {_tradingState.IsAlertEnabled}");
        }

        public void HandleResetDirectionTriggered()
        {
            _tradingState.SetInitialTradeDirection();
            _eventManager.PrintMessage($"ResetDirectionTriggered: {_tradingState.TriggerStrikePrice}");
            _eventManager.PrintMessage($"ResetDirectionTriggered: {_tradingState.StandardInverse}");
            _eventManager.PrintMessage($"ResetDirectionTriggered: {_tradingState.SelectedTradeDirection}");
        }

        public void UpdateSelectedTradeDirection(Direction direction)
        {
            _tradingState.SelectedTradeDirection = direction;

            _eventManager.PrintMessage($"SelectedTradeDirection: {_tradingState.SelectedTradeDirection}");
        }

        public void UpdateStandardInverse(Direction direction)
        {
            if (direction == Direction.Standard || direction == Direction.Inverse)
            {
                _tradingState.StandardInverse = direction;
            }
            else
            {
                _tradingState.StandardInverse = Direction.Standard;
            }

            _eventManager.PrintMessage($"StandardInverse: {_tradingState.StandardInverse}");
        }

        public void HandleCloseTriggered()
        {
            _tradingEvents.CloseTriggered();
        }

        public void HandleTriggerStrikePriceTriggered(double price)
        {
            _tradingState.TriggerStrikePrice = price;

            _eventManager.PrintMessage($"TriggerStrikePrice: {_tradingState.TriggerStrikePrice}");
        }

        public void HandleAddSelectedStrategyTriggered(string name)
        {
            _tradingState.AddStrategyByName(name);
            _eventManager.PrintMessage($"Added: {name}");
            _eventManager.PrintMessage($"Count: {_tradingState.SelectedStrategies.Count}");
        }

        public void HandleRemoveSelectedStrategyTriggered(string name)
        {
            _tradingState.RemoveStrategyByName(name);
            _eventManager.PrintMessage($"Removed: {name}");
            _eventManager.PrintMessage($"Count: {_tradingState.SelectedStrategies.Count}");
        }

        #endregion
    }
}
