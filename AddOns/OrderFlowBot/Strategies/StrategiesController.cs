using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public class StrategiesController
    {
        private readonly OrderFlowBotState _orderFlowBotState;
        private readonly OrderFlowBotDataBars _dataBars;
        private readonly StrategiesConfig _strategiesConfig;
        private readonly List<IStrategyInterface> _strategies;
        private readonly List<TechnicalLevels> _technicalLevels;

        public StrategiesController(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, StrategiesConfig strategiesConfig, List<TechnicalLevels> technicalLevels)
        {
            _orderFlowBotState = orderFlowBotState;
            _dataBars = dataBars;
            _strategiesConfig = strategiesConfig;
            _strategies = new List<IStrategyInterface>();
            _technicalLevels = technicalLevels;

            InitializeStrategies();

            if (_orderFlowBotState.BackTestingEnabled)
            {
                EnableBackTesting();
            }
            _technicalLevels = technicalLevels;
        }

        private void InitializeStrategies()
        {
            // Dynamically creates the strategies based on the name in the config.
            foreach (var strategyConfig in _strategiesConfig.StrategiesConfigList)
            {
                string fullClassName = String.Format("{0}.{1}", this.GetType().Namespace, strategyConfig.Name);

                Type strategyType = Type.GetType(fullClassName);

                if (strategyType != null && typeof(IStrategyInterface).IsAssignableFrom(strategyType))
                {
                    var strategyInstance = (IStrategyInterface)Activator.CreateInstance(strategyType, _orderFlowBotState, _dataBars, strategyConfig.Name, _technicalLevels);

                    if (strategyInstance != null)
                    {
                        _strategies.Add(strategyInstance);
                    }
                }
            }
        }

        public void EnableBackTesting()
        {
            _orderFlowBotState.SelectedTradeDirection = Direction.Any;
            _orderFlowBotState.SelectedStrategies.Add(_orderFlowBotState.BackTestingStrategyName);
        }

        public void CheckStrategies()
        {
            if (_orderFlowBotState.BackTestingEnabled)
            {
                var backtestingStrategy = _strategies.FirstOrDefault(strategy => strategy.Name == _orderFlowBotState.BackTestingStrategyName);

                if (backtestingStrategy != null)
                {
                    backtestingStrategy.CheckStrategy();

                    if (backtestingStrategy.ValidStrategyDirection != Direction.Flat)
                    {
                        _orderFlowBotState.ValidStrategy = backtestingStrategy.Name;
                        _orderFlowBotState.ValidStrategyDirection = backtestingStrategy.ValidStrategyDirection;
                    }
                }

                return;
            }

            // Check only selected strategies
            foreach (var strategy in _strategies)
            {
                if (!_orderFlowBotState.SelectedStrategies.Contains(strategy.Name))
                {
                    continue;
                }

                strategy.CheckStrategy();

                if (strategy.ValidStrategyDirection != Direction.Flat)
                {
                    _orderFlowBotState.ValidStrategy = strategy.Name;

                    // Continue with found valid strategy direction with Trend mode selected
                    _orderFlowBotState.ValidStrategyDirection = strategy.ValidStrategyDirection;
                }
            }
        }

        public void ResetBackTestingStrategy()
        {
            _orderFlowBotState.ValidStrategy = "None";

            var backtestingStrategy = _strategies.FirstOrDefault(strategy => strategy.Name == _orderFlowBotState.BackTestingStrategyName);

            if (backtestingStrategy != null)
            {
                backtestingStrategy.ValidStrategyDirection = Direction.Flat;
            }
        }

        // Set all strategies false
        public void ResetStrategies()
        {
            _orderFlowBotState.ValidStrategyDirection = Direction.Flat;

            foreach (var strategy in _strategies)
            {
                strategy.ValidStrategyDirection = Direction.Flat;
                this.RemoveSelectedStrategy(strategy.Name);
            }
        }

        public void ResetTradeDirection()
        {
            _orderFlowBotState.SelectedTradeDirection = Direction.Flat;
            ResetValidStrategy();
        }

        public void ResetValidStrategy()
        {
            // Reset valid strategies and direction without removing them
            _orderFlowBotState.ValidStrategy = "None";
            _orderFlowBotState.ValidStrategyDirection = Direction.Flat;

            foreach (var strategy in _strategies)
            {
                strategy.ValidStrategyDirection = Direction.Flat;
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
