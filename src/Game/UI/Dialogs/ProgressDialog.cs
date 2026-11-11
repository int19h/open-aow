using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class ProgressDialog : DataModule
{
    public ProgressDialog(Component owner) : base(owner)
    { }

    public Label Title;
    public Image TCeltLineL;
    public Frame BCeltL;
    public Label StatusLbl;
    public Window ProgDlg;
    public Gauge Bar;
    public Frame TCeltL;
    public Image AOWImage1;
    public Image AOWImage2;
    public IvTranslator Translator;
}
