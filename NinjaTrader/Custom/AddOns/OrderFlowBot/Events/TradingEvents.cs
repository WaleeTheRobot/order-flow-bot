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
        public event Action<bool> OnTradeManagementSetAutoTradeTriggered;
        public event Action<bool> OnTradeManagementSetAlertTriggered;

        public TradingEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered when the trading state is requested.
        /// This is used to get the read only trading state.
        /// </summary>
        public IReadOnlyTradingState GetTradingState()
        {
            return _eventManager.InvokeEvent(() => OnGetTradingState?.Invoke());
        }

        /// <summary>
        /// Event triggered when the strategy entry is found.
        /// This is used to set the strategy triggered state.
        /// </summary>
        public void StrategyTriggered(StrategyData strategyTriggeredData)
        {
            _eventManager.InvokeEvent(OnStrategyTriggered, strategyTriggeredData);
        }

        /// <summary>
        /// Event triggered when the found strategy entry completed updating the trading state.
        /// This is used to notify consumers that the found strategy entry has been set in the trading state.
        /// </summary>
        public void StrategyTriggeredProcessed()
        {
            _eventManager.InvokeEvent(OnStrategyTriggeredProcessed);
        }

        /// <summary>
        /// Event triggered when the order closes.
        /// This is used to notify consumers that the strategy order closed.
        /// </summary>
        public void ResetTradingState()
        {
            _eventManager.InvokeEvent(OnResetTradingState);
        }

        /// <summary>
        /// Event triggered when the Auto Enabled/Disabled button is clicked.
        /// This is used to enable or disable the auto trading.
        /// </summary>
        public void TradeManagementSetAutoTradeTriggered(bool isEnabled)
        {
            _eventManager.InvokeEvent(OnTradeManagementSetAutoTradeTriggered, isEnabled);
        }

        /// <summary>
        /// Event triggered when the Alert Enabled/Disabled button is clicked.
        /// This is used to enable or disable the alert.
        /// </summary>
        public void TradeManagementSetAlertTriggered(bool isEnabled)
        {
            _eventManager.InvokeEvent(OnTradeManagementSetAlertTriggered, isEnabled);
        }
    }
}
