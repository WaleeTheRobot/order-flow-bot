using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
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

        public DataBarService(EventsContainer eventsContainer)
        {
            _eventManager = eventsContainer.EventManager;

            _dataBarEvents = eventsContainer.DataBarEvents;
            _dataBarEvents.OnUpdateCurrentDataBar += HandleUpdateCurrentDataBar;
            _dataBarEvents.OnUpdateCurrentDataBarList += HandleUpdateCurrentDataBarList;
            _dataBarEvents.OnGetCurrentDataBar += HandleGetCurrentDataBar;
            _dataBarEvents.OnGetDataBars += HandleGetDataBars;
            _dataBarEvents.OnPrintDataBar += HandlePrintDataBar;

            _dataBars = new List<DataBar>();
            _currentDataBar = new DataBar();
        }

        private void HandleUpdateCurrentDataBar(DataBarDataProvider dataBarDataProvider)
        {
            _currentDataBar.SetCurrentDataBar(dataBarDataProvider);
            _dataBarEvents.UpdatedCurrentDataBar(_currentDataBar, _dataBars);
        }

        private void HandleUpdateCurrentDataBarList()
        {
            _dataBars.Add(_currentDataBar);
            _currentDataBar = new DataBar();
        }

        private DataBar HandleGetCurrentDataBar()
        {
            return _currentDataBar;
        }

        private List<DataBar> HandleGetDataBars()
        {
            return _dataBars;
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
