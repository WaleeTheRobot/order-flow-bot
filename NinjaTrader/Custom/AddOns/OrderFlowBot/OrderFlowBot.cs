#region Using declarations
using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using NinjaTrader.NinjaScript.BarsTypes;
using System;
using System.Collections.Generic;
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
        public const string GROUP_NAME_TESTING = "Testing";
    }

    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_GENERAL, 0)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_DATA_BAR, 1)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_TESTING, 2)]
    public partial class OrderFlowBot : Strategy
    {
        private EventsContainer _eventsContainer;
        [SuppressMessage("SonarLint", "S4487", Justification = "Instantiated for event handling")]
        private ServicesContainer _servicesContainer;
        private EventManager _eventManager;
        private TradingEvents _tradingEvents;
        private DataBarEvents _dataBarEvents;

        private IReadOnlyDataBar _currentDataBar;
        private IReadOnlyTradingState _currentTradingState;

        private DataBarDataProvider _dataBarDataProvider;

        #region General Properties

        [NinjaScriptProperty]
        [Display(Name = "Version", Description = "OrderFlowBot version.", Order = 0, GroupName = GroupConstants.GROUP_NAME_GENERAL)]
        [ReadOnly(true)]
        public string Version
        {
            get { return "3.0.0"; }
            set { }
        }

        #endregion

        #region Data Bar Properties

        [NinjaScriptProperty]
        [Display(Name = "TicksPerLevel *", Description = "Set this to the same ticks per level that is being used.", Order = 0, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
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

                BackTestingEnabled = false;
                BackTestingStrategyName = "Stacked Imbalances";
                Target = 40;
                Stop = 40;
                Quantity = 1;

                TicksPerLevel = 1;
                ImbalanceRatio = 1.5;
                StackedImbalance = 3;
                ImbalanceMinDelta = 10;
                ValueAreaPercentage = 70;
            }
            else if (State == State.Configure)
            {
                SetConfigs();
            }
            else if (State == State.DataLoaded)
            {
                _dataBarDataProvider = new DataBarDataProvider();

                _eventsContainer = new EventsContainer();
                _servicesContainer = new ServicesContainer(_eventsContainer);

                _eventManager = _eventsContainer.EventManager;
                _tradingEvents = _eventsContainer.TradingEvents;
                _dataBarEvents = _eventsContainer.DataBarEvents;

                _eventsContainer.EventManager.OnPrintMessage += HandlePrintMessage;

                InitializeStrategyManager();
            }
        }

        [SuppressMessage("SonarLint", "S125", Justification = "Commented code may be used later")]
        protected override void OnBarUpdate()
        {
            if (CurrentBars[0] < BarsRequiredToTrade)
            {
                return;
            }

            if (BarsInProgress == 0 && IsFirstTickOfBar)
            {
                _eventsContainer.DataBarEvents.UpdateCurrentDataBarList();

                /*
                _eventsContainer.DataBarEvents.PrintDataBar(new DataBarPrintConfig
                {
                    BarsAgo = 1,
                    ShowBasic = true,
                    ShowDeltas = false,
                    ShowImbalances = false,
                    ShowPrices = false,
                    ShowRatios = false,
                    ShowVolumes = false,
                    ShowBidAskVolumePerBar = false,
                });
                */
            }

            _eventsContainer.DataBarEvents.UpdateCurrentDataBar(GetDataBarDataProvider(DataBarConfig.Instance));
        }

        #region DataBar Setup and Debugging

        private IDataBarDataProvider GetDataBarDataProvider(IDataBarConfig config, int barsAgo = 0)
        {
            _dataBarDataProvider.Time = ToTime(Time[barsAgo]);
            _dataBarDataProvider.CurrentBar = CurrentBars[0];
            _dataBarDataProvider.BarsAgo = barsAgo;
            _dataBarDataProvider.High = High[barsAgo];
            _dataBarDataProvider.Low = Low[barsAgo];
            _dataBarDataProvider.Open = Open[barsAgo];
            _dataBarDataProvider.Close = Close[barsAgo];

            VolumetricBarsType volumetricBar = Bars.BarsSeries.BarsType as VolumetricBarsType;
            _dataBarDataProvider.VolumetricBar = PopulateCustomVolumetricBar(volumetricBar, config);

            return _dataBarDataProvider;
        }

        private ICustomVolumetricBar PopulateCustomVolumetricBar(VolumetricBarsType volumetricBar, IDataBarConfig config)
        {
            ICustomVolumetricBar customBar = new CustomVolumetricBar();

            double high = _dataBarDataProvider.High;
            double low = _dataBarDataProvider.Low;

            var volumes = volumetricBar.Volumes[_dataBarDataProvider.CurrentBar - _dataBarDataProvider.BarsAgo];

            customBar.TotalVolume = volumes.TotalVolume;
            customBar.TotalBuyingVolume = volumes.TotalBuyingVolume;
            customBar.TotalSellingVolume = volumes.TotalSellingVolume;

            volumes.GetMaximumVolume(null, out double pointOfControl);
            customBar.PointOfControl = pointOfControl;

            // Get bid/ask volume for each price in bar
            List<BidAskVolume> bidAskVolumeList = new List<BidAskVolume>();
            int ticksPerLevel = config.TicksPerLevel;
            int totalLevels = 0;
            int counter = 0;

            while (high >= low)
            {
                if (counter == 0)
                {
                    BidAskVolume bidAskVolume = new BidAskVolume
                    {
                        Price = high,
                        BidVolume = volumes.GetBidVolumeForPrice(high),
                        AskVolume = volumes.GetAskVolumeForPrice(high)
                    };

                    bidAskVolumeList.Add(bidAskVolume);
                }

                if (counter == ticksPerLevel - 1)
                {
                    counter = 0;
                }
                else
                {
                    counter++;
                }

                totalLevels++;
                high -= config.TickSize;
            }

            // Remove the first item if total levels are not divisible by ticksPerLevel and more than 4 levels
            // Sometimes bidAskVolumeList doesn't correlate visually due to an extra level or lack of a level
            // This seems to resolve probably many of the realistic scenarios
            if (totalLevels % ticksPerLevel > 0 && bidAskVolumeList.Count > 4)
            {
                bidAskVolumeList.RemoveAt(0);
            }

            customBar.BidAskVolumes = bidAskVolumeList;

            // Deltas
            customBar.BarDelta = volumes.BarDelta;
            customBar.MinSeenDelta = volumes.MinSeenDelta;
            customBar.MaxSeenDelta = volumes.MaxSeenDelta;
            customBar.DeltaSh = volumes.DeltaSh;
            customBar.DeltaSl = volumes.DeltaSl;
            customBar.CumulativeDelta = volumes.CumulativeDelta;
            customBar.DeltaPercentage = Math.Round(volumes.GetDeltaPercent(), 2);
            customBar.DeltaChange = volumes.BarDelta - volumetricBar.Volumes[_dataBarDataProvider.CurrentBar - _dataBarDataProvider.BarsAgo - 1].BarDelta;

            return customBar;
        }

        private void SetConfigs()
        {
            DataBarConfig.Instance.TicksPerLevel = TicksPerLevel;
            DataBarConfig.Instance.TickSize = TickSize;
            DataBarConfig.Instance.StackedImbalance = StackedImbalance;
            DataBarConfig.Instance.ImbalanceRatio = ImbalanceRatio;
            DataBarConfig.Instance.ImbalanceMinDelta = ImbalanceMinDelta;
            DataBarConfig.Instance.ValueAreaPercentage = ValueAreaPercentage;
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

        #endregion
    }
}
