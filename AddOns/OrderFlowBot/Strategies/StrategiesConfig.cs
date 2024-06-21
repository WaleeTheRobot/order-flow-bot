using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public class StrategiesImplementation
    {
        // Important. This must match the file name.
        public string Name { get; set; }
        public string ButtonLabel { get; set; }
    }

    public class StrategiesConfig
    {
        public List<StrategiesImplementation> StrategiesConfigList { get; set; }

        public StrategiesConfig()
        {
            // Note that the checks will iterate through the list. Insertion order matters, if you want to prioritize strategy checks.
            StrategiesConfigList = new List<StrategiesImplementation>
            {
                new StrategiesImplementation
                {
                    Name = "DeltaChaser",
                    ButtonLabel = "Delta Chaser"
                },
                new StrategiesImplementation
                {
                    Name = "StackedImbalances",
                    ButtonLabel = "Stacked Imbalances"
                },
                new StrategiesImplementation
                {
                    Name = "VolumeSequencing",
                    ButtonLabel = "Volume Sequencing"
                }
            };
        }
    }
}
