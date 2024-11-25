# OrderFlowBot

[![Actions Status](https://github.com/WaleeTheRobot/order-flow-bot/workflows/Build%20and%20Test/badge.svg)](https://github.com/WaleeTheRobot/order-flow-bot/actions)
[![Actions Status](https://github.com/WaleeTheRobot/order-flow-bot/workflows/Formatting/badge.svg)](https://github.com/WaleeTheRobot/order-flow-bot/actions)
[![Actions Status](https://github.com/WaleeTheRobot/order-flow-bot/workflows/SonarCloud/badge.svg)](https://github.com/WaleeTheRobot/order-flow-bot/actions)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=WaleeTheRobot_order-flow-bot&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=WaleeTheRobot_order-flow-bot)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=WaleeTheRobot_order-flow-bot&metric=bugs)](https://sonarcloud.io/summary/new_code?id=WaleeTheRobot_order-flow-bot)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=WaleeTheRobot_order-flow-bot&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=WaleeTheRobot_order-flow-bot)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=WaleeTheRobot_order-flow-bot&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=WaleeTheRobot_order-flow-bot)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=WaleeTheRobot_order-flow-bot&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=WaleeTheRobot_order-flow-bot)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=WaleeTheRobot_order-flow-bot&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=WaleeTheRobot_order-flow-bot)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=WaleeTheRobot_order-flow-bot&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=WaleeTheRobot_order-flow-bot)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=WaleeTheRobot_order-flow-bot&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=WaleeTheRobot_order-flow-bot)

<img src="./images/screenshot.png" alt="OrderFlowBot" style="display: block; margin: 0 auto">

A bot used for trading order flow with a selected ATM strategy. The recommended way to use OrderFlowBot is semi-automated trading to **ASSIST** with entries, but fully automated is an option. OrderFlowBot uses the selected ATM strategy. Just create your ATM strategies and select the one you want to use for the trade. You can create your own strategies and easily hook it to OrderFlowBot for semi-automated or fully automated trading.

# Important

Currently, it does require the user to have some basic programming knowledge for creating strategies.

Requires the lifetime NinjaTrader license for the volumetric data or the Order Flow + subscription.

Make sure Tick Replay is Checked.

Make sure you have ATM strategies.

OrderFlowBot may not work if using a version of NinjaTrader below 8.1.2.1. This is the minimum version supporting features up to C# 8. The below are information about OrderFlowBot usage.

Consider increasing the ticks per level in the data series for less liquid assets and update it to be the same in the OrderFlowBot properties.

You can find the latest release at https://github.com/WaleeTheRobot/order-flow-bot/releases or you can just fork the repository.

# Issues

Sometimes NinjaTrader will complain about an import failed. You can just open the zip file from the release and copy the OrderFlowBot directory into the Add On directory on your computer after removing the previous OrderFlowBot directory. It's normally located at: `C:\Users\<username>\Documents\NinjaTrader 8\bin\Custom\AddOns`. Afterwards, open NinjaTrader and click `New` > `NinjaScript Editor`. Click the NinjaScript Editor and press `F5`. It'll take a few seconds and you'll hear a sound. The icon at the bottom left corner of it will disappear when it's done compiling. Close the NinjaScript Editor and you should be good to go.

# Control Panel

## Trade Management

This section has options to manage OrderFlowBot, quickly clear other sections and close a trade that was triggered from a strategy.

#### Enabled/Disabled

The Enabled/Disabled button is used for enabling and disabling trading. An ideal scenario is to use this is during economic releases. You can click to disable so the bot doesn't check for entries. You can then enable it again afterwards.

- Enables or disables the bot from looking for strategies
- Enables or disables all other buttons
- Closes ATM positions

--- TODO ---

#### Auto

Automatically trades the selected strategies for both long and short. This is **NOT** recommended unless you have a profitable strategy and know what you are doing. Only custom created advanced strategies should be considered if this option is used.

- Disables Reset Direction
- Disables Trigger Strike Price, Long and Short
- Resets Trigger Strike Price, Long and Short

#### Alert

This is useful if you want to see your strategy entries, but want to further analyze for an actual entry. You can manually place the trade when it satisfies your requirements. Note that the close in the Trade Management section will not close manual entries.

- Does not submit an order
- Draws a triangle and plays sound based on triggered strategy

#### Close

Use this to close an ATM position triggered by a strategy.

- Closes ATM position triggered by a strategy
- Resets the Trigger Strike Price
- Resets selected Long/Short

#### Reset Direction

Resets the Trade Direction section.

#### Reset Strategies

Resets the Strategies section.

## Trade Direction

This section contains the options for triggering a trade direction. No entries will be considered if Long or Short options are not selected. Selecting both Long and Short options will enable the bot to look for both long and short entries.

#### Trigger Strike Price

The trigger strike price acts as the starting point for the strategy to begin seeking an entry. However, this will only work if the strategy includes the necessary logic to check and verify the requirements. For instance, if your strategy is designed to validate certain conditions and includes logic for the trigger strike price, it will only execute these checks when the trigger price is reached. This is similar to a limit order, but in this case, the strategy only starts evaluating conditions once the limit price is hit.

#### Standard/Inverse

This will execute using either the default standard order or the inverse order. For example, if a strategy triggers a long position, a standard order will open a long position. However, if the inverse option is selected, the same long trigger will instead open a short position. This can be useful in range-bound markets where the strategy's signals align with market movements, but the market often rejects the anticipated direction and moves in the opposite way.

#### Long

Select this to look for long trades. Resets after position exits.

#### Short

Select this to look for short trades. Resets after position exits.

## Entries

This does not allow multiple entries on the same bar. However, multiple entries may appear on the same bar when backtesting even though it should in the next bar.

## Backtesting

You can backtest your strategies by enabling the backtesting. Entries uses the 1 tick data series for better granularity. You won't be able to use high resolution option.

## Strategies

I'm still deciding how to approach this section to support users without coding skills who want to implement their own custom strategies. For now, I've included the Stacked Imbalance Strategy as a simple example to help you create your own strategies. It doesn't have adjustable properties, so you'll need to modify and compile it yourself if you wish to use or customize it.

#### Stacked Imbalances

This strategy is the common stacked imbalances strategy. It will trigger based on finding stacked imbalances. Note that just because it found a stacked imbalance doesn't mean it can revert shortly afterwards causing those stacked imbalances to become invalid. This is just an example strategy and you should customize it to your own liking.

#### Long

- Previous bar is bullish
- Current bar is bullish
- Current bar is above fast EMA
- Triggers at trigger strike price if its used
- Has valid ask stacked imbalances

#### Short

- Previous bar is bearish
- Current bar is bearish
- Current bar is below fast EMA
- Triggers at trigger strike price if its used
- Has valid bid stacked imbalances

## Build Your Own Strategy

You can design your own custom trading strategy and leverage all the properties of the data bar. To get started, simply create a new file and place it in the `Models/Strategies/Implementations` directory. Ensure your class inherits from `StrategyBase`.

For guidance, check out the `StackedImbalances` class as an example. Once you've completed your implementation, compile the project, and OFB will automatically generate a button for your new strategy in the control panel, giving you greater control over its operation.
