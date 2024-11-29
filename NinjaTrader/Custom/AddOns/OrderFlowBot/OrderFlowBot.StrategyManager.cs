using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.NinjaScript.DrawingTools;
using System;
using System.IO;
using System.Windows.Media;

namespace NinjaTrader.NinjaScript.Strategies
{
    // Manages strategies that requires NinjaScript namespace
    public partial class OrderFlowBot : Strategy
    {
        private string _triggeredName;
        private string _alertSoundFilePath;
        private string _atmStrategyId;
        private bool _isAtmStrategyCreated;

        public void InitializeStrategyManager()
        {
            _tradingEvents.OnStrategyTriggeredProcessed += HandleStrategyTriggeredProcessed;
            _tradingEvents.OnCloseTriggered += HandleCloseAtmPosition;

            _currentTradingState = _tradingEvents.GetTradingState();
            _triggeredName = "";
            _alertSoundFilePath = "";
            _atmStrategyId = "";
            _isAtmStrategyCreated = false;

            // Sound
            string baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NinjaTrader 8", "bin", "Custom", "AddOns", "OrderFlowBot", "Assets");
            string alertSoundFilePath = Path.Combine(baseDirectory, "alert.wav");

            _alertSoundFilePath = File.Exists(alertSoundFilePath)
                ? alertSoundFilePath
                : @"C:\Program Files\NinjaTrader 8\sounds\Alert2.wav";
        }

        protected override void OnExecutionUpdate(
            Execution execution,
            string executionId,
            double price,
            int quantity,
            MarketPosition marketPosition,
            string orderId,
            DateTime time)
        {
            if (Position.MarketPosition == MarketPosition.Flat)
            {
                ResetBacktestStrategy();
                _strategiesEvents.ResetStrategyData();
            }
        }

        private void HandleStrategyTriggeredProcessed()
        {
            _currentDataBar = _dataBarEvents.GetCurrentDataBar();
            _triggeredName = _currentTradingState.TriggeredName;

            ProcessTriggeredStrategy();
        }

        private void ProcessTriggeredStrategy()
        {
            if (BacktestEnabled)
            {
                ProcessBacktestTriggeredStrategy();
            }
            else
            {
                ProcessAtmTriggeredStrategy();
            }
        }

        #region Back Test

        private void ProcessBacktestTriggeredStrategy()
        {
            if (Position.MarketPosition != MarketPosition.Flat && _currentTradingState.HasMarketPosition)
            {
                return;
            }

            if (_currentTradingState.TriggeredDirection == Direction.Long)
            {
                SetProfitTarget(_triggeredName, CalculationMode.Ticks, Target);
                SetStopLoss(_triggeredName, CalculationMode.Ticks, Stop, false);
                // Enter using tick series
                EnterLong(1, Quantity, _triggeredName);

                _tradingEvents.LastTradedBarNumberTriggered(_currentDataBar.BarNumber);
                _tradingEvents.MarketPositionTriggered(true);
                _eventManager.PrintMessage($"Enter Long | {_currentDataBar.Time} {_triggeredName}");

                return;
            }

            if (_currentTradingState.TriggeredDirection == Direction.Short)
            {
                SetProfitTarget(_triggeredName, CalculationMode.Ticks, Target);
                SetStopLoss(_triggeredName, CalculationMode.Ticks, Stop, false);
                // Enter using tick series
                EnterShort(1, Quantity, _triggeredName);

                _tradingEvents.LastTradedBarNumberTriggered(_currentDataBar.BarNumber);
                _tradingEvents.MarketPositionTriggered(true);
                _eventManager.PrintMessage($"Enter Short | {_currentDataBar.Time} {_triggeredName}");
            }
        }

        private void ResetBacktestStrategy()
        {
            _eventManager.PrintMessage($"Exit | {_currentDataBar.Time} {_triggeredName}", true);

            // Prevent re-entry on exit bar
            _tradingEvents.LastTradedBarNumberTriggered(_dataBarEvents.GetCurrentDataBar().BarNumber);
            _tradingEvents.ResetTriggeredTradingState();
            _tradingEvents.MarketPositionTriggered(false);
        }

        #endregion

        #region ATM

