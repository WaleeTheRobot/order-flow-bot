using OrderFlowBotTestFiles.Common;

namespace OrderFlowBotTestFiles.Files.Dependencies
{
    public class Ratios
    {
        public double AskRatio { get; set; }
        public double BidRatio { get; set; }
        public bool HasValidAskRatio { get; set; }
        public bool HasValidBidRatio { get; set; }

        public void SetRatios(List<BidAskVolume> bidAskVolumes, bool validBidAskVolumes)
        {
            if (!validBidAskVolumes)
                return;

            double secondBottomBid, bottomBid;
            GetBottomBidVolumes(bidAskVolumes, out secondBottomBid, out bottomBid);
            BidRatio = CalculateRatio(secondBottomBid, bottomBid);
            HasValidBidRatio = IsValidRatio(BidRatio);

            double topAsk, secondTopAsk;
            GetTopAskVolumes(bidAskVolumes, out topAsk, out secondTopAsk);
            AskRatio = CalculateRatio(secondTopAsk, topAsk);
            HasValidAskRatio = IsValidRatio(AskRatio);
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

        private bool IsValidRatio(double ratio)
        {
            return ratio > OrderFlowBotProperties.ValidRatio;
        }
    }
}
