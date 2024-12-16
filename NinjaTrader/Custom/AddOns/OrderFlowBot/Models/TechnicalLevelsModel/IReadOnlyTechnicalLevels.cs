using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel
{
    public interface IReadOnlyTechnicalLevels
    {
        Ema Ema { get; }
        Atr Atr { get; }
    }
}
