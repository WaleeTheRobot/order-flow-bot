namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base
{
    public class Ema : IEma
    {
        public double FastEma { get; set; }
        public double SlowEma { get; set; }

        public Ema()
        {
        }
    }
}
