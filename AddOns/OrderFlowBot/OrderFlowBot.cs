#region Using declarations
using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns;
using NinjaTrader.Custom.AddOns.OrderFlowBot;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript.Indicators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
    public static class GroupConstants
    {
        public const string GROUP_NAME_GENERAL = "Order Flow Bot";
        public const string GROUP_NAME_DATA_BAR = "Data Bar";
        public const string GROUP_NAME_STRATEGIES = "Strategies";
        public const string GROUP_NAME_INDICATORS = "Indicators";
        public const string GROUP_NAME_TECHNICAL_LEVELS = "Technical Levels";
        public const string GROUP_NAME_TESTING = "Testing";
    }

    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_GENERAL, 0)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_DATA_BAR, 1)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_STRATEGIES, 2)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_INDICATORS, 3)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_TECHNICAL_LEVELS, 4)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_TESTING, 5)]
    public partial class OrderFlowBot : Strategy
    {
        #region Variables

        private OrderFlowBotState _orderFlowBotState;
        private OrderFlowBotDataBars _dataBars;

        private List<TechnicalLevels> _technicalLevels;

        private StrategiesConfig _strategiesConfig;
        private StrategiesController _strategiesController;

        private bool _entryLong;
        private bool _entryShort;
        private string _entryName;
        private string _atmStrategyId;
        private bool _isAtmStrategyCreated;
        // Prevent entry on same bar
        private int _lastTradeBarNumber;

        #endregion

        #region General Properties

        [NinjaScriptProperty]
        [Display(Name = "Version", Description = "OrderFlowBot version.", Order = 0, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public string Version
        {
            get { return "2.1.2"; }
            private set { }
        }

        [NinjaScriptProperty]
        [Display(Name = "Min Bars Required To Trade", Description = "The minimum bars required to trade.", Order = 1, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public int MinBarsRequiredToTrade { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Trigger Strike Price Threshold Ticks", Description = "The threshold above and below the trigger strike price for triggering in ticks.", Order = 2, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public int TriggerStrikePriceThresholdTicks { get; set; }

        #endregion

        #region DataBar Properties

        [NinjaScriptProperty]
        [Display(Name = "Imbalance Ratio", Description = "The minimum imbalance ratio.", Order = 0, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ImbalanceRatio { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Stacked Imbalance", Description = "The minimum number for a stacked imbalance.", Order = 1, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public int StackedImbalance { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Imbalance Volume", Description = "The minimum number of volume for a valid imbalance.", Order = 2, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public long ValidImbalanceVolume { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Exhaustion Ratio", Description = "The valid exhaustion ratio for comparing top and bottom.", Order = 3, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ValidExhaustionRatio { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Absorption Ratio", Description = "The valid absorption ratio for comparing top and bottom.", Order = 4, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ValidAbsorptionRatio { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Volume Sequencing", Description = "The valid number of price to check for volume sequencing.", Order = 5, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public int ValidVolumeSequencing { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Volume Sequencing Minimum Volume", Description = "The valid number of volume to check for volume sequencing.", Order = 6, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public long ValidVolumeSequencingMinimumVolume { get; set; }

        #endregion

        #region Strategies Properties

        [NinjaScriptProperty]
        [Display(Name = "Delta Chaser Delta", Description = "Min delta will be less than this number as negative for bearish. Max delta will be more than this number as positive for bullish.", Order = 0, GroupName = GroupConstants.GROUP_NAME_STRATEGIES)]
        public int DeltaChaserDelta { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Stacked Imbalance Valid Open TSP", Description = "Enable to enter only if open above Trigger Strike Price for long or below it for short if TSP exist.", Order = 5, GroupName = GroupConstants.GROUP_NAME_STRATEGIES)]
        public bool StackedImbalanceValidOpenTSP { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Volume Sequencing Valid Open TSP", Description = "Enable to enter only if open above Trigger Strike Price for long or below it for short if TSP exist.", Order = 7, GroupName = GroupConstants.GROUP_NAME_STRATEGIES)]
        public bool VolumeSequencingValidOpenTSP { get; set; }

        #endregion

        #region Indicators Properties

        [NinjaScriptProperty]
        [Display(Name = "Ratios Enabled", Description = "Enable to display ratios indicator", Order = 0, GroupName = GroupConstants.GROUP_NAME_INDICATORS)]
        public bool RatiosEnabled { get; set; }

        #endregion

        #region Technical Levels

        [NinjaScriptProperty]
        [Display(Name = "Required Ticks for Broken", Description = "The required ticks to consider a level broken.", Order = 1, GroupName = GroupConstants.GROUP_NAME_TECHNICAL_LEVELS)]
        public float RequiredTicksForBroken { get; set; }

        #endregion

        #region Back Test Properties

        [NinjaScriptProperty]
        [Display(Name = "Back Testing Enabled", Description = "Enable this to back test all strategies and directions.", Order = 0, GroupName = GroupConstants.GROUP_NAME_TESTING)]
        public bool BackTestingEnabled { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Back Testing Strategy Name", Description = "The strategy name to back test. This should be the same as the file name.", Order = 1, GroupName = GroupConstants.GROUP_NAME_TESTING)]
        public string BackTestingStrategyName { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Quantity", Description = "The name order quantity.", Order = 2, GroupName = GroupConstants.GROUP_NAME_TESTING)]
        public int Quantity { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Target", Description = "The target in ticks.", Order = 3, GroupName = GroupConstants.GROUP_NAME_TESTING)]
        public int Target { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Stop", Description = "The stop in ticks.", Order = 4, GroupName = GroupConstants.GROUP_NAME_TESTING)]
        public int Stop { get; set; }

        #endregion

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"A bot trading order flow";
                Name = "OrderFlowBot";
                Calculate = Calculate.OnEachTick;
                EntriesPerDirection = 1;
                EntryHandling = EntryHandling.AllEntries;
                IsExitOnSessionCloseStrategy = true;
                ExitOnSessionCloseSeconds = 30;
                IsFillLimitOnTouch = false;
                MaximumBarsLookBack = MaximumBarsLookBack.TwoHundredFiftySix;
                OrderFillResolution = OrderFillResolution.Standard;
                StartBehavior = StartBehavior.WaitUntilFlat;
                TimeInForce = TimeInForce.Gtc;
                TraceOrders = false;
                RealtimeErrorHandling = RealtimeErrorHandling.StopCancelClose;
                StopTargetHandling = StopTargetHandling.PerEntryExecution;
                // Disable this property for performance gains in Strategy Analyzer optimizations
                // See the Help Guide for additional information
                IsInstantiatedOnEachOptimizationIteration = true;

                Slippage = 2;
                IncludeCommission = true;

                // General 
                MinBarsRequiredToTrade = 20;
                TriggerStrikePriceThresholdTicks = 16;

                // DataBar
                ImbalanceRatio = 1.5;
                StackedImbalance = 3;
                ValidImbalanceVolume = 10;
                ValidExhaustionRatio = 15;
                ValidAbsorptionRatio = 1.4;
                ValidVolumeSequencing = 4;
                ValidVolumeSequencingMinimumVolume = 500;

                // Strategies
                DeltaChaserDelta = 150;

                StackedImbalanceValidOpenTSP = true;

                VolumeSequencingValidOpenTSP = true;

                // Indicators
                RatiosEnabled = false;

                // Technical Levels
                RequiredTicksForBroken = 4;

                // Backtesting
                BackTestingEnabled = false;
                BackTestingStrategyName = "StackedImbalances";
                Quantity = 1;
                Target = 16;
                Stop = 16;
            }
            else if (State == State.DataLoaded)
            {
                _dataBars = new OrderFlowBotDataBars(
                    new OrderFlowBotDataBarConfigValues
                    {
                        TickSize = TickSize,
                        ImbalanceRatio = ImbalanceRatio,
                        StackedImbalance = StackedImbalance,
                        ValidImbalanceVolume = ValidImbalanceVolume,
                        ValidExhaustionRatio = ValidExhaustionRatio,
                        ValidAbsorptionRatio = ValidAbsorptionRatio,
                        ValidVolumeSequencing = ValidVolumeSequencing,
                        ValidVolumeSequencingMinimumVolume = ValidVolumeSequencingMinimumVolume
                    }
                );

                _orderFlowBotState = new OrderFlowBotState();
                _orderFlowBotState.BackTestingEnabled = BackTestingEnabled;
                _orderFlowBotState.BackTestingStrategyName = BackTestingStrategyName;

                _technicalLevels = new List<TechnicalLevels>
                {
                    new TechnicalLevels(CurrentBars[0], RequiredTicksForBroken * TickSize),
                    new TechnicalLevels(CurrentBars[1], RequiredTicksForBroken * TickSize)
                };

                _strategiesConfig = new StrategiesConfig();
                _strategiesController = new StrategiesController(_orderFlowBotState, _dataBars, _strategiesConfig, _technicalLevels);

                OrderFlowBotStrategiesProperties.Initialize(
                     new OrderFlowBotStrategiesPropertiesValues
                     {
                         DeltaChaserDelta = DeltaChaserDelta,
                         StackedImbalanceValidOpenTSP = StackedImbalanceValidOpenTSP,
                         VolumeSequencingValidOpenTSP = VolumeSequencingValidOpenTSP,
                     }
                );

                ControlPanelSetStateDataLoaded();
                AddIndicators();
            }
            else if (State == State.Terminated)
            {
                ControlPanelSetStateTerminated();
            }
            else if (State == State.Configure)
            {
                AddDataSeries(BarsPeriodType.Minute, 5);
            }
        }

        protected override void OnExecutionUpdate(Execution execution, string executionId, double price, int quantity, MarketPosition marketPosition, string orderId, DateTime time)
        {
            if (Position.MarketPosition == MarketPosition.Flat)
            {
                Reset();
            }
        }

        protected override void OnBarUpdate()
        {
            // Include all look back bars
            if (CurrentBar < MinBarsRequiredToTrade)
                return;

            if (BarsInProgress == 0 && IsFirstTickOfBar)
            {
                UpdateSupportResistanceLevels();

                // Ensure we are setting the last bar in bars with the completed previous data
                _dataBars.SetOrderFlowDataBarBase(GetOrderFlowDataBarBase(1));
                _dataBars.UpdateDataBars();

                //PrintDataBar(_dataBars.Bars.Last());
            }

            // 5 Minute
            if (BarsInProgress == 1 && IsFirstTickOfBar)
            {
                UpdateSupportResistanceLevels(1);
            }

            _dataBars.SetOrderFlowDataBarBase(GetOrderFlowDataBarBase(0));
            _dataBars.SetCurrentDataBar();

            // Ensures that no trades will go through since the strategies will not be checked
            if (_orderFlowBotState.DisableTrading)
            {
                return;
            }

            if (_orderFlowBotState.TriggerStrikePrice != 0 && !_orderFlowBotState.StrikePriceTriggered)
            {
                // Only allow strategy check when close is within threshold
                double thresholdPrice = TickSize * TriggerStrikePriceThresholdTicks;
                double upperLimit = _orderFlowBotState.TriggerStrikePrice + thresholdPrice;
                double lowerLimit = _orderFlowBotState.TriggerStrikePrice - thresholdPrice;

                if (Close[0] <= upperLimit && Close[0] >= lowerLimit)
                {
                    _orderFlowBotState.StrikePriceTriggered = true;
                    PrintOutput(String.Format("Triggered | {0}", _orderFlowBotState.TriggerStrikePrice));
                }
                else
                {
                    return;
                }
            }

            UpdateTriggerStrikeTextBoxBackground();

            if (Position.MarketPosition == MarketPosition.Flat && BackTestingEnabled)
            {
                CheckStrategies();

                if (_entryLong)
                {
                    SetProfitTarget(_entryName, CalculationMode.Ticks, Target);
                    SetStopLoss(_entryName, CalculationMode.Ticks, Stop, false);
                    EnterLong(Quantity, _entryName);

                    _lastTradeBarNumber = _dataBars.Bar.BarNumber;
                }
                if (_entryShort)
                {
                    SetProfitTarget(_entryName, CalculationMode.Ticks, Target);
                    SetStopLoss(_entryName, CalculationMode.Ticks, Stop, false);
                    EnterShort(Quantity, _entryName);
                    _lastTradeBarNumber = _dataBars.Bar.BarNumber;
                }

                return;
            }

            CheckAtmStrategies();
        }

        private void AddIndicators()
        {
            if (RatiosEnabled)
            {
                Ratios ratios = Ratios();
                ratios.InitializeWith(_dataBars);
                AddChartIndicator(ratios);
            }
        }

        private void Reset()
        {
            PrintOutput(String.Format("Exit | {0}", _entryName));

            _entryLong = false;
            _entryShort = false;
            _entryName = "";

            // Prevent re-entry on previous exit bar
            _lastTradeBarNumber = _dataBars.Bar.BarNumber + 1;

            _strategiesController.ResetBackTestingStrategy();
            _orderFlowBotState.ValidStrategyDirection = Direction.Flat;
        }

        private void ResetAtm()
        {
            PrintOutput(String.Format("Exit | {0}", _entryName));

            _entryLong = false;
            _entryShort = false;
            _entryName = "";

            _atmStrategyId = null;
            _isAtmStrategyCreated = false;

            ResetTradeDirection();

            // Prevent re-entry on previous exit bar
            _lastTradeBarNumber = _dataBars.Bar.BarNumber + 1;
        }

        private bool AllowCheckStrategies()
        {
            if (_orderFlowBotState.SelectedTradeDirection == Direction.Flat || _dataBars.Bar.BarNumber <= _lastTradeBarNumber)
            {
                return false;
            }

            return true;
        }

        private bool AllowAtmCheckStrategies()
        {
            if (_orderFlowBotState.SelectedTradeDirection == Direction.Flat || _dataBars.Bar.BarNumber <= _lastTradeBarNumber)
            {
                return false;
            }

            return true;
        }

        private void CheckStrategies()
        {
            if (!AllowCheckStrategies())
            {
                return;
            }

            _strategiesController.CheckStrategies();

            if (_orderFlowBotState.ValidStrategyDirection == Direction.Long)
            {
                _entryLong = true;
                _entryName = _orderFlowBotState.ValidStrategy.ToString();

                PrintOutput(String.Format("Enter Long | {0}", _entryName));

                return;
            }

            if (_orderFlowBotState.ValidStrategyDirection == Direction.Short)
            {
                _entryShort = true;
                _entryName = _orderFlowBotState.ValidStrategy.ToString();

                PrintOutput(String.Format("Enter Short | {0}", _entryName));

                return;
            }
        }

        private void CheckAtmStrategies()
        {
            if (State < State.Realtime || BackTestingEnabled)
            {
                return;
            }

            if (_isAtmStrategyCreated)
            {
                if (AtmIsFlat() && (_orderFlowBotState.ValidStrategyDirection == Direction.Long ||
                    _orderFlowBotState.ValidStrategyDirection == Direction.Short))
                {
                    ResetAtm();
                    ControlPanelOnExecutionUpdate();
                }
            }

            if (AtmIsFlat())
            {
                if (!AllowAtmCheckStrategies())
                {
                    return;
                }

                _strategiesController.CheckStrategies();

                if (_orderFlowBotState.ValidStrategyDirection == Direction.Long)
                {
                    _atmStrategyId = GetAtmStrategyUniqueId();
                    _lastTradeBarNumber = _dataBars.Bar.BarNumber;
                    _entryName = _orderFlowBotState.ValidStrategy.ToString();

                    string atmTemplateName = ChartControl.OwnerChart.ChartTrader.AtmStrategy.Template;

                    Print(String.Format("***** {0} *****", atmTemplateName));
                    PrintOutput(String.Format("Enter Long | {0}", _entryName));

                    AtmStrategyCreate(OrderAction.Buy, OrderType.Market, 0, 0, TimeInForce.Day, _atmStrategyId, atmTemplateName, _atmStrategyId, (atmCallbackErrorCode, atmCallbackId) =>
                    {
                        if (atmCallbackId == _atmStrategyId)
                        {
                            if (atmCallbackErrorCode == ErrorCode.NoError)
                            {
                                _isAtmStrategyCreated = true;
                            }
                        }
                    });
                }

                if (_orderFlowBotState.ValidStrategyDirection == Direction.Short)
                {
                    _atmStrategyId = GetAtmStrategyUniqueId();
                    _lastTradeBarNumber = _dataBars.Bar.BarNumber;
                    _entryName = _orderFlowBotState.ValidStrategy.ToString();

                    string atmTemplateName = ChartControl.OwnerChart.ChartTrader.AtmStrategy.Template;

                    Print(String.Format("***** {0} *****", atmTemplateName));
                    PrintOutput(String.Format("Enter Short | {0}", _entryName));

                    AtmStrategyCreate(OrderAction.Sell, OrderType.Market, 0, 0, TimeInForce.Day, _atmStrategyId, atmTemplateName, _atmStrategyId, (atmCallbackErrorCode, atmCallbackId) =>
                    {
                        if (atmCallbackId == _atmStrategyId)
                        {
                            if (atmCallbackErrorCode == ErrorCode.NoError)
                            {
                                _isAtmStrategyCreated = true;
                            }
                        }
                    });
                }
            }
        }

        private bool AtmIsFlat()
        {
            if (_atmStrategyId == null || !_isAtmStrategyCreated)
            {
                return true;
            }

            return GetAtmStrategyMarketPosition(_atmStrategyId) == MarketPosition.Flat;
        }

        private void CloseAtmPosition()
        {
            if (_atmStrategyId != null)
            {
                AtmStrategyClose(_atmStrategyId);
            }
        }
    }
}
