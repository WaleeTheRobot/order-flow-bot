using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.StrategiesIndicators
{
    public class StrategiesIndicators
    {
        // Important. This must match the file name.
        public string Name { get; set; }
        public string ButtonLabel { get; set; }
        public bool IsStrategy { get; set; }
        public bool IsIndicator { get; set; }
    }

    public class StrategiesIndicatorsConfig
    {
        public List<StrategiesIndicators> StrategiesIndicatorsConfigList { get; set; }

        public StrategiesIndicatorsConfig()
        {
            StrategiesIndicatorsConfigList = new List<StrategiesIndicators>
            {
                new StrategiesIndicators
                {
                    Name = "StackedImbalances",
                    ButtonLabel = "Stacked Imbalances",
                    IsStrategy = true,
                    IsIndicator = false,
                },
                new StrategiesIndicators
                {
                    Name = "DeltaDivergence",
                    ButtonLabel = "DeltaDivergence",
                    IsStrategy = true,
                    IsIndicator = false,
                },
                new StrategiesIndicators
                {
                    Name = "Ratios",
                    ButtonLabel = "",
                    IsStrategy = false,
                    IsIndicator = true,
                },
                new StrategiesIndicators
                {
                    Name = "RatiosLastExhaustionAbsorptionPrice",
                    ButtonLabel = "",
                    IsStrategy = false,
                    IsIndicator = true,
                }
            };
        }
    }
}
