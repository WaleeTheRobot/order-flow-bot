using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;

namespace OrderFlowBotUnitTests.Data
{
    public class SinglePrintData
    {
        public List<BidAskVolume> bidAskVolumeListDefault;

        public SinglePrintData()
        {
            bidAskVolumeListDefault = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4434.25,
                    BidVolume = 10,
                    AskVolume = 10
                },
                new BidAskVolume
                {
                    Price = 4434.00,
                    BidVolume = 50,
                    AskVolume = 146
                },
                new BidAskVolume
                {
                    Price = 4433.75,
                    BidVolume = 130,
                    AskVolume = 20
                },
                new BidAskVolume
                {
                    Price = 4433.50,
                    BidVolume = 58,
                    AskVolume = 75
                },
                new BidAskVolume
                {
                    Price = 4433.25,
                    BidVolume = 10,
                    AskVolume = 10
                }
            };
        }

        public List<BidAskVolume> GetValidAskSinglePrint()
        {
            List<BidAskVolume> bidAskVolumeListTop = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 1,
                    AskVolume = 9
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListTop);
            combinedList.AddRange(bidAskVolumeListDefault);

            return combinedList;
        }

        public List<BidAskVolume> GetValidBidSinglePrint()
        {
            List<BidAskVolume> bidAskVolumeListBottom = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4435.25,
                    BidVolume = 9,
                    AskVolume = 2
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListDefault);
            combinedList.AddRange(bidAskVolumeListBottom);

            return combinedList;
        }
    }
}