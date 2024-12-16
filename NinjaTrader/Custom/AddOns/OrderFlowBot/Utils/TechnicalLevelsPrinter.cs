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
                PrintEma(technicalLevels);
            }

            if (config.ShowAtr)
            {
                PrintAtr(technicalLevels);
            }

            Print("\n");
        }

        private static void PrintEma(IReadOnlyTechnicalLevels technicalLevels)
        {
            Print("**** EMA ****");
            Print(string.Format("Fast: {0}", technicalLevels.Ema.FastEma.ToString()));
            Print(string.Format("Slow: {0}", technicalLevels.Ema.SlowEma.ToString()));
        }

        private static void PrintAtr(IReadOnlyTechnicalLevels technicalLevels)
        {
            Print("**** ATR ****");
            Print(string.Format("Value: {0}", technicalLevels.Atr.Value.ToString()));
        }

        private static void Print(string message)
        {
            _eventManager.PrintMessage(message);
        }
    }
}
