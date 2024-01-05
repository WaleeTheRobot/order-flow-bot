using OrderFlowBotTestFiles;
using OrderFlowBotTestFiles.Files.Dependencies;
using OrderFlowBotUnitTests.Data;

namespace OrderFlowBotUnitTests
{
    public class ImbalancesTest
    {
        private readonly ImbalancesData _data;

        public ImbalancesTest()
        {
            _data = new ImbalancesData();
        }

        private void AssertVolumes(List<ImbalancePrice> imbalances, params long[] expectedVolumes)
        {
            for (int i = 0; i < expectedVolumes.Length; i++)
            {
                Assert.Equal(expectedVolumes[i], imbalances[i].Volume);
            }
        }

        [Fact(DisplayName = "Bid Ask Imbalances 1 should be correctly calculated")]
        public void BidAskImbalance1()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            dataBar.Imbalances.SetImbalances(_data.GetBidAskVolumeListImbalances1(), true);

            List<ImbalancePrice> bidImbalances = dataBar.Imbalances.BidImbalances;
            List<ImbalancePrice> askImbalances = dataBar.Imbalances.AskImbalances;

            List<ImbalancePrice> bidStackedImbalances = dataBar.Imbalances.BidStackedImbalances;
            List<ImbalancePrice> askStackedImbalances = dataBar.Imbalances.AskStackedImbalances;

            Assert.Equal(9, bidImbalances.Count);
            AssertVolumes(bidImbalances, 328, 724, 511, 399, 118, 269, 183, 115, 123);

            Assert.Single(askImbalances);
            AssertVolumes(askImbalances, 504);

            Assert.Empty(askStackedImbalances);
            Assert.Equal(5, bidStackedImbalances.Count);
            AssertVolumes(bidStackedImbalances, 118, 269, 183, 115, 123);
        }

        [Fact(DisplayName = "Bid Ask Imbalances 2 should be correctly calculated")]
        public void BidAskImbalance2()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            dataBar.Imbalances.SetImbalances(_data.GetBidAskVolumeListImbalances2(), true);

            List<ImbalancePrice> bidImbalances = dataBar.Imbalances.BidImbalances;
            List<ImbalancePrice> askImbalances = dataBar.Imbalances.AskImbalances;

            List<ImbalancePrice> bidStackedImbalances = dataBar.Imbalances.BidStackedImbalances;
            List<ImbalancePrice> askStackedImbalances = dataBar.Imbalances.AskStackedImbalances;

            Assert.Single(bidImbalances);
            AssertVolumes(bidImbalances, 583);

            Assert.Equal(7, askImbalances.Count);
            AssertVolumes(askImbalances, 18, 305, 163, 126, 1743, 1002, 149);

            Assert.Empty(bidStackedImbalances);
            Assert.Equal(3, askStackedImbalances.Count);
            AssertVolumes(askStackedImbalances, 305, 163, 126);
        }
    }
}
