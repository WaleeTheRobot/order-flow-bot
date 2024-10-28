using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components.Controls;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components
{
    public class TradeManagementGrid : GridBase
    {
        public TradeManagementGrid(
            ServicesContainer servicesContainer,
            UserInterfaceEvents userInterfaceEvents
        ) : base("Trade Management", servicesContainer, userInterfaceEvents)
        {
        }

        public override void InitializeInitialToggleState()
        {
            initialToggleState = new Dictionary<string, bool>
            {
                { ButtonName.ENABLED, true },
                { ButtonName.AUTO, false },
                { ButtonName.ALERT, false },
                { ButtonName.CLOSE, false },
                { ButtonName.RESET_DIRECTION, false },
                { ButtonName.RESET_STRATEGIES, false },
            };
        }

        protected override void AddButtons()
        {
            var buttonModels = new List<ButtonModel>
            {
                new ButtonModel
                {
                    Name = ButtonName.ENABLED,
                    Content = "Disabled",
                    ToggledContent = "Enabled",
                    BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_YELLOW_COLOR,
                    ToggledBackgroundColor = CustomColors.BUTTON_RED_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = true,
                    InitialToggleState = initialToggleState[ButtonName.ENABLED]
                },
                new ButtonModel
                {
                    Name = ButtonName.AUTO,
                    Content = "Auto Disabled",
                    ToggledContent = "Auto Enabled",
                    BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                    ToggledBackgroundColor = CustomColors.BUTTON_BG_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = true,
                    InitialToggleState = initialToggleState[ButtonName.AUTO]
                },
                new ButtonModel
                {
                    Name = ButtonName.ALERT,
                    Content = "Alert Disabled",
                    ToggledContent = "Alert Enabled",
                    BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                    ToggledBackgroundColor = CustomColors.BUTTON_BG_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = true,
                    InitialToggleState = initialToggleState[ButtonName.ALERT]
                },
                new ButtonModel
                {
                    Name = ButtonName.CLOSE,
                    Content = "Close",
                    BackgroundColor = CustomColors.BUTTON_BG_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = initialToggleState[ButtonName.CLOSE]
                },
                new ButtonModel
                {
                    Name = ButtonName.RESET_DIRECTION,
                    Content = "Reset Direction",
                    BackgroundColor = CustomColors.BUTTON_BG_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = initialToggleState[ButtonName.RESET_DIRECTION]
                },
                new ButtonModel
                {
                    Name = ButtonName.RESET_STRATEGIES,
                    Content = "Reset Strategies",
                    BackgroundColor = CustomColors.BUTTON_BG_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = initialToggleState[ButtonName.RESET_STRATEGIES]
                }
            };

            for (int i = 0; i < buttonModels.Count; i++)
            {
                var config = buttonModels[i];
                var button = new CustomButton(config).Button;
                buttons[config.Name] = button;

                // +1 because row 0 is the heading
                int row = (i / 2) + 1;
                int column = i % 2;

                AddButtonToGrid(button, row, column);
            }
        }

        public override void HandleButtonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ButtonState state = (ButtonState)button.Tag;
            string buttonName = state.Config.Name;

            switch (buttonName)
            {
                case ButtonName.ENABLED:
                    userInterfaceEvents.EnabledDisabledTriggered(state.IsToggled);
                    break;

                case ButtonName.AUTO:
                    userInterfaceEvents.AutoTradeTriggered(state.IsToggled);
                    break;

                case ButtonName.ALERT:
                    userInterfaceEvents.AlertTriggered(state.IsToggled);
                    break;

                case ButtonName.CLOSE:
                    userInterfaceEvents.CloseTriggered();
                    userInterfaceEvents.ResetTriggerStrikePrice();
                    break;

                case ButtonName.RESET_DIRECTION:
                    // Implement reset direction functionality
                    break;

                case ButtonName.RESET_STRATEGIES:
                    // Implement reset strategies functionality
                    break;

                default:
                    throw new ArgumentException($"Unknown button tag: {buttonName}");
            }
        }

        public override void HandleAutoTradeTriggered(bool isEnabled)
        {
            SetButtonEnabled(buttons[ButtonName.RESET_DIRECTION], !isEnabled);
        }
    }
}
