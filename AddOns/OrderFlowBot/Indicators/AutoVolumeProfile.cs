using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators
{
    public class AutoVolumeProfile : Indicator
    {
        private OrderFlowBotDataBars _dataBars;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Indicator to show the value area high, low and point of control.";
                Name = "AutoVolumeProfile";
                Calculate = Calculate.OnEachTick;
                IsOverlay = true;
                DisplayInDataBox = true;
                DrawOnPricePanel = true;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
                //Disable this property if your indicator requires custom values that cumulate with each new market data event. 
                //See Help Guide for additional information.
                IsSuspendedWhileInactive = true;
            }
        }

        protected override void OnBarUpdate()
        {
        }

        public void InitializeWith(OrderFlowBotDataBars dataBars)
        {
            _dataBars = dataBars;
        }

        protected override void OnRender(ChartControl chartControl, ChartScale chartScale)
        {
            OrderFlowBotDataBar previousBar = _dataBars.Bars.Last();

            SharpDX.Direct2D1.SolidColorBrush valueAreaHighLowBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, SharpDX.Color.SlateGray);
            SharpDX.Direct2D1.SolidColorBrush pointOfControlBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, SharpDX.Color.Pink);

            float chartWidth = (float)chartScale.Width;
            float fixedLength = 600;
            float rightOffset = 25;

            float pocYValue = chartScale.GetYByValue(previousBar.AutoVolumeProfile.PointOfControl);
            SharpDX.Vector2 pocStartPoint = new SharpDX.Vector2(chartWidth - rightOffset, pocYValue);
            SharpDX.Vector2 pocEndPoint = new SharpDX.Vector2(chartWidth - rightOffset - fixedLength, pocYValue);
            RenderTarget.DrawLine(pocStartPoint, pocEndPoint, pointOfControlBrush, 2);

            float vahYValue = chartScale.GetYByValue(previousBar.AutoVolumeProfile.ValueAreaHigh);
            SharpDX.Vector2 vahStartPoint = new SharpDX.Vector2(chartWidth - rightOffset, vahYValue);
            SharpDX.Vector2 vahEndPoint = new SharpDX.Vector2(chartWidth - rightOffset - fixedLength, vahYValue);
            RenderTarget.DrawLine(vahStartPoint, vahEndPoint, valueAreaHighLowBrush, 2);

            float valYValue = chartScale.GetYByValue(previousBar.AutoVolumeProfile.ValueAreaLow);
            SharpDX.Vector2 valStartPoint = new SharpDX.Vector2(chartWidth - rightOffset, valYValue);
            SharpDX.Vector2 valEndPoint = new SharpDX.Vector2(chartWidth - rightOffset - fixedLength, valYValue);
            RenderTarget.DrawLine(valStartPoint, valEndPoint, valueAreaHighLowBrush, 2);

            valueAreaHighLowBrush.Dispose();
            pointOfControlBrush.Dispose();
        }
    }
}


#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
    public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
    {
        private AutoVolumeProfile[] cacheAutoVolumeProfile;
        public AutoVolumeProfile AutoVolumeProfile()
        {
            return AutoVolumeProfile(Input);
        }

        public AutoVolumeProfile AutoVolumeProfile(ISeries<double> input)
        {
            if (cacheAutoVolumeProfile != null)
                for (int idx = 0; idx < cacheAutoVolumeProfile.Length; idx++)
                    if (cacheAutoVolumeProfile[idx] != null && cacheAutoVolumeProfile[idx].EqualsInput(input))
                        return cacheAutoVolumeProfile[idx];
            return CacheIndicator<AutoVolumeProfile>(new AutoVolumeProfile(), input, ref cacheAutoVolumeProfile);
        }
    }
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
    public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
    {
        public Indicators.AutoVolumeProfile AutoVolumeProfile()
        {
            return indicator.AutoVolumeProfile(Input);
        }

        public Indicators.AutoVolumeProfile AutoVolumeProfile(ISeries<double> input)
        {
            return indicator.AutoVolumeProfile(input);
        }
    }
}

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
    {
        public Indicators.AutoVolumeProfile AutoVolumeProfile()
        {
            return indicator.AutoVolumeProfile(Input);
        }

        public Indicators.AutoVolumeProfile AutoVolumeProfile(ISeries<double> input)
        {
            return indicator.AutoVolumeProfile(input);
        }
    }
}

#endregion
