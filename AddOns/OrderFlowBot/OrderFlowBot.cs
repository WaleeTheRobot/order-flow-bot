#region Using declarations
using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns;
using NinjaTrader.Custom.AddOns.OrderFlowBot;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies;
using NinjaTrader.NinjaScript.Indicators;
using System;
using System.ComponentModel.DataAnnotations;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
    public static class GroupConstants
    {
        public const string GROUP_NAME_STRATEGY = "Order Flow Bot";
        public const string GROUP_NAME_DATA_BAR = "Data Bar";
        public const string GROUP_NAME_INDICATORS = "Indicators";
        public const string GROUP_NAME_TESTING = "Order Flow Bot Testing";
    }

    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_STRATEGY, 1)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_DATA_BAR, 2)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_INDICATORS, 3)]
    public partial class OrderFlowBot : Strategy
    {
        #region Variables

        private OrderFlowBotState _orderFlowBotState;
        private OrderFlowBotDataBars _dataBars;
        private StrategiesController _strategiesController;

        private bool _entryLong;
        private bool _entryShort;
        private string _entryName;
        // Prevent entry on same bar
        private int _lastTradeBarNumber;

        // Indicators
        private AutoVolumeProfile _autoVolumeProfile;
        private Ratios _ratios;

        #endregion

        #region Properties

        [NinjaScriptProperty]
        [Display(Name = "Quantity", Description = "The name order quantity.", Order = 0, GroupName = GroupConstants.GROUP_NAME_STRATEGY)]
        public int Quantity { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Target", Description = "The target in ticks.", Order = 1, GroupName = GroupConstants.GROUP_NAME_STRATEGY)]
        public int Target { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Stop", Description = "The stop in ticks.", Order = 2, GroupName = GroupConstants.GROUP_NAME_STRATEGY)]
        public int Stop { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Back Testing Enabled", Description = "Enable this to back test all strategies and directions.", Order = 3, GroupName = GroupConstants.GROUP_NAME_STRATEGY)]
        public bool BackTestingEnabled { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Manage Opened Positions Enabled", Description = "Enable this to managed open positions.", Order = 4, GroupName = GroupConstants.GROUP_NAME_STRATEGY)]
        public bool ManageOpenedPositionsEnabled { get; set; }

        #endregion

        #region DataBar Properties

        [NinjaScriptProperty]
        [Display(Name = "Look Back Bars", Description = "The maximum bars to look back.", Order = 0, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public int LookBackBars { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Imbalance Ratio", Description = "The minimum imbalance ratio.", Order = 1, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ImbalanceRatio { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Stacked Imbalance", Description = "The minimum number for a stacked imbalance.", Order = 2, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public int StackedImbalance { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Bid Volume", Description = "The valid bid volume.", Order = 3, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public long ValidBidVolume { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Ask Volume", Description = "The valid ask volume.", Order = 4, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public long ValidAskVolume { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Exhaustion Ratio", Description = "The valid exhaustion ratio for comparing top and bottom.", Order = 5, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ValidExhaustionRatio { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Absorption Ratio", Description = "The valid absorption ratio for comparing top and bottom.", Order = 6, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ValidAbsorptionRatio { get; set; }

        #endregion

        #region Indicators Properties

        [NinjaScriptProperty]
        [Display(Name = "Auto Volume Profile Enabled", Description = "Enable the auto volume profile.", Order = 0, GroupName = GroupConstants.GROUP_NAME_INDICATORS)]
        public bool AutoVolumeProfileEnabled { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Auto Volume Profile Look Back Bars", Description = "The look back bars for auto volume profile.", Order = 1, GroupName = GroupConstants.GROUP_NAME_INDICATORS)]
        public int AutoVolumeProfileLookBackBars { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Ratios Enabled", Description = "Enable Ratios.", Order = 2, GroupName = GroupConstants.GROUP_NAME_INDICATORS)]
        public bool RatiosEnabled { get; set; }

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

                // Backtesting based on default settings
                Slippage = 2;
                IncludeCommission = true;

                // OrderFlowBot
                Quantity = 1;
                Target = 16;
                Stop = 16;
                BackTestingEnabled = false;
                ManageOpenedPositionsEnabled = true;

                // DataBar
                LookBackBars = 4;
                ImbalanceRatio = 1.5;
                ValidBidVolume = 0;
                ValidAskVolume = 0;
                StackedImbalance = 3;
                ValidExhaustionRatio = 15;
                ValidAbsorptionRatio = 1.4;

                // Indicators
                AutoVolumeProfileEnabled = false;
                AutoVolumeProfileLookBackBars = 4;
                RatiosEnabled = true;
            }
            else if (State == State.Configure)
            {
                OrderFlowBotPropertiesConfig config = new OrderFlowBotPropertiesConfig
                {
                    TickSize = TickSize,
                    LookBackBars = LookBackBars,
                    ImbalanceRatio = ImbalanceRatio,
                    ValidBidVolume = ValidBidVolume,
                    ValidAskVolume = ValidAskVolume,
                    StackedImbalance = StackedImbalance,
                    ValidExhaustionRatio = ValidExhaustionRatio,
                    ValidAbsorptionRatio = ValidAbsorptionRatio,
                    AutoVolumeProfileEnabled = AutoVolumeProfileEnabled,
                    AutoVolumeProfileLookBackBars = AutoVolumeProfileLookBackBars,
                };

                OrderFlowBotProperties.Initialize(config);
            }
            else if (State == State.DataLoaded)
            {
                _dataBars = new OrderFlowBotDataBars();
                _orderFlowBotState = new OrderFlowBotState();
                _orderFlowBotState.BackTestingEnabled = BackTestingEnabled;
                _strategiesController = new StrategiesController(_orderFlowBotState, _dataBars);

                ControlPanelSetStateDataLoaded();

                // Indicators
                if (AutoVolumeProfileEnabled)
                {
                    _autoVolumeProfile = AutoVolumeProfile();
                    _autoVolumeProfile.InitializeWith(_dataBars);
                    AddChartIndicator(_autoVolumeProfile);
                }

                if (RatiosEnabled)
                {
                    _ratios = Ratios();
                    _ratios.InitializeWith(_dataBars);
                    AddChartIndicator(_ratios);
                }
            }
            else if (State == State.Terminated)
            {
                ControlPanelSetStateTerminated();
            }
        }

        protected override void OnExecutionUpdate(Execution execution, string executionId, double price, int quantity, MarketPosition marketPosition, string orderId, DateTime time)
        {
            if (Position.MarketPosition == MarketPosition.Flat)
            {
                // Prevent re-entry on previous exit bar
                _lastTradeBarNumber = _dataBars.Bar.BarNumber + 1;

                _entryLong = false;
                _entryShort = false;

                _strategiesController.ResetStrategies();
                _orderFlowBotState.ValidStrategyDirection = Direction.Flat;

                if (_orderFlowBotState.BackTestingEnabled)
                {
                    return;
                }

                // Reset if AutoTrade is disabled
                if (!_orderFlowBotState.AutoTradeEnabled)
                {
                    _orderFlowBotState.SelectedTradeDirection = Direction.Flat;

                    SetAllButtonsInactive();
                    ControlPanelOnExecutionUpdate();
                }
            }
        }

        protected override void OnBarUpdate()
        {
            // Include all look back bars
            if (CurrentBar < LookBackBars)
                return;

            if (IsFirstTickOfBar)
            {
                // Get previous bar since we can miss the top or bottom of the bar in the data
                _dataBars.Bars.Add(GetDataBar(_dataBars.Bars, 1));
            }

            _dataBars.Bar = GetDataBar(_dataBars.Bars, 0);

            if (Position.MarketPosition == MarketPosition.Flat)
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
            }

            if (Position.MarketPosition == MarketPosition.Long && ManageOpenedPositionsEnabled)
            {
                // Update logic for managing positions
                // Exit position if current or future bar becomes bearish and has stacked imbalances
                if (_dataBars.Bar.BarType == BarType.Bearish && _dataBars.Bar.Imbalances.HasBidStackedImbalances)
                {
                    ExitLong();
                }
            }

            if (Position.MarketPosition == MarketPosition.Short && ManageOpenedPositionsEnabled)
            {
                // Update logic for managing positions
                // Exit position if current or future bar becomes bullish and has stacked imbalances
                if (_dataBars.Bar.BarType == BarType.Bullish && _dataBars.Bar.Imbalances.HasAskStackedImbalances)
                {
                    ExitShort();
                }
            }
        }

        private void CheckStrategies()
        {
            if (_orderFlowBotState.SelectedTradeDirection == Direction.Flat || _dataBars.Bar.BarNumber <= _lastTradeBarNumber)
            {
                return;
            }

            _strategiesController.CheckStrategies();

            if (_orderFlowBotState.ValidStrategyDirection == Direction.Long)
            {
                _entryLong = true;
                _entryName = _orderFlowBotState.ValidStrategy.ToString();

                //PrintDataBar(_dataBars.Bar, _dataBars.Bars.Last());

                return;
            }

            if (_orderFlowBotState.ValidStrategyDirection == Direction.Short)
            {
                _entryShort = true;
                _entryName = _orderFlowBotState.ValidStrategy.ToString();

                //PrintDataBar(_dataBars.Bar, _dataBars.Bars.Last());

                return;
            }
        }

    }
}
