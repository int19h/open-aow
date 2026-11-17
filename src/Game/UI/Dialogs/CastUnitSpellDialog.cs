#nullable disable
// Translated from AoWCastUnitSpellDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
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
public partial class CastUnitSpellDialog : UIModule
{
    public CastUnitSpellDialog(UIContext ui) : base(ui)
    { }

    public Window Window { get; private set; }
    public Panel UnitPanel { get; private set; }
    public Panel TopPanel { get; private set; }
    public Label SpellUpkeepLabel { get; private set; }
    public Label SpellManaLabel { get; private set; }
    public Label CastingManaLabel { get; private set; }
    public Label SpellNameLabel { get; private set; }
    public Label CasterNameLabel { get; private set; }
    public Label CastingPointsLabel { get; private set; }
    public CheckBox Unit1CheckBox { get; private set; }
    public CheckBox Unit2CheckBox { get; private set; }
    public CheckBox Unit3CheckBox { get; private set; }
    public CheckBox Unit4CheckBox { get; private set; }
    public CheckBox Unit5CheckBox { get; private set; }
    public CheckBox Unit6CheckBox { get; private set; }
    public CheckBox Unit7CheckBox { get; private set; }
    public CheckBox Unit8CheckBox { get; private set; }
    public Image SpellFrame { get; private set; }
    public Image FaceFrame { get; private set; }
    public Label SelectionInfoLabel { get; private set; }

    public UnitSpellCaster Caster { get; set; }

    // AoWCastUnitSpellDlg.pas:79
    public void ClearCaster()
    {
        _ = Caster.Release();
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
            Rect rectangle = new(X + 5, Y + 4, X + 46, Y + 56);
            surface.ClipRectangle = rectangle;
            alignment = Caster.TargetUnits.GetUnit(index).GetAlignment();
            if ((int)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                LibraryImage image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X + 25 - (image.Width / 2), Y + 30 - (image.Height / 2), surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _ = Caster.TargetUnits.GetUnit(index).Show(X - 8, Y - 4, surface, info);
            if ((((int)(((uint)Caster.ValidSelection) >> index)) & 1) == 0)
            {
                LibraryImage image = UI.Graphics.UnitBackgrounds[7];
                image.ShowClipped(X + 25 - (image.Width / 2), Y + 30 - (image.Height / 2), surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWCastUnitSpellDlg.pas:111
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWCastUnitSpellDlg.pas:117
    public void CastButClick()
    {
        string reason = "";
        UI.General.PlayButtonSound(0);
        if (Caster.CanCast(ref reason))
        {
            _ = Caster.Cast();
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
        Troop u;
        Rect r = default;
        u = StrategicMap.Current.UnitControl.FindUnit(Caster.CasterSourceId);
        if (u != null)
        {
            if (u.GetFace() != null)
            {
                r.Left = FaceFrame.WinLeft + TopPanel.WinLeft;
                r.Right = r.Left + FaceFrame.WinWidth;
                r.Top = FaceFrame.WinTop + TopPanel.WinTop;
                r.Bottom = r.Top + FaceFrame.WinHeight;

                u.GetFace().ShowStretched(r, surface);
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
        Rect r = default;
        if (Caster != null)
        {
            spell = GamePack.Current.SpellControl.GetSpell(Caster.CasterSpellId);
            if (spell != null)
            {
                r.Left = SpellFrame.WinLeft + TopPanel.WinLeft;
                r.Right = r.Left + SpellFrame.WinWidth;
                r.Top = SpellFrame.WinTop + TopPanel.WinTop;
                r.Bottom = r.Top + SpellFrame.WinHeight;

                if (spell.CastImages.GetImageSequence(10) != null)
                {
                    _ = spell.CastImages.Show(10, r.Left + 5, r.Top + 15, 0, surface);
                }
            }
        }
    }

    // AoWCastUnitSpellDlg.pas:214
    public void DlgHide() => ClearCaster();

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
        CheckBox[] checks = [Unit1CheckBox, Unit2CheckBox, Unit3CheckBox, Unit4CheckBox, Unit5CheckBox, Unit6CheckBox, Unit7CheckBox, Unit8CheckBox];
        for (int i = 0; i < checks.Length; i++)
        {
            checks[i].Checked = (Caster.SelectionMask & (1 << i)) != 0;
        }

        if (Caster.SelectionMask != oldSelection)
        {
            UI.General.PlayButtonSound((byte)(sender.Checked ? 1 : 2));
        }

        if (Caster.SelectionMask == 0)
        {
            SelectionInfoLabel.Text = "";
            return;
        }
        string reason = "";
        SelectionInfoLabel.TextColor = Caster.CanCast(ref reason) ? 0 : 149;
        // AoW.exe loads the two-byte separator
        SelectionInfoLabel.Text = UI.Engine.TranslateResourceString(s_requiredManaText) +
            ": " + IntToStr(Caster.GetRequiredMana());
    }

    // AoWCastUnitSpellDlg.pas:183
    public void U1PreDraw(CheckBox sender, LockedSurface surface, int slot) =>
        DrawUnit(slot, UnitPanel.WinLeft + sender.WinLeft, UnitPanel.WinTop + sender.WinTop, ref surface);

    private static readonly string s_requiredManaText = "Required Mana";
}
