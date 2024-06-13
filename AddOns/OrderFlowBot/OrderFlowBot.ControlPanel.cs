using NinjaTrader.Gui.Chart;
using System;
using System.Collections.Generic;
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

        private ChartTab _chartTab;
        private Chart _chartWindow;
        private Grid _chartTraderGrid, _chartTraderButtonsGrid, _mainGrid, _topGrid;
        private bool _panelActive;
        private TabItem _tabItem;

        // Colors
        private string _buttonNeutral = "#6c757d";
        private string _buttonActive = "#284b63";

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
                    UpdateStrategyButtons();
                }));
            }
        }

        private void CreateWPFControls()
        {
            _chartWindow = Window.GetWindow(ChartControl.Parent) as Gui.Chart.Chart;

            if (_chartWindow == null)
            {
                return;
            }

            // Chart Trader area grid
            _chartTraderGrid = (_chartWindow.FindFirst("ChartWindowChartTraderControl") as ChartTrader).Content as Grid;

            if (_chartTraderGrid == null)
            {
                return;
            }

            // Existing Chart Trader buttons
            _chartTraderButtonsGrid = _chartTraderGrid.Children[0] as Grid;

            if (_chartTraderButtonsGrid == null)
            {
                return;
            }

            // Create main grid with rows for each section
            _mainGrid = new Grid
            {
                Margin = new Thickness(0, 50, 0, 0),
                Background = GetSolidColorBrushFromHex("#32373b")
            };

            _mainGrid.RowDefinitions.Add(new RowDefinition());
            _mainGrid.RowDefinitions.Add(new RowDefinition());
            _mainGrid.RowDefinitions.Add(new RowDefinition());
            _mainGrid.RowDefinitions.Add(new RowDefinition());

            TopGrid();
            Grid.SetRow(_topGrid, 0);
            _mainGrid.Children.Add(_topGrid);

            if (!BackTestingEnabled)
            {
                TradeManagementGrid();
                DirectionGrid();
                StrategiesGrid();

                Grid.SetRow(_tradeManagementPanel, 1);
                Grid.SetRow(_directionPanel, 2);
                Grid.SetRow(_strategiesPanel, 3);

                _mainGrid.Children.Add(_tradeManagementPanel);
                _mainGrid.Children.Add(_directionPanel);
                _mainGrid.Children.Add(_strategiesPanel);
            }

            if (TabSelected()) InsertWPFControls();

            _chartWindow.MainTabControl.SelectionChanged += TabChangedHandler;
        }

        private void TopGrid()
        {
            _topGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 10),
                Background = GetSolidColorBrushFromHex(_buttonActive)
            };

            Grid.SetColumnSpan(_topGrid, 2);

            _topGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _topGrid.ColumnDefinitions.Add(new ColumnDefinition());

            _topGrid.RowDefinitions.Add(new RowDefinition());
            _topGrid.RowDefinitions.Add(new RowDefinition());

            TextBlock orderFlowLabel = new TextBlock()
            {
                FontFamily = ChartControl.Properties.LabelFont.Family,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = ChartControl.Properties.ChartText,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(3, 3, 3, 3),
                Text = BackTestingEnabled ? "OrderFlowBot Panel Disabled" : "OrderFlowBot"
            };

            Grid.SetRow(orderFlowLabel, 0);
            Grid.SetColumnSpan(orderFlowLabel, 2);

            _topGrid.Children.Add(orderFlowLabel);
        }

        private void PrintOutput(string message)
        {
            TriggerCustomEvent(o =>
            {
                Print(string.Format("{0} | {1}", ToTime(Time[0]), message));
            }, null);
        }

        private TextBlock GetHeaderText(string value)
        {
            return new TextBlock
            {
                Text = value,
                FontWeight = FontWeights.Bold,
                FontFamily = ChartControl.Properties.LabelFont.Family,
                FontSize = 14,
                Foreground = GetSolidColorBrushFromHex("#d9d9d9"),
                Margin = new Thickness(0, 0, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
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
                FontSize = 11,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Visibility = Visibility.Visible
            };
        }

        private void SetButtonBackground(Grid grid, Dictionary<string, ButtonInfo> buttons, bool isActive, string buttonLabel)
        {
            if (buttons.ContainsKey(buttonLabel))
            {
                buttons[buttonLabel].IsActive = isActive;
            }

            Button button = FindChild<Button>(grid, buttonLabel);

            if (button != null)
            {
                button.Background = isActive ? GetSolidColorBrushFromHex(_buttonActive) : GetSolidColorBrushFromHex(_buttonNeutral);
            }
        }

        private Button CreateButton(string label, RoutedEventHandler clickHandler, int row, int column)
        {
            Button button = GetStyleButton(label);
            button.Click += clickHandler;
            button.Background = GetSolidColorBrushFromHex(_buttonNeutral);
            button.Tag = label;
            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
            return button;
        }

        #endregion

        private SolidColorBrush GetSolidColorBrushFromHex(string hexColor)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
        }

        private bool CheckATMStrategyLoaded()
        {
            bool atmStrategyLoaded = true;

            try
            {
                string template = ChartControl?.OwnerChart?.ChartTrader?.AtmStrategy?.Template;

                if (template == null)
                {
                    MessageBox.Show("ATM Strategy template is not loaded.", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    atmStrategyLoaded = false;
                }
            }
            catch (Exception ex)
            {
                atmStrategyLoaded = false;
            }

            return atmStrategyLoaded;
        }

        // Find child by tag
        private T FindChild<T>(DependencyObject parent, string tag) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                T childAsT = child as T;
                if (childAsT != null && (child as FrameworkElement)?.Tag?.ToString() == tag)
                    return childAsT;

                T childOfChild = FindChild<T>(child, tag);
                if (childOfChild != null)
                    return childOfChild;
            }

            return null;
        }

        private void DisposeWPFControls()
        {
            if (_chartWindow != null)
                _chartWindow.MainTabControl.SelectionChanged -= TabChangedHandler;

            DisposeStrategyButtons();
            DisposeDirectionButtons();
            DisposeManagementButtons();

            RemoveWPFControls();
        }

        private void InsertWPFControls()
        {
            if (_panelActive)
                return;

            Grid.SetRow(_mainGrid, (_chartTraderGrid.RowDefinitions.Count - 1));
            _chartTraderGrid.Children.Add(_mainGrid);

            _panelActive = true;
        }

        private void RemoveWPFControls()
        {
            if (!_panelActive)
                return;

            if (_chartTraderButtonsGrid != null || _mainGrid != null)
            {
                _chartTraderGrid.Children.Remove(_mainGrid);
            }

            _panelActive = false;
        }

        private bool TabSelected()
        {
            bool tabSelected = false;

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
