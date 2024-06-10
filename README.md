# OrderFlowBot

In case you see this... it's still in development. So you probably shouldn't use it.

A bot used for trading order flow with a selected ATM strategy. The recommended way to use OrderFlowBot is semi-automated trading to assist with entries, but fully automated is an option.

OrderFlowBot uses the selected ATM strategy. Just create your ATM strategies and select the one you want to use for the trade.

You can create your own strategies and easily hook it to OrderFlowBot for semi-automated or fully automated trading. You can also use the simple strategies to look for entries.

Indicators can also be created with data from the OrderFlowBot DataBar for usage when the OrderFlowBot is enabled.

# Important

Requires the lifetime NinjaTrader license for the volumetric data or the Order Flow + subscription.

Make sure Tick Replay is Checked.

Make sure you have ATM strategies.

OrderFlowBot may not work if using a version of NinjaTrader below 8.1.2.1. This is the minimum version supporting features up to C# 8. The below are information about OrderFlowBot usage.

Consider increasing the ticks per level in the data series for less liquid assets.

For developing, you can copy the OrderFlowBot folder into your local NinjaTrader AddOns folder.

For usage, you can download the zip containing the word import in the release page. You can import this zip file similar to importing a normal NinjaTrader Add-On. https://github.com/WaleeTheRobot/order-flow-bot/releases

# Control Panel

## Trade Management

This section has options to manage OrderFlowBot, quickly clear other sections and close trades.

#### Enabled/Disabled

Resets all sections and disables or enable the sections. No strategies will be checked when disabled is selected. This can only be activated when there aren't any positions opened.

#### Auto

Automatically trades the selected strategies. This is NOT recommended, but is an option. Only custom created strategies should be considered if this option is used. Both the default strategies "Delta Chaser and Range Rebound" are NOT designed for fully automated trading.

#### Reset Direction

Resets the Trade Direction section.

#### Reset Strategies

Resets the Strategies section.

#### Close

Closes any ATM position and resets the Trade Direction section.

## Trade Direction

This section contains the inputs for triggering a trade direction.

#### Trigger Strike Price

The strike price to trigger the strategy to start looking for an entry. A threshold in the strategies properties section is set to allow for a buffer for triggering. The trigger strike price will only be considered if there is a value set in the input.

#### Long

Select this to look for long trades.

#### Short

Select this to look for short trades.

## Strategies

This section contains the custom created strategies and are dynamically created from the `StrategiesConfig`. The default strategies here can be used as examples to create your own custom strategy.

#### Delta Chaser

This strategy is designed for trading pullbacks on a trend or larger price ranges. Trade the structure with appropriate targets on higher volatility times.

#### Range Rebound

This strategy is designed for trading smaller price ranges aiming to capitalize on mean reversion. Trade the edges with smaller targets on lower volatility times.
