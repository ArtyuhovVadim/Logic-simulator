using System.Runtime.CompilerServices;
using System.Windows;

namespace LogicSimulator.Controls.Themes;

public static class ResourceKeys
{
    #region Window style resource keys

    public static readonly ComponentResourceKey WindowBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey WindowBorderBrushFocused = CreateInstance();

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

    public static readonly ComponentResourceKey TextBoxBackground = CreateInstance();

    public static readonly ComponentResourceKey TextBoxForeground = CreateInstance();

    public static readonly ComponentResourceKey TextBoxBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey TextBoxMouseOverBorderBrush = CreateInstance();

    public static readonly ComponentResourceKey TextBoxSelectionBrush = CreateInstance();

    #endregion

    #region Menu style resource keys

    public static readonly ComponentResourceKey MenuBackground = CreateInstance();

    public static readonly ComponentResourceKey MenuForeground = CreateInstance();

    #endregion

    #region MenuItem style resource keys

    public static readonly ComponentResourceKey MenuStaticBackground = CreateInstance();

    public static readonly ComponentResourceKey MenuStaticBorder = CreateInstance();

    public static readonly ComponentResourceKey MenuStaticForeground = CreateInstance();

    public static readonly ComponentResourceKey MenuDisabledForeground = CreateInstance();

    public static readonly ComponentResourceKey MenuItemSelectedBackground = CreateInstance();

    public static readonly ComponentResourceKey MenuItemSelectedBorder = CreateInstance();

    public static readonly ComponentResourceKey MenuItemHighlightBackground = CreateInstance();

    public static readonly ComponentResourceKey MenuItemHighlightBorder = CreateInstance();

    public static readonly ComponentResourceKey MenuItemHighlightDisabledBackground = CreateInstance();

    public static readonly ComponentResourceKey MenuItemHighlightDisabledBorder = CreateInstance();

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

    private static ComponentResourceKey CreateInstance([CallerMemberName] string fieldName = null)
    {
        return new ComponentResourceKey(typeof(ResourceKeys), fieldName!);
    }
}