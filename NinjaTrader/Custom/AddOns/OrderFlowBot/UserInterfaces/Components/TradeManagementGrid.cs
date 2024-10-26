using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
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
    public class TradeManagementGrid : Grid, IComponentSetup
    {
        private readonly TradingEvents _tradingEvents;
        private readonly UserInterfaceEvents _userInterfaceEvents;
        private Button _enabledButton, _autoButton, _resetDirectionButton, _resetStrategiesButton, _alertButton, _closeButton;
        private readonly Dictionary<string, Button> _buttons;
        private Grid _buttonGrid;

        public TradeManagementGrid(EventsContainer eventsContainer, UserInterfaceEvents userInterfaceEvents)
        {
            _tradingEvents = eventsContainer.TradingEvents;

            _userInterfaceEvents = userInterfaceEvents;
            _userInterfaceEvents.OnEnabledDisabledTriggered += HandleEnabledDisabledTriggered;

            _buttons = new Dictionary<string, Button>();
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();

            _buttonGrid = new Grid
            {
                Margin = new Thickness(4, 4, 0, 0)
            };
            _buttonGrid.ColumnDefinitions.Add(col1);
            _buttonGrid.ColumnDefinitions.Add(col2);

            AddLabel();
            CreateButtons();
            AddButtonsToDictionary();
            AddButtonsToGrid();

            this.Children.Add(_buttonGrid);
        }

        public void Ready()
        {
            SetAllButtonsEnabled(true);
        }

        private void AddLabel()
        {
            TextBlock headingLabel = TextHeadingLabelUtils.GetHeadingLabel("Trade Management");

            _buttonGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            _buttonGrid.Children.Add(headingLabel);
            Grid.SetColumnSpan(headingLabel, 2);
        }

        #region Buttons
        private void CreateButtons()
        {
            _enabledButton = ButtonUtils.GetButton(new ButtonModel
            {
                Content = "Disabled",
                ToggledContent = "Enabled",
                BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                HoverBackgroundColor = CustomColors.BUTTON_YELLOW_COLOR,
                ToggledBackgroundColor = CustomColors.BUTTON_RED_COLOR,
                TextColor = CustomColors.TEXT_COLOR,
                ClickHandler = (Action<object, RoutedEventArgs>)HandleEnabledButtonClick,
                IsToggleable = true,
                InitialToggleState = true
            });

            _autoButton = ButtonUtils.GetButton(new ButtonModel
            {
                Content = "Auto Disabled",
                ToggledContent = "Auto Enabled",
                BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                ToggledBackgroundColor = CustomColors.BUTTON_BG_COLOR,
                TextColor = CustomColors.TEXT_COLOR,
                ClickHandler = (Action<object, RoutedEventArgs>)HandleAutoButtonClick,
                IsToggleable = true,
                InitialToggleState = false
            });

            _alertButton = ButtonUtils.GetButton(new ButtonModel
            {
                Content = "Alert Disabled",
                ToggledContent = "Alert Enabled",
                BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                ToggledBackgroundColor = CustomColors.BUTTON_BG_COLOR,
                TextColor = CustomColors.TEXT_COLOR,
                ClickHandler = (Action<object, RoutedEventArgs>)HandleAlertButtonClick,
                IsToggleable = true,
                InitialToggleState = false
            });

            _closeButton = ButtonUtils.GetButton(new ButtonModel
            {
                Content = "Close",
                BackgroundColor = CustomColors.BUTTON_BG_COLOR,
                HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                TextColor = CustomColors.TEXT_COLOR,
                ClickHandler = (Action<object, RoutedEventArgs>)HandleCloseButtonClick,
                IsToggleable = false
            });

            _resetDirectionButton = ButtonUtils.GetButton(new ButtonModel
            {
                Content = "Reset Direction",
                BackgroundColor = CustomColors.BUTTON_BG_COLOR,
                HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                TextColor = CustomColors.TEXT_COLOR,
                ClickHandler = (Action<object, RoutedEventArgs>)HandleResetDirectionButtonClick,
                IsToggleable = false
            });

            _resetStrategiesButton = ButtonUtils.GetButton(new ButtonModel
            {
                Content = "Reset Strategies",
                BackgroundColor = CustomColors.BUTTON_BG_COLOR,
                HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                TextColor = CustomColors.TEXT_COLOR,
                ClickHandler = (Action<object, RoutedEventArgs>)HandleStrategiesButtonClick,
                IsToggleable = false
            });
        }

        private void AddButtonsToDictionary()
        {
            _buttons["Enabled"] = _enabledButton;
            _buttons["Auto"] = _autoButton;
            _buttons["Alert"] = _alertButton;
            _buttons["Close"] = _closeButton;
            _buttons["ResetDirection"] = _resetDirectionButton;
            _buttons["ResetStrategies"] = _resetStrategiesButton;
        }

        private void AddButtonsToGrid()
        {
            AddButtonToGrid(_buttonGrid, _enabledButton, 1, 0);
            AddButtonToGrid(_buttonGrid, _autoButton, 1, 1);

            AddButtonToGrid(_buttonGrid, _alertButton, 2, 0);
            AddButtonToGrid(_buttonGrid, _closeButton, 2, 1);

            AddButtonToGrid(_buttonGrid, _resetDirectionButton, 3, 0);
            AddButtonToGrid(_buttonGrid, _resetStrategiesButton, 3, 1);
        }

        private void AddButtonToGrid(Grid grid, Button button, int row, int column)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Children.Add(button);
            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
        }

        public void SetButtonEnabled(string buttonName, bool isEnabled)
        {
            if (_buttons.ContainsKey(buttonName))
            {
                _buttons[buttonName].IsEnabled = isEnabled;
            }
        }

        public void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            ButtonState autoState = (ButtonState)_buttons["Auto"].Tag;
            ButtonState alertState = (ButtonState)_buttons["Alert"].Tag;
            autoState.IsToggled = false;
            alertState.IsToggled = false;

            foreach (var buttonPair in _buttons)
            {
                if (buttonPair.Key != "Enabled")
                {
                    buttonPair.Value.IsEnabled = isEnabled;
                }
            }
        }

        private void SetAllButtonsEnabled(bool isEnabled)
        {
            foreach (var button in _buttons.Values)
            {
                button.IsEnabled = isEnabled;
            }
        }

        #endregion

        #region Button Handlers

        private void HandleEnabledButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ButtonState state = (ButtonState)button.Tag;

            _userInterfaceEvents.EnabledDisabledTriggered(state.IsToggled);
        }

        private void HandleAutoButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ButtonState state = (ButtonState)button.Tag;
            _tradingEvents.TradeManagementSetAutoTradeTriggered(state.IsToggled);
        }

        private void HandleAlertButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ButtonState state = (ButtonState)button.Tag;
            _tradingEvents.TradeManagementSetAlertTriggered(state.IsToggled);
        }

        private void HandleCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            //_eventsContainer.EventManager.PrintMessage("Close Button Clicked");
        }

        private void HandleResetDirectionButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _ = (ButtonState)button.Tag;
            // _eventsContainer.EventManager.PrintMessage("Reset Direction Clicked");
        }

        private void HandleStrategiesButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            //_eventsContainer.EventManager.PrintMessage("Strategies Button Clicked");
        }

        #endregion
    }
}
