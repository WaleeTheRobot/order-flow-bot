using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private Grid _strategiesGrid;
        private Dictionary<string, ButtonInfo> _strategyButtons;

        private void StrategiesGrid()
        {
            AddStrategyButtons();

            _topGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 10)
            };

            Grid.SetColumnSpan(_topGrid, 2);

            _topGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _topGrid.ColumnDefinitions.Add(new ColumnDefinition());

            _topGrid.RowDefinitions.Add(new RowDefinition());
            _topGrid.RowDefinitions.Add(new RowDefinition());

            TextBlock orderFlowLabel = new TextBlock()
            {
                FontFamily = ChartControl.Properties.LabelFont.Family,
                FontSize = 15,
                Foreground = ChartControl.Properties.ChartText,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(3, 50, 3, 3),
                Text = BackTestingEnabled ? "Order Flow Bot Panel Disabled" : "Order Flow Bot"
            };

            Grid.SetRow(orderFlowLabel, 0);
            Grid.SetColumnSpan(orderFlowLabel, 2);

            _topGrid.Children.Add(orderFlowLabel);
        }

        private void AddStrategyButtons()
        {
            // Dynamically add buttons for each strategy with event handler
            foreach (var strategy in _strategiesConfig.StrategiesConfigList)
            {
                string buttonLabel = strategy.ButtonLabel;

                if (!string.IsNullOrEmpty(buttonLabel))
                {
                    _strategyButtons.Add(buttonLabel, new ButtonInfo(
                        (sender, e) => StrategyButtonClick(buttonLabel, strategy.Name),
                        false,
                        buttonLabel,
                        strategy.Name
                    ));
                }
            }
        }

        private void UpdateStrategyButtons()
        {
            foreach (string buttonLabel in _strategyButtons.Keys)
            {
                string strategyName = _strategyButtons[buttonLabel].Name;
                bool isActive = _strategiesController.StrategyExists(strategyName);
                string outputMessage = isActive ? String.Format("{0} Enabled", strategyName) : String.Format("{0} Disabled", strategyName);

                SetButtonBackground(_strategiesGrid, _strategyButtons, isActive, buttonLabel);
                PrintOutput(outputMessage);
            }
        }

        private void DisableEnableStrategyButtons()
        {
            foreach (var pair in _strategyButtons)
            {
                Button button = FindChild<Button>(_strategiesGrid, pair.Key);

                if (button != null)
                {
                    button.IsEnabled = !_orderFlowBotState.DisableTrading;
                    button.Background = new SolidColorBrush(Colors.DimGray);
                }

                pair.Value.IsActive = false;
            }
        }

        private void DisposeStrategyButtons()
        {
            if (_strategyButtons == null)
                return;

            foreach (var item in _strategyButtons)
            {
                string label = item.Key;
                ButtonInfo info = item.Value;

                Button button = _mainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Content.ToString() == label);

                if (button != null)
                {
                    button.Click -= info.Handler;
                    _mainGrid.Children.Remove(button);
                }
            }

            _strategyButtons.Clear();
        }

        private void StrategyButtonClick(string buttonLabel, string strategyName)
        {
            ButtonInfo button;
            if (!_strategyButtons.TryGetValue(buttonLabel, out button))
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

            SetButtonBackground(_strategiesGrid, _strategyButtons, isActive, buttonLabel);
            PrintOutput(outputMessage);
            ForceRefresh();
        }
    }
}
