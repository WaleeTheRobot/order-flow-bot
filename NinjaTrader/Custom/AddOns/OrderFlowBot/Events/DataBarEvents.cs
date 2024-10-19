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
        public event Action<IDataBarPrintConfig> OnPrintDataBar;
        public event Action OnUpdatedCurrentDataBar;
        public event Func<IReadOnlyDataBar> OnGetCurrentDataBar;
        public event Func<List<IReadOnlyDataBar>> OnGetDataBars;

        public DataBarEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered when the current DataBar needs to be updated.
        /// This is used update the current DataBar.
        /// </summary>
        public void UpdateCurrentDataBar(IDataBarDataProvider dataBarDataProvider)
        {
            _eventManager.InvokeEvent(OnUpdateCurrentDataBar, dataBarDataProvider);
        }

        /// <summary>
        /// Event triggered when the current DataBar needs to be added to the list.
        /// This is used update the DataBar list with the current DataBar.
        /// </summary>
        public void UpdateCurrentDataBarList()
        {
            _eventManager.InvokeEvent(OnUpdateCurrentDataBarList);
        }

        /// <summary>
        /// Event triggered when the current DataBar is updated.
        /// This is used when the current DataBar is updated with new data.
        /// </summary>
        public void UpdatedCurrentDataBar()
        {
            _eventManager.InvokeEvent(OnUpdatedCurrentDataBar);
        }

        /// <summary>
        /// Event triggered when current DataBar is requested.
        /// This is used to get the current read only DataBar.
        /// </summary>
        public IReadOnlyDataBar GetCurrentDataBar()
        {
            return _eventManager.InvokeEvent(() => OnGetCurrentDataBar?.Invoke());
        }

        /// <summary>
        /// Event triggered when current DataBar list is requested.
        /// This is used to get the current read only DataBar list.
        /// </summary>
        public List<IReadOnlyDataBar> GetDataBars()
        {
            return _eventManager.InvokeEvent(() => OnGetDataBars?.Invoke());
        }

        /// <summary>
        /// Event triggered when printing of DataBar is requested.
        /// This is used to print the DataBar for debugging purposes.
        /// </summary>
        public void PrintDataBar(IDataBarPrintConfig dataBarPrintConfig)
        {
            _eventManager.InvokeEvent(OnPrintDataBar, dataBarPrintConfig);
        }
    }
}
