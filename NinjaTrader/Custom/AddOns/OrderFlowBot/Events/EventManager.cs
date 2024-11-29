using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class EventManager
    {
        public event Action<string, bool> OnPrintMessage;

        public void InvokeEvent(Action eventHandler)
        {
            try
            {
                eventHandler?.Invoke();
            }
            catch (Exception ex)
            {
                PrintMessage($"Error invoking event: {ex.Message}");
            }
        }

        public void InvokeEvent<T>(Action<T> eventHandler, T arg)
        {
            try
            {
                eventHandler?.Invoke(arg);
            }
            catch (Exception ex)
            {
                PrintMessage($"Error invoking event: {ex.Message}");
            }
        }

        public void InvokeEvent<T1, T2>(Action<T1, T2> eventHandler, T1 arg1, T2 arg2)
        {
            try
            {
                eventHandler?.Invoke(arg1, arg2);
            }
            catch (Exception ex)
            {
                PrintMessage($"Error invoking event: {ex.Message}");
            }
        }

        public T InvokeEvent<T>(Func<T> eventHandler)
        {
            try
            {
                if (eventHandler != null)
                {
                    return eventHandler();
                }

                PrintMessage("Event handler is null");
                return default;
            }
            catch (Exception ex)
            {
                PrintMessage($"Error invoking event: {ex.Message}");
                return default;
            }
        }

        public void PrintMessage(string eventMessage, bool addNewLine = false)
        {
            InvokeEvent(OnPrintMessage, eventMessage, addNewLine);
        }
    }
}
