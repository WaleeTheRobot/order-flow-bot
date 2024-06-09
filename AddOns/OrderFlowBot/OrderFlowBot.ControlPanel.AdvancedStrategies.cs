using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private Grid _advancedStrategiesGrid;
        private StackPanel _advancedStrategiesPanel;
        private Dictionary<string, ButtonInfo> _strategyButtons;

        private void AdvancedStrategiesGrid()
        {
            AddStrategyButtons();

            _advancedStrategiesGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 15),
            };

            _advancedStrategiesGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _advancedStrategiesGrid.ColumnDefinitions.Add(new ColumnDefinition());

            int strategyCount = _strategyButtons.Count;
            int rows = (int)Math.Ceiling(strategyCount / 2.0);

            // Dynamically add rows based on the number of strategies
            for (int j = 0; j < rows; j++)
            {
                _advancedStrategiesGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Add strategy buttons to the grid
            int index = 0;
            foreach (var buttonInfo in _strategyButtons.Values)
            {
                int row = index / 2;
                int column = index % 2;

                Button strategyButton = CreateButton(buttonInfo.DisplayLabel, buttonInfo.Handler, row, column);

                Grid.SetRow(strategyButton, row);
                Grid.SetColumn(strategyButton, column);

                _advancedStrategiesGrid.Children.Add(strategyButton);
                index++;
            }

            TextBlock headerText = GetHeaderText("Advanced Strategies");

            _advancedStrategiesPanel = new StackPanel();

            _advancedStrategiesPanel.Children.Add(headerText);
            _advancedStrategiesPanel.Children.Add(_advancedStrategiesGrid);
        }

        private void AddStrategyButtons()
        {
            _strategyButtons = new Dictionary<string, ButtonInfo>();

            // Dynamically add buttons for each strategy with event handler
            foreach (var strategy in _strategiesConfig.StrategiesConfigList)
            {
                string buttonLabel = strategy.ButtonLabel;

                if (!string.IsNullOrEmpty(buttonLabel))
                {
                    _strategyButtons.Add(buttonLabel, new ButtonInfo(
                        (sender, e) => StrategyAdvancedButtonClick(buttonLabel, strategy.Name),
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

                SetButtonBackground(_advancedStrategiesGrid, _strategyButtons, isActive, buttonLabel);
                PrintOutput(outputMessage);
            }
        }

        private void DisableEnableAdvancedStrategyButtons()
        {
            ResetAdvancedStrategies();

            foreach (var item in _strategyButtons)
            {
                Button button = FindChild<Button>(_advancedStrategiesGrid, item.Key);

                if (button != null)
                {
                    button.IsEnabled = !_orderFlowBotState.DisableTrading;
                    button.Background = GetSolidColorBrushFromHex(_buttonNeutral);
                }

                item.Value.IsActive = false;

            }
        }

        private void ResetAdvancedStrategies()
        {
            _strategiesController.ResetStrategies();

            if (_orderFlowBotState.SelectedStrategies.Count == 0)
            {
                PrintOutput("Advanced Strategies Reset");
            }
            else
            {
                PrintOutput("Advanced Strategies Did Not Reset");
            }

            bool noSelectedStrategies = _orderFlowBotState.SelectedStrategies.Count == 0;

            foreach (var item in _strategyButtons)
            {
                item.Value.IsActive = !noSelectedStrategies;
                SetButtonBackground(_advancedStrategiesGrid, _strategyButtons, !noSelectedStrategies, item.Key);
            }
        }

        private void DisposeAdvancedStrategyButtons()
        {
            if (_strategyButtons == null)
                return;

            foreach (var item in _strategyButtons)
            {
                string label = item.Key;
                ButtonInfo info = item.Value;

                Button button = _advancedStrategiesGrid.Children.OfType<Button>().FirstOrDefault(b => b.Content.ToString() == label);

                if (button != null)
                {
                    button.Click -= info.Handler;
                    _advancedStrategiesGrid.Children.Remove(button);
                }
            }

            _strategyButtons.Clear();
        }

        private void StrategyAdvancedButtonClick(string buttonLabel, string strategyName)
        {
            if (!CheckATMStrategyLoaded())
            {
                return;
            }

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

            SetButtonBackground(_advancedStrategiesGrid, _strategyButtons, isActive, buttonLabel);
            PrintOutput(outputMessage);
            ForceRefresh();
        }
    }
}
