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
        public static List<BidAskVolume> DefaultBidAskVolumeListImbalances1 { get; set; }
        public static List<BidAskVolume> DefaultBidAskVolumeListImbalances2 { get; set; }

        static CommonData()
        {
            OrderFlowBotPropertiesConfig config = new OrderFlowBotPropertiesConfig
            {
                TickSize = 0.25,
                ImbalanceRatio = 1.5,
                StackedImbalance = 3,
                ValidBidVolume = 0,
                ValidAskVolume = 0,
                ValidExhaustionRatio = 15,
                ValidAbsorptionRatio = 1.4
            };

            OrderFlowBotProperties.Initialize(config);

            DefaultBidAskVolumeList = new List<BidAskVolume>();
            DefaultBidAskVolumeList2 = new List<BidAskVolume>();
            DefaultBidAskVolumeList3 = new List<BidAskVolume>();
            CombinedDefaultBidAskVolumeLists = new List<List<BidAskVolume>>();

            DefaultBidAskVolumeListImbalances1 = new List<BidAskVolume>();
            DefaultBidAskVolumeListImbalances2 = new List<BidAskVolume>();

            SetDefaultBidAskVolumeLists();
            SetCombinedDefaultBidAskVolumeLists();
            SetDefaultBidAskVolumeListsImbalances();
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

        private static void SetDefaultBidAskVolumeListsImbalances()
        {
            DefaultBidAskVolumeListImbalances1 = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4824.00,
                    BidVolume = 51,
                    AskVolume = 110
                },
                new BidAskVolume
                {
                    Price = 4823.75,
                    BidVolume = 328,
                    AskVolume = 463
                },
                new BidAskVolume
                {
                    Price = 4823.50,
                    BidVolume = 694,
                    AskVolume = 739
                },
                new BidAskVolume
                {
                    Price = 4823.25,
                    BidVolume = 516,
                    AskVolume = 448
                },
                new BidAskVolume
                {
                    Price = 4823.00,
                    BidVolume = 412,
                    AskVolume = 383
                },
                new BidAskVolume
                {
                    Price = 4822.75,
                    BidVolume = 724,
                    AskVolume = 504
                },
                new BidAskVolume
                {
                    Price = 4822.50,
                    BidVolume = 323,
                    AskVolume = 167
                },
                new BidAskVolume
                {
                    Price = 4822.25,
                    BidVolume = 511,
                    AskVolume = 218
                },
                new BidAskVolume
                {
                    Price = 4822.00,
                    BidVolume = 165,
                    AskVolume = 90
                },
                new BidAskVolume
                {
                    Price = 4821.75,
                    BidVolume = 399,
                    AskVolume = 303
                },
                new BidAskVolume
                {
                    Price = 4821.50,
                    BidVolume = 211,
                    AskVolume = 21
                },
                new BidAskVolume
                {
                    Price = 4821.25,
                    BidVolume = 118,
                    AskVolume = 0
                },
                new BidAskVolume
                {
                    Price = 4821.00,
                    BidVolume = 269,
                    AskVolume = 0
                },
                new BidAskVolume
                {
                    Price = 4820.75,
                    BidVolume = 183,
                    AskVolume = 0
                },
                new BidAskVolume
                {
                    Price = 4820.50,
                    BidVolume = 115,
                    AskVolume = 0
                },
                new BidAskVolume
                {
                    Price = 4820.25,
                    BidVolume = 123,
                    AskVolume = 0
                }
            };

            DefaultBidAskVolumeListImbalances2 = new List<BidAskVolume>
            {
                new BidAskVolume
                {
                    Price = 4824.00,
                    BidVolume = 0,
                    AskVolume = 18
                },
                new BidAskVolume
                {
                    Price = 4823.75,
                    BidVolume = 1,
                    AskVolume = 36
                },
                new BidAskVolume
                {
                    Price = 4823.50,
                    BidVolume = 35,
                    AskVolume = 161
                },
                new BidAskVolume
                {
                    Price = 4823.25,
                    BidVolume = 190,
                    AskVolume = 305
                },
                new BidAskVolume
                {
                    Price = 4823.00,
                    BidVolume = 195,
                    AskVolume = 163
                },
                new BidAskVolume
                {
                    Price = 4822.75,
                    BidVolume = 38,
                    AskVolume = 126
                },
                new BidAskVolume
                {
                    Price = 4822.50,
                    BidVolume = 6,
                    AskVolume = 121
                },
                new BidAskVolume
                {
                    Price = 4822.25,
                    BidVolume = 130,
                    AskVolume = 313
                },
                new BidAskVolume
                {
                    Price = 4822.00,
                    BidVolume = 583,
                    AskVolume = 732
                },
                new BidAskVolume
                {
                    Price = 4821.75,
                    BidVolume = 793,
                    AskVolume = 1743
                },
                new BidAskVolume
                {
                    Price = 4821.50,
                    BidVolume = 883,
                    AskVolume = 1002
                },
                new BidAskVolume
                {
                    Price = 4821.25,
                    BidVolume = 636,
                    AskVolume = 625
                },
                new BidAskVolume
                {
                    Price = 4821.00,
                    BidVolume = 527,
                    AskVolume = 491
                },
                new BidAskVolume
                {
                    Price = 4820.75,
                    BidVolume = 370,
                    AskVolume = 250
                },
                new BidAskVolume
                {
                    Price = 4820.50,
                    BidVolume = 239,
                    AskVolume = 149
                },
                new BidAskVolume
                {
                    Price = 4820.25,
                    BidVolume = 59,
                    AskVolume = 48
                }
            };
        }
    }
}
