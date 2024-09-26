using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class DataBarEvents
    {
        private readonly EventManager _eventManager;
        public event Action<DataBarDataProvider> OnUpdateCurrentDataBar;
        public event Action OnUpdateCurrentDataBarList;
        public event Action<DataBarPrintConfig> OnPrintDataBar;

        public DataBarEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public void UpdateCurrentDataBar(DataBarDataProvider dataBarDataProvider)
        {
            _eventManager.InvokeEvent(OnUpdateCurrentDataBar, dataBarDataProvider);
        }

        public void UpdateCurrentDataBarList()
        {
            _eventManager.InvokeEvent(OnUpdateCurrentDataBarList);
        }

        public void PrintDataBar(DataBarPrintConfig dataBarPrintConfig)
        {
            _eventManager.InvokeEvent(OnPrintDataBar, dataBarPrintConfig);
        }
    }
}
