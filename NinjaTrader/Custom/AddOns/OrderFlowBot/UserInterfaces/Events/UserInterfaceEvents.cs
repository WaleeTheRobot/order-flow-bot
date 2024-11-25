using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
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
        public event Action OnResetStrategiesTriggered;
        public event Action<double> OnTriggerStrikePriceTriggered;
        public event Action OnResetTriggerStrikePrice;
        public event Action<string> OnAddSelectedStrategyTriggered;
        public event Action<string> OnRemoveSelectedStrategyTriggered;

        public UserInterfaceEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Triggered when the Enable/Disable button is clicked.
        /// </summary>
        /// <remarks>
        /// This method performs the following actions:
        /// <list type="bullet">
        /// <item><description>Toggles if trading is enabled or disabled by updating <see cref="TradingState.IsTradingEnabled"/>.</description></item>
        /// <item><description>Resets the <see cref="TradingState.SetInitialTriggeredState"/> to its default configuration.</description></item>
        /// <item><description>Disables all other control panel buttons except for the Enable/Disable button.</description></item>
        /// <item><description>Closes all triggered ATM open positions.</description></item>
        /// </list>
        /// </remarks>
        public void EnabledDisabledTriggered(bool isEnabled)
        {
            _eventManager.InvokeEvent(OnEnabledDisabledTriggered, isEnabled);
        }

        /// <summary>
        /// Triggered when the Auto Enabled/Disabled button is clicked.
        /// </summary>
        /// <remarks>
        /// This method performs the following actions:
        /// <list type="bullet">
        /// <item><description>Toggles if auto trading is enabled or disabled by updating <see cref="TradingState.IsAutoTradeEnabled"/>.</description></item>
        /// <item><description>Disables Reset Direction, Trigger Strike Price, Long and Short buttons.</description></item>
        /// </list>
        /// </remarks>
        public void AutoTradeTriggered(bool isEnabled)
        {
            _eventManager.InvokeEvent(OnAutoTradeTriggered, isEnabled);
        }

        /// <summary>
        /// Triggered when the Alert button is clicked.
        /// </summary>
        /// /// <remarks>
        /// This method performs the following actions:
        /// <list type="bullet">
        /// <item><description>Toggles if alert is enabled or disabled by updating <see cref="TradingState.IsAlertEnabled"/>.</description></item>
        /// <item><description>Draws a triangle and plays a sound instead of entering a position.</description></item>
        /// <item><description>Resets the <see cref="TradingState.SetInitialTriggeredState"/> to its default configuration.</description></item>
        /// </list>
        /// </remarks>
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
        /// Triggered when the Close button is clicked.
        /// </summary>
        /// <remarks>
        /// This method performs the following actions:
        /// <list type="bullet">
        /// <item><description>Closes the triggered ATM positions.</description></item>
        /// <item><description>Resets the <see cref="TradingState.SetInitialTriggeredState"/> to its default configuration.</description></item>
        /// </list>
        /// </remarks>
        public void CloseTriggered()
        {
            _eventManager.InvokeEvent(OnCloseTriggered);
        }

        /// <summary>
        /// Triggered when the Reset Direction button is clicked.
        /// </summary>
        /// <remarks>
        /// This method performs the following actions:
        /// <list type="bullet">
        /// <item><description>Resets Trigger Strike Price, Standard/Inverse, Long and Short field/buttons.</description></item>
        /// <item><description>Resets the <see cref="TradingState.SetInitialTradeDirection"/> to its default configuration.</description></item>
        /// </list>
        /// </remarks>
        public void ResetDirectionTriggered()
        {
            _eventManager.InvokeEvent(OnResetDirectionTriggered);
        }

        /// <summary>
        /// Triggered when the Reset Strategies button is clicked.
        /// </summary>
        /// <remarks>
        /// This method performs the following actions:
        /// <list type="bullet">
        /// <item><description>Resets buttons for the strategies.</description></item>
        /// <item><description>Calls <see cref="TradingState.RemoveAllSelectedStrategies"/>.</description></item>
        /// </list>
        /// </remarks>
        public void ResetStrategiesTriggered()
        {
            _eventManager.InvokeEvent(OnResetStrategiesTriggered);
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
