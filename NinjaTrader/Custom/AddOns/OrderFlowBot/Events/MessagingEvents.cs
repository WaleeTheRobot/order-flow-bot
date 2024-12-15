using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Events
{
    public class MessagingEvents
    {
        private readonly EventManager _eventManager;
        public event Func<string, string> OnGetAnalysis;

        public MessagingEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Event triggered when current analysis is requested.
        /// This is used to get the a json object from an external service.
        /// </summary>
        /// <param name="metrics">The metrics needed for an external service.</param>
        /// <returns>Json object.</returns>
        public string GetAnalysis(string metrics)
        {
            return _eventManager.InvokeEvent(() => OnGetAnalysis?.Invoke(metrics));
        }
    }
}
