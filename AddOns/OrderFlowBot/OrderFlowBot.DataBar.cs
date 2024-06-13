using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;

namespace NinjaTrader.NinjaScript.Strategies
{
    // Set custom values that needs to access NinjaScript methods since it gives issues when extending elsewhere
    public partial class OrderFlowBot : Strategy
    {
        private OrderFlowDataBarBase GetOrderFlowDataBarBase(int barsAgo)
        {
            NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType volumetricBar = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

            OrderFlowDataBarBase baseBar = new OrderFlowDataBarBase
            {
                VolumetricBar = volumetricBar,
                BarsAgo = barsAgo,
                Time = ToTime(Time[barsAgo]),
                CurrentBar = CurrentBar,
                High = High[barsAgo],
                Low = Low[barsAgo],
                Open = Open[barsAgo],
                Close = Close[barsAgo],
            };

            return baseBar;
        }
    }
}
