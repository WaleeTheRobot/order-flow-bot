using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class TradingEvents
    {
        private readonly EventManager _eventManager;
        public event Func<TradingState> OnGetTradingState;
        public event Action<StrategyTriggeredDataProvider> OnStrategyTriggered;
        public event Action OnStrategyTriggeredProcessed;
        public event Action OnResetTradingState;

        public TradingEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public TradingState GetTradingState()
        {
            return _eventManager.InvokeEvent(() => OnGetTradingState?.Invoke());
        }

        public void StrategyTriggered(StrategyTriggeredDataProvider strategyTriggeredDataProvider)
        {
            _eventManager.InvokeEvent(OnStrategyTriggered, strategyTriggeredDataProvider);
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
