using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using System.Windows;
using System.Windows.Controls;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Utils
{
    public static class TextHeadingLabelUtils
    {
        public static TextBlock GetHeadingLabel(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 14,
                Foreground = UserInterfaceUtils.GetSolidColorBrushFromHex(CustomColors.TEXT_COLOR),
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };
        }
    }
}
