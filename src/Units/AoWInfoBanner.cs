// Translated from AoWInfoBanner.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class TInfoBanner : TDataModule
{
    public TInfoBanner(TComponent owner) : base(owner)
    { }

    public TAOWLabel HeaderLbl;
    public TAOWMemo InfoMemo;
    public TAOWMenu BannerPopup;
    public TAOWVScrollBar InfoMemoSB;

    // AoWInfoBanner.pas:35
    public void ShowInfo(string Header, TStrings Description)
    {
        HeaderLbl.Text = Header;
        var text = Description.Text;
        InfoMemo.Items.Text = text;
    }

    // AoWInfoBanner.pas:41
    public void BannerPopupMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Button == TMouseButton.mbRight)
        {
            BannerPopup.Visible = false;
        }
    }

    internal static void SetInfoBanner(TInfoBanner Banner, string Caption, TStrings Description)
    {
        Banner.HeaderLbl.Text = Caption;
        var text = Description.Text;
        Banner.InfoMemo.Items.Text = text;
    }

}
