using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class ProgressDialog : DataModule
{
    public ProgressDialog(Component owner) : base(owner)
    { }

    public Label TitleLabel;
    public Label StatusLabel;
    public Window Window;
    public Gauge ProgressBar;
}
