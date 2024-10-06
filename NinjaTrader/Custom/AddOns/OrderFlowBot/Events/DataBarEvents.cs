using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class DataBarEvents
    {
        private readonly EventManager _eventManager;
        public event Action<IDataBarDataProvider> OnUpdateCurrentDataBar;
        public event Action OnUpdateCurrentDataBarList;
        public event Action<DataBarPrintConfig> OnPrintDataBar;
        public event Action<DataBar, List<DataBar>> OnUpdatedCurrentDataBar;
        public event Func<DataBar> OnGetCurrentDataBar;
        public event Func<List<DataBar>> OnGetDataBars;

        public DataBarEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public void UpdateCurrentDataBar(IDataBarDataProvider dataBarDataProvider)
        {
            _eventManager.InvokeEvent(OnUpdateCurrentDataBar, dataBarDataProvider);
        }

        public void UpdateCurrentDataBarList()
        {
            _eventManager.InvokeEvent(OnUpdateCurrentDataBarList);
        }

        public void UpdatedCurrentDataBar(DataBar currentDataBar, List<DataBar> dataBars)
        {
            _eventManager.InvokeEvent(OnUpdatedCurrentDataBar, currentDataBar, dataBars);
        }

        public DataBar GetCurrentDataBar()
        {
            return _eventManager.InvokeEvent(() => OnGetCurrentDataBar?.Invoke());
        }

        public List<DataBar> GetDataBars()
        {
            return _eventManager.InvokeEvent(() => OnGetDataBars?.Invoke());
        }

        public void PrintDataBar(DataBarPrintConfig dataBarPrintConfig)
        {
            _eventManager.InvokeEvent(OnPrintDataBar, dataBarPrintConfig);
        }
    }
}
