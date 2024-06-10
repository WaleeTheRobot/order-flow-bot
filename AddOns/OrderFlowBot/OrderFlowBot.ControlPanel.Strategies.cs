using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private Grid _strategiesGrid;
        private StackPanel _strategiesPanel;
        private Dictionary<string, ButtonInfo> _strategyButtons;

        private void StrategiesGrid()
        {
            AddStrategyButtons();

            _strategiesGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 15),
            };

            _strategiesGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _strategiesGrid.ColumnDefinitions.Add(new ColumnDefinition());

            int strategyCount = _strategyButtons.Count;
            int rows = (int)Math.Ceiling(strategyCount / 2.0);

            // Dynamically add rows based on the number of strategies
            for (int j = 0; j < rows; j++)
            {
                _strategiesGrid.RowDefinitions.Add(new RowDefinition());
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

                _strategiesGrid.Children.Add(strategyButton);
                index++;
            }

            TextBlock headerText = GetHeaderText("Strategies");

            _strategiesPanel = new StackPanel();

            _strategiesPanel.Children.Add(headerText);
            _strategiesPanel.Children.Add(_strategiesGrid);
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
            ResetStrategies();

            foreach (var item in _strategyButtons)
            {
                Button button = FindChild<Button>(_strategiesGrid, item.Key);

                if (button != null)
                {
                    button.IsEnabled = !_orderFlowBotState.DisableTrading;
                    button.Background = GetSolidColorBrushFromHex(_buttonNeutral);
                }

                item.Value.IsActive = false;

            }
        }

        private void ResetStrategies()
        {
            _strategiesController.ResetStrategies();

            if (_orderFlowBotState.SelectedStrategies.Count == 0)
            {
                PrintOutput("Strategies Reset");
            }
            else
            {
                PrintOutput("Strategies Did Not Reset");
            }

            bool noSelectedStrategies = _orderFlowBotState.SelectedStrategies.Count == 0;

            foreach (var item in _strategyButtons)
            {
                item.Value.IsActive = !noSelectedStrategies;
                SetButtonBackground(_strategiesGrid, _strategyButtons, !noSelectedStrategies, item.Key);
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

                Button button = _strategiesGrid.Children.OfType<Button>().FirstOrDefault(b => b.Content.ToString() == label);

                if (button != null)
                {
                    button.Click -= info.Handler;
                    _strategiesGrid.Children.Remove(button);
                }
            }

            _strategyButtons.Clear();
        }

        private void StrategyButtonClick(string buttonLabel, string strategyName)
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

            SetButtonBackground(_strategiesGrid, _strategyButtons, isActive, buttonLabel);
            PrintOutput(outputMessage);
            ForceRefresh();
        }
    }
}
