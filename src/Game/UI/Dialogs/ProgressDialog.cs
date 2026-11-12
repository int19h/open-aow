using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class ProgressDialog : UIModule
{
    public ProgressDialog(UIContext ui) : base(ui)
    { }

    public Label TitleLabel;
    public Label StatusLabel;
    public Window Window;
    public Gauge ProgressBar;
}
