using NinjaTrader.Custom.AddOns;
using NinjaTrader.Gui.Chart;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        public class ButtonInfo
        {
            public RoutedEventHandler Handler { get; set; }
            public bool IsActive { get; set; }

            public ButtonInfo(RoutedEventHandler handler, bool isActive)
            {
                Handler = handler;
                IsActive = isActive;
            }
        }

        #region Variables

        private ChartTab _chartTab;
        private Chart _chartWindow;
        private Grid _chartTraderGrid, _chartTraderButtonsGrid, _orderFlowBotDirectionGrid;
        private bool _panelActive;
        private TabItem _tabItem;
        private Dictionary<string, ButtonInfo> _buttonMap;

        // Labels
        private const string LONG_BUTTON_LABEL = "Long";
        private const string SHORT_BUTTON_LABEL = "Short";

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
                        SetButtonBackground(false, buttonLabel);
                    }

                    UpdateSelectedTradeDirection();
                    PrintOutput("Strategies Disabled");
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
                FontSize = 12
            };
        }

        private void DefineButtons()
        {
            _buttonMap = new Dictionary<string, ButtonInfo>
            {
                { LONG_BUTTON_LABEL, new ButtonInfo(LongButtonClick, false) },
                { SHORT_BUTTON_LABEL, new ButtonInfo(ShortButtonClick, false) }
            };

            // Dynamically add buttons for each strategy with event handler
            foreach (var strategyIndicator in _strategiesIndicatorsConfig.StrategiesIndicatorsConfigList)
            {
                string buttonLabel = strategyIndicator.ButtonLabel;

                if (!string.IsNullOrEmpty(buttonLabel))
                {
                    _buttonMap.Add(buttonLabel, new ButtonInfo(
                        (sender, e) => StrategyButtonClick(buttonLabel, strategyIndicator.Name),
                        false
                    ));
                }
            }
        }

        private void CreateButtons()
        {
            int index = 0;

            foreach (var item in _buttonMap)
            {
                string label = item.Key;
                ButtonInfo info = item.Value;
                Button button = GetStyleButton(label);
                button.Click += info.Handler;

                if (label == LONG_BUTTON_LABEL)
                {
                    Grid.SetRow(button, 1);
                    Grid.SetColumn(button, 0);
                }
                else if (label == SHORT_BUTTON_LABEL)
                {
                    Grid.SetRow(button, 1);
                    Grid.SetColumn(button, 1);
                }
                else
                {
                    // For any other button, set it to take the entire row
                    Grid.SetRow(button, index);
                    Grid.SetColumnSpan(button, 2);
                }

                _orderFlowBotDirectionGrid.Children.Add(button);

                index++;
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

            Button button = _orderFlowBotDirectionGrid.Children.OfType<Button>().FirstOrDefault(b => b.Content.ToString() == buttonLabel);

            if (button != null)
            {
                if (isActive)
                {
                    button.Background = new SolidColorBrush(Colors.DodgerBlue);
                }
                else
                {
                    button.Background = new SolidColorBrush(Colors.Transparent);
                }
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

        private void LongButtonClick(object sender, RoutedEventArgs e)
        {
            DirectionButtonClick(LONG_BUTTON_LABEL);
        }

        private void ShortButtonClick(object sender, RoutedEventArgs e)
        {
            DirectionButtonClick(SHORT_BUTTON_LABEL);
        }

        private void StrategyButtonClick(string buttonLabel, string strategyName)
        {
            if (!_buttonMap.TryGetValue(buttonLabel, out ButtonInfo button))
            {
                PrintOutput("Button not found: " + buttonLabel);
                return;
            }

            bool isActive = !button.IsActive;
            button.IsActive = isActive;

            string outputMessage = isActive ? $"{strategyName} Enabled" : $"{strategyName} Disabled";

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

            // Rows subtract 1 because Long and Short buttons are in one row
            for (int i = 0; i <= _buttonMap.Count - 1; i++)
            {
                _orderFlowBotDirectionGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Label
            TextBlock orderFlowLabel = new TextBlock()
            {
                FontFamily = ChartControl.Properties.LabelFont.Family,
                FontSize = 13,
                Foreground = ChartControl.Properties.ChartText,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 50, 0, 5),
                Text = string.Format("Order Flow Bot")
            };

            Grid.SetRow(orderFlowLabel, 0);
            Grid.SetColumnSpan(orderFlowLabel, 2);

            _orderFlowBotDirectionGrid.Children.Add(orderFlowLabel);

            CreateButtons();
        }

        private void CreateWPFControls()
        {
            _chartWindow = Window.GetWindow(ChartControl.Parent) as Gui.Chart.Chart;

            if (_chartWindow == null)
                return;

            // Chart Trader area grid
            _chartTraderGrid = (_chartWindow.FindFirst("ChartWindowChartTraderControl") as ChartTrader).Content as System.Windows.Controls.Grid;

            // Existing Chart Trader buttons
            _chartTraderButtonsGrid = _chartTraderGrid.Children[0] as Grid;

            SetOrderFlowDirectionGrid();

            if (TabSelected())
                InsertWPFControls();

            _chartWindow.MainTabControl.SelectionChanged += TabChangedHandler;
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