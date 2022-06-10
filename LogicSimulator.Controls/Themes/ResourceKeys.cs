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

    private static ComponentResourceKey CreateInstance([CallerMemberName] string fieldName = null)
    {
        return new ComponentResourceKey(typeof(ResourceKeys), fieldName!);
    }
}