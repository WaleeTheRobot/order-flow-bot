using OrderFlowBotTestFiles.Common;

namespace OrderFlowBotTestFiles.Files.Dependencies
{
    public struct ImbalancePrice
    {
        public double Price;
        public long Volume;
    }

    public class Imbalances
    {
        public List<ImbalancePrice> BidStackedImbalances { get; set; }
        public List<ImbalancePrice> AskStackedImbalances { get; set; }
        public bool HasBidStackedImbalances { get; set; }
        public bool HasAskStackedImbalances { get; set; }

        public Imbalances()
        {
            AskStackedImbalances = new List<ImbalancePrice>();
            BidStackedImbalances = new List<ImbalancePrice>();
        }

        private bool IsValidBidImbalance(List<BidAskVolume> bidAskVolumes, int index, long validBidVolume, long validAskVolume, double imbalanceRatio)
        {
            long ask = bidAskVolumes[index - 1].AskVolume;
            long bid = bidAskVolumes[index].BidVolume;

            if (bid < validBidVolume || ask < validAskVolume)
            {
                return false;
            }

            // Diagonal calculations
            if (ask == 0)
            {
                return bid >= imbalanceRatio;
            }

            return bid / ask >= imbalanceRatio;
        }

        private bool IsValidAskImbalance(List<BidAskVolume> bidAskVolumes, int index, long validBidVolume, long validAskVolume, double imbalanceRatio)
        {
            long ask = bidAskVolumes[index].AskVolume;
            long bid = bidAskVolumes[index + 1].BidVolume;

            if (bid < validBidVolume || ask < validAskVolume)
            {
                return false;
            }

            // Diagonal calculations
            if (bid == 0)
            {
                return ask >= imbalanceRatio;
            }

            return ask / bid >= imbalanceRatio;
        }

        public void SetStackedImbalances(List<BidAskVolume> bidAskVolumes, bool validBidAskVolumes)
        {
            if (!validBidAskVolumes)
                return;

            double imbalanceRatio = OrderFlowBotProperties.ImbalanceRatio;
            long validBidVolume = OrderFlowBotProperties.ValidBidVolume;
            long validAskVolume = OrderFlowBotProperties.ValidAskVolume;
            int stackedImbalance = OrderFlowBotProperties.StackedImbalance;

            int totalBidStackedImbalance = 0;
            int totalAskStackedImbalance = 0;

            List<ImbalancePrice> askImbalancePriceList = new List<ImbalancePrice>();
            List<ImbalancePrice> bidImbalancePriceList = new List<ImbalancePrice>();

            for (int i = 0; i < bidAskVolumes.Count; i++)
            {
                // This is high of bar. Can calculate ask, but not bid.
                if (i == 0)
                {
                    bool isValidAskImbalance = IsValidAskImbalance(bidAskVolumes, i, validBidVolume, validAskVolume, imbalanceRatio);

                    if (isValidAskImbalance)
                    {
                        totalAskStackedImbalance++;

                        askImbalancePriceList.Add(new ImbalancePrice
                        {
                            Price = bidAskVolumes[i].Price,
                            Volume = bidAskVolumes[i].AskVolume
                        });

                        return;
                    }

                    if (totalAskStackedImbalance <= stackedImbalance)
                    {
                        totalAskStackedImbalance = 0;
                    }
                }
                // This is low of bar. Can calculate bid, but not ask.
                else if (i == bidAskVolumes.Count - 1)
                {
                    bool isValidBidImbalance = IsValidBidImbalance(bidAskVolumes, i, validBidVolume, validAskVolume, imbalanceRatio);

                    if (isValidBidImbalance)
                    {
                        totalBidStackedImbalance++;

                        bidImbalancePriceList.Add(new ImbalancePrice
                        {
                            Price = bidAskVolumes[i].Price,
                            Volume = bidAskVolumes[i].BidVolume
                        });

                        return;
                    }

                    if (totalBidStackedImbalance <= stackedImbalance)
                    {
                        totalBidStackedImbalance = 0;
                    }
                }
                else
                {
                    bool isValidAskImbalance = IsValidAskImbalance(bidAskVolumes, i, validBidVolume, validAskVolume, imbalanceRatio);
                    bool isValidBidImbalance = IsValidBidImbalance(bidAskVolumes, i, validBidVolume, validAskVolume, imbalanceRatio);

                    if (isValidAskImbalance)
                    {
                        totalAskStackedImbalance++;

                        askImbalancePriceList.Add(new ImbalancePrice
                        {
                            Price = bidAskVolumes[i].Price,
                            Volume = bidAskVolumes[i].AskVolume
                        });
                    }
                    else
                    {
                        if (totalAskStackedImbalance <= stackedImbalance)
                        {
                            totalAskStackedImbalance = 0;
                        }
                    }

                    if (isValidBidImbalance)
                    {
                        totalBidStackedImbalance++;

                        bidImbalancePriceList.Add(new ImbalancePrice
                        {
                            Price = bidAskVolumes[i].Price,
                            Volume = bidAskVolumes[i].BidVolume
                        });
                    }
                    else
                    {
                        if (totalBidStackedImbalance <= stackedImbalance)
                        {
                            totalBidStackedImbalance = 0;
                        }
                    }
                }
            }

            this.HasBidStackedImbalances = totalBidStackedImbalance >= stackedImbalance;
            this.HasAskStackedImbalances = totalAskStackedImbalance >= stackedImbalance;
            this.BidStackedImbalances = bidImbalancePriceList;
            this.AskStackedImbalances = askImbalancePriceList;
        }
    }
}
