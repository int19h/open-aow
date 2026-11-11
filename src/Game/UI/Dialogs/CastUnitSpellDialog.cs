// Translated from AoWCastUnitSpellDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// retained unit spell caster
public partial class CastUnitSpellDialog : DataModule
{
    public CastUnitSpellDialog(Component owner) : base(owner)
    { }

    public Gauge Unit1Gge;
    public Gauge Unit2Gge;
    public Gauge Unit3Gge;
    public Gauge Unit4Gge;
    public Gauge Unit5Gg;
    public Gauge Unit6Gg;
    public Gauge Unit7Gg;
    public Gauge Unit8Gg;
    public Button CastBut;
    public Image TitleIcon;
    public Button Close;
    public Label Title;
    public Button CancelBut;
    public Window Dlg;
    public Label SelectLbl;
    public Panel UnitPnl;
    public Panel TopPnl;
    public Label SpellUpkeepLbl;
    public Label SpellManaLbl;
    public Label CastingManaLbl;
    public Label SpellNameLbl;
    public Label CasterNameLbl;
    public Label CastingPointsLbl;
    public CheckBox U1;
    public CheckBox U2;
    public CheckBox U3;
    public CheckBox U4;
    public CheckBox U5;
    public CheckBox U6;
    public CheckBox U7;
    public CheckBox U8;
    public Image DecoL;
    public Image DecoR;
    public Image SpellFr;
    public Image FaceFrame;
    public Frame AoWFrame1;
    public Panel SelectionInfoPnl;
    public Label SelectionInfoLbl;
    public IvTranslator Translator;
    public UnitSpellCaster FCaster;

    // AoWCastUnitSpellDlg.pas:79
    public void ClearCaster()
    {
        FCaster.Release();
        FCaster = null;
    }

    // AoWCastUnitSpellDlg.pas:85
    public void DrawUnit(int Index, int X, int Y, ref LockedSurface Surface)
    {
        Units.Alignment Alignment;
        UnitShowInfo Info;
        Rect OldClip;
        if (Index < FCaster.FTargetUnits.GetCount())
        {
            OldClip = Surface.ClipRect;
            var rect = new Rect(X + 5, Y + 4, X + 46, Y + 56);
            Surface.ClipRect = rect;
            Alignment = FCaster.FTargetUnits.GetUnit(Index).GetAlignment();
            if ((int)Alignment < DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                var image = DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, Surface);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            FCaster.FTargetUnits.GetUnit(Index).ShowEx(X - 8, Y - 4, Surface, Info);
            if ((((int)(((uint)FCaster.FValidSelection) >> Index)) & 1) == 0)
            {
                var image = DialogHosts.IntGfxMod.UnitBGs[7];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, Surface);
            }

            Surface.ClipRect = OldClip;
        }
    }

    // AoWCastUnitSpellDlg.pas:111
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWCastUnitSpellDlg.pas:117
    public void CastButClick()
    {
        string Reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (FCaster.CanCast(ref Reason))
        {
            FCaster.Cast();
            Dlg.Visible = false;
        }
        else
        {
            AoWHSMap.Current.ShowMessage(Reason);
        }
    }

    // AoWCastUnitSpellDlg.pas:163
    public void FaceFramePreDraw(LockedSurface Surface)
    {
        Troop U;
        Rect R = default;
        U = AoWHSMap.Current.FUnitControl.FindUnit(FCaster.FCasterSourceID);
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
    public void SpellFrPreDraw(LockedSurface Surface)
    {
        Spell Spell;
        Rect R = default;
        if (FCaster != null)
        {
            Spell = AoWHSSet.Current.FSpellControl.GetSpell(FCaster.FCasterSpellID);
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

    public void U1Change(CheckBox sender, int slot)
    {
        byte oldSelection = FCaster.FSelectionMask;
        int bit = 1 << slot;
        FCaster.SetSelection((byte)(sender.Checked
            ? FCaster.FAllowMultipleTargets ? FCaster.FSelectionMask | bit : bit
            : FCaster.FSelectionMask & ~bit));
        var checks = new[] { U1, U2, U3, U4, U5, U6, U7, U8 };
        for (int i = 0; i < checks.Length; i++) checks[i].Checked = (FCaster.FSelectionMask & (1 << i)) != 0;
        if (FCaster.FSelectionMask != oldSelection)
            DialogHosts.General.PlayButtonSound((byte)(sender.Checked ? 1 : 2));
        if (FCaster.FSelectionMask == 0)
        {
            SelectionInfoLbl.Text = "";
            return;
        }
        string reason = "";
        SelectionInfoLbl.TextColor = FCaster.CanCast(ref reason) ? 0 : 149;
        // AoW.exe loads the two-byte separator
        SelectionInfoLbl.Text = AbilityRules.TranslateRStr(RequiredManaRStr) +
            ": " + IntToStr(FCaster.GetRequiredMana());
    }

    // AoWCastUnitSpellDlg.pas:183
    public void U1PreDraw(CheckBox sender, LockedSurface surface, int slot) =>
        DrawUnit(slot, UnitPnl.WinLeft + sender.WinLeft, UnitPnl.WinTop + sender.WinTop, ref surface);

    private static readonly string RequiredManaRStr = "Required Mana";
}
