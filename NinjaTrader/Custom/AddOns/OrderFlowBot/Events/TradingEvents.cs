﻿using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
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
        public event Action OnCloseTriggered;

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
        /// Event triggered when the close button is clicked.
        /// This is used to notify consumers to close positions.
        /// </summary>
        public void CloseTriggered()
        {
            _eventManager.InvokeEvent(OnCloseTriggered);
        }
    }
}
