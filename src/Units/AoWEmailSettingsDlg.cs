// Translated from AoWEmailSettingsDlg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWEmailSettingsDlg
{
    // { TEmailSettingsDlg (instance size 144, VMT 0040EBE8) }
    public partial class TEmailSettingsDlg : Aow.Rtl.TDataModule
    {
        public TEmailSettingsDlg(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        // { +005C }
        public Aow.Units.AoWPanel.TAOWPanel OKCancelPnl;
        // { +0060 }
        public Aow.Units.AoWButton.TAOWButton OKBtn;
        // { +0064 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel LocalEmailAddressLbl;
        // { +006C }
        public Aow.Units.AoWEdit.TAOWEdit LocalEmailAddressEdit;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel SMTPServerLbl;
        // { +0074 }
        public Aow.Units.AoWEdit.TAOWEdit SMTPServerEdit;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel AttachmentDirectoryLbl;
        // { +007C }
        public Aow.Units.AoWEdit.TAOWEdit AttachmentDirectoryEdit;
        // { +0080 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0088 }
        public Aow.Rtl.TIvTranslator Translator;
        // AoWEmailSettingsDlg.pas:49
        // { +008C }
        public void OKBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetLocalEmailAddress(LocalEmailAddressEdit.Text);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetSMTPServer(SMTPServerEdit.Text);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetAttachmentDirectory(AttachmentDirectoryEdit.Text);
            Dlg.Visible = false;
        }

        // AoWEmailSettingsDlg.pas:57
        public void CancelBtnClick(Aow.Rtl.TObject Sender)
        {
            Dlg.Visible = false;
        }

        // AoWEmailSettingsDlg.pas:60
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWEmailSettingsDlg
    {
    }
}
