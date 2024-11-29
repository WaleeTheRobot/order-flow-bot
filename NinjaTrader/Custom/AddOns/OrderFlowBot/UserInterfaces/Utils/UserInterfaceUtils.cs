using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                throw new InvalidOperationException($"Error creating SolidColorBrush from hex color: {hexColor}", ex);
            }
        }

        public static void ForceRefreshButton(Button button)
        {
            // Button redraw workaround
            // Quickly disables and enables button to force the button state to visually update
            button.IsEnabled = false;
            button.IsEnabled = true;
        }

        public static Image CreateCogIcon(Action onClickAction)
        {
            var cogIcon = new Image
            {
                Source = new BitmapImage(new Uri(Path.Combine(UserInterfaceConfig.Instance.AssetsPath, "cog.png"))),
                Width = 13,
                Height = 13,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 4, 0, 0),
                Cursor = Cursors.Hand
            };

            cogIcon.MouseLeftButtonUp += (s, e) => onClickAction?.Invoke();

            return cogIcon;
        }
    }
}
