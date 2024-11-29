using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Models;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components.Controls
{
    public class ButtonState
    {
        public bool IsToggled { get; set; }
        public ButtonModel Config { get; set; }
    }

    public class CustomButton
    {
        public Button Button { get; private set; }

        public CustomButton(ButtonModel config)
        {
            Button = CreateButton(config);
        }

        private Button CreateButton(ButtonModel config)
        {
            var button = new Button
            {
                Name = config.Name,
                Content = config.IsToggleable && config.InitialToggleState ? config.ToggledContent : config.Content,
                FontSize = 14,
                Visibility = Visibility.Visible,
                Foreground = UserInterfaceUtils.GetSolidColorBrushFromHex(config.TextColor),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Tag = new ButtonState { IsToggled = config.IsToggleable && config.InitialToggleState, Config = config },
                Background = UserInterfaceUtils.GetSolidColorBrushFromHex(config.BackgroundColor),
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(8),
                Margin = new Thickness(3)
            };

            button.Style = CreateCustomButtonStyle(button);

            if (config.ClickHandler != null)
            {
                button.Click += async (sender, e) =>
                {
                    if (config.IsToggleable)
                    {
                        ToggleButton(button);
                    }

                    if (config.ClickHandler is Action<object, RoutedEventArgs> syncHandler)
                    {
                        syncHandler(sender, e);
                    }
                    else if (config.ClickHandler is Func<object, RoutedEventArgs, Task> asyncHandler)
                    {
                        await asyncHandler(sender, e);
                    }
                };
            }

            button.IsEnabledChanged += (sender, e) => UpdateButtonState(button, button.IsEnabled);
            button.MouseEnter += (sender, e) => button.Background = UserInterfaceUtils.GetSolidColorBrushFromHex(config.HoverBackgroundColor);
            button.MouseLeave += (sender, e) => UpdateButtonState(button, button.IsEnabled);

            UpdateButtonState(button, false);

            return button;
        }

        private Style CreateCustomButtonStyle(Button button)
        {
            var style = new Style(typeof(Button));

            var config = ((ButtonState)button.Tag).Config;

            style.Setters.Add(new Setter(Control.TemplateProperty, CreateButtonTemplate()));

            style.Triggers.Add(new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true,
                Setters = { new Setter(Control.BackgroundProperty, UserInterfaceUtils.GetSolidColorBrushFromHex(config.HoverBackgroundColor)) }
            });

            style.Triggers.Add(new Trigger
            {
                Property = System.Windows.Controls.Primitives.ButtonBase.IsPressedProperty,
                Value = true,
                Setters = { new Setter(Control.BackgroundProperty, UserInterfaceUtils.GetSolidColorBrushFromHex(config.HoverBackgroundColor)) }
            });

            style.Triggers.Add(new Trigger
            {
                Property = UIElement.IsEnabledProperty,
                Value = false,
                Setters =
                {
                    new Setter(Control.BackgroundProperty, UserInterfaceUtils.GetSolidColorBrushFromHex(CustomColors.BUTTON_DISABLED_BG_COLOR)),
                    new Setter(UIElement.OpacityProperty, 0.5)
                }
            });

            return style;
        }

        private ControlTemplate CreateButtonTemplate()
        {
            var template = new ControlTemplate(typeof(Button));

            var border = new FrameworkElementFactory(typeof(Border));
            border.SetBinding(Border.BackgroundProperty, new Binding("Background") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            border.SetBinding(Border.BorderBrushProperty, new Binding("BorderBrush") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            border.SetBinding(Border.BorderThicknessProperty, new Binding("BorderThickness") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            border.SetBinding(Border.PaddingProperty, new Binding("Padding") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });

            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            border.AppendChild(contentPresenter);

            template.VisualTree = border;

            return template;
        }

        private static void UpdateButtonState(Button button, bool isEnabled)
        {
            var state = (ButtonState)button.Tag;
            var config = state.Config;

            if (isEnabled)
            {
                if (config.IsToggleable)
                {
                    // Toggled is enabling
                    button.Background = UserInterfaceUtils.GetSolidColorBrushFromHex(state.IsToggled ? config.BackgroundColor : config.ToggledBackgroundColor);
                    button.Content = state.IsToggled && config.IsToggleable ? config.ToggledContent : config.Content;
                }
                else
                {
                    button.Background = UserInterfaceUtils.GetSolidColorBrushFromHex(config.BackgroundColor);
                    button.Content = state.IsToggled && config.IsToggleable ? config.ToggledContent : config.Content;
                }


                button.Opacity = 1;
            }
            else
            {
                button.Background = UserInterfaceUtils.GetSolidColorBrushFromHex(CustomColors.BUTTON_DISABLED_BG_COLOR);
                button.Opacity = 0.5;
            }

            button.IsEnabled = isEnabled;
        }

        private static void ToggleButton(Button button)
        {
            var state = (ButtonState)button.Tag;
            state.IsToggled = !state.IsToggled;
            UpdateButtonState(button, button.IsEnabled);
        }
    }
}
