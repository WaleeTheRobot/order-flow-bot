using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public abstract class StrategyBase : IStrategy
    {
        protected readonly EventsContainer eventsContainer;
        protected IReadOnlyDataBar currentDataBar;
        protected List<IReadOnlyDataBar> currentDataBars;
        public StrategyData StrategyData { get; set; }

        protected StrategyBase(EventsContainer eventsContainer)
        {
            this.eventsContainer = eventsContainer;
            currentDataBar = new DataBar(DataBarConfig.Instance);
            currentDataBars = new List<IReadOnlyDataBar>();

            StrategyData = new StrategyData
            {
                Name = "",
                TriggeredDirection = Direction.Flat,
                StrategyTriggered = false
            };
        }

        public virtual StrategyData CheckStrategy()
        {
            currentDataBar = GetCurrentDataBar();
            currentDataBars = GetDataBars();

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

        protected IReadOnlyTradingState GetCurrentTradingState()
        {
            return eventsContainer.TradingEvents.GetTradingState();
        }

        public abstract bool CheckLong();

        public abstract bool CheckShort();

        public virtual void ResetStrategyTriggeredData()
        {
            StrategyData.UpdateTriggeredDataProvider(
                Direction.Flat,
                false
            );
        }

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
    }
}
