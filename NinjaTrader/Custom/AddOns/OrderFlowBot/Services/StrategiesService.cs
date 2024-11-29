using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    public class StrategiesService
    {
        private readonly EventManager _eventManager;
        private readonly TradingEvents _tradingEvents;
        private readonly IReadOnlyTradingState _tradingState;
        private readonly List<StrategyBase> _strategies;

        public StrategiesService(EventsContainer eventsContainer)
        {
            _eventManager = eventsContainer.EventManager;

            var strategiesEvents = eventsContainer.StrategiesEvents;
            strategiesEvents.OnGetStrategies += HandleGetStrategies;

            var dataBarEvents = eventsContainer.DataBarEvents;
            dataBarEvents.OnUpdatedCurrentDataBar += HandleUpdatedCurrentDataBar;

            _tradingEvents = eventsContainer.TradingEvents;
            _tradingState = _tradingEvents.GetTradingState();

            _strategies = new List<StrategyBase>();

            InitializeStrategies(eventsContainer);
        }

        private void InitializeStrategies(EventsContainer eventsContainer)
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string implementationsPath = Path.Combine(userFolder, "Documents", "NinjaTrader 8", "bin", "Custom", "AddOns", "OrderFlowBot", "Models", "Strategies", "Implementations");

            if (!Directory.Exists(implementationsPath))
            {
                _eventManager.PrintMessage($"Directory not found: {implementationsPath}");
                return;
            }

            string[] files = Directory.GetFiles(implementationsPath, "*.cs");

            foreach (string file in files)
            {
                string className = Path.GetFileNameWithoutExtension(file);
                Type type = Type.GetType($"NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies.Implementations.{className}");

                if (type != null)
                {
                    try
                    {
                        object instance = CreateInstance(type, eventsContainer);
                        if (instance != null)
                        {
                            _strategies.Add((StrategyBase)instance);
                        }
                    }
                    catch (Exception ex)
                    {
                        _eventManager.PrintMessage($"Error creating instance of {className}: {ex.Message}");
                    }
                }
                else
                {
                    _eventManager.PrintMessage($"Could not find type for {className}");
                }
            }
        }

        private object CreateInstance(Type type, EventsContainer eventsContainer)
        {
            // Update to reflect any changes in parameters
            ConstructorInfo ctor = type.GetConstructor(new[] { typeof(EventsContainer) });

            if (ctor != null)
            {
                return ctor.Invoke(new object[] { eventsContainer });
            }

            _eventManager.PrintMessage($"No suitable constructor found for {type.Name}");
            return null;
        }

        private void HandleUpdatedCurrentDataBar()
        {
            // No further checks needed if:
            // - Trading is disabled
            // - Strategy is already triggered
            // - Selected trade direction is flat
            // - Failed last traded bar number requirement
            if (
                !_tradingState.IsTradingEnabled ||
                _tradingState.StrategyTriggered ||
                _tradingState.SelectedTradeDirection == Direction.Flat ||
                _tradingState.CurrentBarNumber <= _tradingState.LastTradedBarNumber)
            {
                return;
            }

            foreach (StrategyBase strategy in _strategies)
            {
                if (_tradingState.IsBacktestEnabled)
                {
                    if (_tradingState.BacktestStrategyName != strategy.StrategyData.Name)
                    {
                        continue;
                    }
                }
                else
                {
                    // Ensure the strategy is in the selected strategies list
                    if (!_tradingState.SelectedStrategies.Contains(strategy.StrategyData.Name))
                    {
                        continue;
                    }
                }

                // Check the strategy and trigger events if necessary
                var strategyData = strategy.CheckStrategy();

                if (strategyData.StrategyTriggered)
                {
                    _tradingEvents.StrategyTriggered(strategyData);
                    return;
                }
            }
        }

        private List<StrategyBase> HandleGetStrategies()
        {
            return _strategies;
        }
    }
}
