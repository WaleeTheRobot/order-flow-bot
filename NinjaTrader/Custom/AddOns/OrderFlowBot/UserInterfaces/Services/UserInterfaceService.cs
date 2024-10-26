using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Services
{
    public class UserInterfaceService
    {
        private readonly EventManager _eventManager;
        private readonly UserInterfaceEvents _userInterfaceEvents;

        private readonly TradeManagementGrid _tradeManagementGrid;

        public UserInterfaceService(
            EventManager eventManager,
            UserInterfaceEvents userInterfaceEvents,
            TradeManagementGrid tradeManagementGrid
        )
        {
            _eventManager = eventManager;

            _userInterfaceEvents = userInterfaceEvents;
            _userInterfaceEvents.OnEnabledDisabledTriggered += HandleEnabledDisabledTriggered;

            _tradeManagementGrid = tradeManagementGrid;
        }

        private void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            _tradeManagementGrid.HandleEnabledDisabledTriggered(isEnabled);
        }
    }
}
