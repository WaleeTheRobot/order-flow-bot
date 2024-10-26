namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components
{
    public interface IComponentSetup
    {
        void InitializeComponent();
        void Ready();
        void HandleEnabledDisabledTriggered(bool isEnabled);
    }
}
