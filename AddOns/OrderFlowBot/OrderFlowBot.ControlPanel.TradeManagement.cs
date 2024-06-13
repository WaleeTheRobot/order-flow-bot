using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private Grid _tradeManagementGrid;
        private StackPanel _tradeManagementPanel;
        private Button _disableButton;
        private Button _resetDirectionButton;
        private Button _resetStrategiesButton;
        private Button _autoButton;
        private Button _closeButton;
        private Dictionary<string, ButtonInfo> _tradeManagementButtons;

        private const string DISABLE_BUTTON_LABEL = "Enabled";
        private const string AUTO_BUTTON_LABEL = "Auto";
        private const string CLOSE_BUTTON_LABEL = "Close";
        private const string RESET_DIRECTION_BUTTON_LABEL = "Reset Direction";
        private const string RESET_STRATEGIES_BUTTON_LABEL = "Reset Strategies";

        private void TradeManagementGrid()
        {
            AddTradeManagementButtons();

            _tradeManagementGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 15),
            };

            _tradeManagementGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _tradeManagementGrid.ColumnDefinitions.Add(new ColumnDefinition());

            _tradeManagementGrid.RowDefinitions.Add(new RowDefinition());
            _tradeManagementGrid.RowDefinitions.Add(new RowDefinition());
            _tradeManagementGrid.RowDefinitions.Add(new RowDefinition());

            _disableButton = CreateButton(DISABLE_BUTTON_LABEL, DisableButtonClick, 0, 0);
            _autoButton = CreateButton(AUTO_BUTTON_LABEL, AutoButtonClick, 0, 1);
            _resetDirectionButton = CreateButton(RESET_DIRECTION_BUTTON_LABEL, ResetDirectionButtonClick, 1, 0);
            _resetStrategiesButton = CreateButton(RESET_STRATEGIES_BUTTON_LABEL, ResetStrategiesButtonClick, 1, 1);
            _closeButton = CreateButton(CLOSE_BUTTON_LABEL, CloseButtonClick, 2, 0);

            _tradeManagementGrid.Children.Add(_disableButton);
            _tradeManagementGrid.Children.Add(_autoButton);
            _tradeManagementGrid.Children.Add(_resetDirectionButton);
            _tradeManagementGrid.Children.Add(_resetStrategiesButton);
            _tradeManagementGrid.Children.Add(_closeButton);


            TextBlock headerText = GetHeaderText("Trade Management");

            _tradeManagementPanel = new StackPanel();

            _tradeManagementPanel.Children.Add(headerText);
            _tradeManagementPanel.Children.Add(_tradeManagementGrid);
        }

        private void AddTradeManagementButtons()
        {
            _tradeManagementButtons = new Dictionary<string, ButtonInfo>
            {
                { DISABLE_BUTTON_LABEL, new ButtonInfo(DisableButtonClick, false, DISABLE_BUTTON_LABEL) },
                { AUTO_BUTTON_LABEL, new ButtonInfo(AutoButtonClick, false, AUTO_BUTTON_LABEL) },
                { CLOSE_BUTTON_LABEL, new ButtonInfo(DisableButtonClick, false, CLOSE_BUTTON_LABEL) },
                { RESET_DIRECTION_BUTTON_LABEL, new ButtonInfo(AutoButtonClick, false, RESET_DIRECTION_BUTTON_LABEL) },
                { RESET_STRATEGIES_BUTTON_LABEL, new ButtonInfo(AutoButtonClick, false, RESET_STRATEGIES_BUTTON_LABEL) }
            };
        }

        private void DisposeManagementButtons()
        {
            if (_tradeManagementButtons == null)
                return;

            foreach (var item in _tradeManagementButtons)
            {
                string label = item.Key;
                ButtonInfo info = item.Value;

                Button button = _tradeManagementGrid.Children.OfType<Button>().FirstOrDefault(b => b.Content.ToString() == label);

                if (button != null)
                {
                    button.Click -= info.Handler;
                    _tradeManagementGrid.Children.Remove(button);
                }
            }

            _tradeManagementButtons.Clear();
        }

        private void DisableEnableTradeManagementButtons()
        {
            foreach (var item in _tradeManagementButtons)
            {
                if (item.Key != DISABLE_BUTTON_LABEL)
                {
                    Button button = FindChild<Button>(_tradeManagementGrid, item.Key);

                    if (button != null)
                    {
                        button.IsEnabled = !_orderFlowBotState.DisableTrading;
                        button.Background = GetSolidColorBrushFromHex(_buttonNeutral);
                    }

                    item.Value.IsActive = false;
                }
            }
        }

        private void DisableButtonClick(object sender, RoutedEventArgs e)
        {
            if (Position.MarketPosition == MarketPosition.Flat && AtmIsFlat())
            {
                bool currentDisableState = !_tradeManagementButtons[DISABLE_BUTTON_LABEL].IsActive;

                _orderFlowBotState.DisableTrading = currentDisableState;
                _strategiesController.ResetStrategies();

                _tradeManagementButtons[DISABLE_BUTTON_LABEL].IsActive = _orderFlowBotState.DisableTrading;

                // Update disable button
                Button disableButton = FindChild<Button>(_tradeManagementGrid, DISABLE_BUTTON_LABEL);
                disableButton.Content = _orderFlowBotState.DisableTrading ? "Disabled" : DISABLE_BUTTON_LABEL;
                disableButton.Background = _orderFlowBotState.DisableTrading ? new SolidColorBrush(Colors.DarkRed) : GetSolidColorBrushFromHex(_buttonNeutral);

                // Disable Auto Trade
                _orderFlowBotState.AutoTradeEnabled = false;

                DisableEnableTradeManagementButtons();
                DisableEnableDirectionButtons(!_orderFlowBotState.DisableTrading);
                DisableEnableStrategyButtons();

                PrintOutput(_orderFlowBotState.DisableTrading ? "Trading Disabled" : "Trading Enabled");

                ForceRefresh();
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            CloseAtmPosition();
            PrintOutput("ATM Position Closed");

            ResetTradeDirection();
        }

        private void ResetDirectionButtonClick(object sender, RoutedEventArgs e)
        {
            ResetTradeDirection();
        }

        private void ResetStrategiesButtonClick(object sender, RoutedEventArgs e)
        {
            ResetStrategies();
        }

        private void AutoButtonClick(object sender, RoutedEventArgs e)
        {
            if (!CheckATMStrategyLoaded())
            {
                return;
            }

            if (Position.MarketPosition == MarketPosition.Flat && AtmIsFlat())
            {
                bool currentDisableState = !_tradeManagementButtons[AUTO_BUTTON_LABEL].IsActive;

                _orderFlowBotState.AutoTradeEnabled = currentDisableState;

                // Reset directions before checking
                DisableEnableDirectionButtons(!_orderFlowBotState.AutoTradeEnabled);

                _orderFlowBotState.SelectedTradeDirection = _orderFlowBotState.AutoTradeEnabled ? Direction.Any : Direction.Flat;
                _tradeManagementButtons[AUTO_BUTTON_LABEL].IsActive = _orderFlowBotState.AutoTradeEnabled;

                // Update auto button
                Button autoButton = FindChild<Button>(_tradeManagementGrid, AUTO_BUTTON_LABEL);
                autoButton.Background = _orderFlowBotState.AutoTradeEnabled ? new SolidColorBrush(Colors.DarkGreen) : GetSolidColorBrushFromHex(_buttonNeutral);

                Button resetDirectionButton = FindChild<Button>(_tradeManagementGrid, RESET_DIRECTION_BUTTON_LABEL);

                if (resetDirectionButton != null)
                {
                    resetDirectionButton.IsEnabled = !_orderFlowBotState.AutoTradeEnabled;
                    resetDirectionButton.Background = GetSolidColorBrushFromHex(_buttonNeutral);
                }

                PrintOutput(_orderFlowBotState.AutoTradeEnabled ? "Auto Trading Enabled" : "Auto Trading Disabled");
                Print(string.Format("Trade Direction: {0}", _orderFlowBotState.SelectedTradeDirection));

                ForceRefresh();
            }
        }
    }
}
