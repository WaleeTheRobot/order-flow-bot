using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public abstract class StrategyBase : IStrategyInterface
    {
        protected readonly OrderFlowBotState orderFlowBotState;
        protected readonly OrderFlowBotDataBars dataBars;
        public abstract OrderFlowBotStrategy Name { get; set; }
        public abstract Direction ValidStrategyDirection { get; set; }

        protected StrategyBase(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, OrderFlowBotStrategy name)
        {
            this.orderFlowBotState = orderFlowBotState;
            this.dataBars = dataBars;
            this.Name = name;
            this.ValidStrategyDirection = Direction.Flat;
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

        protected List<OrderFlowBotDataBar> GetLastNBars(int n)
        {
            if (dataBars.Bars == null)
                return new List<OrderFlowBotDataBar>();

            return dataBars.Bars.Skip(Math.Max(0, dataBars.Bars.Count - n)).ToList();
        }
    }
}
