using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class TradeAnalysisEvents
    {
        private readonly EventManager _eventManager;
        public event Action<TradeType> OnAddTrainingEntry;
        public event Action<int> OnAddTrainingExit;
        public event Func<TradeType, List<IReadOnlyDataBar>, IReadOnlyDataBar, string> OnGetTradeData;

        public TradeAnalysisEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered for adding entry for training data.
        /// This is used to add the entry bar with pre-trade bars for a trade in the training data.
        /// </summary>
        public void AddTrainingEntry(TradeType tradeType)
        {
            _eventManager.InvokeEvent(OnAddTrainingEntry, tradeType);
        }

        /// <summary>
        /// Event triggered for adding exit for training data.
        /// This is used to add the exit bar for a trade in the training data.
        /// </summary>
        public void AddTrainingExit(int winLoss)
        {
            _eventManager.InvokeEvent(OnAddTrainingExit, winLoss);
        }


        /// <summary>
        /// Event triggered when current TradeData is requested.
        /// This is used to get the serialized TradeData based on the received DataBars.
        /// </summary>
        /// <param name="tradeType">The direction of the trade.</param>
        /// <param name="dataBars">The list of IReadOnlyDataBar objects.</param>
        /// <param name="currentDataBar">The current IReadOnlyDataBar object.</param>
        /// <returns>Serialized TradeData based on the provided DataBars.</returns>
        public string GetTradeData(TradeType tradeType, List<IReadOnlyDataBar> dataBars, IReadOnlyDataBar currentDataBar)
        {
            return _eventManager.InvokeEvent(() => OnGetTradeData.Invoke(tradeType, dataBars, currentDataBar));
        }
    }
}
