using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;

namespace OrderFlowBotUnitTests.Data
{
    public class RatiosData
    {
        public List<BidAskVolume> bidAskVolumeListDefault;

        public RatiosData()
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
                    BidVolume = 176,
                    AskVolume = 146
                },
                new BidAskVolume
                {
                    Price = 4433.75,
                    BidVolume = 130,
                    AskVolume = 137
                },
                new BidAskVolume
                {
                    Price = 4433.50,
                    BidVolume = 58,
                    AskVolume = 45
                },
                new BidAskVolume
                {
                    Price = 4433.25,
                    BidVolume = 150,
                    AskVolume = 25
                }
            };
        }

        public List<BidAskVolume> GetBidAskRatio()
        {
            List<BidAskVolume> bidAskVolumeListTop = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 21,
                    AskVolume = 5
                }
            };

            List<BidAskVolume> bidAskVolumeListBottom = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 10,
                    AskVolume = 0
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListTop);
            combinedList.AddRange(bidAskVolumeListDefault);
            combinedList.AddRange(bidAskVolumeListBottom);

            return combinedList;
        }

        public List<BidAskVolume> GetBidAskRatioZero()
        {
            List<BidAskVolume> bidAskVolumeListTop = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 21,
                    AskVolume = 0
                }
            };

            List<BidAskVolume> bidAskVolumeListBottom = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 0,
                    AskVolume = 0
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListTop);
            combinedList.AddRange(bidAskVolumeListDefault);
            combinedList.AddRange(bidAskVolumeListBottom);

            return combinedList;
        }

        public List<BidAskVolume> GetValidBidAskRatio()
        {
            List<BidAskVolume> bidAskVolumeListTop = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 21,
                    AskVolume = 5
                }
            };

            List<BidAskVolume> bidAskVolumeListBottom = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 5,
                    AskVolume = 0
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListTop);
            combinedList.AddRange(bidAskVolumeListDefault);
            combinedList.AddRange(bidAskVolumeListBottom);

            return combinedList;
        }

        public List<BidAskVolume> GetValidBidAskExhaustion()
        {
            List<BidAskVolume> bidAskVolumeListTop = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4434.75,
                    BidVolume = 0,
                    AskVolume = 5
                },
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 79,
                    AskVolume = 84
                }
            };

            List<BidAskVolume> bidAskVolumeListBottom = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 43,
                    AskVolume = 42
                },
                new BidAskVolume
                {
                    Price = 4432.75,
                    BidVolume = 1,
                    AskVolume = 0
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListTop);
            combinedList.AddRange(bidAskVolumeListDefault);
            combinedList.AddRange(bidAskVolumeListBottom);

            return combinedList;
        }

        public List<BidAskVolume> GetValidBidAskAbsorption()
        {
            List<BidAskVolume> bidAskVolumeListTop = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 158,
                    AskVolume = 163,
                },
                new BidAskVolume
                {
                    Price = 4432.75,
                    BidVolume = 360,
                    AskVolume = 168
                }
            };

            List<BidAskVolume> bidAskVolumeListBottom = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 99,
                    AskVolume = 33,
                },
                new BidAskVolume
                {
                    Price = 4432.75,
                    BidVolume = 71,
                    AskVolume = 7
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListTop);
            combinedList.AddRange(bidAskVolumeListDefault);
            combinedList.AddRange(bidAskVolumeListBottom);

            return combinedList;
        }

        public List<BidAskVolume> GetInValidBidAskRatio()
        {
            List<BidAskVolume> bidAskVolumeListTop = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 21,
                    AskVolume = 180
                }
            };

            List<BidAskVolume> bidAskVolumeListBottom = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 130,
                    AskVolume = 0
                }
            };

            List<BidAskVolume> combinedList = new List<BidAskVolume>();

            combinedList.AddRange(bidAskVolumeListTop);
            combinedList.AddRange(bidAskVolumeListDefault);
            combinedList.AddRange(bidAskVolumeListBottom);

            return combinedList;
        }
    }
}
