using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class StrategiesEvents
    {
        private readonly EventManager _eventManager;
        public event Func<List<StrategyBase>> OnGetStrategies;
        public event Action OnResetStrategyData;

        public StrategiesEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered for getting the strategies.
        /// This is used to get the strategies.
        /// </summary>
        public List<StrategyBase> GetStrategies()
        {
            return _eventManager.InvokeEvent(() => OnGetStrategies?.Invoke());
        }

        /// <summary>
        /// Event triggered for resetting the strategy data.
        /// This is used to reset the strategy data.
        /// </summary>
        public void ResetStrategyData()
        {
            _eventManager.InvokeEvent(OnResetStrategyData);
        }
    }
}
