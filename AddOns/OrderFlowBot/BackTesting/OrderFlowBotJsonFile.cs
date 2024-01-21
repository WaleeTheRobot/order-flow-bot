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
        private readonly string _filePath;

        public OrderFlowBotJsonFile()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _filePath = Path.Combine(desktopPath, "orderflowbot-trades.json");

            File.WriteAllText(_filePath, "[]");
        }

        public void Append(OrderFlowBotDataBars dataBars, long entryBarNumber, double pnl, string entryType)
        {
            List<OrderFlowBotTrade> existingData = ReadExistingData();

            existingData.Add(GetOrderFlowBotTrade(dataBars, entryBarNumber, pnl, entryType));

            string updatedJson = JsonConvert.SerializeObject(existingData, Formatting.Indented);
            File.WriteAllText(_filePath, updatedJson);
        }

        private List<OrderFlowBotTrade> ReadExistingData()
        {
            if (File.Exists(_filePath))
            {
                string jsonData = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<OrderFlowBotTrade>>(jsonData) ?? new List<OrderFlowBotTrade>();
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
