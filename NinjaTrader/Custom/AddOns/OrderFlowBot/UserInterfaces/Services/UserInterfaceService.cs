using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;
using System.Collections.Generic;
using Direction = NinjaTrader.Custom.AddOns.OrderFlowBot.Configs.Direction;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Services
{
    public class UserInterfaceService
    {
        private readonly TradingService _tradingService;

        private readonly UserInterfaceEvents _userInterfaceEvents;

        private readonly List<IGrid> grids;

        public UserInterfaceService(
            ServicesContainer servicesContainer,
            UserInterfaceEvents userInterfaceEvents,
            IGrid tradeManagementGrid,
            IGrid tradeDirectionGrid
        )
        {
            _tradingService = servicesContainer.TradingService;

            _userInterfaceEvents = userInterfaceEvents;
            _userInterfaceEvents.OnEnabledDisabledTriggered += HandleEnabledDisabledTriggered;
            _userInterfaceEvents.OnAutoTradeTriggered += HandleAutoTradeTriggered;
            _userInterfaceEvents.OnAlertTriggered += HandleAlertTriggered;
            _userInterfaceEvents.OnDirectionTriggered += HandleDirectionTriggered;
            _userInterfaceEvents.OnStandardTriggered += HandleStandardTriggered;
            _userInterfaceEvents.OnCloseTriggered += HandleCloseTriggered;
            _userInterfaceEvents.OnResetDirectionTriggered += HandleResetDirectionTriggered;
            _userInterfaceEvents.OnTriggerStrikePriceTriggered += HandleTriggerStrikePriceTriggered;

            grids = new List<IGrid>
            {
                tradeManagementGrid,
                tradeDirectionGrid
            };
        }

        private void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            foreach (var grid in grids)
            {
                grid?.HandleEnabledDisabledTriggered(isEnabled);
            }

            _tradingService.UpdateIsTradingEnabled(isEnabled);
        }

        private void HandleAutoTradeTriggered(bool isEnabled)
        {
            foreach (var grid in grids)
            {
                grid?.HandleAutoTradeTriggered(isEnabled);
            }

            _tradingService.UpdateIsAutoTradeEnabled(isEnabled);
            _tradingService.UpdateSelectedTradeDirection(isEnabled ? Direction.Any : Direction.Flat);

            if (isEnabled)
            {
                _tradingService.HandleTriggerStrikePriceTriggered(0);
            }
        }

        private void HandleAlertTriggered(bool isEnabled)
        {
            _tradingService.UpdateIsAlertEnabled(isEnabled);
        }

        private void HandleDirectionTriggered(Direction direction)
        {
            _tradingService.UpdateSelectedTradeDirection(direction);
        }

        private void HandleStandardTriggered(Direction direction)
        {
            _tradingService.UpdateStandardInverse(direction);
        }

        private void HandleCloseTriggered()
        {
            _tradingService.HandleCloseTriggered();
        }

        private void HandleResetDirectionTriggered()
        {
            _tradingService.HandleResetDirectionTriggered();
        }

        private void HandleTriggerStrikePriceTriggered(double price)
        {
            _tradingService.HandleTriggerStrikePriceTriggered(price);
        }
    }
}
