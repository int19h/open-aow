// Translated from AoWInfoBanner.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class InfoBanner : DataModule
{
    public InfoBanner(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWLabel HeaderLbl;
    public AOWMemo InfoMemo;
    public AOWMenu BannerPopup;
    public AOWVScrollBar InfoMemoSB;

    // AoWInfoBanner.pas:35
    public void ShowInfo(string Header, StringCollection Description)
    {
        HeaderLbl.Text = Header;
        var text = Description.Text;
        InfoMemo.Items.Text = text;
    }

    // AoWInfoBanner.pas:41
    public void BannerPopupMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (Button == MouseButton.Right)
        {
            BannerPopup.Visible = false;
        }
    }

    internal static void SetInfoBanner(InfoBanner Banner, string Caption, StringCollection Description)
    {
        Banner.HeaderLbl.Text = Caption;
        var text = Description.Text;
        Banner.InfoMemo.Items.Text = text;
    }

}
