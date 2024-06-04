using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies
{
    public struct ImbalancePrice
    {
        public double Price;
        public long Volume;
    }

    public class Imbalances
    {
        public List<ImbalancePrice> BidImbalances { get; set; }
        public List<ImbalancePrice> AskImbalances { get; set; }
        public List<ImbalancePrice> BidStackedImbalances { get; set; }
        public List<ImbalancePrice> AskStackedImbalances { get; set; }
        public bool HasBidStackedImbalances { get; set; }
        public bool HasAskStackedImbalances { get; set; }

        public Imbalances()
        {
            AskImbalances = new List<ImbalancePrice>();
            BidImbalances = new List<ImbalancePrice>();
            AskStackedImbalances = new List<ImbalancePrice>();
            BidStackedImbalances = new List<ImbalancePrice>();
        }

        private bool IsValidBidImbalance(List<BidAskVolume> bidAskVolumes, int index, long validImbalanceVolume, double imbalanceRatio)
        {
            long ask = bidAskVolumes[index - 1].AskVolume;
            long bid = bidAskVolumes[index].BidVolume;

            if (bid < validImbalanceVolume || ask < validImbalanceVolume)
            {
                return false;
            }

            // Diagonal calculations
            if (ask == 0)
            {
                return bid >= imbalanceRatio;
            }

            return (double)bid / ask >= imbalanceRatio;
        }

        private bool IsValidAskImbalance(List<BidAskVolume> bidAskVolumes, int index, long validImbalanceVolume, double imbalanceRatio)
        {
            long ask = bidAskVolumes[index].AskVolume;
            long bid = bidAskVolumes[index + 1].BidVolume;

            if (bid < validImbalanceVolume || ask < validImbalanceVolume)
            {
                return false;
            }

            // Diagonal calculations
            if (bid == 0)
            {
                return ask >= imbalanceRatio;
            }

            return (double)ask / bid >= imbalanceRatio;
        }

        public void SetImbalances(List<BidAskVolume> bidAskVolumes, bool validBidAskVolumes)
        {
            if (!validBidAskVolumes)
                return;

            double imbalanceRatio = OrderFlowBotDataBarConfig.ImbalanceRatio;
            long validImbalanceVolume = OrderFlowBotDataBarConfig.ValidImbalanceVolume;

            List<ImbalancePrice> askImbalancePriceList = new List<ImbalancePrice>();
            List<ImbalancePrice> bidImbalancePriceList = new List<ImbalancePrice>();

            for (int i = 0; i < bidAskVolumes.Count; i++)
            {
                // This is high of bar. Can calculate ask, but not bid.
                if (i == 0)
                {
                    bool isValidAskImbalance = IsValidAskImbalance(bidAskVolumes, i, validImbalanceVolume, imbalanceRatio);

                    if (isValidAskImbalance)
                    {
                        askImbalancePriceList.Add(new ImbalancePrice
                        {
                            Price = bidAskVolumes[i].Price,
                            Volume = bidAskVolumes[i].AskVolume
                        });
                    }
                }
                // This is low of bar. Can calculate bid, but not ask.
                else if (i == bidAskVolumes.Count - 1)
                {
                    bool isValidBidImbalance = IsValidBidImbalance(bidAskVolumes, i, validImbalanceVolume, imbalanceRatio);

                    if (isValidBidImbalance)
                    {
                        bidImbalancePriceList.Add(new ImbalancePrice
                        {
                            Price = bidAskVolumes[i].Price,
                            Volume = bidAskVolumes[i].BidVolume
                        });
                    }
                }
                else
                {
                    bool isValidAskImbalance = IsValidAskImbalance(bidAskVolumes, i, validImbalanceVolume, imbalanceRatio);
                    bool isValidBidImbalance = IsValidBidImbalance(bidAskVolumes, i, validImbalanceVolume, imbalanceRatio);

                    if (isValidAskImbalance)
                    {
                        askImbalancePriceList.Add(new ImbalancePrice
                        {
                            Price = bidAskVolumes[i].Price,
                            Volume = bidAskVolumes[i].AskVolume
                        });
                    }

                    if (isValidBidImbalance)
                    {
                        bidImbalancePriceList.Add(new ImbalancePrice
                        {
                            Price = bidAskVolumes[i].Price,
                            Volume = bidAskVolumes[i].BidVolume
                        });
                    }
                }
            }

            SetStackedImbalances(bidImbalancePriceList, askImbalancePriceList);

            this.BidImbalances = bidImbalancePriceList;
            this.AskImbalances = askImbalancePriceList;
        }

        private void SetStackedImbalances(List<ImbalancePrice> bidImbalancePriceList, List<ImbalancePrice> askImbalancePriceList)
        {
            int stackedImbalance = OrderFlowBotDataBarConfig.StackedImbalance;
            double tickSize = OrderFlowBotDataBarConfig.TickSize;

            this.BidStackedImbalances.Clear();
            this.AskStackedImbalances.Clear();

            ProcessStackedImbalances(bidImbalancePriceList, stackedImbalance, tickSize, isBid: true);
            ProcessStackedImbalances(askImbalancePriceList, stackedImbalance, tickSize, isBid: false);
        }

        private void ProcessStackedImbalances(List<ImbalancePrice> imbalancePriceList, int stackedImbalance, double tickSize, bool isBid)
        {
            List<ImbalancePrice> tempImbalanceList = new List<ImbalancePrice>();

            for (int i = 0; i < imbalancePriceList.Count; i++)
            {
                tempImbalanceList.Add(imbalancePriceList[i]);

                bool isLastItem = (i == imbalancePriceList.Count - 1);
                bool isNextItemWithinTickSize = !isLastItem && Math.Abs(imbalancePriceList[i + 1].Price - imbalancePriceList[i].Price) <= tickSize;

                if (!isNextItemWithinTickSize || isLastItem)
                {
                    if (tempImbalanceList.Count >= stackedImbalance)
                    {
                        if (isBid)
                            this.BidStackedImbalances.AddRange(tempImbalanceList);
                        else
                            this.AskStackedImbalances.AddRange(tempImbalanceList);
                    }
                    tempImbalanceList.Clear();
                }
            }

            this.HasBidStackedImbalances = BidStackedImbalances.Count > 0;
            this.HasAskStackedImbalances = AskStackedImbalances.Count > 0;
        }
    }
}
