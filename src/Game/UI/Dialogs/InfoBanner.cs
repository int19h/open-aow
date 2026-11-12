using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class InfoBanner : UIModule
{
    public InfoBanner(UIContext ui) : base(ui)
    { }

    public Label HeaderLabel;
    public Memo InfoMemo;
    public Menu BannerMenu;

    // AoWInfoBanner.pas:35
    public void ShowInfo(string header, StringCollection description)
    {
        HeaderLabel.Text = header;
        var text = description.Text;
        InfoMemo.Items.Text = text;
    }

    // AoWInfoBanner.pas:41
    public void BannerPopupMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right)
        {
            BannerMenu.Visible = false;
        }
    }

    internal static void SetInfoBanner(InfoBanner banner, string caption, StringCollection description)
    {
        banner.HeaderLabel.Text = caption;
        var text = description.Text;
        banner.InfoMemo.Items.Text = text;
    }

}
