using NinjaTrader.Cbi;
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
        private Button _disableButton;
        private Button _resetDirectionButton;
        private Button _resetAdvancedStrategiesButton;
        private Button _resetSimpleStrategiesButton;
        private Button _autoButton;
        private Button _closeButton;
        private Dictionary<string, ButtonInfo> _tradeManagementButtons;

        private const string DISABLE_BUTTON_LABEL = "Enabled";
        private const string AUTO_BUTTON_LABEL = "Auto";
        private const string CLOSE_BUTTON_LABEL = "Close";
        private const string RESET_DIRECTION_BUTTON_LABEL = "Reset Direction";
        private const string RESET_ADVANCED_BUTTON_LABEL = "Reset Advanced";
        private const string RESET_SIMPLE_BUTTON_LABEL = "Reset Simple";

        private void TradeManagementGrid()
        {
            AddTradeManagementButtons();

            _tradeManagementGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 10)
            };

            _tradeManagementGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _tradeManagementGrid.ColumnDefinitions.Add(new ColumnDefinition());

            _tradeManagementGrid.RowDefinitions.Add(new RowDefinition());
            _tradeManagementGrid.RowDefinitions.Add(new RowDefinition());
            _tradeManagementGrid.RowDefinitions.Add(new RowDefinition());

            _disableButton = CreateButton(DISABLE_BUTTON_LABEL, DisableButtonClick, 0, 0);
            _autoButton = CreateButton(AUTO_BUTTON_LABEL, AutoButtonClick, 0, 1);
            _resetDirectionButton = CreateButton(RESET_DIRECTION_BUTTON_LABEL, ResetDirectionButtonClick, 1, 0);
            _resetAdvancedStrategiesButton = CreateButton(RESET_ADVANCED_BUTTON_LABEL, ResetAdvancedButtonClick, 1, 1);
            _resetSimpleStrategiesButton = CreateButton(RESET_SIMPLE_BUTTON_LABEL, ResetSimpleButtonClick, 2, 0);
            _closeButton = CreateButton(CLOSE_BUTTON_LABEL, CloseButtonClick, 2, 1);

            _tradeManagementGrid.Children.Add(_disableButton);
            _tradeManagementGrid.Children.Add(_autoButton);
            _tradeManagementGrid.Children.Add(_resetDirectionButton);
            _tradeManagementGrid.Children.Add(_resetAdvancedStrategiesButton);
            _tradeManagementGrid.Children.Add(_resetSimpleStrategiesButton);
            _tradeManagementGrid.Children.Add(_closeButton);
        }

        private void AddTradeManagementButtons()
        {
            _tradeManagementButtons = new Dictionary<string, ButtonInfo>
            {
                { DISABLE_BUTTON_LABEL, new ButtonInfo(DisableButtonClick, false, DISABLE_BUTTON_LABEL) },
                { AUTO_BUTTON_LABEL, new ButtonInfo(AutoButtonClick, false, AUTO_BUTTON_LABEL) },
                { CLOSE_BUTTON_LABEL, new ButtonInfo(DisableButtonClick, false, CLOSE_BUTTON_LABEL) },
                { RESET_DIRECTION_BUTTON_LABEL, new ButtonInfo(AutoButtonClick, false, RESET_DIRECTION_BUTTON_LABEL) },
                { RESET_ADVANCED_BUTTON_LABEL, new ButtonInfo(AutoButtonClick, false, RESET_ADVANCED_BUTTON_LABEL) },
                { RESET_SIMPLE_BUTTON_LABEL, new ButtonInfo(AutoButtonClick, false, RESET_SIMPLE_BUTTON_LABEL) }
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
            foreach (var pair in _tradeManagementButtons)
            {
                if (pair.Key != DISABLE_BUTTON_LABEL)
                {
                    Button button = FindChild<Button>(_tradeManagementGrid, pair.Key);

                    if (button != null)
                    {
                        button.IsEnabled = !_orderFlowBotState.DisableTrading;
                        button.Background = new SolidColorBrush(Colors.DimGray);
                    }

                    pair.Value.IsActive = false;
                }
            }
        }

        private void DisableButtonClick(object sender, RoutedEventArgs e)
        {
            if (Position.MarketPosition == MarketPosition.Flat && AtmPosition() == MarketPosition.Flat)
            {
                bool currentDisableState = !_tradeManagementButtons[DISABLE_BUTTON_LABEL].IsActive;

                _orderFlowBotState.DisableTrading = currentDisableState;
                _strategiesController.ResetStrategies();

                _tradeManagementButtons[DISABLE_BUTTON_LABEL].IsActive = _orderFlowBotState.DisableTrading;

                // Update disable button
                Button disableButton = FindChild<Button>(_tradeManagementGrid, DISABLE_BUTTON_LABEL);
                disableButton.Content = _orderFlowBotState.DisableTrading ? "Disabled" : DISABLE_BUTTON_LABEL;
                disableButton.Background = _orderFlowBotState.DisableTrading ? new SolidColorBrush(Colors.DarkRed) : new SolidColorBrush(Colors.DimGray);

                DisableEnableTradeManagementButtons();
                DisableEnableDirectionButtons();
                // DisableEnableStrategyButtons();

                PrintOutput(_orderFlowBotState.DisableTrading ? "Trading Disabled" : "Trading Enabled");

                ForceRefresh();
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            CloseAtmPosition();
            PrintOutput("ATM Positions Closed");
        }

        private void ResetDirectionButtonClick(object sender, RoutedEventArgs e)
        {
            ResetDirection();
            ResetTriggerStrikeTextBox();
        }

        private void ResetAdvancedButtonClick(object sender, RoutedEventArgs e)
        {
            /*_strategiesController.ResetStrategies();

            foreach (var pair in _tradeManagementButtons)
            {
                pair.Value.IsActive = false;
                SetButtonBackground(_tradeManagementButtons, false, pair.Key);
            }

            ClearTriggerStrikeTextBox();*/

            PrintOutput("Advanced Strategies Reset");
        }

        private void ResetSimpleButtonClick(object sender, RoutedEventArgs e)
        {
            /*_strategiesController.ResetStrategies();

            foreach (var pair in _tradeManagementButtons)
            {
                pair.Value.IsActive = false;
                SetButtonBackground(_tradeManagementButtons, false, pair.Key);
            }

            ClearTriggerStrikeTextBox();*/

            PrintOutput("Simple Strategies Reset");
        }

        private void AutoButtonClick(object sender, RoutedEventArgs e)
        {
            PrintOutput("Auto clicked.");

            if (!CheckATMStrategyLoaded())
            {
                return;
            }
        }
    }
}
