using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using System.Windows.Controls;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components
{
    public class TradeDirectionGrid : Grid, IComponentSetup
    {
        private EventsContainer _eventsContainer;

        public TradeDirectionGrid(EventsContainer eventsContainer)
        {
            _eventsContainer = eventsContainer;

            InitializeComponent();
        }

        public void InitializeComponent()
        {
            // Trigger Strike Price, Standard/Reverse mode, Long, Short
        }

        public void Ready()
        {
            // Do something
        }

        public void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            /*foreach (var buttonPair in _buttons)
            {
                if (buttonPair.Key != "Enabled")
                {
                    buttonPair.Value.IsEnabled = isEnabled;
                }
            }*/
        }
    }
}
