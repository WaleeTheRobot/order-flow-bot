using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public interface IStrategy
    {
        string Name { get; set; }
        Direction ValidStrategyDirection { get; set; }

        void CheckStrategy();
        void CheckLong();
        void CheckShort();
    }
}
