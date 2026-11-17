#nullable disable
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.UI.Dialogs;

public partial class ProgressDialog(UIContext ui) : UIModule(ui)
{
    public Label TitleLabel { get; private set; }
    public Label StatusLabel { get; private set; }
    public Window Window { get; private set; }
    public Gauge ProgressBar { get; private set; }
}
