using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events
{
    // Events triggered by UI interaction
    public class UserInterfaceEvents
    {
        private readonly EventManager _eventManager;
        public event Action<bool> OnEnabledDisabledTriggered;
        public event Action<bool> OnAutoTradeTriggered;
        public event Action<bool> OnAlertTriggered;
        public event Action<Direction> OnDirectionTriggered;
        public event Action<Direction> OnStandardTriggered;
        public event Action OnCloseTriggered;
        public event Action OnResetDirectionTriggered;
        public event Action<double> OnTriggerStrikePriceTriggered;
        public event Action OnResetTriggerStrikePrice;
        public event Action<string> OnAddSelectedStrategyTriggered;
        public event Action<string> OnRemoveSelectedStrategyTriggered;

        public UserInterfaceEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered when the Enabled/Disabled button is clicked.
        /// This is used to enable or disable the control panel.
        /// </summary>
        public void EnabledDisabledTriggered(bool isEnabled)
        {
            _eventManager.InvokeEvent(OnEnabledDisabledTriggered, isEnabled);
        }

        /// <summary>
        /// Event triggered when the Auto Enabled/Disabled button is clicked.
        /// This is used to enable or disable the auto trading.
        /// </summary>
        public void AutoTradeTriggered(bool isEnabled)
        {
            _eventManager.InvokeEvent(OnAutoTradeTriggered, isEnabled);
        }

        /// <summary>
        /// Event triggered when the Alert Enabled/Disabled button is clicked.
        /// This is used to enable or disable the alert.
        /// </summary>
        public void AlertTriggered(bool isEnabled)
        {
            _eventManager.InvokeEvent(OnAlertTriggered, isEnabled);
        }

        /// <summary>
        /// Event triggered when the Standard/Inverse button is clicked.
        /// This is used to enable trades for standard or inverse.
        /// </summary>
        public void StandardTriggered(Direction direction)
        {
            _eventManager.InvokeEvent(OnStandardTriggered, direction);
        }

        /// <summary>
        /// Event triggered when the Long/Short button is clicked.
        /// This is used to enable trades for the selected option.
        /// </summary>
        public void DirectionTriggered(Direction direction)
        {
            _eventManager.InvokeEvent(OnDirectionTriggered, direction);
        }

        /// <summary>
        /// Event triggered when the Close button is clicked.
        /// This is used to close positions.
        /// </summary>
        public void CloseTriggered()
        {
            _eventManager.InvokeEvent(OnCloseTriggered);
        }

        /// <summary>
        /// Event triggered for resetting Trade Direction section.
        /// This is used to reset the Trade Direction section.
        /// </summary>
        public void ResetDirectionTriggered()
        {
            _eventManager.InvokeEvent(OnResetDirectionTriggered);
        }

        /// <summary>
        /// Event triggered when Trigger Strike Price changes.
        /// This is used to update the trigger strike price in the trading state.
        /// </summary>
        public void TriggerStrikePriceTriggered(double price)
        {
            _eventManager.InvokeEvent(OnTriggerStrikePriceTriggered, price);
        }

        /// <summary>
        /// Event triggered for resetting Trigger Strike Price.
        /// This is used to reset the Trigger Strike Price.
        /// </summary>
        public void ResetTriggerStrikePrice()
        {
            _eventManager.InvokeEvent(OnResetTriggerStrikePrice);
        }

        /// <summary>
        /// Event triggered for adding a selected strategy.
        /// This is used add a selected strategy.
        /// </summary>
        public void AddSelectedStrategyTriggered(string name)
        {
            _eventManager.InvokeEvent(OnAddSelectedStrategyTriggered, name);
        }

        /// <summary>
        /// Event triggered for removing a selected strategy.
        /// This is used remove a selected strategy.
        /// </summary>
        public void RemoveSelectedStrategyTriggered(string name)
        {
            _eventManager.InvokeEvent(OnRemoveSelectedStrategyTriggered, name);
        }
    }
}
