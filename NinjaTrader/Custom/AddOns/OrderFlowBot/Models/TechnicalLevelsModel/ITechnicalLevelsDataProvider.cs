using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel
{
    public interface ITechnicalLevelsDataProvider
    {
        Ema Ema { get; set; }
        Atr Atr { get; set; }
    }
}
