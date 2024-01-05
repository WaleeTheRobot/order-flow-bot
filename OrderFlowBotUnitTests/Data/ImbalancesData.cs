using OrderFlowBotTestFiles.Files.Dependencies;

namespace OrderFlowBotUnitTests.Data
{
    public class ImbalancesData
    {
        public ImbalancesData()
        {

        }

        public List<BidAskVolume> GetBidAskVolumeListImbalances1()
        {
            return CommonData.DefaultBidAskVolumeListImbalances1;
        }

        public List<BidAskVolume> GetBidAskVolumeListImbalances2()
        {
            return CommonData.DefaultBidAskVolumeListImbalances2;
        }
    }
}
