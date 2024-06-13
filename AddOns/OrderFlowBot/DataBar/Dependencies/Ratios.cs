using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies
{
    public class Ratios
    {
        public double AskRatio { get; set; }
        public double BidRatio { get; set; }
        public bool HasValidAskExhaustionRatio { get; set; }
        public bool HasValidBidExhaustionRatio { get; set; }
        public bool HasValidAskAbsorptionRatio { get; set; }
        public bool HasValidBidAbsorptionRatio { get; set; }

        public void SetRatios(List<BidAskVolume> bidAskVolumes, bool validBidAskVolumes, BarType dataBarType)
        {
            if (!validBidAskVolumes)
                return;

            double secondBottomBid, bottomBid;
            GetBottomBidVolumes(bidAskVolumes, out secondBottomBid, out bottomBid);
            BidRatio = CalculateRatio(secondBottomBid, bottomBid);
            HasValidBidExhaustionRatio = IsValidExhaustedRatio(BidRatio) && dataBarType == BarType.Bullish;
            HasValidBidAbsorptionRatio = IsValidAbsorptionRatio(BidRatio) && dataBarType == BarType.Bullish;

            double topAsk, secondTopAsk;
            GetTopAskVolumes(bidAskVolumes, out topAsk, out secondTopAsk);
            AskRatio = CalculateRatio(secondTopAsk, topAsk);
            HasValidAskExhaustionRatio = IsValidExhaustedRatio(AskRatio) && dataBarType == BarType.Bearish;
            HasValidAskAbsorptionRatio = IsValidAbsorptionRatio(AskRatio) && dataBarType == BarType.Bearish;
        }

        public void SetLastRatioPrices(List<OrderFlowBotDataBar> dataBars)
        {
            double lastBidPrice = 0;
            double lastAskPrice = 0;

            for (int i = dataBars.Count - 1; i >= 0; i--)
            {
                var dataBar = dataBars[i];

                if ((lastAskPrice == 0) && (dataBar.Ratios.HasValidAskExhaustionRatio || dataBar.Ratios.HasValidAskAbsorptionRatio))
                {
                    lastAskPrice = dataBar.Prices.High;
                }

                if ((lastBidPrice == 0) && (dataBar.Ratios.HasValidBidExhaustionRatio || dataBar.Ratios.HasValidBidAbsorptionRatio))
                {
                    lastBidPrice = dataBar.Prices.Low;
                }

                if (lastBidPrice != 0 && lastAskPrice != 0)
                {
                    break;
                }
            }
        }

        private void GetBottomBidVolumes(List<BidAskVolume> bidAskVolumes, out double secondBottomBid, out double bottomBid)
        {
            int lastIndex = bidAskVolumes.Count - 1;
            secondBottomBid = bidAskVolumes[lastIndex - 1].BidVolume;
            bottomBid = bidAskVolumes[lastIndex].BidVolume;
        }

        private void GetTopAskVolumes(List<BidAskVolume> bidAskVolumes, out double topAsk, out double secondTopAsk)
        {
            topAsk = bidAskVolumes[0].AskVolume;
            secondTopAsk = bidAskVolumes[1].AskVolume;
        }

        private double CalculateRatio(double numerator, double denominator)
        {
            if (numerator == 0 && denominator == 0)
            {
                return 0;
            }
            if (denominator == 0)
            {
                return Math.Round(numerator, 2);
            }

            return Math.Round(numerator / denominator, 2);
        }

        private bool IsValidExhaustedRatio(double ratio)
        {
            return ratio > OrderFlowBotDataBarConfig.ValidExhaustionRatio;
        }

        private bool IsValidAbsorptionRatio(double ratio)
        {
            return ratio < OrderFlowBotDataBarConfig.ValidAbsorptionRatio;
        }
    }
}
