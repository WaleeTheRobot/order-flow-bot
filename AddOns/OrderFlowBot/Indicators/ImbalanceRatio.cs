using NinjaTrader.Custom.AddOns;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.Gui.Chart;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.Indicators
{
    public class BidAskRatio
    {
        public int BarNumber { get; set; }
        public BarType BarType { get; set; }
        public double BidRatio { get; set; }
        public bool HasValidBidRatio { get; set; }
        public double AskRatio { get; set; }
        public bool HasValidAskRatio { get; set; }
    }

    public class ImbalanceRatio : Indicator
    {
        private OrderFlowBotDataBars _dataBars;

        private Dictionary<int, BidAskRatio> _bidAskRatios;
        private List<int> _drawnBars;
        private bool _isFirstOnRender;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Indicator to show the bid and ask imbalance ratios for high and low of bar.";
                Name = "ImbalanceRatio";
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
                _bidAskRatios = new Dictionary<int, BidAskRatio>();
                _drawnBars = new List<int>();
                _isFirstOnRender = true;
            }
        }

        public void InitializeWith(OrderFlowBotDataBars dataBars)
        {
            _dataBars = dataBars;
        }

        protected override void OnBarUpdate()
        {
            // Need to update the historical bars before OnRender
            UpdateBidAskRatios();
        }

        protected override void OnRender(ChartControl chartControl, ChartScale chartScale)
        {
            UpdateBidAskRatios();

            base.OnRender(chartControl, chartScale);

            float textHeight = 20;
            float gap = 20;

            var regularTextFormat = new TextFormat(Core.Globals.DirectWriteFactory, "Arial", 12);
            var boldTextFormat = new TextFormat(Core.Globals.DirectWriteFactory, "Arial", FontWeight.Bold, FontStyle.Normal, 16);

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
                DrawBarDetails(idx, chartControl, chartScale, regularTextFormat, boldTextFormat, textHeight, gap);
            }

            // Draw the current bar and add it to the list
            DrawBarDetails(ChartBars.ToIndex, chartControl, chartScale, regularTextFormat, boldTextFormat, textHeight, gap);
            if (!_drawnBars.Contains(ChartBars.ToIndex))
            {
                _drawnBars.Add(ChartBars.ToIndex);
            }

            regularTextFormat.Dispose();
            boldTextFormat.Dispose();
        }

        private void DrawBarDetails(int idx, ChartControl chartControl, ChartScale chartScale, TextFormat regularTextFormat, TextFormat boldTextFormat, float textHeight, float gap)
        {
            double highValue = High.GetValueAt(idx);
            double lowValue = Low.GetValueAt(idx);

            double yHigh = chartScale.GetYByValue(highValue) - (textHeight + gap);
            double yLow = chartScale.GetYByValue(lowValue) + gap;

            double x = chartControl.GetXByBarIndex(ChartBars, idx);

            BidAskRatio currentBidAskRatio = null;
            if (_bidAskRatios.ContainsKey(idx))
            {
                currentBidAskRatio = _bidAskRatios[idx];
            }

            string textToRenderAsk = currentBidAskRatio != null ? currentBidAskRatio.AskRatio.ToString() : "";
            string textToRenderBid = currentBidAskRatio != null ? currentBidAskRatio.BidRatio.ToString() : "";

            TextFormat currentAskFormat = regularTextFormat;
            TextFormat currentBidFormat = regularTextFormat;

            bool hasValidAskRatio = false;
            bool hasValidBidRatio = false;

            if (currentBidAskRatio != null)
            {
                hasValidAskRatio = currentBidAskRatio.HasValidAskRatio;
                hasValidBidRatio = currentBidAskRatio.HasValidBidRatio;

                if (hasValidAskRatio)
                {
                    currentAskFormat = boldTextFormat;
                }

                if (hasValidBidRatio)
                {
                    currentBidFormat = boldTextFormat;
                }
            }

            var askBrush = new SolidColorBrush(RenderTarget, GetBarColor("ask", hasValidAskRatio));
            var bidBrush = new SolidColorBrush(RenderTarget, GetBarColor("bid", hasValidBidRatio));

            if (currentBidAskRatio != null && currentBidAskRatio.BarType == BarType.Bullish)
            {
                RenderTarget.DrawText(textToRenderBid, currentBidFormat, new SharpDX.RectangleF((float)x, (float)yLow, 100, textHeight), bidBrush);
            }

            if (currentBidAskRatio != null && currentBidAskRatio.BarType == BarType.Bearish)
            {
                RenderTarget.DrawText(textToRenderAsk, currentAskFormat, new SharpDX.RectangleF((float)x, (float)yHigh, 100, textHeight), askBrush);
            }

            askBrush.Dispose();
            bidBrush.Dispose();
        }

        private void UpdateBidAskRatios()
        {
            int currentBarNumber = _dataBars.Bar.BarNumber;

            if (_bidAskRatios.ContainsKey(currentBarNumber))
            {
                _bidAskRatios[currentBarNumber].BarType = _dataBars.Bar.BarType;
                _bidAskRatios[currentBarNumber].BidRatio = _dataBars.Bar.Ratios.BidRatio;
                _bidAskRatios[currentBarNumber].HasValidBidRatio = _dataBars.Bar.Ratios.HasValidBidRatio;
                _bidAskRatios[currentBarNumber].AskRatio = _dataBars.Bar.Ratios.AskRatio;
                _bidAskRatios[currentBarNumber].HasValidAskRatio = _dataBars.Bar.Ratios.HasValidAskRatio;
            }
            else
            {
                BidAskRatio newBidAskRatio = new BidAskRatio
                {
                    BarNumber = currentBarNumber,
                    BarType = _dataBars.Bar.BarType,
                    BidRatio = _dataBars.Bar.Ratios.BidRatio,
                    HasValidBidRatio = _dataBars.Bar.Ratios.HasValidBidRatio,
                    AskRatio = _dataBars.Bar.Ratios.AskRatio,
                    HasValidAskRatio = _dataBars.Bar.Ratios.HasValidAskRatio
                };

                _bidAskRatios.Add(currentBarNumber, newBidAskRatio);
            }
        }

        private SharpDX.Color4 GetBarColor(string type, bool hasValidRatio)
        {
            if (type == "ask")
            {
                return hasValidRatio ? new SharpDX.Color4(1, 0.55f, 0, 1) : new SharpDX.Color4(0.5f, 0.5f, 0.5f, 1);
            }
            else if (type == "bid")
            {
                return hasValidRatio ? new SharpDX.Color4(0, 0.8f, 0.8f, 1) : new SharpDX.Color4(0.5f, 0.5f, 0.5f, 1);
            }
            return new SharpDX.Color4(0.5f, 0.5f, 0.5f, 1); // Default to SlateGray
        }
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
    public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
    {
        private ImbalanceRatio[] cacheImbalanceRatio;
        public ImbalanceRatio ImbalanceRatio()
        {
            return ImbalanceRatio(Input);
        }

        public ImbalanceRatio ImbalanceRatio(ISeries<double> input)
        {
            if (cacheImbalanceRatio != null)
                for (int idx = 0; idx < cacheImbalanceRatio.Length; idx++)
                    if (cacheImbalanceRatio[idx] != null && cacheImbalanceRatio[idx].EqualsInput(input))
                        return cacheImbalanceRatio[idx];
            return CacheIndicator<ImbalanceRatio>(new ImbalanceRatio(), input, ref cacheImbalanceRatio);
        }
    }
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
    public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
    {
        public Indicators.ImbalanceRatio ImbalanceRatio()
        {
            return indicator.ImbalanceRatio(Input);
        }

        public Indicators.ImbalanceRatio ImbalanceRatio(ISeries<double> input)
        {
            return indicator.ImbalanceRatio(input);
        }
    }
}

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
    {
        public Indicators.ImbalanceRatio ImbalanceRatio()
        {
            return indicator.ImbalanceRatio(Input);
        }

        public Indicators.ImbalanceRatio ImbalanceRatio(ISeries<double> input)
        {
            return indicator.ImbalanceRatio(input);
        }
    }
}

#endregion