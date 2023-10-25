using OrderFlowBotTestFiles.Files.Dependencies;

namespace OrderFlowBotUnitTests.Data
{
    public class VolumeProfileData
    {
        public VolumeProfileData()
        {

        }

        public List<BidAskVolume> GetBidAskVolumeList()
        {
            return CommonData.DefaultBidAskVolumeList;
        }

        public Dictionary<double, long> GetBidAskVolumeListVolumeProfileSortedVolumes()
        {
            Dictionary<double, long> sortedVolumes = new Dictionary<double, long>()
            {
                { 4434.50, 26 },
                { 4434.25, 278 },
                { 4434.00, 322 },
                { 4433.75, 267 },
                { 4433.50, 103 },
                { 4433.25, 175 },
                { 4433.00, 5 }
            };

            return sortedVolumes;
        }

        public List<List<BidAskVolume>> GetCombinedBidAskVolumeList()
        {
            return CommonData.CombinedDefaultBidAskVolumeLists;
        }

        public Dictionary<double, long> GetCombinedBidAskVolumeListVolumeProfileSortedVolumes()
        {
            Dictionary<double, long> sortedVolumes = new Dictionary<double, long>()
            {
                { 4436.00, 26},
                { 4435.75, 278},
                { 4435.50, 322},
                { 4435.25, 267},
                { 4435.00, 103},
                { 4434.75, 175},
                { 4434.50, 31 },
                { 4434.25, 278 },
                { 4434.00, 322 },
                { 4433.75, 267 },
                { 4433.50, 129 },
                { 4433.25, 453 },
                { 4433.00, 327 },
                { 4432.75, 267},
                { 4432.50, 103},
                { 4432.25, 175},
                { 4432.00, 5}
            };

            return sortedVolumes;
        }
    }
}
