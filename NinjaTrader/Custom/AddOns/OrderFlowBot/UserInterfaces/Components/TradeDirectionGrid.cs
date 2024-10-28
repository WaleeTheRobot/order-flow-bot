using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components.Controls;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Models;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components
{
    public class TradeDirectionGrid : GridBase
    {
        TextBox _triggerStrikeTextBox;

        public TradeDirectionGrid(
            ServicesContainer servicesContainer,
            UserInterfaceEvents userInterfaceEvents
        ) : base("Trade Direction", servicesContainer, userInterfaceEvents)
        {
            userInterfaceEvents.OnResetTriggerStrikePrice += HandleResetTriggerStrikePrice;
        }

        public override void Ready()
        {
            base.Ready();
            _triggerStrikeTextBox.IsEnabled = true;
        }

        public override void InitializeInitialToggleState()
        {
            initialToggleState = new Dictionary<string, bool>
            {
                { ButtonName.STANDARD, false },
                { ButtonName.LONG, false },
                { ButtonName.SHORT, false },
            };
        }

        public override void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            base.HandleEnabledDisabledTriggered(isEnabled);
            _triggerStrikeTextBox.IsEnabled = isEnabled;
        }

        protected override void InitializeGrid()
        {
            base.InitializeGrid();

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());

            AddTriggerStrikePrice();
        }

        private void AddTriggerStrikePrice()
        {
            TextBlock triggerStrikePriceLabel = new TextBlock()
            {
                FontSize = 11,
                Foreground = UserInterfaceUtils.GetSolidColorBrushFromHex(CustomColors.TEXT_COLOR),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(3, 3, 3, 3),
                Text = "Trigger Strike Price"
            };

            _triggerStrikeTextBox = new NumericTextBox("The strike price to trigger the strategy").TextBox;
            _triggerStrikeTextBox.TextChanged += (s, e) =>
            {
                string input = _triggerStrikeTextBox.Text;

                // Set to 0 if empty
                if (string.IsNullOrWhiteSpace(input))
                {
                    userInterfaceEvents.TriggerStrikePriceTriggered(0);
                    return;
                }

                // Allow input input after starting with decimal
                if (input == ".")
                {
                    // Don't set strike price to 0 yet, let the user continue typing
                    return;
                }

                double strikePrice;

                if (double.TryParse(input, out strikePrice))
                {
                    userInterfaceEvents.TriggerStrikePriceTriggered(strikePrice);
                }
                else
                {
                    // Set to 0 if invalid
                    userInterfaceEvents.TriggerStrikePriceTriggered(0);
                }
            };

            grid.Children.Add(triggerStrikePriceLabel);
            Grid.SetRow(triggerStrikePriceLabel, 1);
            Grid.SetColumn(triggerStrikePriceLabel, 0);
            Grid.SetColumnSpan(triggerStrikePriceLabel, 2);

            grid.Children.Add(_triggerStrikeTextBox);
            Grid.SetRow(_triggerStrikeTextBox, 2);
            Grid.SetColumn(_triggerStrikeTextBox, 0);
            Grid.SetColumnSpan(_triggerStrikeTextBox, 2);
        }

        protected override void AddButtons()
        {
            var buttonModels = new List<ButtonModel>
            {
                new ButtonModel
                {
                    Name = ButtonName.STANDARD,
                    Content = "Standard",
                    ToggledContent = "Inverse",
                    BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                    ToggledBackgroundColor = CustomColors.BUTTON_BG_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = true,
                    InitialToggleState = initialToggleState[ButtonName.STANDARD]
                },
                new ButtonModel
                {
                    Name = ButtonName.LONG,
                    Content = "Long",
                    ToggledContent = "Long",
                    BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                    ToggledBackgroundColor = CustomColors.BUTTON_BG_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = true,
                    InitialToggleState = initialToggleState[ButtonName.LONG]
                },
                new ButtonModel
                {
                    Name = ButtonName.SHORT,
                    Content = "Short",
                    ToggledContent = "Short",
                    BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                    HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                    ToggledBackgroundColor = CustomColors.BUTTON_BG_COLOR,
                    TextColor = CustomColors.TEXT_COLOR,
                    ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                    IsToggleable = true,
                    InitialToggleState = initialToggleState[ButtonName.SHORT]
                }
            };

            // Handle two-column span for Standard button
            for (int i = 0; i < buttonModels.Count; i++)
            {
                var config = buttonModels[i];
                if (config.Name == ButtonName.STANDARD)
                {
                    var button = new CustomButton(config).Button;
                    buttons[config.Name] = button;

                    // Starting at row 3 since row 0 is heading, row 1 is label, row 2 is text box
                    int row = 3;
                    AddButtonToGrid(button, row, 0);
                    Grid.SetColumnSpan(button, 2);

                    break;
                }
            }

            // Handle one-column span for remaining buttons
            int buttonIndex = 0;
            for (int i = 0; i < buttonModels.Count; i++)
            {
                var config = buttonModels[i];
                if (config.Name != ButtonName.STANDARD)
                {
                    var button = new CustomButton(config).Button;
                    buttons[config.Name] = button;

                    // Start at row 4 below the Standard button
                    int row = (buttonIndex / 2) + 4;
                    int column = buttonIndex % 2;

                    AddButtonToGrid(button, row, column);
                    buttonIndex++;
                }
            }
        }

        public override void HandleButtonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ButtonState state = (ButtonState)button.Tag;
            string buttonName = state.Config.Name;

            switch (buttonName)
            {
                case ButtonName.STANDARD:
                    userInterfaceEvents.StandardTriggered
                    (
                        state.IsToggled ? Direction.Inverse : Direction.Standard
                    );
                    break;
                case ButtonName.LONG:
                    HandleLongShortButtonClick();

                    break;
                case ButtonName.SHORT:
                    HandleLongShortButtonClick();
                    break;

                default:
                    throw new ArgumentException($"Unknown button tag: {buttonName}");
            }
        }

        private void HandleLongShortButtonClick()
        {
            ButtonState longState = (ButtonState)buttons[ButtonName.LONG].Tag;
            ButtonState shortState = (ButtonState)buttons[ButtonName.SHORT].Tag;

            bool isLongEnabled = longState.IsToggled;
            bool isShortEnabled = shortState.IsToggled;

            switch ((isLongEnabled, isShortEnabled))
            {
                case (true, true):
                    userInterfaceEvents.DirectionTriggered(Direction.Any);
                    break;
                case (true, false):
                    userInterfaceEvents.DirectionTriggered(Direction.Long);
                    break;
                case (false, true):
                    userInterfaceEvents.DirectionTriggered(Direction.Short);
                    break;
                default:
                    userInterfaceEvents.DirectionTriggered(Direction.Flat);
                    break;
            }
        }

        public override void HandleAutoTradeTriggered(bool isEnabled)
        {
            _triggerStrikeTextBox.IsEnabled = !isEnabled;
            _triggerStrikeTextBox.Text = "";

            // Reset the initial toggle state and enable/disable
            foreach (var buttonName in buttons.Keys)
            {
                ButtonState buttonState = (ButtonState)buttons[buttonName].Tag;

                if (buttonName == ButtonName.STANDARD)
                {
                    continue;
                }

                if (initialToggleState.ContainsKey(buttonName))
                {
                    buttonState.IsToggled = initialToggleState[buttonName];
                }

                SetButtonEnabled(buttons[buttonName], !isEnabled);
            }
        }

        private void HandleResetTriggerStrikePrice()
        {
            _triggerStrikeTextBox.Text = "";
        }
    }
}
