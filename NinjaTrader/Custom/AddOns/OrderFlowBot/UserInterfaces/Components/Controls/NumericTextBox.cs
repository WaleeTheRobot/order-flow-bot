using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Components.Controls
{
    public class NumericTextBox
    {
        public TextBox TextBox { get; private set; }
        private readonly DispatcherTimer _debounceTimer;
        public event EventHandler DebouncedTextChanged;

        public NumericTextBox(string toolTip)
        {
            TextBox = new TextBox
            {
                Height = 30,
                Margin = new Thickness(3, 3, 3, 3),
                ToolTip = toolTip,
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = UserInterfaceUtils.GetSolidColorBrushFromHex(CustomColors.INPUT_FIELD_COLOR),
                IsEnabled = false
            };

            TextBox.PreviewKeyDown += PreviewKeyDown;

            _debounceTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300)
            };
            _debounceTimer.Tick += DebounceTimer;

            TextBox.TextChanged += (s, e) =>
            {
                _debounceTimer.Stop();
                _debounceTimer.Start();
            };
        }

        private void DebounceTimer(object sender, EventArgs e)
        {
            _debounceTimer.Stop();

            DebouncedTextChanged?.Invoke(this, EventArgs.Empty);
        }

        private static void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBoxSender = (TextBox)sender;
            string currentText = textBoxSender.Text;
            int selectionStart = textBoxSender.SelectionStart;

            // Allow arrow keys for navigation
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false; // Allow the arrow key press to pass through
                return;
            }

            // Allow backspace and delete operations
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = false;
                return;
            }

            // Handle numeric key press
            if ((e.Key >= Key.D0 && e.Key <= Key.D9 && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ||
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 && Keyboard.IsKeyToggled(Key.NumLock)))
            {
                int decimalIndex = currentText.IndexOf('.');

                // Check if input is after the decimal and already has two digits
                if (decimalIndex != -1 && selectionStart > decimalIndex && currentText.Length - decimalIndex > 2)
                {
                    e.Handled = true;
                }
                else
                {
                    char num = (char)('0' + (e.Key - (e.Key >= Key.NumPad0 ? Key.NumPad0 : Key.D0)));
                    ReplaceText(textBoxSender, num.ToString());
                    e.Handled = true;
                }
                return;
            }

            // Handle decimal point
            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                if (!currentText.Contains('.'))
                {
                    ReplaceText(textBoxSender, ".");
                }
                e.Handled = true;
                return;
            }

            // Handle negative sign
            if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                if (selectionStart == 0 && !currentText.StartsWith("-"))
                {
                    ReplaceText(textBoxSender, "-");
                }
                e.Handled = true;
                return;
            }

            // Block any other key input
            e.Handled = true;
        }

        private static void ReplaceText(TextBox textBox, string newText)
        {
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;
            string currentText = textBox.Text;

            // Replace selected text or insert new text at the current cursor position
            string updatedText = currentText.Remove(selectionStart, selectionLength).Insert(selectionStart, newText);

            // Check if the text has more than one decimal point
            if (updatedText.Count(c => c == '.') > 1)
            {
                return;
            }

            textBox.Text = updatedText;
            textBox.SelectionStart = selectionStart + newText.Length;
        }
    }
}
