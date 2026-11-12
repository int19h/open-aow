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

    public Window Dlg;
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
    public Image SpellFr;
    public Image FaceFrame;
    public Label SelectionInfoLbl;

    public UnitSpellCaster Caster { get; set; }

    // AoWCastUnitSpellDlg.pas:79
    public void ClearCaster()
    {
        Caster.Release();
        Caster = null;
    }

    // AoWCastUnitSpellDlg.pas:85
    public void DrawUnit(int index, int X, int Y, ref LockedSurface surface)
    {
        Units.Alignment alignment;
        UnitShowInfo info;
        Rect oldClip;
        if (index < Caster.TargetUnits.GetCount())
        {
            oldClip = surface.ClipRectangle;
            var rectangle = new Rect(X + 5, Y + 4, X + 46, Y + 56);
            surface.ClipRectangle = rectangle;
            alignment = Caster.TargetUnits.GetUnit(index).GetAlignment();
            if ((int)alignment < DialogHosts.UIGraphics.UnitBGs.Count)
            {
                var image = DialogHosts.UIGraphics.UnitBGs[(int)alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            Caster.TargetUnits.GetUnit(index).Show(X - 8, Y - 4, surface, info);
            if ((((int)(((uint)Caster.ValidSelection) >> index)) & 1) == 0)
            {
                var image = DialogHosts.UIGraphics.UnitBGs[7];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, surface);
            }

            surface.ClipRectangle = oldClip;
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
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (Caster.CanCast(ref reason))
        {
            Caster.Cast();
            Dlg.Visible = false;
        }
        else
        {
            AoWHSMap.Current.ShowMessage(reason);
        }
    }

    // AoWCastUnitSpellDlg.pas:163
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Troop U;
        Rect R = default;
        U = AoWHSMap.Current.UnitControl.FindUnit(Caster.CasterSourceId);
        if (U != null)
        {
            if (U.GetFace() != null)
            {
                R.Left = this.FaceFrame.WinLeft + TopPnl.WinLeft;
                R.Right = R.Left + this.FaceFrame.WinWidth;
                R.Top = this.FaceFrame.WinTop + TopPnl.WinTop;
                R.Bottom = R.Top + this.FaceFrame.WinHeight;

                U.GetFace().ShowStretched(R, surface);
            }
        }
    }

    // AoWCastUnitSpellDlg.pas:188
    public void U1Draw()
    {
    }

    // AoWCastUnitSpellDlg.pas:191
    public void SpellFrPreDraw(LockedSurface surface)
    {
        Spell spell;
        Rect R = default;
        if (Caster != null)
        {
            spell = AoWHSSet.Current.SpellControl.GetSpell(Caster.CasterSpellId);
            if (spell != null)
            {
                R.Left = this.SpellFr.WinLeft + TopPnl.WinLeft;
                R.Right = R.Left + this.SpellFr.WinWidth;
                R.Top = this.SpellFr.WinTop + TopPnl.WinTop;
                R.Bottom = R.Top + this.SpellFr.WinHeight;

                if (spell.CastImages.GetImageSequence(10) != null)
                {
                    spell.CastImages.Show(10, R.Left + 5, R.Top + 15, 0, surface);
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
        if (Dlg.Visible && Caster != null)
        {
            if (!Caster.Valid())
            {
                Dlg.Visible = false;
            }
        }
    }

    public void U1Change(CheckBox sender, int slot)
    {
        byte oldSelection = Caster.SelectionMask;
        int bit = 1 << slot;
        Caster.SetSelection((byte)(sender.Checked
            ? Caster.AllowMultipleTargets ? Caster.SelectionMask | bit : bit
            : Caster.SelectionMask & ~bit));
        var checks = new[] { U1, U2, U3, U4, U5, U6, U7, U8 };
        for (int i = 0; i < checks.Length; i++) checks[i].Checked = (Caster.SelectionMask & (1 << i)) != 0;
        if (Caster.SelectionMask != oldSelection)
            DialogHosts.General.PlayButtonSound((byte)(sender.Checked ? 1 : 2));
        if (Caster.SelectionMask == 0)
        {
            SelectionInfoLbl.Text = "";
            return;
        }
        string reason = "";
        SelectionInfoLbl.TextColor = Caster.CanCast(ref reason) ? 0 : 149;
        // AoW.exe loads the two-byte separator
        SelectionInfoLbl.Text = AbilityRules.TranslateResourceString(s_requiredManaText) +
            ": " + IntToStr(Caster.GetRequiredMana());
    }

    // AoWCastUnitSpellDlg.pas:183
    public void U1PreDraw(CheckBox sender, LockedSurface surface, int slot) =>
        DrawUnit(slot, UnitPnl.WinLeft + sender.WinLeft, UnitPnl.WinTop + sender.WinTop, ref surface);

    private static readonly string s_requiredManaText = "Required Mana";
}
