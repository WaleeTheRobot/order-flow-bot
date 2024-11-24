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
        private bool _blockingAtmIsFlat;

        public void InitializeStrategyManager()
        {
            _tradingEvents.OnStrategyTriggeredProcessed += HandleStrategyTriggeredProcessed;
            _tradingEvents.OnCloseTriggered += HandleCloseAtmPosition;

            _currentTradingState = _tradingEvents.GetTradingState();
            _triggeredName = "";
            _alertSoundFilePath = "";
            _atmStrategyId = "";
            _isAtmStrategyCreated = false;
            _blockingAtmIsFlat = true;

            // Sound
            string baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NinjaTrader 8", "bin", "Custom", "AddOns", "OrderFlowBot", "Media");
            string alertSoundFilePath = Path.Combine(baseDirectory, "alert.wav");

            if (File.Exists(alertSoundFilePath))
            {
                _alertSoundFilePath = alertSoundFilePath;
            }
            else
            {
                // Fallback
                _alertSoundFilePath = @"C:\Program Files\NinjaTrader 8\sounds\Alert2.wav";
            }
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
                _eventsContainer.StrategiesEvents.ResetStrategyData();
            }
        }

        private void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            Print(isEnabled);
            Print("SM: Handling Trading enabled " + _currentTradingState.IsTradingEnabled);

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
            if (Position.MarketPosition == MarketPosition.Flat)
            {
                if (_currentTradingState.TriggeredDirection == Direction.Long)
                {
                    SetProfitTarget(_triggeredName, CalculationMode.Ticks, Target);
                    SetStopLoss(_triggeredName, CalculationMode.Ticks, Stop, false);
                    // Enter using tick series
                    EnterLong(1, Quantity, _triggeredName);

                    _tradingEvents.LastTradedBarNumberTriggered(_currentDataBar.BarNumber);
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
                    _eventManager.PrintMessage($"Enter Short | {_currentDataBar.Time} {_triggeredName}");
                }
            }
        }

        private void ResetBacktestStrategy()
        {
            _eventManager.PrintMessage($"Exit | {_currentDataBar.Time} {_triggeredName}", true);

            // Prevent re-entry on exit bar
            _tradingEvents.LastTradedBarNumberTriggered(_dataBarEvents.GetCurrentDataBar().BarNumber);
            _tradingEvents.ResetTriggeredTradingState();
        }

        #endregion

        #region ATM

        private void ProcessAtmTriggeredStrategy()
        {
            if (State < State.Realtime)
            {
                return;
            }

            if (_blockingAtmIsFlat)
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
            else
            {
                // ATM strategy active
                if (_isAtmStrategyCreated && AtmIsFlat() && (_currentTradingState.TriggeredDirection == Direction.Long ||
                      _currentTradingState.TriggeredDirection == Direction.Short))
                {
                    // ATM strategy exited. Reset
                    ResetAtm();
                }
            }
        }

        private void EnterAtmPosition(bool isLong)
        {
            string entryDirection = isLong ? "Long" : "Short";

            _atmStrategyId = GetAtmStrategyUniqueId();

            string atmTemplateName = ChartControl.OwnerChart.ChartTrader.AtmStrategy.Template;

            _eventManager.PrintMessage($"***** {atmTemplateName} *****");

            if (_currentTradingState.IsAlertEnabled)
            {
                Print("Alert");
            }

            _tradingEvents.LastTradedBarNumberTriggered(_currentDataBar.BarNumber);
            _eventManager.PrintMessage($"Enter {entryDirection} | {_currentDataBar.Time} {_triggeredName}");

            _blockingAtmIsFlat = false;

            if (isLong)
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
                    }
                });
            }
        }

        private void ResetAtm()
        {
            _atmStrategyId = null;
            _isAtmStrategyCreated = false;
            _blockingAtmIsFlat = true;

            _eventManager.PrintMessage($"Exit | {_currentDataBar.Time} {_triggeredName}", true);

            // Prevent re-entry on exit bar
            _tradingEvents.LastTradedBarNumberTriggered(_dataBarEvents.GetCurrentDataBar().BarNumber);
            _tradingEvents.ResetTriggeredTradingState();
        }

        private bool AtmIsFlat()
        {
            if (_atmStrategyId == null || _blockingAtmIsFlat)
            {
                return true;
            }

            return GetAtmStrategyMarketPosition(_atmStrategyId) == MarketPosition.Flat;
        }

        private void HandleCloseAtmPosition()
        {
            if (_atmStrategyId != null)
            {
                AtmStrategyClose(_atmStrategyId);
                ResetAtm();
                _eventManager.PrintMessage($"ATM Position Closed", true);
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
