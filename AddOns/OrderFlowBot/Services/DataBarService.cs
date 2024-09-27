using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Utils;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    public class DataBarService
    {
        private readonly EventManager _eventManager;
        private readonly DataBarEvents _dataBarEvents;
        private List<DataBar> _dataBars;
        private DataBar _currentDataBar;

        public DataBarService(EventManager eventManager, DataBarEvents dataBarEvents)
        {
            _eventManager = eventManager;

            _dataBarEvents = dataBarEvents;
            _dataBarEvents.OnUpdateCurrentDataBar += HandleUpdateCurrentDataBar;
            _dataBarEvents.OnUpdateCurrentDataBarList += HandleUpdateCurrentDataBarList;
            _dataBarEvents.OnPrintDataBar += HandlePrintDataBar;

            _dataBars = new List<DataBar>();
            _currentDataBar = new DataBar();
        }

        private void HandleUpdateCurrentDataBar(DataBarDataProvider dataBarDataProvider)
        {
            _currentDataBar.SetCurrentDataBar(dataBarDataProvider);
        }

        private void HandleUpdateCurrentDataBarList()
        {
            _dataBars.Add(_currentDataBar);
            _currentDataBar = new DataBar();
        }

        private void HandlePrintDataBar(DataBarPrintConfig dataBarPrintConfig)
        {
            DataBar dataBar;
            int barsAgo = dataBarPrintConfig.BarsAgo;

            if (barsAgo == 0)
            {
                dataBar = _currentDataBar;
            }
            else
            {
                dataBar = _dataBars[_dataBars.Count - barsAgo];
            }

            DataBarPrinter.PrintDataBar(_eventManager, dataBar, dataBarPrintConfig);
        }
    }
}
