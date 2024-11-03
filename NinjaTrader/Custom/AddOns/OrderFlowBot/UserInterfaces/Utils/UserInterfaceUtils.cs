﻿using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Utils
{
    public static class UserInterfaceUtils
    {
        public static SolidColorBrush GetSolidColorBrushFromHex(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor))
            {
                throw new ArgumentException("Hex color string cannot be null or empty.");
            }

            if (!hexColor.StartsWith("#"))
            {
                hexColor = "#" + hexColor;
            }

            try
            {
                Color color = (Color)ColorConverter.ConvertFromString(hexColor);
                return new SolidColorBrush(color);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException($"Invalid hex color format: {hexColor}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating SolidColorBrush from hex color: {hexColor}", ex);
            }
        }

        public static void ForceRefreshButton(Button button)
        {
            // Button redraw workaround
            // Quickly disables and enables button to force the button state to visually update
            button.IsEnabled = false;
            button.IsEnabled = true;
        }
    }
}