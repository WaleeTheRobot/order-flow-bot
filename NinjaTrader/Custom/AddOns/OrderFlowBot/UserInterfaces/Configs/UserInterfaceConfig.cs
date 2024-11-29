namespace NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs
{
    public class UserInterfaceConfig
    {
        private static readonly UserInterfaceConfig _instance = new UserInterfaceConfig();

        private UserInterfaceConfig()
        {
        }

        public static UserInterfaceConfig Instance
        {
            get
            {
                return _instance;
            }
        }

        public string AssetsPath { get; set; }
    }

    public static class CustomColors
    {
        public const string MAIN_GRID_BG_COLOR = "#2C2C34";
        public const string MAIN_HEADING_BG_COLOR = "#005A9C";
        public const string BUTTON_BG_COLOR = "#6082B6";
        public const string BUTTON_HOVER_BG_COLOR = "#6F8FAF";
        public const string BUTTON_DISABLED_BG_COLOR = "#808080";
        public const string BUTTON_GREEN_COLOR = "#097969";
        public const string BUTTON_YELLOW_COLOR = "#DAA520";
        public const string BUTTON_RED_COLOR = "#D2042D";
        public const string TEXT_COLOR = "#E7E7E7";
        public const string INPUT_FIELD_COLOR = "#272829";
    }

    public static class ButtonName
    {
        // Trade Management
        public const string ENABLED = "Enabled";
        public const string AUTO = "Auto";
        public const string ALERT = "Alert";
        public const string CLOSE = "Close";
        public const string RESET_DIRECTION = "ResetDirection";
        public const string RESET_STRATEGIES = "ResetStrategies";

        // Trade Direction
        public const string STANDARD = "Standard";
        public const string LONG = "Long";
        public const string SHORT = "Short";
    }
}
