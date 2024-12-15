using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars
{
    public interface IDataBarDataProvider
    {
        ICustomVolumetricBar VolumetricBar { get; set; }
        int Time { get; set; }
        int Day { get; set; }
        int CurrentBar { get; set; }
        int BarsAgo { get; set; }
        double High { get; set; }
        double Low { get; set; }
        double Open { get; set; }
        double Close { get; set; }
        CumulativeDeltaBar CumulativeDeltaBar { get; set; }
    }
}
