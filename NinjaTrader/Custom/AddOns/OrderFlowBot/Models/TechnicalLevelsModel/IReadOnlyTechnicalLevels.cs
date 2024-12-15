using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel
{
    public interface IReadOnlyTechnicalLevels
    {
        int BarNumber { get; }
        Ema Ema { get; }
        Atr Atr { get; }
    }
}
