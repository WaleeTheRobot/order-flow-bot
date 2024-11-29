using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Services;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Utils;
using NinjaTrader.Gui.Chart;
using System.Windows;
using System.Windows.Controls;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private ChartTab _chartTab;
        private Chart _chartWindow;
        private Grid _chartTraderGrid, _mainGrid;
        private TradeManagementGrid _tradeManagementGrid;
        private TradeDirectionGrid _tradeDirectionGrid;
        private StrategiesGrid _strategiesGrid;
        private bool _panelActive;
        private TabItem _tabItem;
        private TextBlock _orderFlowLabel;

        private UserInterfaceEvents _userInterfaceEvents;
        private UserInterfaceService _userInterfaceService;

        public void InitializeUIManager()
        {
            LoadControlPanel();
        }

        private void LoadControlPanel()
        {
            ChartControl?.Dispatcher.InvokeAsync(() =>
            {
                _userInterfaceEvents = new UserInterfaceEvents(_eventManager);

                CreateWPFControls();

                _userInterfaceService =
                    new UserInterfaceService(
                        _servicesContainer,
                        _userInterfaceEvents,
                        _tradeManagementGrid,
                        _tradeDirectionGrid,
                        _strategiesGrid
                    );
            });
        }

        private void UnloadControlPanel()
        {
            ChartControl?.Dispatcher.InvokeAsync(DisposeWPFControls);
        }

        private void ReadyControlPanel()
        {
            ChartControl?.Dispatcher.InvokeAsync(() =>
            {
                _userInterfaceEvents.OnUpdateControlPanelLabel += UpdateControlPanelLabel;

                if (!BacktestEnabled)
                {
                    if (TimeEnabled && !_validTimeRange)
                    {
                        UpdateValidStartEndTimeUserInterface(false);
                    }
                    else if (ValidDailyProfitLossHit())
                    {
                        UpdateDailyProfitLossUserInterface();
                    }
                    else
                    {
                        UpdateControlPanelLabel("OrderFlowBot");

                        _tradeManagementGrid.Ready();
                        _tradeDirectionGrid.Ready();
                        _strategiesGrid.Ready();
                    }

                    // Enable the grids
                    _tradeManagementGrid.IsEnabled = true;
                    _tradeDirectionGrid.IsEnabled = true;
                    _strategiesGrid.IsEnabled = true;
                }
                else
                {
                    UpdateControlPanelLabel("Backtesting");
                }
            });
        }

        private void UpdateControlPanelLabel(string text)
        {
            if (_orderFlowLabel.Dispatcher.CheckAccess())
            {
                HandleUpdateControlPanelLabel(text);
            }
            else
            {
                _orderFlowLabel.Dispatcher.Invoke(() => HandleUpdateControlPanelLabel(text));
            }
        }

        private void HandleUpdateControlPanelLabel(string text)
        {
            _orderFlowLabel.Text = text;
        }

        private void CreateWPFControls()
        {
            _chartWindow = Window.GetWindow(ChartControl.Parent) as Gui.Chart.Chart;

            if (_chartWindow == null)
            {
                return;
            }

            _chartTraderGrid = (_chartWindow.FindFirst("ChartWindowChartTraderControl") as ChartTrader).Content as Grid;

            if (_chartTraderGrid == null)
            {
                return;
            }

            // Create main grid with rows for each section
            _mainGrid = new Grid
            {
                Margin = new Thickness(0, 50, 0, 0),
                Background = UserInterfaceUtils.GetSolidColorBrushFromHex(CustomColors.MAIN_GRID_BG_COLOR)
            };

            _mainGrid.RowDefinitions.Add(new RowDefinition());
            _mainGrid.RowDefinitions.Add(new RowDefinition());
            _mainGrid.RowDefinitions.Add(new RowDefinition());
            _mainGrid.RowDefinitions.Add(new RowDefinition());

            _orderFlowLabel = new TextBlock()
            {
                FontFamily = ChartControl.Properties.LabelFont.Family,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = UserInterfaceUtils.GetSolidColorBrushFromHex(CustomColors.TEXT_COLOR),
                Background = UserInterfaceUtils.GetSolidColorBrushFromHex(CustomColors.MAIN_HEADING_BG_COLOR),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0),
                Padding = new Thickness(4),
                Text = "Loading..."
            };

            _tradeManagementGrid = new TradeManagementGrid(_servicesContainer, _userInterfaceEvents) { IsEnabled = false };
            _tradeDirectionGrid = new TradeDirectionGrid(_servicesContainer, _userInterfaceEvents, _tradingEvents) { IsEnabled = false };
            _strategiesGrid = new StrategiesGrid(_servicesContainer, _userInterfaceEvents, _eventsContainer.StrategiesEvents) { IsEnabled = false };

            Grid.SetRow(_orderFlowLabel, 0);
            Grid.SetRow(_tradeManagementGrid, 1);
            Grid.SetRow(_tradeDirectionGrid, 2);
            Grid.SetRow(_strategiesGrid, 3);

            _mainGrid.Children.Add(_orderFlowLabel);
            _mainGrid.Children.Add(_tradeManagementGrid);
            _mainGrid.Children.Add(_tradeDirectionGrid);
            _mainGrid.Children.Add(_strategiesGrid);

            if (TabSelected())
            {
                InsertWPFControls();
            }

            _chartWindow.MainTabControl.SelectionChanged += TabChangedHandler;
        }

        private void DisposeWPFControls()
        {
            if (_chartWindow != null)
            {
                _chartWindow.MainTabControl.SelectionChanged -= TabChangedHandler;
            }

            RemoveWPFControls();
        }

        private void InsertWPFControls()
        {
            if (_panelActive)
            {
                return;
            }

            Grid.SetRow(_mainGrid, (_chartTraderGrid.RowDefinitions.Count - 1));
            _chartTraderGrid.Children.Add(_mainGrid);

            _panelActive = true;
        }

        private void RemoveWPFControls()
        {
            if (!_panelActive)
            {
                return;
            }

            if (_tradeManagementGrid != null ||
                _tradeDirectionGrid != null ||
                _strategiesGrid != null ||
                _mainGrid != null)
            {
                _chartTraderGrid.Children.Remove(_mainGrid);
            }

            _panelActive = false;
        }

        private bool TabSelected()
        {
            bool tabSelected = false;

            foreach (TabItem tab in _chartWindow.MainTabControl.Items)
            {
                if ((tab.Content as ChartTab).ChartControl == ChartControl && tab == _chartWindow.MainTabControl.SelectedItem)
                {
                    tabSelected = true;
                }
            }

            return tabSelected;
        }

        private void TabChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }

            _tabItem = e.AddedItems[0] as TabItem;
            if (_tabItem == null)
            {
                return;
            }

            _chartTab = _tabItem.Content as ChartTab;
            if (_chartTab == null)
            {
                return;
            }

            if (TabSelected())
            {
                InsertWPFControls();
            }
            else
            {
                RemoveWPFControls();
            }
        }
    }
}
