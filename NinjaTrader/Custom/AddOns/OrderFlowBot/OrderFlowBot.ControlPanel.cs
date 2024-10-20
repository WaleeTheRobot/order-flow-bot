using NinjaTrader.Gui.Chart;
using System.Windows;
using System.Windows.Controls;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private ChartTab _chartTab;
        private Chart _chartWindow;
        private Grid _chartTraderGrid, _tradeManagementGrid, _tradeDirectionGrid, _strategiesGrid;
        private bool _panelActive;
        private TabItem _tabItem;

        public void InitializeUIManager()
        {
            ControlPanelSetStateDataLoaded();
        }

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

            /*// Existing Chart Trader buttons
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

            if (TabSelected()) InsertWPFControls();*/

            _chartWindow.MainTabControl.SelectionChanged += TabChangedHandler;
        }

        private void DisposeWPFControls()
        {
            if (_chartWindow != null)
                _chartWindow.MainTabControl.SelectionChanged -= TabChangedHandler;

            //DisposeStrategyButtons();
            // DisposeDirectionButtons();
            //  DisposeManagementButtons();

            RemoveWPFControls();
        }

        private void InsertWPFControls()
        {
            if (_panelActive)
                return;

            // Grid.SetRow(_mainGrid, (_chartTraderGrid.RowDefinitions.Count - 1));
            // _chartTraderGrid.Children.Add(_mainGrid);

            _panelActive = true;
        }

        private void RemoveWPFControls()
        {
            if (!_panelActive)
                return;

            /* if (_chartTraderButtonsGrid != null || _mainGrid != null)
             {
                 _chartTraderGrid.Children.Remove(_mainGrid);
             }*/

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
