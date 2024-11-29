using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TechnicalLevelsModel;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Utils
{
    public static class TechnicalLevelsPrinter
    {
        private static EventManager _eventManager;

        public static void PrintTechnicalLevels(
            EventManager eventManager,
            IReadOnlyTechnicalLevels technicalLevels,
            ITechnicalLevelsPrintConfig config
        )
        {
            _eventManager = eventManager;

            if (config.ShowEma)
            {
                PrintBasic(technicalLevels);
            }

            Print("\n");
        }

        private static void PrintBasic(IReadOnlyTechnicalLevels technicalLevels)
        {
            Print("**** EMA ****");
            Print(string.Format("Fast: {0}", technicalLevels.Ema.FastEma.ToString()));
            Print(string.Format("Slow: {0}", technicalLevels.Ema.SlowEma.ToString()));
        }

        private static void Print(string message)
        {
            _eventManager.PrintMessage(message);
        }
    }
}
