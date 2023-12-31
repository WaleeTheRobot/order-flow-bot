# OrderFlowBot

![Order Flow Bot](./screenshot.png)

### Requires the lifetime NinjaTrader license for the volumetric data

A bot used for trading order flow. The primary focus for OrderFlowBot is semi-automated trading. Strategies can be added and then manually selected for the software to look for entries. Indicators can also be added so they will startup when the OrderFlowBot strategy is enabled.

# Usage

The below are information about OrderFlowBot usage.

### ATM Strategy

OrderFlowBot uses an ATM Strategy. The default is called OrderFlowBot. This will be used when a strategy's requirements our met for entry. You will need to create an ATM Strategy and include it in OrderFlowBot options.

### Buttons

The Long and Short buttons are used with a selected strategy to look for long or short entries. Selecting both will look for both long and short entries. Selecting only one will look for that specific entry. The strategy buttons are dynamically added.

### Backtesting

You can backtest your strategies by enabling the backtesting. This will use the target and stop where you enabled the backtesting. This will backtest all the strategies and can be a way to automate OrderFlowBot, but it is not the primary focus.

# Included Indicators

The below indicators are included for OrderFlowBot and can be used as examples to add any other indicators.

### Ratios

Made popular by Mike from OrderFlows, this shows the bottom divided bid ratios or top divided ask ratios. The ratios will be displayed in color, bold and larger font if it meets the threshold in `ValidExhaustionRatio` or `ValidAbsorptionRatio` properies.

### LastExhaustionAbsorptionPrice

This shows the prices for the last valid exhaustion or valid absorption ratios for the bid and ask.

# Included Strategies

The below is currently the included strategy for OrderFlowBot and can be used as an example to add any other strategies. Note that there is a check to not re-enter on the same bar in the main program so a strategy doesn't re-enter on the same bar. This will prevent multiple entries on a bar, to prevent any false entries.

### Stacked Imbalances

This strategy looks if a bar is bullish or bearish and will enter if ask or bid stacked imbalances are found.

# Adding Strategies and Indicators

The custom DataBar should be used if you are considering adding strategies and indicators. It takes some of the data from the volumetric bars and creates custom bars that you can also add any additional information to.

- Add your strategy or indicator into the config `StrategiesIndicatorsConfigList` in `OrderFlowBot/StrategiesIndicators/StrategiesIndicatorsConfig` file.
- Create the class for your strategy or indicator and add it in `OrderFlowBot/StrategiesIndicators/Strategies/` or `OrderFlowBot/StrategiesIndicators/Indicators/`.
- The strategies are dynamically instantiated based on the config. Make sure to inherit the `StrategyBase` class similar to the `StackedImbalances` strategy.
- For an indicator, you can copy one of the existing indicator and make sure to also modify the NinjaScript generated code to reflect your indicator class. Go to `OrderFlowBot/OrderFlowBot` and add the option to enable or disable it in the `Indicators Properties`. Make sure to set the default value in `OnStateChange()` and add a check in `AddIndicators()`.

# Unit Testing

Not the greatest way to go about it, but essentially just copying over code and testing them with mock data. This is really just used to make sure some of the values are as expected.
