using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar
{
    public class OrderFlowBotDataBars
    {
        public List<OrderFlowBotDataBar> Bars { get; set; }
        public OrderFlowBotDataBar Bar { get; set; }

        public OrderFlowBotDataBars()
        {
            Bars = new List<OrderFlowBotDataBar>();
            Bar = new OrderFlowBotDataBar();
        }
    }
}
