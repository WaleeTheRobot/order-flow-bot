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
        protected DataBar currentDataBar;
        protected List<DataBar> currentDataBars;
        public abstract string Name { get; set; }
        public abstract Direction ValidStrategyDirection { get; set; }

        protected StrategyBase(EventsContainer eventsContainer)
        {
            this.eventsContainer = eventsContainer;
            currentDataBar = new DataBar();
            currentDataBars = new List<DataBar>();
            Name = "";
            ValidStrategyDirection = Direction.Flat;
        }

        public virtual void CheckStrategy()
        {
            currentDataBar = GetCurrentDataBar();
            currentDataBars = GetDataBars();

            if (IsValidLongDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckLong();
            }

            if (IsValidShortDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckShort();
            }
        }

        protected DataBar GetCurrentDataBar()
        {
            return eventsContainer.DataBarEvents.GetCurrentDataBar();
        }

        protected List<DataBar> GetDataBars()
        {
            return eventsContainer.DataBarEvents.GetDataBars();
        }

        protected TradingState GetCurrentTradingState()
        {
            return eventsContainer.TradingEvents.GetTradingState();
        }

        public abstract void CheckLong();

        public abstract void CheckShort();

        protected bool IsValidLongDirection()
        {
            var currentState = GetCurrentTradingState();
            return currentState.SelectedTradeDirection == Direction.Long || currentState.SelectedTradeDirection == Direction.Any;
        }

        protected bool IsValidShortDirection()
        {
            var currentState = GetCurrentTradingState();
            return currentState.SelectedTradeDirection == Direction.Short || currentState.SelectedTradeDirection == Direction.Any;
        }
    }
}
