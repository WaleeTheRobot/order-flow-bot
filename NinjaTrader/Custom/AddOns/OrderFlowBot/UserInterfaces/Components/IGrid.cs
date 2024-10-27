using System.Windows;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components
{
    public interface IGrid
    {
        void InitializeInitialToggleState();
        void InitializeComponent(string label);
        void Ready();
        void HandleEnabledDisabledTriggered(bool isEnabled);
        void HandleAutoTradeTriggered(bool isEnabled);
        void HandleButtonClick(object sender, RoutedEventArgs e);
    }
}
