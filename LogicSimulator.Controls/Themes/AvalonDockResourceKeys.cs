using System.Runtime.CompilerServices;
using System.Windows;

namespace LogicSimulator.Controls.Themes;

public static class AvalonDockResourceKeys
{
    #region Accent Keys

    /// <summary>
    /// Accent Color Key - This Color key is used to accent elements in the UI
    /// (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
    /// </summary>
    public static readonly ComponentResourceKey ControlAccentColorKey = CreateInstance();

    /// <summary>
    /// Accent Brush Key - This Brush key is used to accent elements in the UI
    /// (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
    /// </summary>
    public static readonly ComponentResourceKey ControlAccentBrushKey = CreateInstance();

    #endregion Accent Keys

    #region Brush Keys

    // General
    public static readonly ComponentResourceKey Background = CreateInstance();
    public static readonly ComponentResourceKey PanelBorderBrush = CreateInstance();
    public static readonly ComponentResourceKey TabBackground = CreateInstance();

    // Auto Hide : Tab
    public static readonly ComponentResourceKey AutoHideTabDefaultBackground = CreateInstance();
    public static readonly ComponentResourceKey AutoHideTabDefaultBorder = CreateInstance();
    public static readonly ComponentResourceKey AutoHideTabDefaultText = CreateInstance();

    // Mouse Over Auto Hide Button for (collapsed) Auto Hidden Elements
    public static readonly ComponentResourceKey AutoHideTabHoveredBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey AutoHideTabHoveredBorder = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey AutoHideTabHoveredText = CreateInstance();

    // Document Well : Overflow Button
    public static readonly ComponentResourceKey DocumentWellOverflowButtonDefaultGlyph = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredBorder = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredGlyph = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedBorder = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedGlyph = CreateInstance();

    // Document Well : Tab
    // Selected Document Highlight Header Top color (AccentColor)
    public static readonly ComponentResourceKey DocumentWellTabSelectedActiveBackground = CreateInstance();

    public static readonly ComponentResourceKey DocumentWellTabSelectedActiveText = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabSelectedInactiveBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabSelectedInactiveText = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabUnselectedBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabUnselectedText = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabUnselectedHoveredBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabUnselectedHoveredText = CreateInstance();

    // Document Well : Tab : Button
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveGlyph = CreateInstance();

    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredBorder = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredGlyph = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedBorder = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedGlyph = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveGlyph = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredBorder = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredGlyph = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedBackground = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedBorder = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedGlyph = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredGlyph = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBorder = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredGlyph = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedBorder = CreateInstance();
    public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedGlyph = CreateInstance();

    // Tool Window : Caption
    // Background of selected toolwindow (AccentColor)
    public static readonly ComponentResourceKey ToolWindowCaptionActiveBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionActiveGrip = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionActiveText = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionInactiveBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionInactiveGrip = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionInactiveText = CreateInstance();

    // Tool Window : Caption : Button
    public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveGlyph = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredBorder = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredGlyph = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedBorder = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedGlyph = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveGlyph = CreateInstance();

    // AccentColor
    public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredBorder = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredGlyph = CreateInstance();

    public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedBorder = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedGlyph = CreateInstance();

    // Tool Window : Tab
    public static readonly ComponentResourceKey ToolWindowTabSelectedActiveBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey ToolWindowTabSelectedActiveText = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabSelectedInactiveBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey ToolWindowTabSelectedInactiveText = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabUnselectedBackground = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabUnselectedText = CreateInstance();
    public static readonly ComponentResourceKey ToolWindowTabUnselectedHoveredBackground = CreateInstance();
    // AccentColor
    public static readonly ComponentResourceKey ToolWindowTabUnselectedHoveredText = CreateInstance();

    // Floating Document Window
    public static readonly ComponentResourceKey FloatingDocumentWindowBackground = CreateInstance();
    public static readonly ComponentResourceKey FloatingDocumentWindowBorder = CreateInstance();

    // Floating Tool Window
    public static readonly ComponentResourceKey FloatingToolWindowBackground = CreateInstance();
    public static readonly ComponentResourceKey FloatingToolWindowBorder = CreateInstance();

    // Navigator Window
    public static readonly ComponentResourceKey NavigatorWindowBackground = CreateInstance();
    public static readonly ComponentResourceKey NavigatorWindowForeground = CreateInstance();

    // Background of selected text in Navigator Window (AccentColor)
    public static readonly ComponentResourceKey NavigatorWindowSelectedBackground = CreateInstance();
    public static readonly ComponentResourceKey NavigatorWindowSelectedText = CreateInstance();

