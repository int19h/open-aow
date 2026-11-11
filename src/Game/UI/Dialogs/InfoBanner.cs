using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class InfoBanner : DataModule
{
    public InfoBanner(Component owner) : base(owner)
    { }

    public Label HeaderLbl;
    public Memo InfoMemo;
    public Menu BannerPopup;
    public VerticalScrollBar InfoMemoSB;

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
