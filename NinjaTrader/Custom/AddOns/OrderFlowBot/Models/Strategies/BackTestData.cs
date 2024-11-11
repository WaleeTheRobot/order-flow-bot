namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public class BackTestData
    {
        public string Name { get; set; }
        public bool IsBackTestEnabled { get; set; }

        public BackTestData()
        {
        }

        public BackTestData(
            string name,
            bool isBackTestEnabled
        )
        {
            Name = name;
            IsBackTestEnabled = isBackTestEnabled;
        }
    }
}
