namespace OrderFlowBotTestFiles.Files.Dependencies
{
    public struct BidAskVolume
    {
        public double Price;
        public long BidVolume;
        public long AskVolume;
    }

    public class Volumes
    {
        public double PointOfControl { get; set; }
        public long Volume { get; set; }
        public long BuyingVolume { get; set; }
        public long SellingVolume { get; set; }
        public List<BidAskVolume> BidAskVolumes { get; set; }

        public Volumes()
        {
            BidAskVolumes = new List<BidAskVolume>();
        }

        public bool ValidBidAskVolumes()
        {
            return BidAskVolumes.Count > 2;
        }
    }
}
