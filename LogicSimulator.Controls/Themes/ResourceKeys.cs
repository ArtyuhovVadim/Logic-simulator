using System.Runtime.CompilerServices;
using System.Windows;

namespace LogicSimulator.Controls.Themes;

public static class ResourceKeys
{
    #region Window style resource keys

    public static readonly ComponentResourceKey WindowBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowBorderFocusedBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowCloseButtonBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowCloseButtonMouseOverBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowButtonBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowButtonMouseOverBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowButtonContentBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowTitleBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowContentContainerBackgroundBrush = CreateInstance();

    #endregion

    #region Button style resource keys

    public static readonly ComponentResourceKey ButtonBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey ButtonBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey ButtonForegroundBrush = CreateInstance();

    public static readonly ComponentResourceKey ButtonMouseOverBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey ButtonPressedBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey ButtonPressedBorderBrush = CreateInstance();

    #endregion

    #region TextBox style resource keys

    public static readonly ComponentResourceKey TextBoxBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey TextBoxForegroundBrush = CreateInstance();

    public static readonly ComponentResourceKey TextBoxBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey TextBoxFocusedBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey TextBoxSelectionBrush = CreateInstance();

    #endregion

    #region Menu style resource keys

    public static readonly ComponentResourceKey MenuBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuForegroundBrush = CreateInstance();

    #endregion

    #region MenuItem style resource keys

    public static readonly ComponentResourceKey MenuItemBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemForegroundBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemDisabledForegroundBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemSelectedBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemSelectedBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemHighlightBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemHighlightBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemHighlightDisabledBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey MenuItemHighlightDisabledBorderBrush = CreateInstance();

    #endregion

    #region CheckBox style resource keys

    public static readonly ComponentResourceKey CheckBoxForegroundBrush = CreateInstance();

    public static readonly ComponentResourceKey CheckBoxBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey CheckBoxBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey CheckBoxGlyphBackgroundBrush = CreateInstance();

    public static readonly ComponentResourceKey CheckBoxMouseOverBackground = CreateInstance();

    public static readonly ComponentResourceKey CheckBoxFocusedBorderBrush = CreateInstance();

    #endregion

    #region UserDialogWindow style resource keys

    public static readonly ComponentResourceKey UserDialogWindowBackground = CreateInstance();

	#endregion

	#region Accent Keys
	/// <summary>
	/// Accent Color Key - This Color key is used to accent elements in the UI
	/// (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
	/// </summary>
	public static readonly ComponentResourceKey ControlAccentColorKey = new(typeof(ResourceKeys), "ControlAccentColorKey");

	/// <summary>
	/// Accent Brush Key - This Brush key is used to accent elements in the UI
	/// (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
	/// </summary>
	public static readonly ComponentResourceKey ControlAccentBrushKey = new(typeof(ResourceKeys), "ControlAccentBrushKey");
	#endregion Accent Keys

	#region Brush Keys
	// General
	public static readonly ComponentResourceKey Background = new(typeof(ResourceKeys), "Background");
	public static readonly ComponentResourceKey PanelBorderBrush = new(typeof(ResourceKeys), "PanelBorderBrush");
	public static readonly ComponentResourceKey TabBackground = new(typeof(ResourceKeys), "TabBackground");

	// Auto Hide : Tab
	public static readonly ComponentResourceKey AutoHideTabDefaultBackground = new(typeof(ResourceKeys), "AutoHideTabDefaultBackground");
	public static readonly ComponentResourceKey AutoHideTabDefaultBorder = new(typeof(ResourceKeys), "AutoHideTabDefaultBorder");
	public static readonly ComponentResourceKey AutoHideTabDefaultText = new(typeof(ResourceKeys), "AutoHideTabDefaultText");

	// Mouse Over Auto Hide Button for (collapsed) Auto Hidden Elements
	public static readonly ComponentResourceKey AutoHideTabHoveredBackground = new(typeof(ResourceKeys), "AutoHideTabHoveredBackground");
	// AccentColor
	public static readonly ComponentResourceKey AutoHideTabHoveredBorder = new(typeof(ResourceKeys), "AutoHideTabHoveredBorder");
	// AccentColor
	public static readonly ComponentResourceKey AutoHideTabHoveredText = new(typeof(ResourceKeys), "AutoHideTabHoveredText");

