using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns;
using NinjaTrader.Gui.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        public class ButtonInfo
        {
            public RoutedEventHandler Handler { get; set; }
            public bool IsActive { get; set; }
            public string Name { get; set; }
            public string DisplayLabel { get; set; }

            public ButtonInfo(RoutedEventHandler handler, bool isActive, string displayLabel, string name = "")
            {
                Handler = handler;
                IsActive = isActive;
                Name = name;
                DisplayLabel = displayLabel;
            }
        }

        #region Variables

        private ChartTab _chartTab;
        private Chart _chartWindow;
        private Grid _chartTraderGrid, _chartTraderButtonsGrid, _orderFlowBotDirectionGrid;
        private bool _panelActive;
        private TabItem _tabItem;
        private Dictionary<string, ButtonInfo> _buttonMap;
        private TextBox _triggerStrikeTextBox;
        private Button _resetButton;

        // Labels
        private const string LONG_BUTTON_LABEL = "Long";
        private const string SHORT_BUTTON_LABEL = "Short";
        private const string CLOSE_BUTTON_LABEL = "Close";
        private const string DISABLE_BUTTON_LABEL = "Enabled";
        private const string RESET_BUTTON_LABEL = "Reset";

        #endregion

        private void ControlPanelSetStateDataLoaded()
        {
            if (ChartControl != null)
            {
                ChartControl.Dispatcher.InvokeAsync(() =>
                {
                    CreateWPFControls();
                });
            }
        }

        private void ControlPanelSetStateTerminated()
        {
            if (ChartControl != null)
            {
                ChartControl.Dispatcher.InvokeAsync(() =>
                {
                    DisposeWPFControls();
                });
            }
        }

        private void ControlPanelOnExecutionUpdate()
        {
            if (ChartControl != null)
            {
                ChartControl.Dispatcher.InvokeAsync((() =>
                {
                    foreach (var buttonLabel in _buttonMap.Keys)
                    {
                        if (buttonLabel != LONG_BUTTON_LABEL && buttonLabel != SHORT_BUTTON_LABEL && buttonLabel != DISABLE_BUTTON_LABEL && buttonLabel != CLOSE_BUTTON_LABEL)
                        {
                            var strategyName = _buttonMap[buttonLabel].Name;
                            var isActive = _strategiesController.StrategyExists(strategyName);
                            string outputMessage = isActive ? String.Format("{0} Enabled", strategyName) : String.Format("{0} Disabled", strategyName);

                            SetButtonBackground(isActive, buttonLabel);
                            PrintOutput(outputMessage);
                        }
                        else
                        {
                            SetButtonBackground(false, buttonLabel);
                        }
                    }

                    UpdateSelectedTradeDirection();
                }));
            }
        }

        #region Buttons

        private Button GetStyleButton(string label)
        {
            Style basicButtonStyle = Application.Current.FindResource("BasicEntryButton") as Style;

            return new Button()
            {
                Content = label,
                Height = 30,
                Margin = new Thickness(3, 3, 3, 3),
                Padding = new Thickness(0, 0, 0, 0),
                Style = basicButtonStyle,
                FontSize = 12,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };
        }

        private void DefineButtons()
        {
            _buttonMap = new Dictionary<string, ButtonInfo>
            {
                { LONG_BUTTON_LABEL, new ButtonInfo(LongButtonClick, false, LONG_BUTTON_LABEL) },
                { SHORT_BUTTON_LABEL, new ButtonInfo(ShortButtonClick, false, SHORT_BUTTON_LABEL) },
                { DISABLE_BUTTON_LABEL, new ButtonInfo(DisableButtonClick, false, DISABLE_BUTTON_LABEL) },
                { CLOSE_BUTTON_LABEL, new ButtonInfo(CloseButtonClick, false, CLOSE_BUTTON_LABEL) }
            };

            // Dynamically add buttons for each strategy with event handler
            foreach (var strategy in _strategiesConfig.StrategiesConfigList)
            {
                string buttonLabel = strategy.ButtonLabel;

                if (!string.IsNullOrEmpty(buttonLabel))
                {
                    _buttonMap.Add(buttonLabel, new ButtonInfo(
                        (sender, e) => StrategyButtonClick(buttonLabel, strategy.Name),
                        false,
                        buttonLabel,
                        strategy.Name
                    ));
                }
            }
        }

        private void CreateButtons()
        {
            int index = 0;

            foreach (var item in _buttonMap)
            {
                string tag = item.Key;
                ButtonInfo info = item.Value;
                Button button = GetStyleButton(tag);
                button.Click += info.Handler;
                button.Tag = tag;
                button.Background = new SolidColorBrush(Colors.DimGray);

                if (BackTestingEnabled)
                {
                    button.IsEnabled = false;
                }

                // Set the Grid row and column based on the button tag
                switch (tag)
                {
                    case LONG_BUTTON_LABEL:
                        Grid.SetRow(button, 2);
                        Grid.SetColumn(button, 0);
                        break;
                    case SHORT_BUTTON_LABEL:
                        Grid.SetRow(button, 2);
                        Grid.SetColumn(button, 1);
                        break;
                    case DISABLE_BUTTON_LABEL:
                        Grid.SetRow(button, 3);
                        Grid.SetColumn(button, 0);
                        break;
                    case CLOSE_BUTTON_LABEL:
                        Grid.SetRow(button, 3);
                        Grid.SetColumn(button, 1);
                        break;
                    default:
                        // For any other button, set it to take the entire row
                        Grid.SetRow(button, index + 4);
                        Grid.SetColumnSpan(button, 2);
                        index++;
                        break;
                }

                _orderFlowBotDirectionGrid.Children.Add(button);
            }
        }

        private void PrintOutput(string message)
        {
            TriggerCustomEvent(o =>
            {
                Print(string.Format("{0} | {1}", ToTime(Time[0]), message));
            }, null);
        }

        private void SetButtonBackground(bool isActive, string buttonLabel)
        {
            if (_buttonMap.ContainsKey(buttonLabel))
            {
                _buttonMap[buttonLabel].IsActive = isActive;
            }

            Button button = _orderFlowBotDirectionGrid.Children.OfType<Button>().FirstOrDefault(b => Equals(b.Tag, buttonLabel));

            if (button != null)
            {
                button.Background = isActive ? new SolidColorBrush(Colors.DodgerBlue) : new SolidColorBrush(Colors.DimGray);
            }
        }

        // Direction Buttons
        private void UpdateSelectedTradeDirection()
        {
            bool isLongActive = _buttonMap[LONG_BUTTON_LABEL].IsActive;
            bool isShortActive = _buttonMap[SHORT_BUTTON_LABEL].IsActive;

            if (isLongActive && isShortActive)
            {
                _orderFlowBotState.SelectedTradeDirection = Direction.Any;
                PrintOutput("Long and Short Enabled");
            }
            else if (isLongActive)
            {
                _orderFlowBotState.SelectedTradeDirection = Direction.Long;
                PrintOutput("Long Enabled");
            }
            else if (isShortActive)
            {
                _orderFlowBotState.SelectedTradeDirection = Direction.Short;
                PrintOutput("Short Enabled");
            }
            else
            {
                _orderFlowBotState.SelectedTradeDirection = Direction.Flat;
                SetButtonBackground(isLongActive, LONG_BUTTON_LABEL);
                SetButtonBackground(isShortActive, SHORT_BUTTON_LABEL);
                PrintOutput("Long and Short Disabled");
            }
        }

        private void DirectionButtonClick(string buttonLabel)
        {
            ButtonInfo button = _buttonMap[buttonLabel];
            bool isActive = !button.IsActive;
            button.IsActive = isActive;

            UpdateSelectedTradeDirection();
            SetButtonBackground(isActive, buttonLabel);
            ForceRefresh();
        }

        private void DisableClick()
        {
            // Disable if flat
            if (Position.MarketPosition == MarketPosition.Flat && AtmPosition() == MarketPosition.Flat)
            {
                bool disableAll = !_buttonMap[DISABLE_BUTTON_LABEL].IsActive;

                // Toggle the activation state of the disable button
                _buttonMap[DISABLE_BUTTON_LABEL].IsActive = disableAll;
                // Update the state for trading
                _orderFlowBotState.DisableTrading = disableAll;
                _strategiesController.ResetStrategies();

                // Find the disable button and update its properties
                Button disableButton = _orderFlowBotDirectionGrid.Children.OfType<Button>().FirstOrDefault(b => Equals(b.Tag, DISABLE_BUTTON_LABEL));
                if (disableButton != null)
                {
                    disableButton.Content = disableAll ? "Disabled" : DISABLE_BUTTON_LABEL;
                    disableButton.Background = disableAll ? new SolidColorBrush(Colors.DarkRed) : new SolidColorBrush(Colors.DimGray);
                }

                // Update all other buttons
                foreach (var pair in _buttonMap)
                {
                    // Exclude the disable button itself
                    if (pair.Key != DISABLE_BUTTON_LABEL)
                    {
                        Button button = _orderFlowBotDirectionGrid.Children.OfType<Button>().FirstOrDefault(b => Equals(b.Tag, pair.Key));
                        if (button != null)
                        {
                            button.IsEnabled = !disableAll;
                            button.Background = new SolidColorBrush(Colors.DimGray);
                        }

                        pair.Value.IsActive = false;
                    }
                }

                // Disable/Enable the trigger strike price TextBox and reset Button
                if (_triggerStrikeTextBox != null)
                {
                    _triggerStrikeTextBox.IsEnabled = !disableAll;
                }

                if (_resetButton != null)
                {
                    _resetButton.IsEnabled = !disableAll;
                }

                PrintOutput(disableAll ? "Disable Trading Selected" : "Enable Trading Selected");
                ForceRefresh();
            }
        }

        private void LongButtonClick(object sender, RoutedEventArgs e)
        {
            DirectionButtonClick(LONG_BUTTON_LABEL);
        }

        private void ShortButtonClick(object sender, RoutedEventArgs e)
        {
            DirectionButtonClick(SHORT_BUTTON_LABEL);
        }

        private void DisableButtonClick(object sender, RoutedEventArgs e)
        {
            DisableClick();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            CloseAtmPosition();
        }

        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            _strategiesController.ResetStrategies();

            foreach (var pair in _buttonMap)
            {
                pair.Value.IsActive = false;
                SetButtonBackground(false, pair.Key);
            }

            ClearTriggerStrikeTextBox();

            PrintOutput("Reset button clicked. All inputs cleared and buttons reset.");
        }

        private void StrategyButtonClick(string buttonLabel, string strategyName)
        {
            ButtonInfo button;
            if (!_buttonMap.TryGetValue(buttonLabel, out button))
            {
                PrintOutput("Button not found: " + buttonLabel);
                return;
            }

            bool isActive = !button.IsActive;
            button.IsActive = isActive;

            string outputMessage = isActive ? String.Format("{0} Enabled", strategyName) : String.Format("{0} Disabled", strategyName);

            if (isActive)
            {
                _strategiesController.AddSelectedStrategy(strategyName);
            }
            else
            {
                _strategiesController.RemoveSelectedStrategy(strategyName);
            }

            SetButtonBackground(isActive, buttonLabel);
            PrintOutput(outputMessage);
            ForceRefresh();
        }

        #endregion

        private void SetOrderFlowDirectionGrid()
        {
            _orderFlowBotDirectionGrid = new Grid();

            // Two columns
            Grid.SetColumnSpan(_orderFlowBotDirectionGrid, 2);

            _orderFlowBotDirectionGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _orderFlowBotDirectionGrid.ColumnDefinitions.Add(new ColumnDefinition());

            DefineButtons();

            _orderFlowBotDirectionGrid.RowDefinitions.Add(new RowDefinition());
            _orderFlowBotDirectionGrid.RowDefinitions.Add(new RowDefinition());

            // Rows subtract 1 because Long and Short buttons are in one row
            for (int i = 0; i <= _buttonMap.Count - 1; i++)
            {
                _orderFlowBotDirectionGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Label
            TextBlock orderFlowLabel = new TextBlock()
            {
                FontFamily = ChartControl.Properties.LabelFont.Family,
                FontSize = 15,
                Foreground = ChartControl.Properties.ChartText,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(3, 50, 3, 3),
                Text = BackTestingEnabled ? string.Format("Order Flow Bot Panel Disabled") : string.Format("Order Flow Bot")
            };

            Grid.SetRow(orderFlowLabel, 0);
            Grid.SetColumnSpan(orderFlowLabel, 2);

            _orderFlowBotDirectionGrid.Children.Add(orderFlowLabel);

            CreateButtons();
        }

        private void TriggerStrikeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _orderFlowBotState.StrikePriceTriggered = false;

            TextBox textBox = sender as TextBox;
            if (double.TryParse(textBox.Text, out double value))
            {
                _orderFlowBotState.TriggerStrikePrice = value;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == "-")
                {
                    _orderFlowBotState.TriggerStrikePrice = 0;
                }
            }
        }

        private void ClearTriggerStrikeTextBox()
        {
            _orderFlowBotState.TriggerStrikePrice = 0;
            _orderFlowBotState.StrikePriceTriggered = false;

            ChartControl.Dispatcher.InvokeAsync(() =>
            {
                _triggerStrikeTextBox.Text = "";
                _triggerStrikeTextBox.BorderBrush = Brushes.Transparent;
                _triggerStrikeTextBox.BorderThickness = new Thickness(1);
            });
        }

        private void UpdateTriggerStrikeTextBoxBorder()
        {
            ChartControl.Dispatcher.InvokeAsync(() =>
            {
                _triggerStrikeTextBox.BorderBrush = _orderFlowBotState.StrikePriceTriggered ? Brushes.DarkGreen : Brushes.Transparent;
                _triggerStrikeTextBox.BorderThickness = _orderFlowBotState.StrikePriceTriggered ? new Thickness(2) : new Thickness(1);
            });
        }

        private void CreateWPFControls()
        {
            _chartWindow = Window.GetWindow(ChartControl.Parent) as Gui.Chart.Chart;
            if (_chartWindow == null) return;

            // Chart Trader area grid
            _chartTraderGrid = (_chartWindow.FindFirst("ChartWindowChartTraderControl") as ChartTrader).Content as Grid;

            // Existing Chart Trader buttons
            _chartTraderButtonsGrid = _chartTraderGrid.Children[0] as Grid;

            SetOrderFlowDirectionGrid();

            if (TabSelected()) InsertWPFControls();

            _chartWindow.MainTabControl.SelectionChanged += TabChangedHandler;

            // Create a container grid for the TextBox and Reset Button
            Grid containerGrid = new Grid();
            containerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            containerGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Define rows for the container grid
            containerGrid.RowDefinitions.Add(new RowDefinition()); // Row for the label
            containerGrid.RowDefinitions.Add(new RowDefinition()); // Row for the TextBox and Reset Button

            // Labels
            TextBlock triggerStrikePriceLabel = new TextBlock()
            {
                FontFamily = ChartControl.Properties.LabelFont.Family,
                FontSize = 11,
                Foreground = ChartControl.Properties.ChartText,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(3),
                Text = string.Format("Trigger Strike Price")
            };

            // Create TextBox for Trigger Strike Price
            _triggerStrikeTextBox = new TextBox
            {
                Height = 31,
                Margin = new Thickness(3),
                ToolTip = "Trigger Strike Price",
                VerticalContentAlignment = VerticalAlignment.Center
            };

            // Position the label in the container grid
            Grid.SetRow(triggerStrikePriceLabel, 0);
            Grid.SetColumn(triggerStrikePriceLabel, 0);
            Grid.SetColumnSpan(triggerStrikePriceLabel, 2);

            // Position the TextBox and Reset Button in the same row
            Grid.SetRow(_triggerStrikeTextBox, 1);
            Grid.SetColumn(_triggerStrikeTextBox, 0);

            containerGrid.Children.Add(triggerStrikePriceLabel);
            containerGrid.Children.Add(_triggerStrikeTextBox);

            // Create Reset Button with the same style as other buttons using GetStyleButton method
            _resetButton = GetStyleButton(RESET_BUTTON_LABEL);
            _resetButton.Click += ResetButtonClick;
            _resetButton.Background = new SolidColorBrush(Colors.DimGray);
            Grid.SetRow(_resetButton, 1);
            Grid.SetColumn(_resetButton, 1);
            containerGrid.Children.Add(_resetButton);

            // Add the container grid to the main grid
            Grid.SetRow(containerGrid, 1); // Set to the second row, below the label
            Grid.SetColumnSpan(containerGrid, 2); // Span across both columns
            _orderFlowBotDirectionGrid.Children.Add(containerGrid);

            // Attach event to handle text changes
            _triggerStrikeTextBox.TextChanged += TriggerStrikeTextBox_TextChanged;
            _triggerStrikeTextBox.PreviewKeyDown += TextBox_PreviewKeyDown;

            // Adjusting the buttons to start from the third row
            foreach (var item in _orderFlowBotDirectionGrid.Children.OfType<Button>())
            {
                int currentRow = Grid.GetRow(item);
                Grid.SetRow(item, currentRow + 1);
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBoxSender = (TextBox)sender;

            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                int selectionStart = textBoxSender.SelectionStart;
                int selectionLength = textBoxSender.SelectionLength;

                // If there's a selection, delete the selected text
                if (selectionLength > 0)
                {
                    textBoxSender.Text = textBoxSender.Text.Remove(selectionStart, selectionLength);
                    textBoxSender.SelectionStart = selectionStart;
                }
                else
                {
                    // Handle backspace or delete key press
                    if (e.Key == Key.Back && selectionStart > 0)
                    {
                        textBoxSender.Text = textBoxSender.Text.Remove(selectionStart - 1, 1);
                        textBoxSender.SelectionStart = selectionStart - 1;
                    }
                    else if (e.Key == Key.Delete && selectionStart < textBoxSender.Text.Length)
                    {
                        textBoxSender.Text = textBoxSender.Text.Remove(selectionStart, 1);
                        textBoxSender.SelectionStart = selectionStart;
                    }
                }
                e.Handled = true;
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9 && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 && Keyboard.IsKeyToggled(Key.NumLock)))
            {
                // Handle numeric key press
                char num = (char)('0' + (e.Key - (e.Key >= Key.NumPad0 ? Key.NumPad0 : Key.D0)));
                ReplaceText(textBoxSender, num.ToString());
                e.Handled = true;
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                if (!textBoxSender.Text.Contains('.'))
                {
                    ReplaceText(textBoxSender, ".");
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                if (textBoxSender.SelectionStart == 0 && !textBoxSender.Text.StartsWith("-"))
                {
                    ReplaceText(textBoxSender, "-");
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void ReplaceText(TextBox textBox, string newText)
        {
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            // Replace selected text
            if (selectionLength > 0)
            {
                textBox.Text = textBox.Text.Remove(selectionStart, selectionLength).Insert(selectionStart, newText);
                textBox.SelectionStart = selectionStart + newText.Length;
            }
            else
            {
                // Insert new text at the current cursor position
                textBox.Text = textBox.Text.Insert(selectionStart, newText);
                textBox.SelectionStart = selectionStart + newText.Length;
            }
        }


        private void DisposeWPFControls()
        {
            if (_chartWindow != null)
                _chartWindow.MainTabControl.SelectionChanged -= TabChangedHandler;

            if (_buttonMap == null)
                return;

            foreach (var item in _buttonMap)
            {
                string label = item.Key;
                ButtonInfo info = item.Value;

                Button button = _orderFlowBotDirectionGrid.Children.OfType<Button>().FirstOrDefault(b => b.Content.ToString() == label);

                if (button != null)
                {
                    button.Click -= info.Handler;
                }
            }

            if (_triggerStrikeTextBox != null)
            {
                _triggerStrikeTextBox.TextChanged -= TriggerStrikeTextBox_TextChanged;
                _triggerStrikeTextBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                if (UserControlCollection.Contains(_triggerStrikeTextBox))
                    UserControlCollection.Remove(_triggerStrikeTextBox);
            }

            if (_resetButton != null)
            {
                _resetButton.Click -= ResetButtonClick;
                if (UserControlCollection.Contains(_resetButton))
                    UserControlCollection.Remove(_resetButton);
            }

            RemoveWPFControls();
        }

        private void InsertWPFControls()
        {
            if (_panelActive)
                return;

            Grid.SetRow(_orderFlowBotDirectionGrid, (_chartTraderGrid.RowDefinitions.Count - 1));
            _chartTraderGrid.Children.Add(_orderFlowBotDirectionGrid);

            _panelActive = true;
        }

        private void RemoveWPFControls()
        {
            if (!_panelActive)
                return;

            if (_chartTraderButtonsGrid != null || _orderFlowBotDirectionGrid != null)
            {
                _chartTraderGrid.Children.Remove(_orderFlowBotDirectionGrid);
            }

            _panelActive = false;
        }

        private bool TabSelected()
        {
            bool tabSelected = false;

            // Loop through each tab and see if the tab this indicator is added to is the selected item
            foreach (TabItem tab in _chartWindow.MainTabControl.Items)
                if ((tab.Content as ChartTab).ChartControl == ChartControl && tab == _chartWindow.MainTabControl.SelectedItem)
                    tabSelected = true;

            return tabSelected;
        }

        private void TabChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
                return;

            _tabItem = e.AddedItems[0] as TabItem;
            if (_tabItem == null)
                return;

            _chartTab = _tabItem.Content as ChartTab;
            if (_chartTab == null)
                return;

            if (TabSelected())
                InsertWPFControls();
            else
                RemoveWPFControls();
        }
    }
}