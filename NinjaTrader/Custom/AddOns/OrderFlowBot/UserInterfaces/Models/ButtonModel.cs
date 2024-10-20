using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Models
{
    public class ButtonModel
    {
        public string Content { get; set; }
        public string ToggledContent { get; set; }
        public string BackgroundColor { get; set; }
        public string HoverBackgroundColor { get; set; }
        public string ToggledBackgroundColor { get; set; }
        public string TextColor { get; set; }
        public Delegate ClickHandler { get; set; }
        public bool IsToggleable { get; set; }
        public bool InitialToggleState { get; set; }
    }
}
