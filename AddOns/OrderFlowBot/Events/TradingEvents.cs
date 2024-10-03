using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class TradingEvents
    {
        private readonly EventManager _eventManager;
        public event Func<TradingState> OnGetTradingState;

        public TradingEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public TradingState GetTradingState()
        {
            return _eventManager.InvokeEvent(() => OnGetTradingState?.Invoke());
        }
    }
}
