namespace OrderFlowBotTestFiles
{
    // Close enough...
    public class OrderFlowBotVolumeProfile
    {
        public long HighestVolume { get; set; }
        public double PointOfControl { get; set; }
        public double ValueAreaHigh { get; set; }
        public double ValueAreaLow { get; set; }
        public long TotalVolume { get; set; }
        private Dictionary<double, long> Volumes { get; set; }
        public Dictionary<double, long> SortedVolumes { get; set; }

        public OrderFlowBotVolumeProfile()
        {
            Volumes = new Dictionary<double, long>();
            SortedVolumes = new Dictionary<double, long>();
        }

        public void Reset()
        {
            Volumes = new Dictionary<double, long>();
            SortedVolumes = new Dictionary<double, long>();
            HighestVolume = 0;
            PointOfControl = 0;
            ValueAreaHigh = 0;
            ValueAreaLow = 0;
            TotalVolume = 0;
        }

        public void AddOrUpdateVolume(double price, long volume)
        {
            if (Volumes.ContainsKey(price))
            {
                if (volume == Volumes[price])
                    return;

                Volumes[price] += volume;
            }
            else
            {
                Volumes[price] = volume;
            }

            SetPointOfControl();
        }

        private void SetPointOfControl()
        {
            var maxVolumeEntry = Volumes.OrderByDescending(entry => entry.Value).First();

            PointOfControl = maxVolumeEntry.Key;
            HighestVolume = maxVolumeEntry.Value;
        }

        public void SetTotalVolume()
        {
            // Sort the dictionary by key in descending order
            SortedVolumes = Volumes.OrderByDescending(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value);

            // Reset
            TotalVolume = 0;
            TotalVolume += SortedVolumes.Values.Sum();
        }

        public void CalculateValueArea()
        {
            double cumulativePercentage = 0.70;

            double cumulativeVolume = 0;
            double highBoundary = PointOfControl;
            double lowBoundary = PointOfControl;

            var upperBoundaryDictionary = new Dictionary<double, long>();
            var lowerBoundaryDictionary = new Dictionary<double, long>();

            foreach (KeyValuePair<double, long> kvp in SortedVolumes)
            {
                if (kvp.Key > PointOfControl)
                {
                    upperBoundaryDictionary.Add(kvp.Key, kvp.Value);
                }

                if (kvp.Key < PointOfControl)
                {
                    lowerBoundaryDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            // Reverse the upperBoundaryDictionary
            upperBoundaryDictionary = upperBoundaryDictionary.Reverse().ToDictionary(x => x.Key, x => x.Value);

            double volumeLimit = TotalVolume * cumulativePercentage;

            // Create lists of keys for both upper and lower boundaries
            var upperBoundaryKeys = new List<double>(upperBoundaryDictionary.Keys);
            var lowerBoundaryKeys = new List<double>(lowerBoundaryDictionary.Keys);

            int upperBoundaryIndex = 0;
            int lowerBoundaryIndex = 0;

            while (cumulativeVolume < volumeLimit && (upperBoundaryIndex < upperBoundaryKeys.Count || lowerBoundaryIndex < lowerBoundaryKeys.Count))
            {
                long upperVolume = 0;
                long lowerVolume = 0;

                // Check if there are elements in the upper boundary
                if (upperBoundaryIndex < upperBoundaryKeys.Count && upperBoundaryKeys.Count > 0)
                {
                    upperVolume += upperBoundaryDictionary[upperBoundaryKeys[upperBoundaryIndex]];

                    // Check if there's a next element
                    if (upperBoundaryIndex + 1 < upperBoundaryKeys.Count)
                    {
                        upperVolume += upperBoundaryDictionary[upperBoundaryKeys[upperBoundaryIndex + 1]];
                        highBoundary = upperBoundaryKeys[upperBoundaryIndex + 1];
                    }
                }

                // Check if there are elements in the lower boundary
                if (lowerBoundaryIndex < lowerBoundaryKeys.Count)
                {
                    lowerVolume += lowerBoundaryDictionary[lowerBoundaryKeys[lowerBoundaryIndex]];

                    // Check if there's a next element
                    if (lowerBoundaryIndex + 1 < lowerBoundaryKeys.Count)
                    {
                        lowerVolume += lowerBoundaryDictionary[lowerBoundaryKeys[lowerBoundaryIndex + 1]];
                        lowBoundary = lowerBoundaryKeys[lowerBoundaryIndex + 1];
                    }
                }

                // Determine which boundary has the greater volume
                if (upperVolume >= lowerVolume)
                {
                    cumulativeVolume += upperVolume;
                    if (upperBoundaryIndex + 2 < upperBoundaryKeys.Count)
                    {
                        upperBoundaryIndex += 2;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    cumulativeVolume += lowerVolume;
                    if (lowerBoundaryIndex + 2 < lowerBoundaryKeys.Count)
                    {
                        lowerBoundaryIndex += 2;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            ValueAreaHigh = highBoundary;
            ValueAreaLow = lowBoundary;
        }

        public void CombineVolumeProfiles(List<OrderFlowBotVolumeProfile> orderFlowBotVolumeProfiles)
        {
            Reset();

            Dictionary<double, long> combinedVolumes = new Dictionary<double, long>();

            foreach (OrderFlowBotVolumeProfile volumeProfile in orderFlowBotVolumeProfiles)
            {
                foreach (var kvp in volumeProfile.SortedVolumes)
                {
                    double price = kvp.Key;
                    long volume = kvp.Value;

                    if (combinedVolumes.ContainsKey(price))
                    {
                        combinedVolumes[price] += volume;
                    }
                    else
                    {
                        combinedVolumes[price] = volume;
                    }
                }
            }

            foreach (KeyValuePair<double, long> kvp in combinedVolumes)
            {
                AddOrUpdateVolume(kvp.Key, kvp.Value);
            }

            SetTotalVolume();
            CalculateValueArea();
        }
    }
}
