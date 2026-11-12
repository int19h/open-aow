// Translated from AoWCastUnitSpellDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// retained unit spell caster
public partial class CastUnitSpellDialog : DataModule
{
    public CastUnitSpellDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Panel UnitPanel;
    public Panel TopPanel;
    public Label SpellUpkeepLabel;
    public Label SpellManaLabel;
    public Label CastingManaLabel;
    public Label SpellNameLabel;
    public Label CasterNameLabel;
    public Label CastingPointsLabel;
    public CheckBox Unit1CheckBox;
    public CheckBox Unit2CheckBox;
    public CheckBox Unit3CheckBox;
    public CheckBox Unit4CheckBox;
    public CheckBox Unit5CheckBox;
    public CheckBox Unit6CheckBox;
    public CheckBox Unit7CheckBox;
    public CheckBox Unit8CheckBox;
    public Image SpellFrame;
    public Image FaceFrame;
    public Label SelectionInfoLabel;

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
            if ((int)alignment < DialogHosts.UIGraphics.UnitBackgrounds.Count)
            {
                var image = DialogHosts.UIGraphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            Caster.TargetUnits.GetUnit(index).Show(X - 8, Y - 4, surface, info);
            if ((((int)(((uint)Caster.ValidSelection) >> index)) & 1) == 0)
            {
                var image = DialogHosts.UIGraphics.UnitBackgrounds[7];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWCastUnitSpellDlg.pas:111
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWCastUnitSpellDlg.pas:117
    public void CastButClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (Caster.CanCast(ref reason))
        {
            Caster.Cast();
            Window.Visible = false;
        }
        else
        {
            StrategicMap.Current.ShowMessage(reason);
        }
    }

    // AoWCastUnitSpellDlg.pas:163
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Troop U;
        Rect R = default;
        U = StrategicMap.Current.UnitControl.FindUnit(Caster.CasterSourceId);
        if (U != null)
        {
            if (U.GetFace() != null)
            {
                R.Left = this.FaceFrame.WinLeft + TopPanel.WinLeft;
                R.Right = R.Left + this.FaceFrame.WinWidth;
                R.Top = this.FaceFrame.WinTop + TopPanel.WinTop;
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
            spell = GamePack.Current.SpellControl.GetSpell(Caster.CasterSpellId);
            if (spell != null)
            {
                R.Left = this.SpellFrame.WinLeft + TopPanel.WinLeft;
                R.Right = R.Left + this.SpellFrame.WinWidth;
                R.Top = this.SpellFrame.WinTop + TopPanel.WinTop;
                R.Bottom = R.Top + this.SpellFrame.WinHeight;

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
        if (Window.Visible && Caster != null)
        {
            if (!Caster.Valid())
            {
                Window.Visible = false;
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
        var checks = new[] { Unit1CheckBox, Unit2CheckBox, Unit3CheckBox, Unit4CheckBox, Unit5CheckBox, Unit6CheckBox, Unit7CheckBox, Unit8CheckBox };
        for (int i = 0; i < checks.Length; i++) checks[i].Checked = (Caster.SelectionMask & (1 << i)) != 0;
        if (Caster.SelectionMask != oldSelection)
            DialogHosts.General.PlayButtonSound((byte)(sender.Checked ? 1 : 2));
        if (Caster.SelectionMask == 0)
        {
            SelectionInfoLabel.Text = "";
            return;
        }
        string reason = "";
        SelectionInfoLabel.TextColor = Caster.CanCast(ref reason) ? 0 : 149;
        // AoW.exe loads the two-byte separator
        SelectionInfoLabel.Text = AbilityRules.TranslateResourceString(s_requiredManaText) +
            ": " + IntToStr(Caster.GetRequiredMana());
    }

    // AoWCastUnitSpellDlg.pas:183
    public void U1PreDraw(CheckBox sender, LockedSurface surface, int slot) =>
        DrawUnit(slot, UnitPanel.WinLeft + sender.WinLeft, UnitPanel.WinTop + sender.WinTop, ref surface);

    private static readonly string s_requiredManaText = "Required Mana";
}
