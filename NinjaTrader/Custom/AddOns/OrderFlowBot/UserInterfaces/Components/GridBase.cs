using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components.Controls;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components
{
    public abstract class GridBase : Grid, IGrid
    {
        protected readonly UserInterfaceEvents userInterfaceEvents;
        protected Dictionary<string, Button> buttons;
        protected Grid grid;

        protected Dictionary<string, bool> initialToggleState;

        protected GridBase(
            string label,
            ServicesContainer servicesContainer,
            UserInterfaceEvents userInterfaceEvents
        )
        {
            buttons = new Dictionary<string, Button>();

            this.userInterfaceEvents = userInterfaceEvents;
            this.userInterfaceEvents.OnEnabledDisabledTriggered += HandleEnabledDisabledTriggered;
            this.userInterfaceEvents.OnAutoTradeTriggered += HandleAutoTradeTriggered;

            initialToggleState = new Dictionary<string, bool>();

            InitializeComponent(label);
        }

        public void InitializeComponent(string label)
        {
            InitializeGrid();
            InitializeInitialToggleState();
            AddHeadingLabel(label);
            AddButtons();
        }

        public virtual void Ready()
        {
            SetAllButtonsEnabled(true);
        }

        protected virtual void InitializeGrid()
        {
            grid = new Grid
            {
                Margin = new Thickness(4, 4, 0, 0)
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Children.Add(grid);
        }

        public virtual void AddHeadingLabel(string label)
        {
            TextBlock headingLabel = new TextHeadingLabel(label).Label;

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Children.Add(headingLabel);
            Grid.SetColumnSpan(headingLabel, 2);
        }

        public abstract void InitializeInitialToggleState();

        protected abstract void AddButtons();

        public abstract void HandleAutoTradeTriggered(bool isEnabled);

        public abstract void HandleButtonClick(object sender, RoutedEventArgs e);

        public virtual void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            SetAllButtonsEnabled(isEnabled);
        }

        protected void SetAllButtonsEnabled(bool isEnabled)
        {
            foreach (var button in buttons.Values)
            {
                if (button.Name == ButtonName.ENABLED && !isEnabled)
                {
                    continue;
                }

                button.IsEnabled = isEnabled;
            }
        }

        protected static void SetButtonEnabled(Button button, bool isEnabled)
        {
            button.IsEnabled = isEnabled;
        }

        protected virtual void AddButtonToGrid(Button button, int row, int column)
        {
            while (grid.RowDefinitions.Count <= row)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            if (!grid.Children.Contains(button))
            {
                grid.Children.Add(button);
            }

            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
        }
    }
}
