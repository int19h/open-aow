// Translated from AoWProgressDialog.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

public partial class ProgressDialog : DataModule
{
    public ProgressDialog(Component owner) : base(owner)
    { }

    public AOWLabel Title;
    public AOWImage TCeltLineL;
    public AoWFrame BCeltL;
    public AOWLabel StatusLbl;
    public AOWWindow ProgDlg;
    public AoWGauge Bar;
    public AoWFrame TCeltL;
    public AOWImage AOWImage1;
    public AOWImage AOWImage2;
    public IvTranslator Translator;
}
