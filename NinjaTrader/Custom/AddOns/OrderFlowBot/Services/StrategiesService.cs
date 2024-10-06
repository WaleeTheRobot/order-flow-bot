using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    public class StrategiesService
    {
        private readonly EventManager _eventManager;
        private readonly DataBarEvents _dataBarEvents;
        private readonly TradingEvents _tradingEvents;
        private readonly List<object> _strategies;

        public StrategiesService(EventsContainer eventsContainer)
        {
            _eventManager = eventsContainer.EventManager;

            _dataBarEvents = eventsContainer.DataBarEvents;
            _dataBarEvents.OnUpdatedCurrentDataBar += HandleUpdatedCurrentDataBar;

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
            // Try to find a constructor that matches parameters
            ConstructorInfo ctor = type.GetConstructor(new[] { typeof(EventManager), typeof(DataBarEvents), typeof(TradingEvents) });

            if (ctor != null)
            {
                // Create instance with parameters
                return ctor.Invoke(new object[] { eventsContainer });
            }

            // If not found, try to find a parameterless constructor
            ctor = type.GetConstructor(Type.EmptyTypes);

            if (ctor != null)
            {
                // If found, create instance with no parameters
                return ctor.Invoke(null);
            }

            // Constructors not found
            _eventManager.PrintMessage($"No suitable constructor found for {type.Name}");

            return null;
        }

        private void HandleUpdatedCurrentDataBar(DataBar currentDataBar, List<DataBar> dataBars)
        {
            // loop through strategies and check
        }
    }
}
