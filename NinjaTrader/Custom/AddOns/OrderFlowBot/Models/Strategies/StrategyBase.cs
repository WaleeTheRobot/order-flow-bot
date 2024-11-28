using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public abstract class StrategyBase : IStrategy
    {
        protected readonly EventsContainer eventsContainer;
        protected IReadOnlyDataBar currentDataBar;
        protected List<IReadOnlyDataBar> dataBars;
        protected IReadOnlyTechnicalLevels currentTechnicalLevels;
        protected List<IReadOnlyTechnicalLevels> technicalLevelsList;
        public IStrategyData StrategyData { get; set; }

        protected StrategyBase(EventsContainer eventsContainer)
        {
            this.eventsContainer = eventsContainer;
            currentDataBar = new DataBar(DataBarConfig.Instance);
            dataBars = new List<IReadOnlyDataBar>();
            currentTechnicalLevels = new TechnicalLevels();
            technicalLevelsList = new List<IReadOnlyTechnicalLevels>();

            eventsContainer.StrategiesEvents.OnResetStrategyData += HandleResetStrategyData;

            StrategyData = new StrategyData
            {
                Name = "",
                TriggeredDirection = Direction.Flat,
                StrategyTriggered = false
            };
        }

        public virtual IStrategyData CheckStrategy()
        {
            currentDataBar = GetCurrentDataBar();
            dataBars = GetDataBars();
            currentTechnicalLevels = GetCurrentTechnicalLevels();
            technicalLevelsList = GetGetTechnicalLevels();

            if (IsValidSelectedLongDirection() && CheckLong())
            {
                StrategyData.UpdateTriggeredDataProvider(
                    Direction.Long,
                    true
                );

                return StrategyData;
            }

            if (IsValidSelectedShortDirection() && CheckShort())
            {
                StrategyData.UpdateTriggeredDataProvider(
                    Direction.Short,
                    true
                );

                return StrategyData;
            }

            return StrategyData;
        }

        protected IReadOnlyDataBar GetCurrentDataBar()
        {
            return eventsContainer.DataBarEvents.GetCurrentDataBar();
        }

        protected List<IReadOnlyDataBar> GetDataBars()
        {
            return eventsContainer.DataBarEvents.GetDataBars();
        }

        protected IReadOnlyTechnicalLevels GetCurrentTechnicalLevels()
        {
            return eventsContainer.TechnicalLevelsEvents.GetCurrentTechnicalLevels();
        }

        protected List<IReadOnlyTechnicalLevels> GetGetTechnicalLevels()
        {
            return eventsContainer.TechnicalLevelsEvents.GetTechnicalLevelsList();
        }

        protected IReadOnlyTradingState GetCurrentTradingState()
        {
            return eventsContainer.TradingEvents.GetTradingState();
        }

        public abstract bool CheckLong();

        public abstract bool CheckShort();

        protected bool IsValidSelectedLongDirection()
        {
            var currentState = GetCurrentTradingState();
            return currentState.SelectedTradeDirection == Direction.Long || currentState.SelectedTradeDirection == Direction.Any;
        }

        protected bool IsValidSelectedShortDirection()
        {
            var currentState = GetCurrentTradingState();
            return currentState.SelectedTradeDirection == Direction.Short || currentState.SelectedTradeDirection == Direction.Any;
        }

        private void HandleResetStrategyData()
        {
            StrategyData.TriggeredDirection = Direction.Flat;
            StrategyData.StrategyTriggered = false;
        }
    }
}
