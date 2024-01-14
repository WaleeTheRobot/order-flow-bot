using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;

namespace OrderFlowBotUnitTests.Data
{
    public class VolumeData
    {
        public List<BidAskVolume> bidAskVolumeListDefault;

        public VolumeData()
        {
            bidAskVolumeListDefault = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4434.25,
                    BidVolume = 77,
                    AskVolume = 201
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
                    BidVolume = 150,
                    AskVolume = 25
                }
            };
        }

        public List<BidAskVolume> GetValidAskVolumeSequencing()
        {
            List<BidAskVolume> bidAskVolumeListBottom = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 10,
                    AskVolume = 100
                },
                new BidAskVolume
                {
                    Price = 4432.75,
                    BidVolume = 10,
                    AskVolume = 40
                },
                new BidAskVolume
                {
                    Price = 4432.50,
                    BidVolume = 10,
                    AskVolume = 30
                },
                new BidAskVolume
                {
                    Price = 4432.25,
                    BidVolume = 10,
                    AskVolume = 10
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListDefault);
            combinedList.AddRange(bidAskVolumeListBottom);

            return combinedList;
        }

        public List<BidAskVolume> GetValidBidVolumeSequencing()
        {
            List<BidAskVolume> bidAskVolumeListTop = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4435.25,
                    BidVolume = 10,
                    AskVolume = 100
                },
                new BidAskVolume
                {
                    Price = 4435.00,
                    BidVolume = 100,
                    AskVolume = 40
                },
                new BidAskVolume
                {
                    Price = 4434.75,
                    BidVolume = 150,
                    AskVolume = 100
                },
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 330,
                    AskVolume = 500
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListTop);
            combinedList.AddRange(bidAskVolumeListDefault);

            return combinedList;
        }

        public List<BidAskVolume> GetInValidBidAskTotalList()
        {
            List<BidAskVolume> bidAskVolumeList = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4435.25,
                    BidVolume = 10,
                    AskVolume = 100
                },
                new BidAskVolume
                {
                    Price = 4435.00,
                    BidVolume = 100,
                    AskVolume = 40
                },
                new BidAskVolume
                {
                    Price = 4434.75,
                    BidVolume = 150,
                    AskVolume = 100
                },
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 330,
                    AskVolume = 500
                }
            };

            return bidAskVolumeList;
        }
    }
}