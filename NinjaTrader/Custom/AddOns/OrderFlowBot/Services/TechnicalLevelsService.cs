using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Utils;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    public class TechnicalLevelsService
    {
        private readonly EventManager _eventManager;
        private readonly TechnicalLevelsEvents _technicalLevelsEvents;
        private readonly List<IReadOnlyTechnicalLevels> _technicalLevelsList;
        private TechnicalLevels _currentTechnicalLevels;

        public TechnicalLevelsService(EventsContainer eventsContainer)
        {
            _eventManager = eventsContainer.EventManager;

            _technicalLevelsEvents = eventsContainer.TechnicalLevelsEvents;
            _technicalLevelsEvents.OnUpdateCurrentTechnicalLevels += HandleUpdateCurrentTechnicalLevels;
            _technicalLevelsEvents.OnUpdateTechnicalLevelsList += HandleUpdateTechnicalLevelsList;
            _technicalLevelsEvents.OnGetCurrentTechnicalLevels += HandleGetCurrentTechnicalLevels;
            _technicalLevelsEvents.OnGetTechnicalLevelsList += HandleGetTechnicalLevelsList;
            _technicalLevelsEvents.OnPrintTechnicalLevels += HandlePrintTechnicalLevels;

            _technicalLevelsList = new List<IReadOnlyTechnicalLevels>();
            _currentTechnicalLevels = new TechnicalLevels();
        }

        private void HandleUpdateCurrentTechnicalLevels(ITechnicalLevelsDataProvider dataProvider)
        {
            _currentTechnicalLevels.SetCurrentTechnicalIndicators(dataProvider);
            _technicalLevelsEvents.UpdatedCurrentTechnicalLevels();
        }

        private void HandleUpdateTechnicalLevelsList()
        {
            _technicalLevelsList.Add(_currentTechnicalLevels);
            _currentTechnicalLevels = new TechnicalLevels();
        }

        private IReadOnlyTechnicalLevels HandleGetCurrentTechnicalLevels()
        {
            return _currentTechnicalLevels;
        }

        private List<IReadOnlyTechnicalLevels> HandleGetTechnicalLevelsList()
        {
            return _technicalLevelsList;
        }

        private void HandlePrintTechnicalLevels(ITechnicalLevelsPrintConfig config)
        {
            IReadOnlyTechnicalLevels technicalLevels;
            int barsAgo = config.BarsAgo;

            technicalLevels = (barsAgo == 0) ? _currentTechnicalLevels : _technicalLevelsList[_technicalLevelsList.Count - barsAgo];

            TechnicalLevelsPrinter.PrintTechnicalLevels(_eventManager, technicalLevels, config);
        }
    }
}
