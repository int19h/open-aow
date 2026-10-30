// Translated from AoWCastUnitSpellDlg.pas.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
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
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;

namespace Aow.Units.AoWCastUnitSpellDlg
{
    // retained unit spell caster
    public partial class TCastUnitSpellDlg : Aow.Rtl.TDataModule
    {
        public TCastUnitSpellDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AOWGauge.TAoWGauge Unit1Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit2Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit3Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit4Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit5Gg;
        public Aow.Units.AOWGauge.TAoWGauge Unit6Gg;
        public Aow.Units.AOWGauge.TAoWGauge Unit7Gg;
        public Aow.Units.AOWGauge.TAoWGauge Unit8Gg;
        public Aow.Units.AoWButton.TAOWButton CastBut;
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Units.AoWLabel.TAOWLabel Title;
        public Aow.Units.AoWButton.TAOWButton CancelBut;
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWLabel.TAOWLabel SelectLbl;
        public Aow.Units.AoWPanel.TAOWPanel UnitPnl;
        public Aow.Units.AoWPanel.TAOWPanel TopPnl;
        public Aow.Units.AoWLabel.TAOWLabel SpellUpkeepLbl;
        public Aow.Units.AoWLabel.TAOWLabel SpellManaLbl;
        public Aow.Units.AoWLabel.TAOWLabel CastingManaLbl;
        public Aow.Units.AoWLabel.TAOWLabel SpellNameLbl;
        public Aow.Units.AoWLabel.TAOWLabel CasterNameLbl;
        public Aow.Units.AoWLabel.TAOWLabel CastingPointsLbl;
        public Aow.Units.AoWCheckBox.TAOWCheckBox U1;
        public Aow.Units.AoWCheckBox.TAOWCheckBox U2;
        public Aow.Units.AoWCheckBox.TAOWCheckBox U3;
        public Aow.Units.AoWCheckBox.TAOWCheckBox U4;
        public Aow.Units.AoWCheckBox.TAOWCheckBox U5;
        public Aow.Units.AoWCheckBox.TAOWCheckBox U6;
        public Aow.Units.AoWCheckBox.TAOWCheckBox U7;
        public Aow.Units.AoWCheckBox.TAOWCheckBox U8;
        public Aow.Units.AoWImage.TAOWImage DecoL;
        public Aow.Units.AoWImage.TAOWImage DecoR;
        public Aow.Units.AoWImage.TAOWImage SpellFr;
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        public Aow.Units.AoWPanel.TAOWPanel SelectionInfoPnl;
        public Aow.Units.AoWLabel.TAOWLabel SelectionInfoLbl;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.AoWE.TUnitSpellCaster FCaster;
        // AoWCastUnitSpellDlg.pas:79
        public void ClearCaster()
        {
            FCaster.Release();
            FCaster = null;
        }

        // AoWCastUnitSpellDlg.pas:85
        public void DrawUnit(int Index, int X, int Y, ref Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWE.TAlignment Alignment = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Rtl.TRect OldClip = default;
            var call__1 = FCaster.FTargetUnits.GetCount();
            if (Index < call__1)
            {
                OldClip = Surface.ClipRect;
                var call__2 = Aow.Rtl.Rtl.Rect(X + 5, Y + 4, X + 46, Y + 56);
                Surface.ClipRect = call__2;
                var call__3 = FCaster.FTargetUnits.GetUnit(Index);
                var call__4 = call__3.GetAlignment();
                Alignment = call__4;
                if (((int)Alignment) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                {
                    var image = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[unchecked((int)Alignment)];
                    image.ShowClipped((X + 25) - (image.Width / 2), (Y + 30) - (image.Height / 2), Surface);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoHitPoints, Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                var call__6 = FCaster.FTargetUnits.GetUnit(Index);
                call__6.ShowEx(X - 8, Y - 4, Surface, Info);
                if ((((int)(((uint)FCaster.FValidSelection) >> Index)) & 1) == 0)
                {
                    var image2 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[7];
                    image2.ShowClipped((X + 25) - (image2.Width / 2), (Y + 30) - (image2.Height / 2), Surface);
                }

                Surface.ClipRect = OldClip;
            }
        }

        // AoWCastUnitSpellDlg.pas:111
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWCastUnitSpellDlg.pas:117
        public void CastButClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__8 = FCaster.CanCast(ref Reason);
            if (call__8)
            {
                FCaster.Cast();
                Dlg.Visible = false;
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(Reason);
            }
        }

        // AoWCastUnitSpellDlg.pas:163
        public void FaceFramePreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWE.TAbstractUnit U = default;
            Aow.Rtl.TRect R = default;
            var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FCaster.FCasterSourceID);
            U = call__9;
            if (U != null)
            {
                if (U.GetFace() != null)
                {
                    R.Left = this.FaceFrame.WinLeft + TopPnl.WinLeft;
                    R.Right = R.Left + this.FaceFrame.WinWidth;
                    R.Top = this.FaceFrame.WinTop + TopPnl.WinTop;
                    R.Bottom = R.Top + this.FaceFrame.WinHeight;

                    var call__11 = U.GetFace();
                    call__11.ShowStretched(R, Surface);
                }
            }
        }

        // AoWCastUnitSpellDlg.pas:188
        public void U1Draw()
        {
        }

        // AoWCastUnitSpellDlg.pas:191
        public void SpellFrPreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWE.TSpell Spell = default;
            Aow.Rtl.TRect R = default;
            if (FCaster != null)
            {
                var call__12 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FCaster.FCasterSpellID);
                Spell = call__12;
                if (Spell != null)
                {
                    R.Left = this.SpellFr.WinLeft + TopPnl.WinLeft;
                    R.Right = R.Left + this.SpellFr.WinWidth;
                    R.Top = this.SpellFr.WinTop + TopPnl.WinTop;
                    R.Bottom = R.Top + this.SpellFr.WinHeight;

                    var call__14 = Spell.FCastImages.GetImageSequence(10);
                    if (call__14 != null)
                    {
                        Spell.FCastImages.Show(10, R.Left + 5, R.Top + 15, 0, Surface);
                    }
                }
            }
        }

        // AoWCastUnitSpellDlg.pas:214
        public void DlgHide()
        {
            ClearCaster();
        }

        // AoWCastUnitSpellDlg.pas:219
        public void DlgUpdate()
        {
            if (Dlg.Visible && (FCaster != null))
            {
                var call__15 = FCaster.Valid();
                if (!call__15)
                {
                    Dlg.Visible = false;
                }
            }
        }
    }

    public static partial class AoWCastUnitSpellDlg
    {
    }
}