	// Document Well : Overflow Button
	public static readonly ComponentResourceKey DocumentWellOverflowButtonDefaultGlyph = new(typeof(ResourceKeys), "DocumentWellOverflowButtonDefaultGlyph");
	public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredBackground = new(typeof(ResourceKeys), "DocumentWellOverflowButtonHoveredBackground");
	public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredBorder = new(typeof(ResourceKeys), "DocumentWellOverflowButtonHoveredBorder");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellOverflowButtonHoveredGlyph");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedBackground = new(typeof(ResourceKeys), "DocumentWellOverflowButtonPressedBackground");
	public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedBorder = new(typeof(ResourceKeys), "DocumentWellOverflowButtonPressedBorder");
	public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedGlyph = new(typeof(ResourceKeys), "DocumentWellOverflowButtonPressedGlyph");

	// Document Well : Tab
	// Selected Document Highlight Header Top color (AccentColor)
	public static readonly ComponentResourceKey DocumentWellTabSelectedActiveBackground = new(typeof(ResourceKeys), "DocumentWellTabSelectedActiveBackground");

	public static readonly ComponentResourceKey DocumentWellTabSelectedActiveText = new(typeof(ResourceKeys), "DocumentWellTabSelectedActiveText");
	public static readonly ComponentResourceKey DocumentWellTabSelectedInactiveBackground = new(typeof(ResourceKeys), "DocumentWellTabSelectedInactiveBackground");
	public static readonly ComponentResourceKey DocumentWellTabSelectedInactiveText = new(typeof(ResourceKeys), "DocumentWellTabSelectedInactiveText");
	public static readonly ComponentResourceKey DocumentWellTabUnselectedBackground = new(typeof(ResourceKeys), "DocumentWellTabUnselectedBackground");
	public static readonly ComponentResourceKey DocumentWellTabUnselectedText = new(typeof(ResourceKeys), "DocumentWellTabUnselectedText");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabUnselectedHoveredBackground = new(typeof(ResourceKeys), "DocumentWellTabUnselectedHoveredBackground");
	public static readonly ComponentResourceKey DocumentWellTabUnselectedHoveredText = new(typeof(ResourceKeys), "DocumentWellTabUnselectedHoveredText");

