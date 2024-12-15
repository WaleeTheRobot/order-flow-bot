#region Using declarations
using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript.Indicators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
    public static class GroupConstants
    {
        public const string GROUP_NAME_GENERAL = "General";
        public const string GROUP_NAME_DATA_BAR = "Data Bar";
        public const string GROUP_NAME_TEST = "Backtest";
    }

    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_GENERAL, 0)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_DATA_BAR, 1)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_TEST, 2)]
    public partial class OrderFlowBot : Strategy
    {
        private EventsContainer _eventsContainer;
        [SuppressMessage("SonarLint", "S4487", Justification = "Instantiated for event handling")]
        private ServicesContainer _servicesContainer;
        private EventManager _eventManager;
        private TradingEvents _tradingEvents;
        private StrategiesEvents _strategiesEvents;
        private DataBarEvents _dataBarEvents;

        private IReadOnlyDataBar _currentDataBar;
        private IReadOnlyTradingState _currentTradingState;

        private bool _validTimeRange;
        private bool _timeStartChecked;
        private bool _timeEndChecked;

        #region General Properties

        [NinjaScriptProperty]
        [Display(Name = "Version", Description = "OrderFlowBot version.", Order = 0, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        [ReadOnly(true)]
        public string Version => "3.0.0";

        [NinjaScriptProperty]
        [Display(Name = "Daily Profit Enabled", Description = "Enable this to disable OFB after the daily realized profit is hit.", Order = 1, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public bool DailyProfitEnabled { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Daily Profit", Description = "The daily realized profit to disable OFB.", Order = 2, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public double DailyProfit { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Daily Loss Enabled", Description = "Enable this to disable OFB after the daily realized loss is hit.", Order = 3, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public bool DailyLossEnabled { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Daily Loss", Description = "The daily realized loss to disable OFB.", Order = 4, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public double DailyLoss { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Time Enabled", Description = "Enable this to enable time start/end.", Order = 5, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public bool TimeEnabled { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Time Start", Description = "The allowed time to enable OFB.", Order = 6, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public int TimeStart { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Time End", Description = "The allowed time to disable and close positions for OFB.", Order = 7, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        public int TimeEnd { get; set; }

        #endregion

        #region Data Bar Properties

        [NinjaScriptProperty]
        [Display(Name = "Ticks Per Level *", Description = "Set this to the same ticks per level that is being used.", Order = 0, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public int TicksPerLevel { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Imbalance Ratio", Description = "The minimum imbalance ratio.", Order = 1, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ImbalanceRatio { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Stacked Imbalance", Description = "The minimum number for a stacked imbalance.", Order = 2, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public int StackedImbalance { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Imbalance Min Delta", Description = "The minimum number of delta between the bid and ask for a valid imbalance.", Order = 3, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public long ImbalanceMinDelta { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Value Area Percentage", Description = "The percent to determine the value area.", Order = 4, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ValueAreaPercentage { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Cumulative Delta Period", Description = "The cumulative delta period.", Order = 5, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        [TypeConverter(typeof(CumulativeDeltaSelectedPeriodConverter))]
        public string CumulativeDeltaSelectedPeriod { get; set; }

        #endregion

        #region Backtest Properties

        [NinjaScriptProperty]
        [Display(Name = "Backtest Enabled", Description = "Enable this to backtest all strategies and directions.", Order = 0, GroupName = GroupConstants.GROUP_NAME_TEST)]
        public bool BacktestEnabled { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Backtest Strategy Name", Description = "The strategy name to backtest. This should be the same as the file name.", Order = 1, GroupName = GroupConstants.GROUP_NAME_TEST)]
        public string BacktestStrategyName { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Quantity", Description = "The name order quantity.", Order = 2, GroupName = GroupConstants.GROUP_NAME_TEST)]
        public int Quantity { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Target", Description = "The target in ticks.", Order = 3, GroupName = GroupConstants.GROUP_NAME_TEST)]
        public int Target { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Stop", Description = "The stop in ticks.", Order = 4, GroupName = GroupConstants.GROUP_NAME_TEST)]
        public int Stop { get; set; }

        #endregion

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"An order flow trading bot";
                Name = "_OrderFlowBot";
                Calculate = Calculate.OnEachTick;
                EntriesPerDirection = 1;
                EntryHandling = EntryHandling.AllEntries;
                IsExitOnSessionCloseStrategy = true;
                ExitOnSessionCloseSeconds = 30;
                IsFillLimitOnTouch = false;
                MaximumBarsLookBack = MaximumBarsLookBack.TwoHundredFiftySix;
                OrderFillResolution = OrderFillResolution.Standard;
                Slippage = 0;
                StartBehavior = StartBehavior.WaitUntilFlat;
                TimeInForce = TimeInForce.Gtc;
                TraceOrders = false;
                RealtimeErrorHandling = RealtimeErrorHandling.StopCancelClose;
                StopTargetHandling = StopTargetHandling.PerEntryExecution;
                BarsRequiredToTrade = 20;
                // Disable this property for performance gains in Strategy Analyzer optimizations
                // See the Help Guide for additional information
                IsInstantiatedOnEachOptimizationIteration = true;

                DailyProfitEnabled = false;
                DailyProfit = 1000;
                DailyLossEnabled = false;
                DailyLoss = 1000;
                TimeEnabled = false;
                TimeStart = 093000;
                TimeEnd = 155500;

                BacktestEnabled = false;
                BacktestStrategyName = "Stacked Imbalances";
                Target = 60;
                Stop = 60;
                Quantity = 1;

                TicksPerLevel = 1;
                ImbalanceRatio = 1.5;
                StackedImbalance = 3;
                ImbalanceMinDelta = 10;
                ValueAreaPercentage = 70;
                CumulativeDeltaSelectedPeriod = "Session";
            }
            else if (State == State.Configure)
            {
                SetConfigs();
                AddDataSeries(BarsPeriodType.Tick, 1);
                // Helps with time range check
                AddDataSeries(BarsPeriodType.Minute, 1);
            }
            else if (State == State.DataLoaded)
            {
                // Couldn't suppress this for IDE. Created per documentation example and compiles fine.
                if (CumulativeDeltaSelectedPeriod == "Session")
                {
                    _cumulativeDelta = OrderFlowCumulativeDelta(CumulativeDeltaType.BidAsk, CumulativeDeltaPeriod.Session, 0);
                }
                else if (CumulativeDeltaSelectedPeriod == "Bar")
                {
                    _cumulativeDelta = OrderFlowCumulativeDelta(CumulativeDeltaType.BidAsk, CumulativeDeltaPeriod.Bar, 0);
                }

                InitializeDataBar();
                InitializeTechnicalLevels();

                _eventsContainer = new EventsContainer();
                _servicesContainer = new ServicesContainer(_eventsContainer, new BacktestData
                {
                    Name = BacktestStrategyName,
                    IsBacktestEnabled = BacktestEnabled
                });

                _eventManager = _eventsContainer.EventManager;
                _tradingEvents = _eventsContainer.TradingEvents;
                _strategiesEvents = _eventsContainer.StrategiesEvents;
                _dataBarEvents = _eventsContainer.DataBarEvents;

                _eventsContainer.EventManager.OnPrintMessage += HandlePrintMessage;

                InitializeStrategyManager();

                if (Category != Category.Backtest)
                {
                    InitializeUIManager();
                }
            }
            else if (State == State.Realtime && Category != Category.Backtest)
            {
                ReadyControlPanel();
            }
            else if (State == State.Terminated && Category != Category.Backtest)
            {
                UnloadControlPanel();
            }
        }

        [SuppressMessage("SonarLint", "S125", Justification = "Commented code may be used later")]
        protected override void OnBarUpdate()
        {
            // Ensure we have defaults at the start of the session across multiple sessions
            if (Bars.IsFirstBarOfSession)
            {
                _tradingEvents.ResetTriggeredTradingState();
                _eventsContainer.StrategiesEvents.ResetStrategyData();
                _validTimeRange = false;
                _timeStartChecked = false;
                _timeEndChecked = false;
            }

            if (CurrentBars[0] < BarsRequiredToTrade)
            {
                return;
            }

            // First index data series should be the tick to support the cumulative delta bar
            if (BarsInProgress == 1)
            {
                _cumulativeDelta.Update(_cumulativeDelta.BarsArray[1].Count - 1, 1);
            }

            if (BarsInProgress == 0 && IsFirstTickOfBar)
            {
                _eventsContainer.DataBarEvents.UpdateDataBarList();
                _eventsContainer.TechnicalLevelsEvents.UpdateTechnicalLevelsList();

                //_eventsContainer.DataBarEvents.PrintDataBar(new DataBarPrintConfig
                //{
                //    BarsAgo = 1,
                //    ShowBasic = true,
                //    ShowDeltas = false,
                //    ShowImbalances = false,
                //    ShowPrices = false,
                //    ShowRatios = false,
                //    ShowVolumes = false,
                //    ShowBidAskVolumePerBar = false,
                //    ShowCumulativeDeltaBar = true,
                //});

                //_eventsContainer.TechnicalLevelsEvents.PrintTechnicalLevels(new TechnicalLevelsPrintConfig
                //{
                //    BarsAgo = 1,
                //    ShowEma = true,
                //});
            }

            if (BarsInProgress == 0)
            {
                _eventsContainer.TradingEvents.CurrentBarNumberTriggered(CurrentBars[0]);
                _eventsContainer.DataBarEvents.UpdateCurrentDataBar(GetDataBarDataProvider(DataBarConfig.Instance));
                _eventsContainer.TechnicalLevelsEvents.UpdateCurrentTechnicalLevels(GetTechnicalLevelsDataProvider());
            }

            if (TimeEnabled)
            {
                ValidStartEndTime();

                if (!_validTimeRange)
                {
                    return;
                }
            }

            if (!BacktestEnabled && _currentTradingState.IsTradingEnabled && _userInterfaceEvents != null)
            {
                if (ValidDailyProfitLossHit())
                {
                    UpdateDailyProfitLossUserInterface();
                }

                CheckAtmPosition();
            }
        }

        // Used for debugging event messages
        private void HandlePrintMessage(string eventMessage, bool addNewLine)
        {
            Print(eventMessage);

            if (addNewLine)
            {
                Print("");
            }
        }

        #region Time Range and Profit/Loss

        private bool ValidDailyProfitLossHit()
        {
            if (Account == null)
            {
                return false;
            }

            double realizedProfitLoss = Account.Get(AccountItem.RealizedProfitLoss, Currency.UsDollar);

            if (
                (DailyProfitEnabled && realizedProfitLoss > DailyProfit) ||
                (DailyLossEnabled && realizedProfitLoss < (DailyLoss * -1)))
            {
                return true;
            }

            return false;
        }

        private void UpdateDailyProfitLossUserInterface()
        {
            _servicesContainer.TradingService.HandleEnabledDisabledTriggered(false);

            if (_userInterfaceEvents != null)
            {
                _userInterfaceEvents.UpdateControlPanelLabel("Profit/Loss Hit");
                _userInterfaceEvents.DisableAllControls();
            }
        }

        private void ValidStartEndTime()
        {
            _validTimeRange = ValidTimeRange();

            // Helps enable/disable control panel
            if (!_timeStartChecked && _validTimeRange)
            {
                // Time is within range
                _timeStartChecked = true;
                UpdateValidStartEndTimeUserInterface(true);
            }

            if (_timeStartChecked && !_timeEndChecked && ToTime(Times[2][1]) >= TimeEnd)
            {
                // Initial time start checked and time >= TimeEnd
                _timeEndChecked = true;
                UpdateValidStartEndTimeUserInterface(false);
            }
        }

        private bool ValidTimeRange()
        {
            // Looks at previous bar otherwise it'll trigger the bar building to the time.
            return (ToTime(Times[2][1]) >= TimeStart && ToTime(Times[2][1]) <= TimeEnd);
        }

        private void UpdateValidStartEndTimeUserInterface(bool validStartEndTime)
        {
            _servicesContainer.TradingService.HandleEnabledDisabledTriggered(validStartEndTime);

            if (_userInterfaceEvents != null)
            {
                if (validStartEndTime)
                {
                    _userInterfaceEvents.UpdateControlPanelLabel("OrderFlowBot");
                    _userInterfaceEvents.EnableAllControls();
                }
                else
                {
                    _userInterfaceEvents.UpdateControlPanelLabel("Invalid Time");
                    _userInterfaceEvents.DisableAllControls();
                }
            }
        }

        #endregion
    }

    public class CumulativeDeltaSelectedPeriodConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new[] { "Session", "Bar" });
        }
    }
}
