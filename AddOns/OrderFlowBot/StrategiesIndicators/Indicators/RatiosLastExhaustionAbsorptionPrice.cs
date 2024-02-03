using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.Gui.Chart;

namespace NinjaTrader.NinjaScript.Indicators
{
    public class RatiosLastExhaustionAbsorptionPrice : Indicator
    {
        private OrderFlowBotDataBars _dataBars;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Indicator to draw a line to show the last exhaustion or absorption price for bid and ask.";
                Name = "RatiosLastExhaustionAbsorptionPrice";
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
            using (SharpDX.Direct2D1.SolidColorBrush askBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, SharpDX.Color.DarkOrange))
            using (SharpDX.Direct2D1.SolidColorBrush bidBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, SharpDX.Color.DarkTurquoise))
            {
                float chartWidth = (float)chartScale.Width;
                float fixedLength = 300;
                float rightOffset = 15;

                float[] dashes = { 2.0f, 2.0f };
                var strokeStyleProperties = new SharpDX.Direct2D1.StrokeStyleProperties()
                {
                    DashStyle = SharpDX.Direct2D1.DashStyle.Custom,
                    DashOffset = 0
                };

                using (var strokeStyle = new SharpDX.Direct2D1.StrokeStyle(RenderTarget.Factory, strokeStyleProperties, dashes))
                {
                    float askYValue = chartScale.GetYByValue(_dataBars.Bar.Ratios.LastValidAskRatioPrice);
                    SharpDX.Vector2 askStartPoint = new SharpDX.Vector2(chartWidth - rightOffset, askYValue);
                    SharpDX.Vector2 askEndPoint = new SharpDX.Vector2(chartWidth - rightOffset - fixedLength, askYValue);
                    RenderTarget.DrawLine(askStartPoint, askEndPoint, askBrush, 2, strokeStyle);

                    float bidYValue = chartScale.GetYByValue(_dataBars.Bar.Ratios.LastValidBidRatioPrice);
                    SharpDX.Vector2 bidStartPoint = new SharpDX.Vector2(chartWidth - rightOffset, bidYValue);
                    SharpDX.Vector2 bidEndPoint = new SharpDX.Vector2(chartWidth - rightOffset - fixedLength, bidYValue);
                    RenderTarget.DrawLine(bidStartPoint, bidEndPoint, bidBrush, 2, strokeStyle);
                }
            }
        }

    }
}


#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
    public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
    {
        private RatiosLastExhaustionAbsorptionPrice[] cacheRatiosLastExhaustionAbsorptionPrice;
        public RatiosLastExhaustionAbsorptionPrice RatiosLastExhaustionAbsorptionPrice()
        {
            return RatiosLastExhaustionAbsorptionPrice(Input);
        }

        public RatiosLastExhaustionAbsorptionPrice RatiosLastExhaustionAbsorptionPrice(ISeries<double> input)
        {
            if (cacheRatiosLastExhaustionAbsorptionPrice != null)
                for (int idx = 0; idx < cacheRatiosLastExhaustionAbsorptionPrice.Length; idx++)
                    if (cacheRatiosLastExhaustionAbsorptionPrice[idx] != null && cacheRatiosLastExhaustionAbsorptionPrice[idx].EqualsInput(input))
                        return cacheRatiosLastExhaustionAbsorptionPrice[idx];
            return CacheIndicator<RatiosLastExhaustionAbsorptionPrice>(new RatiosLastExhaustionAbsorptionPrice(), input, ref cacheRatiosLastExhaustionAbsorptionPrice);
        }
    }
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
    public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
    {
        public Indicators.RatiosLastExhaustionAbsorptionPrice RatiosLastExhaustionAbsorptionPrice()
        {
            return indicator.RatiosLastExhaustionAbsorptionPrice(Input);
        }

        public Indicators.RatiosLastExhaustionAbsorptionPrice RatiosLastExhaustionAbsorptionPrice(ISeries<double> input)
        {
            return indicator.RatiosLastExhaustionAbsorptionPrice(input);
        }
    }
}

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
    {
        public Indicators.RatiosLastExhaustionAbsorptionPrice RatiosLastExhaustionAbsorptionPrice()
        {
            return indicator.RatiosLastExhaustionAbsorptionPrice(Input);
        }

        public Indicators.RatiosLastExhaustionAbsorptionPrice RatiosLastExhaustionAbsorptionPrice(ISeries<double> input)
        {
            return indicator.RatiosLastExhaustionAbsorptionPrice(input);
        }
    }
}

#endregion