	// Document Well : Tab : Button
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActiveGlyph");

	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActiveHoveredBackground");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActiveHoveredBorder");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActiveHoveredGlyph");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActivePressedBackground");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActivePressedBorder");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActivePressedGlyph");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactiveGlyph");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactiveHoveredBackground");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactiveHoveredBorder");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactiveHoveredGlyph");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactivePressedBackground");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactivePressedBorder");
	public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactivePressedGlyph");
	public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredGlyph");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBackground");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBorder");
	public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonHoveredGlyph");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonPressedBackground");
	// AccentColor
	public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonPressedBorder");
	public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonPressedGlyph");

	// Tool Window : Caption
	// Background of selected toolwindow (AccentColor)
	public static readonly ComponentResourceKey ToolWindowCaptionActiveBackground = new(typeof(ResourceKeys), "ToolWindowCaptionActiveBackground");
	public static readonly ComponentResourceKey ToolWindowCaptionActiveGrip = new(typeof(ResourceKeys), "ToolWindowCaptionActiveGrip");
	public static readonly ComponentResourceKey ToolWindowCaptionActiveText = new(typeof(ResourceKeys), "ToolWindowCaptionActiveText");
	public static readonly ComponentResourceKey ToolWindowCaptionInactiveBackground = new(typeof(ResourceKeys), "ToolWindowCaptionInactiveBackground");
	public static readonly ComponentResourceKey ToolWindowCaptionInactiveGrip = new(typeof(ResourceKeys), "ToolWindowCaptionInactiveGrip");
	public static readonly ComponentResourceKey ToolWindowCaptionInactiveText = new(typeof(ResourceKeys), "ToolWindowCaptionInactiveText");

	// Tool Window : Caption : Button
	public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActiveGlyph");
	// AccentColor
	public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredBackground = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActiveHoveredBackground");
	// AccentColor
	public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredBorder = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActiveHoveredBorder");
	public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActiveHoveredGlyph");
	// AccentColor
	public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedBackground = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActivePressedBackground");
	// AccentColor
	public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedBorder = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActivePressedBorder");

	public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActivePressedGlyph");
	public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactiveGlyph");

	// AccentColor
	public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredBackground = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactiveHoveredBackground");
	public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredBorder = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactiveHoveredBorder");
	public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactiveHoveredGlyph");

	public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedBackground = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactivePressedBackground");
	public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedBorder = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactivePressedBorder");
	public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactivePressedGlyph");

	// Tool Window : Tab
	public static readonly ComponentResourceKey ToolWindowTabSelectedActiveBackground = new(typeof(ResourceKeys), "ToolWindowTabSelectedActiveBackground");
	// AccentColor
	public static readonly ComponentResourceKey ToolWindowTabSelectedActiveText = new(typeof(ResourceKeys), "ToolWindowTabSelectedActiveText");
	public static readonly ComponentResourceKey ToolWindowTabSelectedInactiveBackground = new(typeof(ResourceKeys), "ToolWindowTabSelectedInactiveBackground");
	// AccentColor
	public static readonly ComponentResourceKey ToolWindowTabSelectedInactiveText = new(typeof(ResourceKeys), "ToolWindowTabSelectedInactiveText");
	public static readonly ComponentResourceKey ToolWindowTabUnselectedBackground = new(typeof(ResourceKeys), "ToolWindowTabUnselectedBackground");
	public static readonly ComponentResourceKey ToolWindowTabUnselectedText = new(typeof(ResourceKeys), "ToolWindowTabUnselectedText");
	public static readonly ComponentResourceKey ToolWindowTabUnselectedHoveredBackground = new(typeof(ResourceKeys), "ToolWindowTabUnselectedHoveredBackground");
	// AccentColor
	public static readonly ComponentResourceKey ToolWindowTabUnselectedHoveredText = new(typeof(ResourceKeys), "ToolWindowTabUnselectedHoveredText");

	// Floating Document Window
	public static readonly ComponentResourceKey FloatingDocumentWindowBackground = new(typeof(ResourceKeys), "FloatingDocumentWindowBackground");
	public static readonly ComponentResourceKey FloatingDocumentWindowBorder = new(typeof(ResourceKeys), "FloatingDocumentWindowBorder");

	// Floating Tool Window
	public static readonly ComponentResourceKey FloatingToolWindowBackground = new(typeof(ResourceKeys), "FloatingToolWindowBackground");
	public static readonly ComponentResourceKey FloatingToolWindowBorder = new(typeof(ResourceKeys), "FloatingToolWindowBorder");

	// Navigator Window
	public static readonly ComponentResourceKey NavigatorWindowBackground = new(typeof(ResourceKeys), "NavigatorWindowBackground");
	public static readonly ComponentResourceKey NavigatorWindowForeground = new(typeof(ResourceKeys), "NavigatorWindowForeground");

	// Background of selected text in Navigator Window (AccentColor)
	public static readonly ComponentResourceKey NavigatorWindowSelectedBackground = new(typeof(ResourceKeys), "NavigatorWindowSelectedBackground");
	public static readonly ComponentResourceKey NavigatorWindowSelectedText = new(typeof(ResourceKeys), "NavigatorWindowSelectedText");

	#region DockingBrushKeys
	// Defines the height and width of the docking indicator buttons that are shown when
	// documents or tool windows are dragged
	public static readonly ComponentResourceKey DockingButtonWidthKey = new(typeof(ResourceKeys), "DockingButtonWidthKey");
	public static readonly ComponentResourceKey DockingButtonHeightKey = new(typeof(ResourceKeys), "DockingButtonHeightKey");

	public static readonly ComponentResourceKey DockingButtonBackgroundBrushKey = new(typeof(ResourceKeys), "DockingButtonBackgroundBrushKey");
	public static readonly ComponentResourceKey DockingButtonForegroundBrushKey = new(typeof(ResourceKeys), "DockingButtonForegroundBrushKey");
	public static readonly ComponentResourceKey DockingButtonForegroundArrowBrushKey = new(typeof(ResourceKeys), "DockingButtonForegroundArrowBrushKey");

	public static readonly ComponentResourceKey DockingButtonStarBorderBrushKey = new(typeof(ResourceKeys), "DockingButtonStarBorderBrushKey");
	public static readonly ComponentResourceKey DockingButtonStarBackgroundBrushKey = new(typeof(ResourceKeys), "DockingButtonStarBackgroundBrushKey");

	// Preview Box is the highlighted rectangle that shows when a drop area in a window is indicated
	public static readonly ComponentResourceKey PreviewBoxBorderBrushKey = new(typeof(ResourceKeys), "PreviewBoxBorderBrushKey");
	public static readonly ComponentResourceKey PreviewBoxBackgroundBrushKey = new(typeof(ResourceKeys), "PreviewBoxBackgroundBrushKey");
    #endregion DockingBrushKeys
    #endregion Brush Keys

    #region MenuKeys

    /// <summary>
    /// Gets the Brush key for the normal Menu separator border color.
    /// </summary>
    public static readonly ComponentResourceKey MenuSeparatorBorderBrushKey = new(typeof(ResourceKeys), "MenuSeparatorBorderBrushKey");

    /// <summary>
    /// Gets the Brush key for the normal background of a Sub-Menu-Item.
    /// </summary>
    public static readonly ComponentResourceKey SubmenuItemBackgroundKey = new(typeof(ResourceKeys), "SubmenuItemBackgroundKey");

    /// <summary>
    /// Gets the Brush key for highlighting the background of a Menu-Item on mouse over.
    /// </summary>
    public static readonly ComponentResourceKey MenuItemHighlightedBackgroundKey = new(typeof(ResourceKeys), "MenuItemHighlightedBackgroundKey");

    /// <summary>
    /// Gets the Brush key for highlighting the background of a Menu-Item on mouse over.
    /// </summary>
    public static readonly ComponentResourceKey SubmenuItemBackgroundHighlightedKey = new(typeof(ResourceKeys), "SubmenuItemBackgroundHighlightedKey");

    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    /// <summary>
    /// Gets the background Brush key for a Menu Repeat button in IsPressed state.
    /// (see context menu below)
    /// </summary>
    public static readonly ComponentResourceKey FocusScrollButtonBrushKey = new(typeof(ResourceKeys), "FocusScrollButtonBrushKey");

    /// <summary>
    /// Gets the background Brush key for a Context-Menu Repeat button in IsPressed state.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey ScrollButtonBrushKey = new(typeof(ResourceKeys), "ScrollButtonBrushKey");

    /// <summary>
    /// Gets the background Brush key of a Checkmark in a menu or context menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey CheckMarkBackgroundBrushKey = new(typeof(ResourceKeys), "CheckMarkBackgroundBrushKey");

    /// <summary>
    /// Gets the border Brush key of a Checkmark in a menu or context menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey CheckMarkBorderBrushKey = new(typeof(ResourceKeys), "CheckMarkBorderBrushKey");

    /// <summary>
    /// Gets the foreground Brush key of a Checkmark in a menu or context menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey CheckMarkForegroundBrushKey = new(typeof(ResourceKeys), "CheckMarkForegroundBrushKey");

    /// <summary>
    /// Gets the background Brush key of a disabled sub-menu-item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey DisabledSubMenuItemBackgroundBrushKey = new(typeof(ResourceKeys), "DisabledSubMenuItemBackgroundBrushKey");

    /// <summary>
    /// Gets the border Brush key of a disabled sub-menu-item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey DisabledSubMenuItemBorderBrushKey = new(typeof(ResourceKeys), "DisabledSubMenuItemBorderBrushKey");

    /// <summary>
    /// Gets the border Brush key of a disabled sub-menu-item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey MenuBorderBrushKey = new(typeof(ResourceKeys), "MenuBorderBrushKey");

    /// <summary>
    /// Gets the normal background Brush key of a menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey MenuBackgroundKey = new(typeof(ResourceKeys), "MenuBackgroundKey");

    /// <summary>
    /// Gets the normal background Brush key of the top level item in a menu (Files, Edit, ...).
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey TopLevelHeaderMenuBackgroundKey = new(typeof(ResourceKeys), "TopLevelHeaderMenuBackgroundKey");

    /// <summary>
    /// Gets the normal text or foreground Brush key of a menu item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey TextBrushKey = new(typeof(ResourceKeys), "TextBrushKey");

    /// <summary>
    /// Gets the normal background Brush key of a selected menu item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey ItemBackgroundSelectedKey = new(typeof(ResourceKeys), "ItemBackgroundSelectedKey");

    /// <summary>
    /// Gets the text or foreground Brush key of a menu item in disabled state.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey ItemTextDisabledKey = new(typeof(ResourceKeys), "ItemTextDisabledKey");

    /// <summary>
    /// Gets the normal background Brush key of a menu item.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey NormalBackgroundBrushKey = new(typeof(ResourceKeys), "NormalBackgroundBrushKey");

    /// <summary>
    /// Gets the background Brush key of a menu item in mouse over state.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey ItemBackgroundHoverKey = new(typeof(ResourceKeys), "ItemBackgroundHoverKey");

    /// <summary>
    /// Gets the Brush key that is applied to draw a drop shadow (if any) below a menu.
    /// (see menu above)
    /// </summary>
    public static readonly ComponentResourceKey DropShadowEffectKey = new(typeof(ResourceKeys), "DropShadowEffectKey");

    #endregion

    private static ComponentResourceKey CreateInstance([CallerMemberName] string fieldName = null)
    {
        return new ComponentResourceKey(typeof(ResourceKeys), fieldName!);
    }
}