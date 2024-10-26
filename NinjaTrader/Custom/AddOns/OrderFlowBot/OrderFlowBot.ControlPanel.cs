using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;
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
        private Grid _chartTraderGrid, _mainGrid, _tradeDirectionGrid, _strategiesGrid;
        private TradeManagementGrid _tradeManagementGrid;
        private bool _panelActive;
        private TabItem _tabItem;

        private UserInterfaceEvents _userInterfaceEvents;

        public void InitializeUIManager()
        {
            LoadControlPanel();
            _userInterfaceEvents = new UserInterfaceEvents(_eventManager);
        }

        private void LoadControlPanel()
        {
            ChartControl?.Dispatcher.InvokeAsync(() =>
            {
                CreateWPFControls();
            });
        }

        private void UnloadControlPanel()
        {
            ChartControl?.Dispatcher.InvokeAsync(() =>
            {
                DisposeWPFControls();
            });
        }

        private void ReadyControlPanel()
        {
            ChartControl?.Dispatcher.InvokeAsync(() =>
            {
                _tradeManagementGrid.Ready();
            });
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

            _tradeManagementGrid = new TradeManagementGrid(_eventsContainer, _userInterfaceEvents);

            Grid.SetRow(_tradeManagementGrid, 0);
            _mainGrid.Children.Add(_tradeManagementGrid);

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

            if (_tradeManagementGrid != null || _mainGrid != null)
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
