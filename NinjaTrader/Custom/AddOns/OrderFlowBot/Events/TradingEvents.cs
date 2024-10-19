using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class TradingEvents
    {
        private readonly EventManager _eventManager;
        public event Func<IReadOnlyTradingState> OnGetTradingState;
        public event Action<StrategyData> OnStrategyTriggered;
        public event Action OnStrategyTriggeredProcessed;
        public event Action OnResetTradingState;

        public TradingEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered when the trading state is requested.
        /// This is used get the read only trading state.
        /// </summary>
        public IReadOnlyTradingState GetTradingState()
        {
            return _eventManager.InvokeEvent(() => OnGetTradingState?.Invoke());
        }

        /// <summary>
        /// Event triggered when a strategy entry is found.
        /// This is used set the strategy triggered state.
        /// </summary>
        public void StrategyTriggered(StrategyData strategyTriggeredData)
        {
            _eventManager.InvokeEvent(OnStrategyTriggered, strategyTriggeredData);
        }

        public void StrategyTriggeredProcessed()
        {
            _eventManager.InvokeEvent(OnStrategyTriggeredProcessed);
        }

        public void ResetTradingState()
        {
            _eventManager.InvokeEvent(OnResetTradingState);
        }
    }
}
