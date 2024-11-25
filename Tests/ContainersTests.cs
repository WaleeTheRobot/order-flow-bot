using Xunit;
using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests
{
    public class ContainersTests
    {
        [Fact]
        public void ShouldInitializeAllEvents()
        {
            var eventsContainer = new EventsContainer();

            Assert.NotNull(eventsContainer.EventManager);
            Assert.NotNull(eventsContainer.DataBarEvents);
            Assert.NotNull(eventsContainer.TradingEvents);
        }

        [Fact]
        public void ShouldInitializeAllServices()
        {
            var eventsContainer = new EventsContainer();
            var backtestData = new BacktestConfigData();
            var servicesContainer = new ServicesContainer(eventsContainer, backtestData);

            Assert.NotNull(servicesContainer.DataBarService);
            Assert.NotNull(servicesContainer.StrategiesService);
            Assert.NotNull(servicesContainer.TradingService);
        }
    }
}
