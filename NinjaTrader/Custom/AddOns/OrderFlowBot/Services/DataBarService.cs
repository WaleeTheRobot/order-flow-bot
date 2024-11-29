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
        private readonly List<IReadOnlyDataBar> _dataBars;
        private DataBar _currentDataBar;

        public DataBarService(EventsContainer eventsContainer)
        {
            _eventManager = eventsContainer.EventManager;

            _dataBarEvents = eventsContainer.DataBarEvents;
            _dataBarEvents.OnUpdateCurrentDataBar += HandleUpdateCurrentDataBar;
            _dataBarEvents.OnUpdateDataBarList += HandleUpdateDataBarList;
            _dataBarEvents.OnGetCurrentDataBar += HandleGetCurrentDataBar;
            _dataBarEvents.OnGetDataBars += HandleGetDataBars;
            _dataBarEvents.OnPrintDataBar += HandlePrintDataBar;

            _dataBars = new List<IReadOnlyDataBar>();
            _currentDataBar = new DataBar(DataBarConfig.Instance);
        }

        private void HandleUpdateCurrentDataBar(IDataBarDataProvider dataProvider)
        {
            _currentDataBar.SetCurrentDataBar(dataProvider);
            _dataBarEvents.UpdatedCurrentDataBar();
        }

        private void HandleUpdateDataBarList()
        {
            _dataBars.Add(_currentDataBar);
            _currentDataBar = new DataBar(DataBarConfig.Instance);
        }

        private IReadOnlyDataBar HandleGetCurrentDataBar()
        {
            return _currentDataBar;
        }

        private List<IReadOnlyDataBar> HandleGetDataBars()
        {
            return _dataBars;
        }

        private void HandlePrintDataBar(IDataBarPrintConfig config)
        {
            IReadOnlyDataBar dataBar;
            int barsAgo = config.BarsAgo;

            if (barsAgo == 0)
            {
                dataBar = _currentDataBar;
            }
            else
            {
                dataBar = _dataBars[_dataBars.Count - barsAgo];
            }

            DataBarPrinter.PrintDataBar(_eventManager, dataBar, config);
        }
    }
}
