// Translated from AoWCastUnitSpellDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using Aow.Units.AOWGauge;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;

namespace Aow.Units.AoWCastUnitSpellDlg
{
    // retained unit spell caster
    public partial class TCastUnitSpellDlg : TDataModule
    {
        public TCastUnitSpellDlg(TComponent owner) : base(owner)
        { }

        public TAoWGauge Unit1Gge;
        public TAoWGauge Unit2Gge;
        public TAoWGauge Unit3Gge;
        public TAoWGauge Unit4Gge;
        public TAoWGauge Unit5Gg;
        public TAoWGauge Unit6Gg;
        public TAoWGauge Unit7Gg;
        public TAoWGauge Unit8Gg;
        public TAOWButton CastBut;
        public TAOWImage TitleIcon;
        public TAOWButton Close;
        public TAOWLabel Title;
        public TAOWButton CancelBut;
        public TAOWWindow Dlg;
        public TAOWLabel SelectLbl;
        public TAOWPanel UnitPnl;
        public TAOWPanel TopPnl;
        public TAOWLabel SpellUpkeepLbl;
        public TAOWLabel SpellManaLbl;
        public TAOWLabel CastingManaLbl;
        public TAOWLabel SpellNameLbl;
        public TAOWLabel CasterNameLbl;
        public TAOWLabel CastingPointsLbl;
        public TAOWCheckBox U1;
        public TAOWCheckBox U2;
        public TAOWCheckBox U3;
        public TAOWCheckBox U4;
        public TAOWCheckBox U5;
        public TAOWCheckBox U6;
        public TAOWCheckBox U7;
        public TAOWCheckBox U8;
        public TAOWImage DecoL;
        public TAOWImage DecoR;
        public TAOWImage SpellFr;
        public TAOWImage FaceFrame;
        public TAoWFrame AoWFrame1;
        public TAOWPanel SelectionInfoPnl;
        public TAOWLabel SelectionInfoLbl;
        public TIvTranslator Translator;
        public TUnitSpellCaster FCaster;
        // AoWCastUnitSpellDlg.pas:79
        public void ClearCaster()
        {
            FCaster.Release();
            FCaster = null;
        }

        // AoWCastUnitSpellDlg.pas:85
        public void DrawUnit(int Index, int X, int Y, ref TSurfaceDesc Surface)
        {
            TAlignment Alignment = default;
            TP2UnitShowInfo Info = default;
            TRect OldClip = default;
            var call__1 = FCaster.FTargetUnits.GetCount();
            if (Index < call__1)
            {
                OldClip = Surface.ClipRect;
                var call__2 = Rect(X + 5, Y + 4, X + 46, Y + 56);
                Surface.ClipRect = call__2;
                var call__3 = FCaster.FTargetUnits.GetUnit(Index);
                var call__4 = call__3.GetAlignment();
                Alignment = call__4;
                if ((int)Alignment < IntGfxMod.UnitBGs.Count)
                {
                    var image = IntGfxMod.UnitBGs[(int)Alignment];
                    image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, Surface);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
                var call__6 = FCaster.FTargetUnits.GetUnit(Index);
                call__6.ShowEx(X - 8, Y - 4, Surface, Info);
                if ((((int)(((uint)FCaster.FValidSelection) >> Index)) & 1) == 0)
                {
                    var image2 = IntGfxMod.UnitBGs[7];
                    image2.ShowClipped(X + 25 - image2.Width / 2, Y + 30 - image2.Height / 2, Surface);
                }

                Surface.ClipRect = OldClip;
            }
        }

        // AoWCastUnitSpellDlg.pas:111
        public void CloseClick()
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWCastUnitSpellDlg.pas:117
        public void CastButClick()
        {
            AnsiString Reason = default;
            General.PlayButtonSound(0);
            var call__8 = FCaster.CanCast(ref Reason);
            if (call__8)
            {
                FCaster.Cast();
                Dlg.Visible = false;
            }
            else
            {
                AoWHSMap.ShowMessage(Reason);
            }
        }

        // AoWCastUnitSpellDlg.pas:163
        public void FaceFramePreDraw(TAoWComponent Sender, TSurfaceDesc Surface)
        {
            TAbstractUnit U = default;
            TRect R = default;
            var call__9 = AoWHSMap.FUnitControl.FindUnit(FCaster.FCasterSourceID);
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
        public void SpellFrPreDraw(TAoWComponent Sender, TSurfaceDesc Surface)
        {
            TSpell Spell = default;
            TRect R = default;
            if (FCaster != null)
            {
                var call__12 = AoWHSSet.FSpellControl.GetSpell(FCaster.FCasterSpellID);
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
            if (Dlg.Visible && FCaster != null)
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
