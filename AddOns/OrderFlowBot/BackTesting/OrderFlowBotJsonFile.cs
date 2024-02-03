using Newtonsoft.Json;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System;
using System.Collections.Generic;
using System.IO;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.BackTesting
{
    public class OrderFlowBotTrade
    {
        public OrderFlowBotDataBar PreviousBar { get; set; }
        public OrderFlowBotDataBar EntryBar { get; set; }
        public double Pnl { get; set; }
        public string EntryType { get; set; }
    }

    public class OrderFlowBotJsonFile
    {
        private readonly string _filePathWinning;
        private readonly string _filePathLosing;

        public OrderFlowBotJsonFile()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _filePathWinning = Path.Combine(desktopPath, "orderflowbot-winning-trades.json");
            _filePathLosing = Path.Combine(desktopPath, "orderflowbot-losing-trades.json");

            File.WriteAllText(_filePathWinning, "[]");
            File.WriteAllText(_filePathLosing, "[]");
        }

        public void Append(OrderFlowBotDataBars dataBars, long entryBarNumber, double pnl, string entryType)
        {
            bool isWinning = pnl > 0;

            List<OrderFlowBotTrade> existingData = ReadExistingData(isWinning);

            existingData.Add(GetOrderFlowBotTrade(dataBars, entryBarNumber, pnl, entryType));

            string updatedJson = JsonConvert.SerializeObject(existingData, Formatting.Indented);

            if (isWinning)
            {
                File.WriteAllText(_filePathWinning, updatedJson);
            }

            else
            {
                File.WriteAllText(_filePathLosing, updatedJson);
            }
        }

        private List<OrderFlowBotTrade> ReadExistingData(bool isWinning)
        {
            if (isWinning)
            {
                if (File.Exists(_filePathWinning))
                {
                    string jsonData = File.ReadAllText(_filePathWinning);
                    return JsonConvert.DeserializeObject<List<OrderFlowBotTrade>>(jsonData) ?? new List<OrderFlowBotTrade>();
                }
            }
            else
            {
                if (File.Exists(_filePathLosing))
                {
                    string jsonData = File.ReadAllText(_filePathLosing);
                    return JsonConvert.DeserializeObject<List<OrderFlowBotTrade>>(jsonData) ?? new List<OrderFlowBotTrade>();
                }
            }

            return new List<OrderFlowBotTrade>();
        }

        private OrderFlowBotTrade GetOrderFlowBotTrade(OrderFlowBotDataBars dataBars, long entryBarNumber, double pnl, string entryType)
        {
            OrderFlowBotTrade orderFlowBotTrade = new OrderFlowBotTrade();

            foreach (var bar in dataBars.Bars)
            {
                if (bar.BarNumber == entryBarNumber)
                {
                    orderFlowBotTrade.EntryBar = bar;
                }
                else if (bar.BarNumber == entryBarNumber - 1)
                {
                    orderFlowBotTrade.PreviousBar = bar;
                }
            }

            orderFlowBotTrade.Pnl = pnl;
            orderFlowBotTrade.EntryType = entryType;

            return orderFlowBotTrade;
        }
    }
}
