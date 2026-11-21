#nullable disable
// Translated from AoWHeroInfoDlg.pas.
using System.Globalization;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.UI.Dialogs;

// done callback, army event, unit-details flag
public sealed partial class HeroInfoDialog(UIContext ui) : UIModule(ui)
{
    public Panel UnitPanel { get; private set; }
    public Window Window { get; private set; }
    public Label HeroNameLabel { get; private set; }
    public Label HeroLevelLabel { get; private set; }
    public Image FaceFrame { get; private set; }
    public Memo MessageMemo { get; private set; }

    private ExecuteDoneEvent _done;
    private ArmyInfoEventLog _armyInfo;
    private bool _detailsOpen;

    // AoWHeroInfoDlg.pas:99
    public void FinishDialog()
    {
        if (_detailsOpen)
        {
            _detailsOpen = false;
            UI.UnitWindow.MainWindow.Priority = UI.ControlWindow.MainWindow.Priority;
            UI.UnitWindow.MainWindow.Visible = false;
        }

        ExecuteDoneEvent done = _done;
        EventLog eventValue = _armyInfo;
        _done = null;
        _armyInfo = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWHeroInfoDlg.pas:118
    public void DrawUnit(int index, in Rect R, ref LockedSurface surface)
    {
        UnitShowInfo info;
        if (index < _armyInfo.ArmyView.Units.Count)
        {
            Rect oldClip = surface.ClipRectangle;
            surface.ClipRectangle = R;
            int x = (R.Right + R.Left) / 2;
            int y = (R.Top + R.Bottom) / 2;
            Units.Alignment alignment = _armyInfo.ArmyView.Units.GetUnit(index).GetAlignment();
            if ((int)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                LibraryImage image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(x - (image.Width / 2), y - (image.Height / 2), lockedSurface: surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = UnitOverlayModes.HitPoints | UnitOverlayModes.Rank;
            _ = _armyInfo.ArmyView.Units.GetUnit(index).Show(x - 33, y - 35, lockedSurface: surface, info);
            if (((_armyInfo.Selection >>> index) & 1) == 0)
            {
                LibraryImage image = UI.Graphics.UnitBackgrounds[7];
                image.ShowClipped(x - (image.Width / 2), y - (image.Height / 2), lockedSurface: surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWHeroInfoDlg.pas:146
    public void CloseBtnClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWHeroInfoDlg.pas:152
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect r = default;
        r.Left = FaceFrame.WinLeft + Window.WinLeft;
        r.Right = FaceFrame.WinWidth + r.Left;
        r.Top = FaceFrame.WinTop + Window.WinTop;
        r.Bottom = FaceFrame.WinHeight + r.Top;

        Hero hero = _armyInfo.ArmyView.Hero(_armyInfo.Selection, types: RosterTables.AllHeroTypes);
        if (hero.GetFace() is not null)
        {
            hero.ShowFace(r, lockedSurface: surface);
        }
        else
        {
            int i = 0;
            while (((1 << i) & _armyInfo.Selection) == 0 && i != _armyInfo.ArmyView.Units.Count)
            {
                i += 1;
            }

            if (i < _armyInfo.ArmyView.Units.Count)
            {
                _armyInfo.ArmyView.Units.GetUnit(i).ShowFace(r, lockedSurface: surface);
            }
        }
    }

    // AoWHeroInfoDlg.pas:175
    public void U1PreDraw(Frame sender, LockedSurface surface, int slot)
    {
        Rect r = default;
        r.Left = sender.WinLeft + UnitPanel.AbsoluteLeft + 1;
        r.Right = sender.WinWidth + r.Left;
        r.Top = sender.WinTop + UnitPanel.AbsoluteTop;
        r.Bottom = sender.WinHeight + r.Top;

        DrawUnit(slot, r, ref surface);
    }

    // AoWHeroInfoDlg.pas:189
    public void U1MouseDown(MouseButton button, ShiftStates shift, int X, int Y, int slot)
    {
        int count = _armyInfo.ArmyView.Units.Count;
        if (slot < count)
        {
            Troop troop = _armyInfo.ArmyView.Units.GetUnit(slot);
            UI.General.SelectedUnit = troop;
            if (button == MouseButton.Left)
            {
                _detailsOpen = true;
                UI.UnitWindow.ShowUnit();
                UI.UnitWindow.MainWindow.Priority = Window.Priority + 1;
            }
        }
    }

    // AoWHeroInfoDlg.pas:203
    public void DlgHide() => FinishDialog();

    // AoWHeroInfoDlg.pas:208
    public void DlgKeyDown(in ushort key, ShiftStates shift)
    {
        if (key is 27 or 32)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowHeroInfo(HeroInfoDialog dialog, ArmyInfoEventLog eventValue, ExecuteDoneEvent done)
    {
        Assert(dialog._armyInfo is null, message: "", file: "AoWHeroInfoDlg.pas", line: 80);
        dialog._done = done;
        dialog._armyInfo = eventValue;
        _ = dialog._armyInfo.AddRef();
        string text = eventValue.Messages.Text;
        dialog.MessageMemo.Items.Text = text;
        Hero hero = dialog._armyInfo.ArmyView.Hero(dialog._armyInfo.Selection, types: RosterTables.AllHeroTypes);
        if (hero is not null)
        {
            dialog.HeroNameLabel.Text = hero.HeroName;
            sbyte level = hero.GetLevel();
            string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.LevelFormat), arguments: [((int)level).ToString(CultureInfo.InvariantCulture)]);
            dialog.HeroLevelLabel.Text = format;
        }
        else
        {
            dialog.HeroNameLabel.Text = "";
            dialog.HeroLevelLabel.Text = "";
        }

        dialog.Window.Visible = true;
        dialog._detailsOpen = false;
    }
}
