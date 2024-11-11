using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components.Controls;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Models;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components
{
    public class StrategiesGrid : GridBase
    {
        private List<StrategyBase> _strategies;

        public StrategiesGrid(
            ServicesContainer servicesContainer,
            UserInterfaceEvents userInterfaceEvents,
            StrategiesEvents strategiesEvents
        ) : base("Strategies", servicesContainer, userInterfaceEvents, strategiesEvents, UserInterfaceUtils.CreateCogIcon(() =>
        {
            // TODO: Handle button
        }))
        {
        }

        public override void InitializeInitialToggleState()
        {
            initialToggleState = new Dictionary<string, bool>();

            _strategies = strategiesEvents.GetStrategies();

            foreach (StrategyBase strategy in _strategies)
            {
                initialToggleState[strategy.StrategyData.Name] = false;
            }
        }

        protected override void AddButtons()
        {
            var buttonModels = _strategies.Select(strategy => new ButtonModel
            {
                Name = strategy.StrategyData.Name.Replace(" ", ""),
                Content = strategy.StrategyData.Name,
                ToggledContent = strategy.StrategyData.Name,
                BackgroundColor = CustomColors.BUTTON_GREEN_COLOR,
                HoverBackgroundColor = CustomColors.BUTTON_HOVER_BG_COLOR,
                ToggledBackgroundColor = CustomColors.BUTTON_BG_COLOR,
                TextColor = CustomColors.TEXT_COLOR,
                ClickHandler = (Action<object, EventArgs>)HandleButtonClick,
                IsToggleable = true,
                InitialToggleState = initialToggleState.ContainsKey(strategy.StrategyData.Name.Replace(" ", ""))
                    && initialToggleState[strategy.StrategyData.Name.Replace(" ", "")]
            }).ToList();

            for (int i = 0; i < buttonModels.Count; i++)
            {
                var config = buttonModels[i];
                var button = new CustomButton(config).Button;
                buttons[config.Name] = button;

                int row = (i / 2) + 1;
                int column = i % 2;

                AddButtonToGrid(button, row, column);
            }
        }

        public override void HandleButtonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ButtonState state = (ButtonState)button.Tag;
            string buttonName = state.Config.Name;

            if (state.IsToggled)
            {
                userInterfaceEvents.AddSelectedStrategyTriggered(buttonName);
            }
            else
            {
                userInterfaceEvents.RemoveSelectedStrategyTriggered(buttonName);
            }
        }

        public override void HandleAutoTradeTriggered(bool isEnabled)
        {
            // Do nothing
        }
    }
}
