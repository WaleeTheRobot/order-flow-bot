using OrderFlowBotTestFiles;
using OrderFlowBotTestFiles.Files.Dependencies;
using OrderFlowBotUnitTests.Data;
using OrderFlowBotUnitTests.Utils;

namespace OrderFlowBotUnitTests
{
    public class VolumeProfileTest
    {
        private readonly VolumeProfileData _data;
        private readonly OrderFlowBotDataBar _dataBar;

        public VolumeProfileTest()
        {
            _data = new VolumeProfileData();
            _dataBar = new OrderFlowBotDataBar();
        }

        // Method to test
        // TODO: Need to update
        private void PopulateVolumeProfile(OrderFlowBotDataBar dataBar)
        {
            OrderFlowBotVolumeProfile volumeProfile = new OrderFlowBotVolumeProfile();

            // Iterate through data from high to low and get the data to update volume profile
            foreach (BidAskVolume bidAskVolume in dataBar.Volumes.BidAskVolumes)
            {
                volumeProfile.AddOrUpdateVolume(bidAskVolume.Price, bidAskVolume.BidVolume + bidAskVolume.AskVolume);
            }

            volumeProfile.SetTotalVolume();
            volumeProfile.CalculateValueArea();

            dataBar.Volumes.VolumeProfile = volumeProfile;
        }

        private OrderFlowBotVolumeProfile GetCombinedVolumeProfile()
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            OrderFlowBotDataBar dataBar2 = new OrderFlowBotDataBar();
            OrderFlowBotDataBar dataBar3 = new OrderFlowBotDataBar();

            List<List<BidAskVolume>> combined = _data.GetCombinedBidAskVolumeList();

            dataBar.Volumes.BidAskVolumes = combined[0];
            dataBar2.Volumes.BidAskVolumes = combined[1];
            dataBar3.Volumes.BidAskVolumes = combined[2];

            PopulateVolumeProfile(dataBar);
            PopulateVolumeProfile(dataBar2);
            PopulateVolumeProfile(dataBar3);

            List<OrderFlowBotVolumeProfile> volumeProfiles = new List<OrderFlowBotVolumeProfile>()
            {
                dataBar.Volumes.VolumeProfile,
                dataBar2.Volumes.VolumeProfile,
                dataBar3.Volumes.VolumeProfile
            };

            // Test volume profile from last three bars
            OrderFlowBotVolumeProfile volumeProfile = new OrderFlowBotVolumeProfile();
            volumeProfile.CombineVolumeProfiles(volumeProfiles);

            return volumeProfile;
        }

        [Fact(DisplayName = "Volume Profile Total Volume should be correctly calculated")]
        public void VolumeProfileTotalVolume()
        {
            _dataBar.Volumes.BidAskVolumes = _data.GetBidAskVolumeList();
            PopulateVolumeProfile(_dataBar);

            Assert.Equal(1176, _dataBar.Volumes.VolumeProfile.TotalVolume);
        }

        [Fact(DisplayName = "Volume Profile POC should be correctly calculated")]
        public void VolumeProfilePOC()
        {
            _dataBar.Volumes.BidAskVolumes = _data.GetBidAskVolumeList();
            PopulateVolumeProfile(_dataBar);

            Assert.Equal(4434.00, _dataBar.Volumes.VolumeProfile.PointOfControl);
        }

        [Fact(DisplayName = "Volume Profile VAH should be correctly calculated")]
        public void VolumeProfileVAH()
        {
            _dataBar.Volumes.BidAskVolumes = _data.GetBidAskVolumeList();
            PopulateVolumeProfile(_dataBar);

            Assert.Equal(4434.50, _dataBar.Volumes.VolumeProfile.ValueAreaHigh);
        }

        [Fact(DisplayName = "Volume Profile VAL should be correctly calculated")]
        public void VolumeProfileVAL()
        {
            _dataBar.Volumes.BidAskVolumes = _data.GetBidAskVolumeList();
            PopulateVolumeProfile(_dataBar);

            Assert.Equal(4433.00, _dataBar.Volumes.VolumeProfile.ValueAreaLow);
        }

        [Fact(DisplayName = "Volume Profile Sorted Volume should be correctly calculated")]
        public void VolumeProfile()
        {
            _dataBar.Volumes.BidAskVolumes = _data.GetBidAskVolumeList();
            PopulateVolumeProfile(_dataBar);

            Assert.Equal(
                _data.GetBidAskVolumeListVolumeProfileSortedVolumes(),
                _dataBar.Volumes.VolumeProfile.SortedVolumes,
                new DictionaryEqualityComparer<double, long>()
            );
        }

        [Fact(DisplayName = "Volume Profile Combined Total Volume should be correctly calculated")]
        public void VolumeProfileCombinedTotalVolume()
        {
            Assert.Equal(3528, GetCombinedVolumeProfile().TotalVolume);
        }

        [Fact(DisplayName = "Volume Profile Combined POC should be correctly calculated")]
        public void VolumeProfileCombinedPOC()
        {
            Assert.Equal(4433.25, GetCombinedVolumeProfile().PointOfControl);
        }

        [Fact(DisplayName = "Volume Profile Combined VAH should be correctly calculated")]
        public void VolumeProfileCombinedVAH()
        {
            Assert.Equal(4435.75, GetCombinedVolumeProfile().ValueAreaHigh);
        }

        [Fact(DisplayName = "Volume Profile Combined VAL should be correctly calculated")]
        public void VolumeProfileCombinedVAL()
        {
            Assert.Equal(4432.25, GetCombinedVolumeProfile().ValueAreaLow);
        }

        [Fact(DisplayName = "Volume Profile Combined Sorted Volume should be correctly calculated")]
        public void VolumeProfileCombined()
        {
            Assert.Equal(
                _data.GetCombinedBidAskVolumeListVolumeProfileSortedVolumes(),
                GetCombinedVolumeProfile().SortedVolumes,
                new DictionaryEqualityComparer<double, long>()
            );
        }
    }
}
