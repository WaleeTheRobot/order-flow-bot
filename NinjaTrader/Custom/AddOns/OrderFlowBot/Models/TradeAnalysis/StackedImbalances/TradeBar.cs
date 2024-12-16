namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TradeAnalysis.StackedImbalances
{
    public class TradeBar
    {
        public SignalBar DataBar { get; set; }
        public SignalBar DeltaBar { get; set; }
        public OrderFlowData OrderFlowData { get; set; }


        public TradeBar() { }

        public TradeBar(
            SignalBar dataBar,
            SignalBar deltaBar,
            OrderFlowData orderFlowData)
        {
            DataBar = dataBar;
            DeltaBar = deltaBar;
            OrderFlowData = orderFlowData;
        }
    }
}
