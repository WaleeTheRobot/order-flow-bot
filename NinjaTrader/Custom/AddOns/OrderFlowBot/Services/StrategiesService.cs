using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
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
        private readonly List<object> _strategies;

        public StrategiesService(EventsContainer eventsContainer)
        {
            _eventManager = eventsContainer.EventManager;

            var dataBarEvents = eventsContainer.DataBarEvents;
            dataBarEvents.OnUpdatedCurrentDataBar += HandleUpdatedCurrentDataBar;

            _tradingEvents = eventsContainer.TradingEvents;

            _strategies = new List<object>();

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
                            _strategies.Add(instance);
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
            // Strategy already triggered
            if (_tradingEvents.GetTradingState().StrategyTriggered)
            {
                return;
            }

            foreach (StrategyBase strategy in _strategies)
            {
                var strategyData = strategy.CheckStrategy();

                if (strategyData.StrategyTriggered)
                {
                    _tradingEvents.StrategyTriggered(strategyData);

                    return;
                }
            }
        }

    }
}
