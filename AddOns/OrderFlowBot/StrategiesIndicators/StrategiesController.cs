using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.StrategiesIndicators.Strategies
{
    public class StrategiesController
    {
        private readonly OrderFlowBotState _orderFlowBotState;
        private readonly OrderFlowBotDataBars _dataBars;
        private readonly StrategiesIndicatorsConfig _strategiesIndicatorsConfig;
        private readonly List<IStrategyInterface> _strategies;

        public StrategiesController(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, StrategiesIndicatorsConfig strategiesIndicatorsConfig)
        {
            _orderFlowBotState = orderFlowBotState;
            _dataBars = dataBars;
            _strategiesIndicatorsConfig = strategiesIndicatorsConfig;
            _strategies = new List<IStrategyInterface>();

            InitializeStrategies();

            if (_orderFlowBotState.BackTestingEnabled)
            {
                EnableBackTesting();
            }
        }

        private void InitializeStrategies()
        {
            // Dynamically creates the strategies based on the name in the config.
            foreach (var strategyConfig in _strategiesIndicatorsConfig.StrategiesIndicatorsConfigList)
            {
                if (strategyConfig.IsStrategy)
                {
                    string fullClassName = $"{this.GetType().Namespace}.{strategyConfig.Name}";

                    Type strategyType = Type.GetType(fullClassName);

                    if (strategyType != null && typeof(IStrategyInterface).IsAssignableFrom(strategyType))
                    {
                        var strategyInstance = (IStrategyInterface)Activator.CreateInstance(strategyType, _orderFlowBotState, _dataBars, strategyConfig.Name);

                        if (strategyInstance != null)
                        {
                            _strategies.Add(strategyInstance);
                        }
                    }
                }
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

                    // Reverse entry if Range mode selected
                    if (_orderFlowBotState.MarketDirection == MarketDirection.Range)
                    {
                        if (strategy.ValidStrategyDirection == Direction.Long)
                        {
                            _orderFlowBotState.ValidStrategyDirection = Direction.Short;

                        }

                        if (strategy.ValidStrategyDirection == Direction.Short)
                        {
                            _orderFlowBotState.ValidStrategyDirection = Direction.Long;
                        }
                    }
                    else
                    {
                        // Continue with found valid strategy direction with Trend mode selected
                        _orderFlowBotState.ValidStrategyDirection = strategy.ValidStrategyDirection;
                    }
                }
            }
        }

        // Set all strategies false
        public void ResetStrategies()
        {
            _orderFlowBotState.ValidStrategy = "None";

            foreach (var strategy in _strategies)
            {
                strategy.ValidStrategyDirection = Direction.Flat;
                this.RemoveSelectedStrategy(strategy.Name);
            }
        }

        public void AddSelectedStrategy(string strategy)
        {
            if (!_orderFlowBotState.SelectedStrategies.Contains(strategy))
            {
                _orderFlowBotState.SelectedStrategies.Add(strategy);
            }
        }

        public void RemoveSelectedStrategy(string strategy)
        {
            _orderFlowBotState.SelectedStrategies.Remove(strategy);
        }

        public bool StrategyExists(string strategy)
        {
            return _orderFlowBotState.SelectedStrategies.Contains(strategy);
        }
    }
}
