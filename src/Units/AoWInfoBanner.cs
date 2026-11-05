// Translated from AoWInfoBanner.pas.
using Aow.Rtl;
using Aow.Units.AOWMenu;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWVScroll;

namespace Aow.Units.AoWInfoBanner
{
    public partial class TInfoBanner : TDataModule
    {
        public TInfoBanner(TComponent owner) : base(owner)
        { }

        public TAOWLabel HeaderLbl;
        public TAOWMemo InfoMemo;
        public TAOWMenu BannerPopup;
        public TAOWVScrollBar InfoMemoSB;
        // AoWInfoBanner.pas:35
        public void ShowInfo(AnsiString Header, TStrings Description)
        {
            HeaderLbl.Text = Header;
            var call__1 = Description.Text;
            InfoMemo.Items.Text = call__1;
        }

        // AoWInfoBanner.pas:41
        public void BannerPopupMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (Button == TMouseButton.mbRight)
            {
                BannerPopup.Visible = false;
            }
        }
    }

    public static partial class AoWInfoBanner
    {
        public static TInfoBanner InfoBanner;
    }
}
