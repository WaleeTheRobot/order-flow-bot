# OrderFlowBot

One day when I'm not lazy and have time I may come back to this, but things seem to work. Just don't use any of the strategies.

A bot used for trading order flow. OrderFlowBot can be used to add existing strategies for automated trading or semi-automated trading. You can also include indicators so you can view the indicators while turning on the strategy. It uses the volumetric data and **requires the lifetime NinjaTrader license**. However, you update the code to remove the volumetric data and use it with whatever you want.

# Indicators

The two indicators below uses the data from the OrderFlowDataBar.

### AutoVolumeProfile

On the first tick of the bar, the OrderFlowDataBar combines the last number of bar's volume profiles based on the `AutoVolumeProfileLookBackBars` property. This draws a line to represent the point of control, value area high and value area low of the combined volume profiles.

### Ratios

Made popular by Mike from OrderFlows, this shows the bottom divided bid ratios or top divided ask ratios. The ratios will be displayed in color, bold and larger font if it meets the threshold in `ValidRatio` property.

### Adding an Indicator

- Add the class in the `Gui` folder

In `OrderFlowBot.cs`

- Instantiate the indicator
- Add the indicator to the State.DataLoaded check

# Adding Buttons For Strategies

Follow the same pattern as Imbalance Ratio Volume Profile.

In `ControlPanel.cs`

- Add a label
- Add a new button in `_buttonMap` dictionary found in `DefineButtons` function
- Add a new function to handle the button click
