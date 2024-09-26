﻿#region Using declarations
using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBarConfigs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;
using NinjaTrader.NinjaScript.BarsTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
    public static class GroupConstants
    {
        public const string GROUP_NAME_GENERAL = "General";
        public const string GROUP_NAME_DATA_BAR = "Data Bar";
    }

    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_GENERAL, 0)]
    [Gui.CategoryOrder(GroupConstants.GROUP_NAME_DATA_BAR, 1)]
    public class OrderFlowBot : Strategy
    {
        private EventManager _eventManager;
        private DataBarEvents _dataBarEvents;

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
        [Display(Name = "TicksPerLevel", Description = "Set this to the same ticks per level that is being used.", Order = 0, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public int TicksPerLevel { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Imbalance Ratio", Description = "The minimum imbalance ratio.", Order = 1, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public double ImbalanceRatio { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Stacked Imbalance", Description = "The minimum number for a stacked imbalance.", Order = 2, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public int StackedImbalance { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Valid Imbalance Volume", Description = "The minimum number of volume for a valid imbalance.", Order = 3, GroupName = GroupConstants.GROUP_NAME_DATA_BAR)]
        public long ValidImbalanceVolume { get; set; }

        #endregion

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"A bot trading order flow";
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

                TicksPerLevel = 1;
            }
            else if (State == State.Configure)
            {
                _dataBarDataProvider = new DataBarDataProvider();

                SetConfigs();

                // Initialize Events
                _eventManager = new EventManager();
                _eventManager.OnPrintMessage += HandlePrintMessage;

                _dataBarEvents = new DataBarEvents(_eventManager);

                // Initialize Services
                new DataBarService(_eventManager, _dataBarEvents);
            }
        }

        protected override void OnBarUpdate()
        {
            if (CurrentBars[0] < BarsRequiredToTrade)
            {
                return;
            }

            if (BarsInProgress == 0)
            {
                if (IsFirstTickOfBar)
                {
                    _dataBarEvents.UpdateCurrentDataBarList();

                    _dataBarEvents.PrintDataBar(new DataBarPrintConfig
                    {
                        BarsAgo = 1,
                        ShowBasic = true,
                        ShowDeltas = true,
                        ShowImbalances = true,
                        ShowPrices = true,
                        ShowRatios = true,
                        ShowVolumes = true,
                        ShowBidAskVolumePerBar = true,
                    });
                }
                else
                {

                }
            }

            _dataBarEvents.UpdateCurrentDataBar(GetDataBarDataProvider());
        }

        private DataBarDataProvider GetDataBarDataProvider(int barsAgo = 0)
        {
            VolumetricBarsType volumetricBar = Bars.BarsSeries.BarsType as VolumetricBarsType;

            _dataBarDataProvider.VolumetricBar = volumetricBar;
            _dataBarDataProvider.Time = ToTime(Time[barsAgo]);
            _dataBarDataProvider.CurrentBar = CurrentBars[0];
            _dataBarDataProvider.BarsAgo = barsAgo;
            _dataBarDataProvider.High = High[barsAgo];
            _dataBarDataProvider.Low = Low[barsAgo];
            _dataBarDataProvider.Open = Open[barsAgo];
            _dataBarDataProvider.Close = Close[barsAgo];

            return _dataBarDataProvider;
        }

        private void SetConfigs()
        {
            DataBarConfig.Instance.TicksPerLevel = TicksPerLevel;
            DataBarConfig.Instance.TickSize = TickSize;
            DataBarConfig.Instance.StackedImbalance = StackedImbalance;
            DataBarConfig.Instance.ImbalanceRatio = ImbalanceRatio;
            DataBarConfig.Instance.ValidImbalanceVolume = ValidImbalanceVolume;
        }

        // Used for debugging event messages
        private void HandlePrintMessage(string eventMessage)
        {
            Print(eventMessage);
        }
    }
}
