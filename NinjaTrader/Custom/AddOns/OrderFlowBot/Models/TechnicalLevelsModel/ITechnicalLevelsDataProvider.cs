using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel
{
    public interface ITechnicalLevelsDataProvider
    {
        int BarNumber { get; set; }
        Ema Ema { get; set; }
    }
}