    #region DockingBrushKeys
    // Defines the height and width of the docking indicator buttons that are shown when
    // documents or tool windows are dragged
    public static readonly ComponentResourceKey DockingButtonWidthKey = CreateInstance();
    public static readonly ComponentResourceKey DockingButtonHeightKey = CreateInstance();

    public static readonly ComponentResourceKey DockingButtonBackgroundBrushKey = CreateInstance();
    public static readonly ComponentResourceKey DockingButtonForegroundBrushKey = CreateInstance();
    public static readonly ComponentResourceKey DockingButtonForegroundArrowBrushKey = CreateInstance();

    public static readonly ComponentResourceKey DockingButtonStarBorderBrushKey = CreateInstance();
    public static readonly ComponentResourceKey DockingButtonStarBackgroundBrushKey = CreateInstance();

    // Preview Box is the highlighted rectangle that shows when a drop area in a window is indicated
    public static readonly ComponentResourceKey PreviewBoxBorderBrushKey = CreateInstance();
    public static readonly ComponentResourceKey PreviewBoxBackgroundBrushKey = CreateInstance();
    #endregion DockingBrushKeys

    #endregion Brush Keys

    #region MenuKeys

    /// <summary>
    /// Gets the Brush key for the normal Menu separator border color.
    /// </summary>
    public static readonly ComponentResourceKey MenuSeparatorBorderBrushKey = CreateInstance();

    /// <summary>
    /// Gets the Brush key for the normal background of a Sub-Menu-Item.
    /// </summary>
    public static readonly ComponentResourceKey SubmenuItemBackgroundKey = CreateInstance();

    /// <summary>
    /// Gets the Brush key for highlighting the background of a Menu-Item on mouse over.
    /// </summary>
    public static readonly ComponentResourceKey MenuItemHighlightedBackgroundKey = CreateInstance();

    /// <summary>
    /// Gets the Brush key for highlighting the background of a Menu-Item on mouse over.
    /// </summary>
    public static readonly ComponentResourceKey SubmenuItemBackgroundHighlightedKey = CreateInstance();

    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    /// <summary>
    /// Gets the background Brush key for a Menu Repeat button in IsPressed state.
    /// (see context menu below)
    /// </summary>
    public static readonly ComponentResourceKey FocusScrollButtonBrushKey = CreateInstance();

    /// <summary>
    /// Gets the background Brush key for a Context-Menu Repeat button in IsPressed state.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey ScrollButtonBrushKey = CreateInstance();

    /// <summary>
    /// Gets the background Brush key of a Checkmark in a menu or context menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey CheckMarkBackgroundBrushKey = CreateInstance();

    /// <summary>
    /// Gets the border Brush key of a Checkmark in a menu or context menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey CheckMarkBorderBrushKey = CreateInstance();

    /// <summary>
    /// Gets the foreground Brush key of a Checkmark in a menu or context menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey CheckMarkForegroundBrushKey = CreateInstance();

    /// <summary>
    /// Gets the background Brush key of a disabled sub-menu-item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey DisabledSubMenuItemBackgroundBrushKey = CreateInstance();

    /// <summary>
    /// Gets the border Brush key of a disabled sub-menu-item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey DisabledSubMenuItemBorderBrushKey = CreateInstance();

    /// <summary>
    /// Gets the border Brush key of a disabled sub-menu-item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey MenuBorderBrushKey = CreateInstance();

    /// <summary>
    /// Gets the normal background Brush key of a menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey MenuBackgroundKey = CreateInstance();

    /// <summary>
    /// Gets the normal background Brush key of the top level item in a menu (Files, Edit, ...).
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey TopLevelHeaderMenuBackgroundKey = CreateInstance();

    /// <summary>
    /// Gets the normal text or foreground Brush key of a menu item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey TextBrushKey = CreateInstance();

    /// <summary>
    /// Gets the normal background Brush key of a selected menu item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey ItemBackgroundSelectedKey = CreateInstance();

    /// <summary>
    /// Gets the text or foreground Brush key of a menu item in disabled state.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey ItemTextDisabledKey = CreateInstance();

    /// <summary>
    /// Gets the normal background Brush key of a menu item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey NormalBackgroundBrushKey = CreateInstance();

    /// <summary>
    /// Gets the background Brush key of a menu item in mouse over state.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey ItemBackgroundHoverKey = CreateInstance();

    /// <summary>
    /// Gets the Brush key that is applied to draw a drop shadow (if any) below a menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey DropShadowEffectKey = CreateInstance();

    #endregion

    private static ComponentResourceKey CreateInstance([CallerMemberName] string? fieldName = null)
    {
        return new ComponentResourceKey(typeof(ResourceKeys), fieldName!);
    }
}