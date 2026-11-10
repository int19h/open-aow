// Translated from AoWCastUnitSpellDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AoWCastUnitSpellDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// retained unit spell caster
public partial class CastUnitSpellDlg : DataModule
{
    public CastUnitSpellDlg(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AoWGauge Unit1Gge;
    public AoWGauge Unit2Gge;
    public AoWGauge Unit3Gge;
    public AoWGauge Unit4Gge;
    public AoWGauge Unit5Gg;
    public AoWGauge Unit6Gg;
    public AoWGauge Unit7Gg;
    public AoWGauge Unit8Gg;
    public AOWButton CastBut;
    public AOWImage TitleIcon;
    public AOWButton Close;
    public AOWLabel Title;
    public AOWButton CancelBut;
    public AOWWindow Dlg;
    public AOWLabel SelectLbl;
    public AOWPanel UnitPnl;
    public AOWPanel TopPnl;
    public AOWLabel SpellUpkeepLbl;
    public AOWLabel SpellManaLbl;
    public AOWLabel CastingManaLbl;
    public AOWLabel SpellNameLbl;
    public AOWLabel CasterNameLbl;
    public AOWLabel CastingPointsLbl;
    public AOWCheckBox U1;
    public AOWCheckBox U2;
    public AOWCheckBox U3;
    public AOWCheckBox U4;
    public AOWCheckBox U5;
    public AOWCheckBox U6;
    public AOWCheckBox U7;
    public AOWCheckBox U8;
    public AOWImage DecoL;
    public AOWImage DecoR;
    public AOWImage SpellFr;
    public AOWImage FaceFrame;
    public AoWFrame AoWFrame1;
    public AOWPanel SelectionInfoPnl;
    public AOWLabel SelectionInfoLbl;
    public IvTranslator Translator;
    public UnitSpellCaster FCaster;

    // AoWCastUnitSpellDlg.pas:79
    public void ClearCaster()
    {
        FCaster.Release();
        FCaster = null;
    }

    // AoWCastUnitSpellDlg.pas:85
    public void DrawUnit(int Index, int X, int Y, ref SurfaceDesc Surface)
    {
        Alignment Alignment;
        P2UnitShowInfo Info;
        global::Aow.Rtl.Rect OldClip;
        if (Index < FCaster.FTargetUnits.GetCount())
        {
            OldClip = Surface.ClipRect;
            var rect = global::Aow.Rtl.RtlUnit.Rect(X + 5, Y + 4, X + 46, Y + 56);
            Surface.ClipRect = rect;
            Alignment = FCaster.FTargetUnits.GetUnit(Index).GetAlignment();
            if ((int)Alignment < Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                var image = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, Surface);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            FCaster.FTargetUnits.GetUnit(Index).ShowEx(X - 8, Y - 4, Surface, Info);
            if ((((int)(((uint)FCaster.FValidSelection) >> Index)) & 1) == 0)
            {
                var image = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[7];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, Surface);
            }

            Surface.ClipRect = OldClip;
        }
    }

    // AoWCastUnitSpellDlg.pas:111
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWCastUnitSpellDlg.pas:117
    public void CastButClick()
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FCaster.CanCast(ref Reason))
        {
            FCaster.Cast();
            Dlg.Visible = false;
        }
        else
        {
            Aow.Game.World.Map.AoWHSMap.Current.ShowMessage(Reason);
        }
    }

    // AoWCastUnitSpellDlg.pas:163
    public void FaceFramePreDraw(SurfaceDesc Surface)
    {
        AbstractUnit U;
        global::Aow.Rtl.Rect R = default;
        U = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FCaster.FCasterSourceID);
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
    public void SpellFrPreDraw(SurfaceDesc Surface)
    {
        Spell Spell;
        global::Aow.Rtl.Rect R = default;
        if (FCaster != null)
        {
            Spell = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FCaster.FCasterSpellID);
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

    public void U1Change(AOWCheckBox sender, int slot)
    {
        byte oldSelection = FCaster.FSelectionMask;
        int bit = 1 << slot;
        FCaster.SetSelection((byte)(sender.Checked
            ? FCaster.FAllowMultipleTargets ? FCaster.FSelectionMask | bit : bit
            : FCaster.FSelectionMask & ~bit));
        var checks = new[] { U1, U2, U3, U4, U5, U6, U7, U8 };
        for (int i = 0; i < checks.Length; i++) checks[i].Checked = (FCaster.FSelectionMask & (1 << i)) != 0;
        if (FCaster.FSelectionMask != oldSelection)
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound((byte)(sender.Checked ? 1 : 2));
        if (FCaster.FSelectionMask == 0)
        {
            SelectionInfoLbl.Text = "";
            return;
        }
        string reason = "";
        SelectionInfoLbl.TextColor = FCaster.CanCast(ref reason) ? 0 : 149;
        // AoW.exe loads the two-byte separator
        SelectionInfoLbl.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.CastUnitSpellDlg.RequiredManaRStr) +
            ": " + IntToStr(FCaster.GetRequiredMana());
    }

    // AoWCastUnitSpellDlg.pas:183
    public void U1PreDraw(AOWCheckBox sender, SurfaceDesc surface, int slot) =>
        DrawUnit(slot, UnitPnl.WinLeft + sender.WinLeft, UnitPnl.WinTop + sender.WinTop, ref surface);

    private static readonly string RequiredManaRStr = "Required Mana";
}
