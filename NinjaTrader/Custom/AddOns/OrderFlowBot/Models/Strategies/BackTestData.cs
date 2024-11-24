namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public class BacktestData
    {
        public string Name { get; set; }
        public bool IsBacktestEnabled { get; set; }

        public BacktestData()
        {
        }

        public BacktestData(
            string name,
            bool isBacktestEnabled
        )
        {
            Name = name;
            IsBacktestEnabled = isBacktestEnabled;
        }
    }
}
