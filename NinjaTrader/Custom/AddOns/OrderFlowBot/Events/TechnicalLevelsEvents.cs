using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class TechnicalLevelsEvents
    {
        private readonly EventManager _eventManager;
        public event Action<ITechnicalLevelsDataProvider> OnUpdateCurrentTechnicalLevels;
        public event Action OnUpdateTechnicalLevelsList;
        public event Action<ITechnicalLevelsPrintConfig> OnPrintTechnicalLevels;
        public event Action OnUpdatedCurrentTechnicalLevels;
        public event Func<IReadOnlyTechnicalLevels> OnGetCurrentTechnicalLevels;
        public event Func<List<IReadOnlyTechnicalLevels>> OnGetTechnicalLevelsList;

        public TechnicalLevelsEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered when the current TechnicalLevels needs to be updated.
        /// This is used to update the current TechnicalLevels.
        /// </summary>
        public void UpdateCurrentTechnicalLevels(ITechnicalLevelsDataProvider dataProvider)
        {
            _eventManager.InvokeEvent(OnUpdateCurrentTechnicalLevels, dataProvider);
        }

        /// <summary>
        /// Event triggered when the current TechnicalLevels needs to be added to the list.
        /// This is used to update the TechnicalLevels list with the current TechnicalLevels.
        /// </summary>
        public void UpdateTechnicalLevelsList()
        {
            _eventManager.InvokeEvent(OnUpdateTechnicalLevelsList);
        }

        /// <summary>
        /// Event triggered when the current TechnicalLevels is updated.
        /// This is used when the current TechnicalLevels is updated with new data.
        /// </summary>
        public void UpdatedCurrentTechnicalLevels()
        {
            _eventManager.InvokeEvent(OnUpdatedCurrentTechnicalLevels);
        }

        /// <summary>
        /// Event triggered when current TechnicalLevels is requested.
        /// This is used to get the current read only TechnicalLevels.
        /// </summary>
        public IReadOnlyTechnicalLevels GetCurrentTechnicalLevels()
        {
            return _eventManager.InvokeEvent(() => OnGetCurrentTechnicalLevels?.Invoke());
        }

        /// <summary>
        /// Event triggered when current TechnicalLevels list is requested.
        /// This is used to get the current read only TechnicalLevels list.
        /// </summary>
        public List<IReadOnlyTechnicalLevels> GetTechnicalLevelsList()
        {
            return _eventManager.InvokeEvent(() => OnGetTechnicalLevelsList?.Invoke());
        }

        /// <summary>
        /// Event triggered when printing of TechnicalLevels is requested.
        /// This is used to print the TechnicalLevels for debugging purposes.
        /// </summary>
        public void PrintTechnicalLevels(ITechnicalLevelsPrintConfig config)
        {
            _eventManager.InvokeEvent(OnPrintTechnicalLevels, config);
        }
    }
}
