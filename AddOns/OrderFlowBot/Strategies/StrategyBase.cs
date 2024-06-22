using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public abstract class StrategyBase : IStrategyInterface
    {
        protected readonly OrderFlowBotState orderFlowBotState;
        protected readonly OrderFlowBotDataBars dataBars;
        protected readonly List<TechnicalLevels> technicalLevels;
        public abstract string Name { get; set; }
        public abstract Direction ValidStrategyDirection { get; set; }

        protected StrategyBase(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, string name, List<TechnicalLevels> technicalLevels)
        {
            this.orderFlowBotState = orderFlowBotState;
            this.dataBars = dataBars;
            Name = name;
            ValidStrategyDirection = Direction.Flat;
            this.technicalLevels = technicalLevels;
        }

        public virtual void CheckStrategy()
        {
            if (IsValidLongDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckLong();
            }

            if (IsValidShortDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckShort();
            }
        }

        public abstract void CheckLong();

        public abstract void CheckShort();

        protected bool IsValidLongDirection()
        {
            return orderFlowBotState.SelectedTradeDirection == Direction.Long || orderFlowBotState.SelectedTradeDirection == Direction.Any;
        }

        protected bool IsValidShortDirection()
        {
            return orderFlowBotState.SelectedTradeDirection == Direction.Short || orderFlowBotState.SelectedTradeDirection == Direction.Any;
        }
    }
}
