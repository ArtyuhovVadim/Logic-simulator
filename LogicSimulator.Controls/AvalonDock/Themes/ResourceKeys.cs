using System.Runtime.CompilerServices;
using System.Windows;

namespace LogicSimulator.Controls.AvalonDock.Themes;

public static class ResourceKeys
{
    public static readonly ComponentResourceKey ControlAccentColorKey = CreateInstance();

    public static readonly ComponentResourceKey Background = CreateInstance();
    public static readonly ComponentResourceKey PanelBorderBrush = CreateInstance();
    public static readonly ComponentResourceKey TabBackground = CreateInstance();

    public static readonly ComponentResourceKey AutoHideTabDefaultBackground = CreateInstance();
    public static readonly ComponentResourceKey AutoHideTabDefaultBorder = CreateInstance();
    public static readonly ComponentResourceKey AutoHideTabDefaultText = CreateInstance();

    public static readonly ComponentResourceKey AutoHideTabHoveredBackground = CreateInstance();

    public static readonly ComponentResourceKey DocumentWellTabSelectedActiveBackground = CreateInstance();

    public static readonly ComponentResourceKey DocumentWellTabSelectedActiveText = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabSelectedInactiveBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabSelectedInactiveText = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabUnselectedBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabUnselectedText = CreateInstance();

    public static readonly ComponentResourceKey DocumentWellTabUnselectedHoveredBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabUnselectedHoveredText = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowCaptionActiveBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionActiveText = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionInactiveBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionInactiveText = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveGlyph = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowTabSelectedActiveBackground = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowTabSelectedActiveText = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabSelectedInactiveBackground = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowTabSelectedInactiveText = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabUnselectedBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabUnselectedText = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabUnselectedHoveredBackground = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowTabUnselectedHoveredText = CreateInstance();

    public static readonly ComponentResourceKey FloatingDocumentWindowBackground = CreateInstance();
    public static readonly ComponentResourceKey FloatingDocumentWindowBorder = CreateInstance();
    public static readonly ComponentResourceKey FloatingDocumentActiveWindowBorder = CreateInstance();

    public static readonly ComponentResourceKey FloatingToolWindowBackground = CreateInstance();
    public static readonly ComponentResourceKey FloatingToolWindowBorder = CreateInstance();

    public static readonly ComponentResourceKey DockingButtonWidthKey = CreateInstance();
    public static readonly ComponentResourceKey DockingButtonHeightKey = CreateInstance();

    public static readonly ComponentResourceKey DockingButtonForegroundBrushKey = CreateInstance();
    public static readonly ComponentResourceKey DockingButtonForegroundArrowBrushKey = CreateInstance();

    public static readonly ComponentResourceKey DockingButtonStarBorderBrushKey = CreateInstance();
    public static readonly ComponentResourceKey DockingButtonStarBackgroundBrushKey = CreateInstance();

    public static readonly ComponentResourceKey PreviewBoxBorderBrushKey = CreateInstance();
    public static readonly ComponentResourceKey PreviewBoxBackgroundBrushKey = CreateInstance();

    private static ComponentResourceKey CreateInstance([CallerMemberName] string? name = null) => new(typeof(ResourceKeys), name!);
}