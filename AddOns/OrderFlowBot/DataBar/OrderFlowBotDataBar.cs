using NinjaTrader.Custom.AddOns;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;

namespace NinjaTrader.NinjaScript.AddOns.OrderFlowBot
{
    public class OrderFlowBotDataBar
    {
        public BarType BarType { get; set; }
        public int Time { get; set; }
        public int BarNumber { get; set; }

        public Prices Prices { get; set; }
        public Ratios Ratios { get; set; }
        public Volumes Volumes { get; set; }
        public Deltas Deltas { get; set; }
        public Imbalances Imbalances { get; set; }

        public OrderFlowBotDataBar()
        {
            Prices = new Prices();
            Ratios = new Ratios();
            Volumes = new Volumes();
            Deltas = new Deltas();
            Imbalances = new Imbalances();
        }

        public void SetBarType()
        {
            if (this.Prices.Open < this.Prices.Close)
            {
                this.BarType = BarType.Bullish;
                return;
            }

            if (this.Prices.Open > this.Prices.Close)
            {
                this.BarType = BarType.Bearish;
                return;
            }

            this.BarType = BarType.Flat;
        }
    }
}
