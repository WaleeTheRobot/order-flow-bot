using NinjaTrader.Custom.AddOns;
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
        private Grid _directionGrid;
        private StackPanel _directionPanel;
        private TextBox _triggerStrikeTextBox;
        private Button _longButton;
        private Button _shortButton;
        private Dictionary<string, ButtonInfo> _directionButtons;

        private string _inputTextBoxBackgroundColor = "#272829";
        private const string LONG_BUTTON_LABEL = "Long";
        private const string SHORT_BUTTON_LABEL = "Short";

        private void DirectionGrid()
        {
            AddDirectionButtons();

            _directionGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 15),
            };
            _directionGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _directionGrid.ColumnDefinitions.Add(new ColumnDefinition());

            _directionGrid.RowDefinitions.Add(new RowDefinition());
            _directionGrid.RowDefinitions.Add(new RowDefinition());
            _directionGrid.RowDefinitions.Add(new RowDefinition());

            TextBlock triggerStrikePriceLabel = new TextBlock()
            {
                FontFamily = ChartControl.Properties.LabelFont.Family,
                FontSize = 11,
                Foreground = ChartControl.Properties.ChartText,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(3),
                Text = "Trigger Strike Price"
            };

            _triggerStrikeTextBox = new TextBox
            {
                Height = 30,
                Margin = new Thickness(3, 3, 3, 3),
                ToolTip = "The strike price to trigger the strategy",
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = GetSolidColorBrushFromHex(_inputTextBoxBackgroundColor)
            };

            Grid.SetRow(triggerStrikePriceLabel, 0);
            Grid.SetColumn(triggerStrikePriceLabel, 0);
            Grid.SetColumnSpan(triggerStrikePriceLabel, 2);

            Grid.SetRow(_triggerStrikeTextBox, 1);
            Grid.SetColumn(_triggerStrikeTextBox, 0);
            Grid.SetColumnSpan(_triggerStrikeTextBox, 2);

            _triggerStrikeTextBox.TextChanged += TriggerStrikeTextBox_TextChanged;
            _triggerStrikeTextBox.PreviewKeyDown += TextBox_PreviewKeyDown;

            _longButton = CreateButton(LONG_BUTTON_LABEL, LongButtonClick, 2, 0);
            _shortButton = CreateButton(SHORT_BUTTON_LABEL, ShortButtonClick, 2, 1);

            _directionGrid.Children.Add(triggerStrikePriceLabel);
            _directionGrid.Children.Add(_triggerStrikeTextBox);
            _directionGrid.Children.Add(_longButton);
            _directionGrid.Children.Add(_shortButton);

            TextBlock headerText = GetHeaderText("Trade Direction");

            _directionPanel = new StackPanel();

            _directionPanel.Children.Add(headerText);
            _directionPanel.Children.Add(_directionGrid);
        }

        private void AddDirectionButtons()
        {
            _directionButtons = new Dictionary<string, ButtonInfo>
            {
                { LONG_BUTTON_LABEL, new ButtonInfo(LongButtonClick, false, LONG_BUTTON_LABEL) },
                { SHORT_BUTTON_LABEL, new ButtonInfo(ShortButtonClick, false, SHORT_BUTTON_LABEL) }
            };
        }

        private void DisposeDirectionButtons()
        {
            if (_directionButtons == null)
                return;

            foreach (var item in _directionButtons)
            {
                string label = item.Key;
                ButtonInfo info = item.Value;

                Button button = _directionGrid.Children.OfType<Button>().FirstOrDefault(b => b.Content.ToString() == label);

                if (button != null)
                {
                    button.Click -= info.Handler;
                    _directionGrid.Children.Remove(button);
                }
            }

            _directionButtons.Clear();
        }

        private void DisableEnableDirectionButtons(bool disableValue)
        {
            ResetTradeDirection();

            foreach (var item in _directionButtons)
            {
                Button button = FindChild<Button>(_directionGrid, item.Key);

                if (button != null)
                {
                    button.IsEnabled = disableValue;
                    button.Background = GetSolidColorBrushFromHex(_buttonNeutral);
                }

                item.Value.IsActive = false;
            }

            // Disable/Enable trigger strike text box
            if (_triggerStrikeTextBox != null)
            {
                _triggerStrikeTextBox.IsEnabled = disableValue;
            }
        }

        private void DirectionButtonClick(string buttonLabel)
        {
            ButtonInfo button = _directionButtons[buttonLabel];
            bool isActive = !button.IsActive;
            button.IsActive = isActive;

            UpdateSelectedTradeDirection();

            if (buttonLabel == LONG_BUTTON_LABEL)
            {
                bool isActiveState = _orderFlowBotState.SelectedTradeDirection == Direction.Any || _orderFlowBotState.SelectedTradeDirection == Direction.Long;
                SetButtonBackground(_directionGrid, _directionButtons, isActiveState, buttonLabel);
            }
            else if (buttonLabel == SHORT_BUTTON_LABEL)
            {
                bool isActiveState = _orderFlowBotState.SelectedTradeDirection == Direction.Any || _orderFlowBotState.SelectedTradeDirection == Direction.Short;
                SetButtonBackground(_directionGrid, _directionButtons, isActiveState, buttonLabel);
            }

            ForceRefresh();
        }

        private void UpdateSelectedTradeDirection()
        {
            bool isLongActive = _directionButtons[LONG_BUTTON_LABEL].IsActive;
            bool isShortActive = _directionButtons[SHORT_BUTTON_LABEL].IsActive;

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
                SetButtonBackground(_directionGrid, _directionButtons, isLongActive, LONG_BUTTON_LABEL);
                SetButtonBackground(_directionGrid, _directionButtons, isShortActive, SHORT_BUTTON_LABEL);
                PrintOutput("Long and Short Disabled");
            }
        }

        private void LongButtonClick(object sender, RoutedEventArgs e)
        {
            if (!CheckATMStrategyLoaded())
            {
                return;
            }

            DirectionButtonClick(LONG_BUTTON_LABEL);
        }

        private void ShortButtonClick(object sender, RoutedEventArgs e)
        {
            if (!CheckATMStrategyLoaded())
            {
                return;
            }

            DirectionButtonClick(SHORT_BUTTON_LABEL);
        }

        private void ResetTradeDirection()
        {
            // Don't reset selected trade direction if auto trade is enabled
            // This will also reset the valid strategy after a trade closes
            if (_orderFlowBotState.AutoTradeEnabled)
            {
                _strategiesController.ResetValidStrategy();

                return;
            }

            ResetTriggerStrikeTextBox();
            _strategiesController.ResetTradeDirection();

            if (_orderFlowBotState.SelectedTradeDirection == Direction.Flat)
            {
                PrintOutput("Long and Short Reset");
            }
            else if (_orderFlowBotState.SelectedTradeDirection == Direction.Any)
            {
                PrintOutput("Long and Short Did Not Reset");
            }
            else if (_orderFlowBotState.SelectedTradeDirection == Direction.Long)
            {
                PrintOutput("Long Did Not Reset");
            }
            else if (_orderFlowBotState.SelectedTradeDirection == Direction.Short)
            {
                PrintOutput("Short Did Not Reset");
            }

            bool isFlat = _orderFlowBotState.SelectedTradeDirection == Direction.Flat;

            foreach (var item in _directionButtons)
            {
                ChartControl.Dispatcher.InvokeAsync(() =>
                {
                    item.Value.IsActive = !isFlat;
                    SetButtonBackground(_directionGrid, _directionButtons, !isFlat, item.Key);
                });
            }
        }

        #region Trigger Strike Price

        private void TriggerStrikeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (double.TryParse(textBox.Text, out double value))
            {
                _orderFlowBotState.TriggerStrikePrice = value;

                // Set to false and let it re-trigger
                _orderFlowBotState.StrikePriceTriggered = false;
                UpdateTriggerStrikeTextBoxBackground();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == "-")
                {
                    _orderFlowBotState.TriggerStrikePrice = 0;
                }
            }
        }

        private void ResetTriggerStrikeTextBox()
        {
            _orderFlowBotState.TriggerStrikePrice = 0;
            _orderFlowBotState.StrikePriceTriggered = false;

            if (_orderFlowBotState.TriggerStrikePrice == 0 && !_orderFlowBotState.StrikePriceTriggered)
            {
                PrintOutput("Trigger Strike Price Reset");
            }
            else
            {
                PrintOutput("Trigger Strike Price Did Not Reset");
            }

            if (_orderFlowBotState.TriggerStrikePrice == 0 && !_orderFlowBotState.StrikePriceTriggered)
            {
                ChartControl.Dispatcher.InvokeAsync(() =>
                {
                    _triggerStrikeTextBox.Text = "";
                    UpdateTriggerStrikeTextBoxBackground();
                });
            }
        }

        private void UpdateTriggerStrikeTextBoxBackground()
        {
            ChartControl.Dispatcher.InvokeAsync(() =>
            {
                if (_triggerStrikeTextBox != null && _orderFlowBotState != null)
                {
                    _triggerStrikeTextBox.Background = _orderFlowBotState.StrikePriceTriggered ? Brushes.DarkGreen : GetSolidColorBrushFromHex(_inputTextBoxBackgroundColor);
                }
            });
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

        #endregion
    }
}