        private void ProcessAtmTriggeredStrategy()
        {
            if (State < State.Realtime && _currentTradingState.HasMarketPosition)
            {
                return;
            }

            if (!_isAtmStrategyCreated)
            {
                if (_currentTradingState.TriggeredDirection == Direction.Long)
                {
                    EnterAtmPosition(true);

                    return;
                }

                if (_currentTradingState.TriggeredDirection == Direction.Short)
                {
                    EnterAtmPosition(false);
                }
            }
        }

        private void EnterAtmPosition(bool isLong)
        {
            bool finalDirection = _currentTradingState.StandardInverse == Direction.Standard ? isLong : !isLong;
            string entryDirection = finalDirection ? "Long" : "Short";

            _atmStrategyId = GetAtmStrategyUniqueId();

            string atmTemplateName = ChartControl.OwnerChart.ChartTrader.AtmStrategy.Template;

            _eventManager.PrintMessage($"***** {atmTemplateName} *****");

            if (_currentTradingState.IsAlertEnabled)
            {
                Print("Alert");
            }

            _tradingEvents.LastTradedBarNumberTriggered(_currentDataBar.BarNumber);
            _eventManager.PrintMessage($"Enter {entryDirection} | {_currentDataBar.Time} {_triggeredName}");

            if (finalDirection)
            {
                if (_currentTradingState.IsAlertEnabled)
                {
                    TradeAlert(true);

                    return;
                }

                AtmStrategyCreate(OrderAction.Buy, OrderType.Market, 0, 0, TimeInForce.Day, _atmStrategyId, atmTemplateName, _atmStrategyId, (atmCallbackErrorCode, atmCallbackId) =>
                {
                    if (atmCallbackId == _atmStrategyId && atmCallbackErrorCode == ErrorCode.NoError)
                    {
                        _isAtmStrategyCreated = true;
                        _tradingEvents.MarketPositionTriggered(true);
                    }
                });
            }
            else
            {
                if (_currentTradingState.IsAlertEnabled)
                {
                    TradeAlert(false);

                    return;
                }

                AtmStrategyCreate(OrderAction.Sell, OrderType.Market, 0, 0, TimeInForce.Day, _atmStrategyId, atmTemplateName, _atmStrategyId, (atmCallbackErrorCode, atmCallbackId) =>
                {
                    if (atmCallbackId == _atmStrategyId && atmCallbackErrorCode == ErrorCode.NoError)
                    {
                        _isAtmStrategyCreated = true;
                        _tradingEvents.MarketPositionTriggered(true);
                    }
                });
            }
        }

        private void ResetAtm()
        {
            _atmStrategyId = null;
            _isAtmStrategyCreated = false;

            _eventManager.PrintMessage($"Exit | {_currentDataBar.Time} {_triggeredName}", true);

            // Prevent re-entry on exit bar
            _tradingEvents.LastTradedBarNumberTriggered(_dataBarEvents.GetCurrentDataBar().BarNumber);
            _tradingEvents.ResetTriggeredTradingState();
            _tradingEvents.MarketPositionTriggered(false);
            _strategiesEvents.ResetStrategyData();

            if (!_currentTradingState.IsAutoTradeEnabled)
            {
                _tradingEvents.ResetTriggerStrikePrice();
                _tradingEvents.ResetSelectedTradeDirection();
                _tradingEvents.PositionClosedWithAutoDisabled();
            }
        }

        private void CheckAtmPosition()
        {
            // ATM created. Check if position exited.
            if (_isAtmStrategyCreated && GetAtmStrategyMarketPosition(_atmStrategyId) == MarketPosition.Flat)
            {
                // ATM strategy exited. Reset
                ResetAtm();
            }
        }

        private void HandleCloseAtmPosition()
        {
            if (_atmStrategyId != "" && State == State.Realtime)
            {
                AtmStrategyClose(_atmStrategyId);
                ResetAtm();
            }
        }

        #endregion

        private void TradeAlert(bool longEntry)
        {
            if (longEntry)
            {
                Draw.TriangleUp(this, "AlertTriangleUp" + CurrentBar, true, 0, Close[0] - TickSize, Brushes.Blue);
            }
            else
            {
                Draw.TriangleDown(this, "AlertTriangleDown" + CurrentBar, true, 0, Close[0] + TickSize, Brushes.Red);
            }

            PlaySound(_alertSoundFilePath);
            ResetAtm();
        }
    }
}
