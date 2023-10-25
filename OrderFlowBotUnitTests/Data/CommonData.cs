using OrderFlowBotTestFiles.Common;
using OrderFlowBotTestFiles.Files.Dependencies;

namespace OrderFlowBotUnitTests.Data
{
    public static class CommonData
    {
        public static List<BidAskVolume> DefaultBidAskVolumeList { get; set; }
        public static List<BidAskVolume> DefaultBidAskVolumeList2 { get; set; }
        public static List<BidAskVolume> DefaultBidAskVolumeList3 { get; set; }
        public static List<List<BidAskVolume>> CombinedDefaultBidAskVolumeLists { get; set; }

        static CommonData()
        {
            OrderFlowBotPropertiesConfig config = new OrderFlowBotPropertiesConfig
            {
                AutoVolumeProfileLookBackBars = 3,
                ImbalanceRatio = 1.5,
                StackedImbalance = 3,
                ValidBidVolume = 0,
                ValidAskVolume = 0,
                ValidRatio = 15
            };

            OrderFlowBotProperties.Initialize(config);

            DefaultBidAskVolumeList = new List<BidAskVolume>();
            DefaultBidAskVolumeList2 = new List<BidAskVolume>();
            DefaultBidAskVolumeList3 = new List<BidAskVolume>();
            CombinedDefaultBidAskVolumeLists = new List<List<BidAskVolume>>();

            SetDefaultBidAskVolumeLists();
            SetCombinedDefaultBidAskVolumeLists();
        }

        private static void SetDefaultBidAskVolumeLists()
        {
            DefaultBidAskVolumeList = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 21,
                    AskVolume = 5
                },
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
                },
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 5,
                    AskVolume = 0
                }
            };

            DefaultBidAskVolumeList2 = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4436.00,
                    BidVolume = 21,
                    AskVolume = 5
                },
                new BidAskVolume
                {
                    Price = 4435.75,
                    BidVolume = 77,
                    AskVolume = 201
                },
                new BidAskVolume
                {
                    Price = 4435.50,
                    BidVolume = 176,
                    AskVolume = 146
                },
                new BidAskVolume
                {
                    Price = 4435.25,
                    BidVolume = 130,
                    AskVolume = 137
                },
                new BidAskVolume
                {
                    Price = 4435.00,
                    BidVolume = 58,
                    AskVolume = 45
                },
                new BidAskVolume
                {
                    Price = 4434.75,
                    BidVolume = 150,
                    AskVolume = 25
                },
                new BidAskVolume
                {
                    Price = 4434.50,
                    BidVolume = 5,
                    AskVolume = 0
                }
            };

            DefaultBidAskVolumeList3 = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4433.50,
                    BidVolume = 21,
                    AskVolume = 5
                },
                new BidAskVolume
                {
                    Price = 4433.25,
                    BidVolume = 77,
                    AskVolume = 201
                },
                new BidAskVolume
                {
                    Price = 4433.00,
                    BidVolume = 176,
                    AskVolume = 146
                },
                new BidAskVolume
                {
                    Price = 4432.75,
                    BidVolume = 130,
                    AskVolume = 137
                },
                new BidAskVolume
                {
                    Price = 4432.50,
                    BidVolume = 58,
                    AskVolume = 45
                },
                new BidAskVolume
                {
                    Price = 4432.25,
                    BidVolume = 150,
                    AskVolume = 25
                },
                new BidAskVolume
                {
                    Price = 4432.00,
                    BidVolume = 5,
                    AskVolume = 0
                }
            };
        }

        // Simulate BidAskVolume list from multiple bars
        private static void SetCombinedDefaultBidAskVolumeLists()
        {
            CombinedDefaultBidAskVolumeLists = new List<List<BidAskVolume>>
            {
                DefaultBidAskVolumeList,
                DefaultBidAskVolumeList2,
                DefaultBidAskVolumeList3
            };
        }
    }
}
