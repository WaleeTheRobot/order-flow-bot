# OrderFlowBot

![Order Flow Bot](./screenshot.png)

### Requires the lifetime NinjaTrader license for the volumetric data

A bot used for trading order flow. OrderFlowBot can be used to add existing strategies for automated trading or semi-automated trading. You can also include indicators so you can view the indicators while turning on the strategy. However, you update the code to remove the volumetric data and use it with whatever you want.

# Indicators

The two example indicators below uses the data from the OrderFlowDataBar.

### AutoVolumeProfile

This is disabled by default. On the first tick of the bar, the OrderFlowDataBar combines the last number of bar's volume profiles based on the `AutoVolumeProfileLookBackBars` property. This draws a line to represent the point of control, value area high and value area low of the combined volume profiles.

### Ratios

Made popular by Mike from OrderFlows, this shows the bottom divided bid ratios or top divided ask ratios. The ratios will be displayed in color, bold and larger font if it meets the threshold in `ValidRatio` property.

### Adding an Indicator

- Add the class in the `Gui` folder

In `OrderFlowBot.cs`

- Instantiate the indicator
- Add the indicator to the State.DataLoaded check

# Strategies

### Don't use the example strategy

Follow the example strategy and implement your own. You can then backtest it with BackTestingEnabled.

# Adding Buttons For Strategies

Follow the same pattern as Imbalance Ratio Volume Profile.

In `ControlPanel.cs`

- Add a label
- Add a new button in `_buttonMap` dictionary found in `DefineButtons` function
- Add a new function to handle the button click

# Testing

Probably not the greatest way to go about it, but essentially just copying over code and testing them with mock data.
