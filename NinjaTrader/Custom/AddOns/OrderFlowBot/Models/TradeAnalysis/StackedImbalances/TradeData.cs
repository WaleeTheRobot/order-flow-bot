using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TradeAnalysis.StackedImbalances
{
    public class TradeData
    {
        public int TradeDirection { get; set; }
        public int? TradeOutcome { get; set; }
        public List<TradeBar> PreTradeBars { get; set; }
        public TradeBar CurrentBar { get; set; }
    }
}
