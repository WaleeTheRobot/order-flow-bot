using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public class StrategiesController
    {
        private readonly OrderFlowBotState _orderFlowBotState;
        private readonly OrderFlowBotDataBars _dataBars;
        private readonly List<IStrategyInterface> _strategies;

        public StrategiesController(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars)
        {
            _orderFlowBotState = orderFlowBotState;
            _dataBars = dataBars;
            _strategies = new List<IStrategyInterface>
            {
                new OrderFlowRatios(_orderFlowBotState, _dataBars, OrderFlowBotStrategy.Ratios)
            };

            if (_orderFlowBotState.BackTestingEnabled)
            {
                EnableBackTesting();
            }
        }

        public void EnableBackTesting()
        {
            _orderFlowBotState.SelectedTradeDirection = Direction.Any;

            foreach (var strategy in _strategies)
            {
                _orderFlowBotState.SelectedStrategies.Add(strategy.Name);
            }
        }

        public void CheckStrategies()
        {
            if (_orderFlowBotState.BackTestingEnabled)
            {
                // Check every strategy
                foreach (var strategy in _strategies)
                {
                    strategy.CheckStrategy();

                    if (strategy.ValidStrategyDirection != Direction.Flat)
                    {
                        _orderFlowBotState.ValidStrategy = strategy.Name;
                        _orderFlowBotState.ValidStrategyDirection = strategy.ValidStrategyDirection;
                    }
                }

                return;
            }

            // Check only selected strategies
            foreach (var strategy in _strategies)
            {
                if (!_orderFlowBotState.SelectedStrategies.Contains(strategy.Name))
                {
                    return;
                }

                strategy.CheckStrategy();

                if (strategy.ValidStrategyDirection != Direction.Flat)
                {
                    _orderFlowBotState.ValidStrategy = strategy.Name;
                    _orderFlowBotState.ValidStrategyDirection = strategy.ValidStrategyDirection;
                }
            }
        }

        // Set all strategies false
        public void ResetStrategies()
        {
            _orderFlowBotState.ValidStrategy = OrderFlowBotStrategy.None;

            foreach (var strategy in _strategies)
            {
                strategy.ValidStrategyDirection = Direction.Flat;
            }
        }

        public void AddSelectedStrategy(OrderFlowBotStrategy strategy)
        {
            if (!_orderFlowBotState.SelectedStrategies.Contains(strategy))
            {
                _orderFlowBotState.SelectedStrategies.Add(strategy);
            }
        }

        public void RemoveSelectedStrategy(OrderFlowBotStrategy strategy)
        {
            _orderFlowBotState.SelectedStrategies.Remove(strategy);
        }
    }
}
