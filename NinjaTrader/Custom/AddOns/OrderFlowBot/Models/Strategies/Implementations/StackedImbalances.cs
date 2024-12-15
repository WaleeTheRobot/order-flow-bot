using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies.Implementations
{
    // Example strategy showing how to use trigger strike price, current, previous bar and technical levels
    public class StackedImbalancesStrategyBar
    {
        public IReadOnlyDataBar CurrentDataBar { get; set; }
        public IReadOnlyDataBar OneBarAgoDataBar { get; set; }
        public IReadOnlyTechnicalLevels CurrentTechnicalLevels { get; set; }
    }

    public class StackedImbalanceStrategy
    {
        public StackedImbalancesStrategyBar StackedImbalancesStrategyBar { get; set; }

        public bool Check(bool isLong)
        {
            return
                IsValidCurrentBar(isLong) &&
                IsValidOneBarAgoBar(isLong) &&
                HasValidStackedImbalances(isLong) &&
                IsValidEma(isLong);
        }

        private bool IsValidCurrentBar(bool isLong)
        {
            BarType currentBarType = StackedImbalancesStrategyBar.CurrentDataBar.BarType;

            return isLong ? currentBarType == BarType.Bullish : currentBarType == BarType.Bearish;
        }

        private bool IsValidOneBarAgoBar(bool isLong)
        {
            BarType oneBarAgoBarType = StackedImbalancesStrategyBar.OneBarAgoDataBar.BarType;

            return isLong ? oneBarAgoBarType == BarType.Bullish : oneBarAgoBarType == BarType.Bearish;
        }

        private bool HasValidStackedImbalances(bool isLong)
        {
            return isLong ?
                StackedImbalancesStrategyBar.CurrentDataBar.Imbalances.HasAskStackedImbalances :
                StackedImbalancesStrategyBar.CurrentDataBar.Imbalances.HasBidStackedImbalances;
        }

        private bool IsValidEma(bool isLong)
        {
            double currentPrice = StackedImbalancesStrategyBar.CurrentDataBar.Prices.Close;
            double currentFastEma = StackedImbalancesStrategyBar.CurrentTechnicalLevels.Ema.FastEma;

            return isLong ? currentPrice > currentFastEma : currentPrice < currentFastEma;
        }
    }

    public class StackedImbalances : StrategyBase
    {
        private readonly int _validBarCount;
        private readonly int _validVolume;
        private readonly int _validDelta;
        private readonly StackedImbalancesStrategyBar _stackedImbalancesStrategyBar;
        private readonly StackedImbalanceStrategy _stackedImbalanceStrategy;
        private DateTime _lastAnalysisCheckTime = DateTime.MinValue;
        private AnalysisResult _lastAnalysisResult = AnalysisResult.CreateInvalid();
        private readonly TimeSpan _analysisCooldown;

        public StackedImbalances(EventsContainer eventsContainer) : base(eventsContainer)
        {
            StrategyData.Name = "Stacked Imbalances";
            _stackedImbalancesStrategyBar = new StackedImbalancesStrategyBar();
            _stackedImbalanceStrategy = new StackedImbalanceStrategy();
            // The longer cooldown is meant for replay testing. This can be adjusted according to your liking. The request can be a lot even within a triggered bar.
            _analysisCooldown = TimeSpan.FromSeconds(MessagingConfig.Instance.MarketEnvironment == EnvironmentType.Live ? 1 : 5);
            _validBarCount = 2;
            _validVolume = 500;
            _validDelta = 100;
        }

        public override bool CheckLong()
        {
            return CheckDirection(isLong: true);
        }

        public override bool CheckShort()
        {
            return CheckDirection(isLong: false);
        }

        private bool CheckDirection(bool isLong)
        {
            _stackedImbalancesStrategyBar.CurrentDataBar = currentDataBar;
            _stackedImbalancesStrategyBar.OneBarAgoDataBar = dataBars[dataBars.Count - 1];
            _stackedImbalancesStrategyBar.CurrentTechnicalLevels = currentTechnicalLevels;

            _stackedImbalanceStrategy.StackedImbalancesStrategyBar = _stackedImbalancesStrategyBar;

            if (!IsValidBarCount() &&
                !IsValidVolume() &&
                !IsValidDelta() &&
                !IsValidTriggerStrikePrice()
            )
            {
                return false;
            }

            bool isValidCheck = _stackedImbalanceStrategy.Check(isLong);
            if (!isValidCheck)
            {
                return false;
            }

            // If analysis is enabled then run analysis on triggered data
            if (IsAnalysisEnabled())
            {
                if (!CanRunAnalysis())
                {
                    return false;
                }
                return IsValidAnalysis(isLong);
            }

            // Analysis is disabled. The strategy check should be valid at this point.
            return !IsAnalysisEnabled();
        }

        private bool IsValidBarCount()
        {
            return dataBars.Count > _validBarCount;
        }

        private bool IsValidVolume()
        {
            return currentDataBar.Volumes.Volume > _validVolume;
        }

        private bool IsValidDelta(bool isLong = false)
        {
            long currentDelta = currentDataBar.Deltas.Delta;

            return isLong ? currentDelta > _validDelta : currentDelta < (_validDelta * -1);
        }

        #region Analysis Checks

        // This is just an example using an external sevice for analysis. You probably won't trigger any trades here.
        // You can just ignore the analysis checks if you aren't using it.

        private static bool IsAnalysisEnabled()
        {
            return MessagingConfig.Instance.ExternalAnalysisServiceEnabled;
        }

        private bool CanRunAnalysis()
        {
            return (DateTime.UtcNow - _lastAnalysisCheckTime) > _analysisCooldown;
        }

        private void UpdateAnalysis(TradeType tradeType)
        {
            try
            {
                // Get the metrics
                string metrics = eventsContainer.TradeAnalysisEvents.GetTradeData(tradeType, dataBars, currentDataBar);
                // Send metrics to be analyzed
                string analysis = eventsContainer.MessagingEvents.GetAnalysis(metrics);
                var response = JsonSerializer.Deserialize<PredictionResponse>(analysis);

                if (response?.Prediction == null)
                {
                    _lastAnalysisResult = AnalysisResult.CreateInvalid();
                }
                else
                {
                    _lastAnalysisResult = new AnalysisResult
                    {
                        IsValid = true,

                        // Buy predictions
                        LongProbability = response.Prediction.Buy.SuccessProb,
                        LongFailProbability = response.Prediction.Buy.FailProb,
                        LongConfidence = response.Prediction.Buy.Confidence,

                        // Sell predictions
                        ShortProbability = response.Prediction.Sell.SuccessProb,
                        ShortFailProbability = response.Prediction.Sell.FailProb,
                        ShortConfidence = response.Prediction.Sell.Confidence,

                        // Store complete prediction stats
                        Buy = response.Prediction.Buy,
                        Sell = response.Prediction.Sell,

                        // All momentum metrics
                        DeltaTrend = response.Prediction.MomentumMetrics.DeltaTrend,
                        CurrentDelta = response.Prediction.MomentumMetrics.CurrentDelta,
                        DeltaDivergence = response.Prediction.MomentumMetrics.DeltaDivergence,
                        ImbalanceShift = response.Prediction.MomentumMetrics.ImbalanceShift,
                        ImbalanceTrend = response.Prediction.MomentumMetrics.ImbalanceTrend,
                        RelativeBarStrength = response.Prediction.MomentumMetrics.RelativeBarStrength,
                        AtrRatio = response.Prediction.MomentumMetrics.AtrRatio,
                        AtrTrend = response.Prediction.MomentumMetrics.AtrTrend,
                        CloseVsOpen = response.Prediction.MomentumMetrics.CloseVsOpen,
                        PriceMomentum = response.Prediction.MomentumMetrics.PriceMomentum,
                        VolumePressure = response.Prediction.MomentumMetrics.VolumePressure
                    };
                }
            }
            catch (JsonException ex)
            {
                eventsContainer.EventManager.PrintMessage($"JSON Deserialization error: {ex.Message}");
                _lastAnalysisResult = AnalysisResult.CreateInvalid();
            }

            _lastAnalysisCheckTime = DateTime.UtcNow;
        }

        private bool IsValidAnalysis(bool checkLong)
        {
            TradeType tradeType = checkLong ? TradeType.Buy : TradeType.Sell;
            UpdateAnalysis(tradeType);

            if (!_lastAnalysisResult.IsValid)
                return false;

            // Basic thresholds
            double minSuccessProb = 0.05;
            double continuationThreshold = 0.75;
            double minDelta = 3.0;
            double minBarStrength = 0.1;
            double minImbalanceShift = 1.0;
            double minAtrRatio = 0.95;

            var stats = checkLong ? _lastAnalysisResult.Buy : _lastAnalysisResult.Sell;

            // If imbalance shift is against our direction, increase required thresholds
            if ((checkLong && _lastAnalysisResult.ImbalanceShift < 0) ||
                (!checkLong && _lastAnalysisResult.ImbalanceShift > 0))
            {
                minSuccessProb *= 1.5;
                continuationThreshold *= 1.1;
                minDelta *= 1.5;
            }

            // Check directional bias
            bool hasDirectionalSignal = checkLong
                ? (_lastAnalysisResult.Buy.SuccessProb > minSuccessProb || _lastAnalysisResult.DeltaTrend > 0)
                : (_lastAnalysisResult.Sell.SuccessProb > minSuccessProb || _lastAnalysisResult.DeltaTrend < 0);

            // Check bar and momentum conditions
            bool hasStructure = _lastAnalysisResult.RelativeBarStrength > minBarStrength &&
                (checkLong ? _lastAnalysisResult.CloseVsOpen > 0 : _lastAnalysisResult.CloseVsOpen < 0);

            // Check market dynamics
            bool hasMarketContext = _lastAnalysisResult.AtrRatio > minAtrRatio &&
                Math.Abs(_lastAnalysisResult.ImbalanceShift) > minImbalanceShift &&
                (checkLong ? _lastAnalysisResult.ImbalanceTrend > 0 : _lastAnalysisResult.ImbalanceTrend < 0);

            bool result = hasDirectionalSignal &&
                hasStructure &&
                hasMarketContext &&
                stats.ContinuationProb > continuationThreshold &&
                Math.Abs(_lastAnalysisResult.CurrentDelta) > minDelta;

            LogAnalysisResults(
                checkLong,
                minSuccessProb,
                minBarStrength,
                minImbalanceShift,
                minAtrRatio,
                continuationThreshold,
                minDelta,
                hasDirectionalSignal,
                hasStructure,
                hasMarketContext,
                result
            );

            return result;
        }

        private void LogAnalysisResults(
        bool checkLong,
        double minSuccessProb,
        double minBarStrength,
        double minImbalanceShift,
        double minAtrRatio,
        double continuationThreshold,
        double minDelta,
        bool hasDirectionalSignal,
        bool hasStructure,
        bool hasMarketContext,
        bool result)
        {
            string direction = checkLong ? "Long" : "Short";
            string signalType = checkLong ? "Bullish" : "Bearish";
            var stats = checkLong ? _lastAnalysisResult.Buy : _lastAnalysisResult.Sell;

            eventsContainer.EventManager.PrintMessage($"{direction} Entry Analysis:");

            // Directional Signals
            eventsContainer.EventManager.PrintMessage("Directional Signals:");
            eventsContainer.EventManager.PrintMessage($"- {direction} Success Prob: {stats.SuccessProb:F4} > {minSuccessProb:F4}");
            eventsContainer.EventManager.PrintMessage($"- Delta Trend: {_lastAnalysisResult.DeltaTrend:F4}");

            // Bar Structure
            eventsContainer.EventManager.PrintMessage("Bar Structure:");
            eventsContainer.EventManager.PrintMessage($"- Bar Strength: {_lastAnalysisResult.RelativeBarStrength:F4} > {minBarStrength:F4}");
            eventsContainer.EventManager.PrintMessage($"- Close vs Open: {_lastAnalysisResult.CloseVsOpen:F4}");

            // Market Context
            eventsContainer.EventManager.PrintMessage("Market Context:");
            eventsContainer.EventManager.PrintMessage($"- ATR Ratio: {_lastAnalysisResult.AtrRatio:F4} > {minAtrRatio:F4}");
            eventsContainer.EventManager.PrintMessage($"- Imbalance Shift: {Math.Abs(_lastAnalysisResult.ImbalanceShift):F4} > {minImbalanceShift:F4}");
            eventsContainer.EventManager.PrintMessage($"- Imbalance Trend: {_lastAnalysisResult.ImbalanceTrend:F4}");

            // Imbalance Analysis
            eventsContainer.EventManager.PrintMessage("Imbalance Analysis:");
            eventsContainer.EventManager.PrintMessage($"- Imbalance Direction: {(checkLong ? _lastAnalysisResult.ImbalanceShift > 0 : _lastAnalysisResult.ImbalanceShift < 0)}");
            eventsContainer.EventManager.PrintMessage($"- Volume Pressure: {_lastAnalysisResult.VolumePressure:F4}");

            // Continuation
            eventsContainer.EventManager.PrintMessage("Continuation:");
            eventsContainer.EventManager.PrintMessage($"- Continuation Prob: {stats.ContinuationProb:F4} > {continuationThreshold:F4}");
            eventsContainer.EventManager.PrintMessage($"- Current Delta: {Math.Abs(_lastAnalysisResult.CurrentDelta):F4} > {minDelta:F4}");

            // Signal Components Summary
            eventsContainer.EventManager.PrintMessage("Signal Components:");
            eventsContainer.EventManager.PrintMessage($"- Has {signalType} Signal: {hasDirectionalSignal}");
            eventsContainer.EventManager.PrintMessage($"- Has {(checkLong ? "Positive" : "Negative")} Structure: {hasStructure}");
            eventsContainer.EventManager.PrintMessage($"- Has Good Market Context: {hasMarketContext}");
            eventsContainer.EventManager.PrintMessage($"Final {direction} result: {result}");
        }

        #endregion
    }

    #region Response

    public class AnalysisResult
    {
        public bool IsValid { get; set; }

        public double LongProbability { get; set; }
        public double LongFailProbability { get; set; }
        public double LongConfidence { get; set; }
        public double ShortProbability { get; set; }
        public double ShortFailProbability { get; set; }
        public double ShortConfidence { get; set; }

        public PredictionStats Buy { get; set; }
        public PredictionStats Sell { get; set; }

        public double DeltaTrend { get; set; }
        public double CurrentDelta { get; set; }
        public double DeltaDivergence { get; set; }
        public double ImbalanceShift { get; set; }
        public double ImbalanceTrend { get; set; }
        public double RelativeBarStrength { get; set; }
        public double AtrRatio { get; set; }
        public double AtrTrend { get; set; }
        public double CloseVsOpen { get; set; }
        public double PriceMomentum { get; set; }
        public double VolumePressure { get; set; }

        public static AnalysisResult CreateInvalid()
        {
            return new AnalysisResult
            {
                IsValid = false,
                LongProbability = 0,
                LongFailProbability = 0,
                LongConfidence = 0,
                ShortProbability = 0,
                ShortFailProbability = 0,
                ShortConfidence = 0,
                Buy = new PredictionStats(),
                Sell = new PredictionStats(),
                DeltaTrend = 0,
                CurrentDelta = 0,
                DeltaDivergence = 0,
                ImbalanceShift = 0,
                ImbalanceTrend = 0,
                RelativeBarStrength = 0,
                AtrRatio = 0,
                AtrTrend = 0,
                CloseVsOpen = 0,
                PriceMomentum = 0,
                VolumePressure = 0
            };
        }
    }

    public class PredictionResponse
    {
        [JsonPropertyName("prediction")]
        public Prediction Prediction { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class Prediction
    {
        [JsonPropertyName("buy")]
        public PredictionStats Buy { get; set; }

        [JsonPropertyName("sell")]
        public PredictionStats Sell { get; set; }

        [JsonPropertyName("momentum_metrics")]
        public MomentumMetrics MomentumMetrics { get; set; }
    }

    public class PredictionStats
    {
        [JsonPropertyName("success_prob")]
        public double SuccessProb { get; set; }

        [JsonPropertyName("fail_prob")]
        public double FailProb { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("continuation_prob")]
        public double ContinuationProb { get; set; }
    }

    public class MomentumMetrics
    {
        [JsonPropertyName("delta_trend")]
        public double DeltaTrend { get; set; }

        [JsonPropertyName("current_delta")]
        public double CurrentDelta { get; set; }

        [JsonPropertyName("delta_divergence")]
        public double DeltaDivergence { get; set; }

        [JsonPropertyName("imbalance_shift")]
        public double ImbalanceShift { get; set; }

        [JsonPropertyName("imbalance_trend")]
        public double ImbalanceTrend { get; set; }

        [JsonPropertyName("relative_bar_strength")]
        public double RelativeBarStrength { get; set; }

        [JsonPropertyName("atr_ratio")]
        public double AtrRatio { get; set; }

        [JsonPropertyName("atr_trend")]
        public double AtrTrend { get; set; }

        [JsonPropertyName("close_vs_open")]
        public double CloseVsOpen { get; set; }

        [JsonPropertyName("price_momentum")]
        public double PriceMomentum { get; set; }

        [JsonPropertyName("volume_pressure")]
        public double VolumePressure { get; set; }
    }

    #endregion
}
