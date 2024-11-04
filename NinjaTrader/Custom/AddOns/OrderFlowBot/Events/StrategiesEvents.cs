using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class StrategiesEvents
    {
        private readonly EventManager _eventManager;
        public event Func<List<object>> OnGetStrategies;

        public StrategiesEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered for getting the strategies.
        /// This is used to get the strategies.
        /// </summary>
        public List<object> GetStrategies()
        {
            return _eventManager.InvokeEvent(() => OnGetStrategies?.Invoke());
        }
    }
}
