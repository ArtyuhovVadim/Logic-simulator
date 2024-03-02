using System.Windows;

namespace LogicSimulator.Resources.Brushes;

public static class BrushesKeys
{
    public static class MainWindow
    {
        public static readonly ComponentResourceKey StatusBarBackground = CreateInstance();

        public static readonly ComponentResourceKey SettingIconBrush = CreateInstance();
    }

    public static class PropertiesView
    {
        public static readonly ComponentResourceKey UndefinedValueBrush = CreateInstance();

        public static readonly ComponentResourceKey ColorPickerSecondUndefinedValueBrush = CreateInstance();

        public static readonly ComponentResourceKey AddVertexIconBrush = CreateInstance();

        public static readonly ComponentResourceKey RemoveVertexIconBrush = CreateInstance();
    }

    public static class MessagesOutputView
    {
        public static readonly ComponentResourceKey ToggleButtonCheckedColor = CreateInstance();

        public static readonly ComponentResourceKey ErrorIconBrush = CreateInstance();
        
        public static readonly ComponentResourceKey WarningIconBrush = CreateInstance();

        public static readonly ComponentResourceKey InformationIconBrush = CreateInstance();

        public static readonly ComponentResourceKey BackgroundIconBrush = CreateInstance();

        public static readonly ComponentResourceKey ClearFilterIconBrush = CreateInstance();

        public static readonly ComponentResourceKey SearchIconBrush = CreateInstance();
    }

    public static class SchemeToolView
    {
        public static readonly ComponentResourceKey Background = CreateInstance();

        public static readonly ComponentResourceKey MouseOverBackground = CreateInstance();

        public static readonly ComponentResourceKey ActiveBackground = CreateInstance();
    }

    public static class SchemeView
    {
        public static readonly ComponentResourceKey StartBackgroundColor = CreateInstance();

        public static readonly ComponentResourceKey EndBackgroundColor = CreateInstance();

        public static readonly ComponentResourceKey GridBackgroundColor = CreateInstance();
        
        public static readonly ComponentResourceKey GridBoldLineColor = CreateInstance();
        
        public static readonly ComponentResourceKey GridLineColor = CreateInstance();

        public static readonly ComponentResourceKey NodeFillColor = CreateInstance();

        public static readonly ComponentResourceKey NodeStrokeColor = CreateInstance();

        public static readonly ComponentResourceKey RectangleSelectionNormalColor = CreateInstance();
        
        public static readonly ComponentResourceKey RectangleSelectionSecantColor = CreateInstance();

        public static readonly ComponentResourceKey ToolIconBrush = CreateInstance();

        public static readonly ComponentResourceKey ToolsPanelBackground = CreateInstance();
    }

    private static ComponentResourceKey CreateInstance() =>
        new(typeof(BrushesKeys), Guid.NewGuid());
}