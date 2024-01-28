using NinjaTrader.Custom.AddOns.OrderFlowBot;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;
using NinjaTrader.Gui.Chart;
using System;
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.Indicators
{
    public class SinglePrintBar
    {
        public List<BidAskVolume> BidAskVolumes { get; set; }
        public bool HasAskSinglePrint { get; set; }
        public bool HasBidSinglePrint { get; set; }
    }

    public class SinglePrint : Indicator
    {
        private OrderFlowBotDataBars _dataBars;
        private OrderFlowBotPropertiesConfig _config;

        private Dictionary<int, SinglePrintBar> _bars;
        private List<int> _drawnBars;
        private bool _isFirstOnRender;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Indicator to show the single print.";
                Name = "SinglePrint";
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
            else if (State == State.DataLoaded)
            {
                _bars = new Dictionary<int, SinglePrintBar>();
                _drawnBars = new List<int>();
                _isFirstOnRender = true;
            }
        }

        public override string DisplayName
        {
            get { return ""; }
        }

        public void InitializeWith(OrderFlowBotDataBars dataBars, OrderFlowBotPropertiesConfig config)
        {
            _dataBars = dataBars;
            _config = config;
        }

        protected override void OnBarUpdate()
        {
            // Need to update the historical bars before OnRender
            UpdateSinglePrint();
        }

        protected override void OnRender(ChartControl chartControl, ChartScale chartScale)
        {
            UpdateSinglePrint();

            base.OnRender(chartControl, chartScale);

            // Add all bars to the list only once
            if (_isFirstOnRender)
            {
                int totalBars = Closes[0].Count;

                for (int idx = 0; idx < totalBars; idx++)
                {
                    _drawnBars.Add(idx);
                }

                _isFirstOnRender = false;
            }

            foreach (var idx in _drawnBars)
            {
                DrawBarDetails(idx, chartControl, chartScale);
            }

            // Draw the current bar and add it to the list
            DrawBarDetails(ChartBars.ToIndex, chartControl, chartScale);
            if (!_drawnBars.Contains(ChartBars.ToIndex))
            {
                _drawnBars.Add(ChartBars.ToIndex);
            }
        }

        private void DrawBarDetails(int idx, ChartControl chartControl, ChartScale chartScale)
        {
            if (!_bars.ContainsKey(idx))
                return;

            SinglePrintBar singlePrintBar = _bars[idx];

            double highValue = High.GetValueAt(idx);
            double lowValue = Low.GetValueAt(idx);
            // Using the next tick to calculate the height
            double oneTickAboveLow = lowValue + _config.TickSize;

            double barWidth = chartControl.BarWidth * 6.2;

            // Calculate the X coordinates for the left and right sides of the bar
            float xCenter = chartControl.GetXByBarIndex(ChartBars, idx);
            float xLeft = xCenter - (float)barWidth / (float)1.58;
            float xRight = xLeft + (float)barWidth;

            // Calculate the fixed vertical size for the rectangle
            float yLowValue = chartScale.GetYByValue(lowValue);
            float yOneTickAboveLow = chartScale.GetYByValue(oneTickAboveLow);
            float fixedVerticalSize = Math.Abs(yLowValue - yOneTickAboveLow);

            if (singlePrintBar.HasAskSinglePrint)
            {
                float yHighAtBar = chartScale.GetYByValue(highValue);
                float yHigh = yHighAtBar - fixedVerticalSize / 2;
                float yLow = yHigh + fixedVerticalSize;
                DrawRectangle(xLeft, yHigh, xRight, yLow, chartControl, chartScale);
            }

            if (singlePrintBar.HasBidSinglePrint)
            {
                float yLowAtBar = chartScale.GetYByValue(lowValue);
                float yLow = yLowAtBar + fixedVerticalSize / 2;
                float yHigh = yLow - fixedVerticalSize;
                DrawRectangle(xLeft, yHigh, xRight, yLow, chartControl, chartScale);
            }
        }


        private void DrawRectangle(float xLeft, float yHigh, float xRight, float yLow, ChartControl chartControl, ChartScale chartScale)
        {
            SharpDX.RectangleF rectBounds = new SharpDX.RectangleF(xLeft, yHigh, xRight - xLeft, yLow - yHigh);

            var rectColor = SharpDX.Color.Red;
            var rectBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, rectColor);
            float strokeWidth = 2f;

            RenderTarget.DrawRectangle(rectBounds, rectBrush, strokeWidth);

            rectBrush.Dispose();
        }

        private void UpdateSinglePrint()
        {
            int currentBarNumber = _dataBars.Bar.BarNumber;

            if (_bars.ContainsKey(currentBarNumber))
            {
                _bars[currentBarNumber].BidAskVolumes = _dataBars.Bar.Volumes.BidAskVolumes;
                _bars[currentBarNumber].HasAskSinglePrint = _dataBars.Bar.Volumes.HasAskSinglePrint;
                _bars[currentBarNumber].HasBidSinglePrint = _dataBars.Bar.Volumes.HasBidSinglePrint;
            }
            else
            {
                SinglePrintBar newSinglePrintBar = new SinglePrintBar
                {
                    BidAskVolumes = _dataBars.Bar.Volumes.BidAskVolumes,
                    HasAskSinglePrint = _dataBars.Bar.Volumes.HasAskSinglePrint,
                    HasBidSinglePrint = _dataBars.Bar.Volumes.HasBidSinglePrint,
                };

                _bars.Add(currentBarNumber, newSinglePrintBar);
            }
        }
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
    public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
    {
        private SinglePrint[] cacheSinglePrint;
        public SinglePrint SinglePrint()
        {
            return SinglePrint(Input);
        }

        public SinglePrint SinglePrint(ISeries<double> input)
        {
            if (cacheSinglePrint != null)
                for (int idx = 0; idx < cacheSinglePrint.Length; idx++)
                    if (cacheSinglePrint[idx] != null && cacheSinglePrint[idx].EqualsInput(input))
                        return cacheSinglePrint[idx];
            return CacheIndicator<SinglePrint>(new SinglePrint(), input, ref cacheSinglePrint);
        }
    }
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
    public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
    {
        public Indicators.SinglePrint SinglePrint()
        {
            return indicator.SinglePrint(Input);
        }

        public Indicators.SinglePrint SinglePrint(ISeries<double> input)
        {
            return indicator.SinglePrint(input);
        }
    }
}

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
    {
        public Indicators.SinglePrint SinglePrint()
        {
            return indicator.SinglePrint(Input);
        }

        public Indicators.SinglePrint SinglePrint(ISeries<double> input)
        {
            return indicator.SinglePrint(input);
        }
    }
}

#endregion