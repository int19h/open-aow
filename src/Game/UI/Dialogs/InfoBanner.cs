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

    // AoWInfoBanner.pas:35
    public void ShowInfo(string header, StringCollection description)
    {
        HeaderLbl.Text = header;
        var text = description.Text;
        InfoMemo.Items.Text = text;
    }

    // AoWInfoBanner.pas:41
    public void BannerPopupMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right)
        {
            BannerPopup.Visible = false;
        }
    }

    internal static void SetInfoBanner(InfoBanner banner, string caption, StringCollection description)
    {
        banner.HeaderLbl.Text = caption;
        var text = description.Text;
        banner.InfoMemo.Items.Text = text;
    }

}
