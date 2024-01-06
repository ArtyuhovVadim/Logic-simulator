using System.Runtime.CompilerServices;
using System.Windows;

namespace LogicSimulator.Controls.AvalonDock.Themes;

public static class ResourceKeys
{
    public static readonly ComponentResourceKey Background = CreateInstance();
    public static readonly ComponentResourceKey PanelBorderBrush = CreateInstance();
    public static readonly ComponentResourceKey TabBackground = CreateInstance();
    public static readonly ComponentResourceKey MenuItemIconBrush = CreateInstance();

    public static readonly ComponentResourceKey ToolButtonBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolButtonForeground = CreateInstance();
    public static readonly ComponentResourceKey ToolButtonHoveredBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolButtonHoveredForeground = CreateInstance();
    public static readonly ComponentResourceKey ToolButtonCheckedBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolButtonCheckedForeground = CreateInstance();
    public static readonly ComponentResourceKey ToolButtonPressedBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolButtonPressedForeground = CreateInstance();

    public static readonly ComponentResourceKey ButtonBackground = CreateInstance();
    public static readonly ComponentResourceKey ButtonForeground = CreateInstance();
    public static readonly ComponentResourceKey ButtonHoveredBackground = CreateInstance();
    public static readonly ComponentResourceKey ButtonHoveredForeground = CreateInstance();
    public static readonly ComponentResourceKey ButtonPressedBackground = CreateInstance();
    public static readonly ComponentResourceKey ButtonPressedForeground = CreateInstance();

    public static readonly ComponentResourceKey AutoHideTabDefaultBackground = CreateInstance();
    public static readonly ComponentResourceKey AutoHideTabDefaultBorder = CreateInstance();
    public static readonly ComponentResourceKey AutoHideTabHoveredBackground = CreateInstance();

    public static readonly ComponentResourceKey DocumentTabSelectedBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentTabActiveBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentTabUnselectedBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentTabUnselectedHoveredBackground = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowCaptionActiveBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionInactiveBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabSelectedBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabUnselectedBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabUnselectedHoveredBackground = CreateInstance();

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