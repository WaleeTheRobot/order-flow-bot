﻿using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base
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
        public double TickSize { get; set; }
        public double ImbalanceRatio { get; set; }
        public long ImbalanceMinDelta { get; set; }
        public int StackedImbalance { get; set; }

        public Imbalances(IDataBarConfig config)
        {
            AskImbalances = new List<ImbalancePrice>();
            BidImbalances = new List<ImbalancePrice>();
            AskStackedImbalances = new List<ImbalancePrice>();
            BidStackedImbalances = new List<ImbalancePrice>();

            TickSize = config.TickSize * config.TicksPerLevel;
            ImbalanceRatio = config.ImbalanceRatio;
            ImbalanceMinDelta = config.ImbalanceMinDelta;
            StackedImbalance = config.StackedImbalance;
        }

        private bool IsValidBidImbalance(List<BidAskVolume> bidAskVolumes, int index)
        {
            long ask = bidAskVolumes[index - 1].AskVolume;
            long bid = bidAskVolumes[index].BidVolume;

            // Needs to have more than imbalance min delta requirement
            if (bid - ask < ImbalanceMinDelta)
            {
                return false;
            }

            // Diagonal calculations
            if (ask == 0)
            {
                return bid >= ImbalanceRatio;
            }

            return (double)bid / ask >= ImbalanceRatio;
        }

        private bool IsValidAskImbalance(List<BidAskVolume> bidAskVolumes, int index)
        {
            long ask = bidAskVolumes[index].AskVolume;
            long bid = bidAskVolumes[index + 1].BidVolume;

            // Needs to have more than imbalance min delta requirement
            if (ask - bid < ImbalanceMinDelta)
            {
                return false;
            }

            // Diagonal calculations
            if (bid == 0)
            {
                return ask >= ImbalanceRatio;
            }

            return (double)ask / bid >= ImbalanceRatio;
        }

        public void SetImbalances(List<BidAskVolume> bidAskVolumes)
        {
            List<ImbalancePrice> askImbalancePriceList = new List<ImbalancePrice>();
            List<ImbalancePrice> bidImbalancePriceList = new List<ImbalancePrice>();

            for (int i = 0; i < bidAskVolumes.Count; i++)
            {
                // This is high of bar. Can calculate ask, but not bid.
                if (i == 0)
                {
                    bool isValidAskImbalance = IsValidAskImbalance(bidAskVolumes, i);

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
                    bool isValidBidImbalance = IsValidBidImbalance(bidAskVolumes, i);

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
                    bool isValidAskImbalance = IsValidAskImbalance(bidAskVolumes, i);
                    bool isValidBidImbalance = IsValidBidImbalance(bidAskVolumes, i);

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

            BidImbalances = bidImbalancePriceList;
            AskImbalances = askImbalancePriceList;
        }

        private void SetStackedImbalances(List<ImbalancePrice> bidImbalancePriceList, List<ImbalancePrice> askImbalancePriceList)
        {
            BidStackedImbalances.Clear();
            AskStackedImbalances.Clear();

            ProcessStackedImbalances(bidImbalancePriceList, isBid: true);
            ProcessStackedImbalances(askImbalancePriceList, isBid: false);
        }

        private void ProcessStackedImbalances(List<ImbalancePrice> imbalancePriceList, bool isBid)
        {
            List<ImbalancePrice> tempImbalanceList = new List<ImbalancePrice>();

            for (int i = 0; i < imbalancePriceList.Count; i++)
            {
                tempImbalanceList.Add(imbalancePriceList[i]);

                bool isLastItem = i == imbalancePriceList.Count - 1;
                bool isNextItemWithinTickSize = !isLastItem && Math.Abs(imbalancePriceList[i + 1].Price - imbalancePriceList[i].Price) <= TickSize;

                if (!isNextItemWithinTickSize || isLastItem)
                {
                    if (tempImbalanceList.Count >= StackedImbalance)
                    {
                        if (isBid)
                            BidStackedImbalances.AddRange(tempImbalanceList);
                        else
                            AskStackedImbalances.AddRange(tempImbalanceList);
                    }
                    tempImbalanceList.Clear();
                }
            }

            HasBidStackedImbalances = BidStackedImbalances.Count > 0;
            HasAskStackedImbalances = AskStackedImbalances.Count > 0;
        }
    }
}
