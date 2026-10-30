// Translated from AoWDeformFaceDlg.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.UnitSpells;
using static Aow.Units.UnitSpells.UnitSpells;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWDeformFaceDlg
{
    // { TDeformFaceDlg: size $90; $8C cosmetic-surgery caster. VMT 00445340. }
    public partial class TDeformFaceDlg : Aow.Rtl.TDataModule
    {
        public TDeformFaceDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame FaceFrame;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton CastBtn;
        // { +0058 }
        public Aow.Units.AoWLabel.TAOWLabel SelectNewFaceLbl;
        // { +005C }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0060 }
        public Aow.Units.AoWFrame.TAoWFrame LeftFace;
        // { +0064 }
        public Aow.Units.AoWFrame.TAoWFrame RightFace;
        // { +0068 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +006C }
        public Aow.Units.AoWImage.TAOWImage CeltM;
        // { +0070 }
        public Aow.Units.AoWImage.TAOWImage CeltL;
        // { +0074 }
        public Aow.Units.AoWImage.TAOWImage CeltR;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton PrevFaceBtn;
        // { +007C }
        public Aow.Units.AoWButton.TAOWButton NextFaceBtn;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel UnitNameLbl;
        // { +0084 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0088 }
        public Aow.Units.UnitSpells.TCosmeticSurgerySpellCaster FCaster;
        // AoWDeformFaceDlg.pas:127
        // { $8C }
        public void Execute(Aow.Units.UnitSpells.TCosmeticSurgerySpellCaster Caster)
        {
            Aow.Units.AoWE.TAbstractUnit U = default;
            FCaster = Caster;
            FCaster.AddRef();
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FCaster.FTargetUnitID);
            U = call__1;
            if (U != null)
            {
                var call__2 = U.GetName();
                UnitNameLbl.Text = call__2;
                Dlg.Visible = true;
            }
            else
            {
                Finish();
            }
        }

        // AoWDeformFaceDlg.pas:121
        public void Finish()
        {
            FCaster.Release();
            FCaster = null;
        }

        // AoWDeformFaceDlg.pas:55
        public void CancelBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWDeformFaceDlg.pas:61
        public void CastBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__3 = FCaster.CanCast(ref Reason);
            if (call__3)
            {
                FCaster.Cast();
                Dlg.Visible = false;
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(Reason);
            }
        }

        // AoWDeformFaceDlg.pas:73
        public void DrawFace(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            int Face = default;
            int Offset = default;
            Aow.Units.AoWE.TFaceResource Resource = default;
            Offset = ((Aow.Rtl.TComponent)Sender).Tag;
            Face = FCaster.FFaceIndex;
            while (Offset < 0)
            {
                Offset += 1;
                var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.PrevFace(Face);
                Face = call__4;
            }

            while (Offset > 0)
            {
                Offset -= 1;
                var call__5 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.NextFace(Face);
                Face = call__5;
            }

            var call__6 = Aow.Rtl.Rtl.Rect((Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + 2, Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, ((Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth) - 2, ((Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight) - 2);
            R = call__6;
            if (Face != -1)
            {
                var call__7 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.GetFaceResource(Face);
                Resource = call__7;
                Resource.AddRef();
                var call__8 = Resource.GetFaceImage();
                call__8.ShowStretched(R, SD);
                Resource.Release();
            }
        }

        // AoWDeformFaceDlg.pas:104
        public void PrevFaceBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__9 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.PrevFace(FCaster.FFaceIndex);
            FCaster.FFaceIndex = call__9;
            Dlg.FDrawn = false;
        }

        // AoWDeformFaceDlg.pas:111
        public void NextFaceBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__10 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.NextFace(FCaster.FFaceIndex);
            FCaster.FFaceIndex = call__10;
            Dlg.FDrawn = false;
        }

        // AoWDeformFaceDlg.pas:118
        public void DlgHide()
        {
            Finish();
        }

        // AoWDeformFaceDlg.pas:142
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWDeformFaceDlg
    {
    }
}
