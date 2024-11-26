using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class TradingEvents
    {
        private readonly EventManager _eventManager;
        public event Func<IReadOnlyTradingState> OnGetTradingState;
        public event Action<IStrategyData> OnStrategyTriggered;
        public event Action OnStrategyTriggeredProcessed;
        public event Action OnResetTriggeredTradingState;
        public event Action OnCloseTriggered;
        public event Action<int> OnLastTradedBarNumberTriggered;
        public event Action<int> OnCurrentBarNumberTriggered;
        public event Action<bool> OnMarketPositionTriggered;
        public event Action OnResetTriggerStrikePrice;
        public event Action OnResetSelectedTradeDirection;
        public event Action OnPositionClosedWithAutoDisabled;

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
        public void StrategyTriggered(IStrategyData strategyTriggeredData)
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
        /// Used to reset the triggered trading state.
        /// </summary>
        public void ResetTriggeredTradingState()
        {
            _eventManager.InvokeEvent(OnResetTriggeredTradingState);
        }

        /// <summary>
        /// Used to close positions.
        /// </summary>
        public void CloseTriggered()
        {
            _eventManager.InvokeEvent(OnCloseTriggered);
        }

        /// <summary>
        /// Event triggered when the last traded bar number is triggered.
        /// This is used to notify consumers with the last traded bar number.
        /// </summary>
        public void LastTradedBarNumberTriggered(int barNumber)
        {
            _eventManager.InvokeEvent(OnLastTradedBarNumberTriggered, barNumber);
        }

        /// <summary>
        /// Event triggered when the current bar number is triggered.
        /// This is used to notify consumers with the current bar number.
        /// </summary>
        public void CurrentBarNumberTriggered(int barNumber)
        {
            _eventManager.InvokeEvent(OnCurrentBarNumberTriggered, barNumber);
        }

        /// <summary>
        /// Event triggered OFB enters a position.
        /// Helps confirm that there is an actual position.
        /// </summary>
        public void MarketPositionTriggered(bool hasMarketPosition)
        {
            _eventManager.InvokeEvent(OnMarketPositionTriggered, hasMarketPosition);
        }

        /// <summary>
        /// Used to rest the trigger strike price.
        /// </summary>
        public void ResetTriggerStrikePrice()
        {
            _eventManager.InvokeEvent(OnResetTriggerStrikePrice);
        }

        /// <summary>
        /// Used to reset the selected trade direction.
        /// </summary>
        public void ResetSelectedTradeDirection()
        {
            _eventManager.InvokeEvent(OnResetSelectedTradeDirection);
        }

        /// <summary>
        /// Triggered when the position exits and auto is disabled.
        /// </summary>
        /// <remarks>
        /// This method performs the following user interface actions:
        /// <list type="bullet">
        /// <item><description>Resets the trigger strike price.</description></item>
        /// <item><description>Resets selected trade direction.</description></item>
        /// </list>
        /// </remarks>
        public void PositionClosedWithAutoDisabled()
        {
            _eventManager.InvokeEvent(OnPositionClosedWithAutoDisabled);
        }
    }
}
