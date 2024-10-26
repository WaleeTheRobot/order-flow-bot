using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events
{
    public class UserInterfaceEvents
    {
        private readonly EventManager _eventManager;
        public event Action<bool> OnEnabledDisabledTriggered;

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
    }
}
