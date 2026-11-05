// Translated from AoWProgressDialog.pas.
using Aow.Rtl;
using Aow.Units.AOWGauge;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWWindow;

namespace Aow.Units.AoWProgressDialog
{
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

    public static partial class AoWProgressDialog
    {
    }
}
