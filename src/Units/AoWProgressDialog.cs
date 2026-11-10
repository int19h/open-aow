// Translated from AoWProgressDialog.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWProgressDialog.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

public partial class TProgressDialog : TDataModule
{
    public TProgressDialog(TComponent owner) : base(owner)
    { }

    public TAOWLabel Title;
    public TAOWImage TCeltLineL;
    public TAoWFrame BCeltL;
    public TAOWLabel StatusLbl;
    public TAOWWindow ProgDlg;
    public TAoWGauge Bar;
    public TAoWFrame TCeltL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TIvTranslator Translator;
}
