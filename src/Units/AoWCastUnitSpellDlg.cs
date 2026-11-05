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

namespace Aow.Units.AoWCastUnitSpellDlg;

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
        TAlignment Alignment;
        TP2UnitShowInfo Info;
        TRect OldClip;
        if (Index < FCaster.FTargetUnits.GetCount())
        {
            OldClip = Surface.ClipRect;
            var rect = Rect(X + 5, Y + 4, X + 46, Y + 56);
            Surface.ClipRect = rect;
            Alignment = FCaster.FTargetUnits.GetUnit(Index).GetAlignment();
            if ((int)Alignment < IntGfxMod.UnitBGs.Count)
            {
                var image = IntGfxMod.UnitBGs[(int)Alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, Surface);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
            FCaster.FTargetUnits.GetUnit(Index).ShowEx(X - 8, Y - 4, Surface, Info);
            if ((((int)(((uint)FCaster.FValidSelection) >> Index)) & 1) == 0)
            {
                var image = IntGfxMod.UnitBGs[7];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, Surface);
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
        string Reason = "";
        General.PlayButtonSound(0);
        if (FCaster.CanCast(ref Reason))
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
        TAbstractUnit U;
        TRect R = default;
        U = AoWHSMap.FUnitControl.FindUnit(FCaster.FCasterSourceID);
        if (U != null)
        {
            if (U.GetFace() != null)
            {
                R.Left = this.FaceFrame.WinLeft + TopPnl.WinLeft;
                R.Right = R.Left + this.FaceFrame.WinWidth;
                R.Top = this.FaceFrame.WinTop + TopPnl.WinTop;
                R.Bottom = R.Top + this.FaceFrame.WinHeight;

                U.GetFace().ShowStretched(R, Surface);
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
        TSpell Spell;
        TRect R = default;
        if (FCaster != null)
        {
            Spell = AoWHSSet.FSpellControl.GetSpell(FCaster.FCasterSpellID);
            if (Spell != null)
            {
                R.Left = this.SpellFr.WinLeft + TopPnl.WinLeft;
                R.Right = R.Left + this.SpellFr.WinWidth;
                R.Top = this.SpellFr.WinTop + TopPnl.WinTop;
                R.Bottom = R.Top + this.SpellFr.WinHeight;

                if (Spell.FCastImages.GetImageSequence(10) != null)
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
            if (!FCaster.Valid())
            {
                Dlg.Visible = false;
            }
        }
    }

    public void U1Change(TAOWCheckBox sender)
    {
        byte oldSelection = FCaster.FSelectionMask;
        int bit = 1 << sender.Tag;
        FCaster.SetSelection((byte)(sender.Checked
            ? FCaster.FAllowMultipleTargets ? FCaster.FSelectionMask | bit : bit
            : FCaster.FSelectionMask & ~bit));
        var checks = new[] { U1, U2, U3, U4, U5, U6, U7, U8 };
        for (int i = 0; i < checks.Length; i++) checks[i].Checked = (FCaster.FSelectionMask & (1 << i)) != 0;
        if (FCaster.FSelectionMask != oldSelection)
            AoWWinMan.AoWWinMan.General.PlayButtonSound((byte)(sender.Checked ? 1 : 2));
        if (FCaster.FSelectionMask == 0)
        {
            SelectionInfoLbl.Text = "";
            return;
        }
        string reason = "";
        SelectionInfoLbl.TextColor = FCaster.CanCast(ref reason) ? 0 : 149;
        // AoW.exe loads the two-byte separator
        SelectionInfoLbl.Text = AoWE.AoWE.TranslateRStr(AoWE.AoWE.RequiredManaRStr) +
            ": " + Rtl.Rtl.IntToStr(FCaster.GetRequiredMana());
    }

    // AoWCastUnitSpellDlg.pas:183. The sender is a unit checkbox.
    public void U1PreDraw(TAoWComponent sender, TSurfaceDesc surface) =>
        DrawUnit(sender.Tag, UnitPnl.WinLeft + sender.WinLeft, UnitPnl.WinTop + sender.WinTop, ref surface);
}

public static partial class AoWCastUnitSpellDlg
{
}
