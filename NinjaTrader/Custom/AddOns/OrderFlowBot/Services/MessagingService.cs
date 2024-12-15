using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using System;
using System.Net.Http;
using System.Text;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    public class MessagingService
    {
        private readonly EventManager _eventManager;
        private readonly HttpClient _httpClient;
        private readonly string _serviceUrl;

        public MessagingService(EventsContainer eventsContainer)
        {
            _eventManager = eventsContainer.EventManager;

            if (MessagingConfig.Instance.ExternalAnalysisServiceEnabled)
            {
                _httpClient = new HttpClient();
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
            }
            else
            {
                _httpClient = null;
            }

            var service = MessagingConfig.Instance.ExternalAnalysisService;
            _serviceUrl = service.StartsWith("http") ? service : $"http://{service}";

            eventsContainer.MessagingEvents.OnGetAnalysis += HandleGetAnalysis;
        }

        private string HandleGetAnalysis(string message)
        {
            if (!MessagingConfig.Instance.ExternalAnalysisServiceEnabled)
            {
                return $"{{\"error\":\"Service not available\"}}";
            }

            try
            {
                _eventManager.PrintMessage("Sending analysis request...");

                // Create the HTTP request
                var content = new StringContent(message, Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync(_serviceUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }

                _eventManager.PrintMessage($"Request failed: {response.StatusCode}");
                return $"{{\"error\":\"Request failed with status {response.StatusCode}\"}}";
            }
            catch (Exception ex)
            {
                _eventManager.PrintMessage($"Error communicating with analysis service: {ex.Message}");
                return "{\"error\":\"Communication failure\"}";
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
