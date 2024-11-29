using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public class BacktestConfigData : IBacktestData
    {
        public string Name { get; set; }
        public bool IsBacktestEnabled { get; set; }

        public BacktestConfigData()
        {
            Name = "Test";
            IsBacktestEnabled = true;
        }

        public void SetNoBacktest()
        {
            Name = "";
            IsBacktestEnabled = false;
        }
    }
}
