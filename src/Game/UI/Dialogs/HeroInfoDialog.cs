// Translated from AoWHeroInfoDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// done callback, army event, unit-details flag
public partial class HeroInfoDialog : DataModule
{
    public HeroInfoDialog(Component owner) : base(owner)
    { }

    public Panel UnitPnl;
    public Window Dlg;
    public Label HeroName;
    public Label HeroLevel;
    public Image FaceFrame;
    public Memo Message;
    private ExecuteDoneEvent _done;
    private ArmyInfoEventLog _armyInfo;
    private bool _detailsOpen;

    // AoWHeroInfoDlg.pas:99
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        EventLog eventValue;
        if (_detailsOpen)
        {
            _detailsOpen = false;
            DialogHosts.UnitWindow.UnitWin.Priority = DialogHosts.ControlWindow.MainWindow.Priority;
            DialogHosts.UnitWindow.UnitWin.Visible = false;
        }

        done = _done;
        eventValue = _armyInfo;
        _done = null;
        _armyInfo = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWHeroInfoDlg.pas:118
    public void DrawUnit(int index, in Rect R, ref LockedSurface surface)
    {
        Units.Alignment alignment;
        int X;
        int Y;
        Rect oldClip;
        UnitShowInfo info;
        if (index < _armyInfo.ArmyView.GetCount())
        {
            oldClip = surface.ClipRectangle;
            surface.ClipRectangle = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            alignment = _armyInfo.ArmyView.GetUnit(index).GetAlignment();
            if ((int)alignment < DialogHosts.UIGraphics.UnitBGs.Count)
            {
                var image = DialogHosts.UIGraphics.UnitBGs[(int)alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _armyInfo.ArmyView.GetUnit(index).Show(X - 33, Y - 35, surface, info);
            if ((((int)(((uint)_armyInfo.Selection) >> index)) & 1) == 0)
            {
                var image = DialogHosts.UIGraphics.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWHeroInfoDlg.pas:146
    public void CloseBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWHeroInfoDlg.pas:152
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect R = default;
        Hero hero;
        int I;
        R.Left = this.FaceFrame.WinLeft + Dlg.WinLeft;
        R.Right = this.FaceFrame.WinWidth + R.Left;
        R.Top = this.FaceFrame.WinTop + Dlg.WinTop;
        R.Bottom = this.FaceFrame.WinHeight + R.Top;

        hero = _armyInfo.ArmyView.Hero(_armyInfo.Selection, RosterTables.AllHeroTypes);
        if (hero.GetFace() != null)
        {
            hero.ShowFace(R, surface);
        }
        else
        {
            I = 0;
            while (((1 << I) & _armyInfo.Selection) == 0 && I != _armyInfo.ArmyView.GetCount())
            {
                I += 1;
            }

            if (I < _armyInfo.ArmyView.GetCount())
            {
                _armyInfo.ArmyView.GetUnit(I).ShowFace(R, surface);
            }
        }
    }

    // AoWHeroInfoDlg.pas:175
    public void U1PreDraw(Frame sender, LockedSurface surface, int slot)
    {
        Rect R = default;
        R.Left = sender.WinLeft + UnitPnl.WinLeft + 1;
        R.Right = sender.WinWidth + R.Left;
        R.Top = sender.WinTop + UnitPnl.WinTop;
        R.Bottom = sender.WinHeight + R.Top;

        DrawUnit(slot, R, ref surface);
    }

    // AoWHeroInfoDlg.pas:189
    public void U1MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        var count = _armyInfo.ArmyView.GetCount();
        if (slot < count)
        {
            var troop = _armyInfo.ArmyView.GetUnit(slot);
            DialogHosts.General.SelectedUnit = troop;
            if (button == MouseButton.Left)
            {
                _detailsOpen = true;
                DialogHosts.UnitWindow.ShowUnit();
                DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
            }
        }
    }

    // AoWHeroInfoDlg.pas:203
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWHeroInfoDlg.pas:208
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27 || key == 32)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowHeroInfo(HeroInfoDialog dialog, ArmyInfoEventLog eventValue, ExecuteDoneEvent done)
    {
        Hero hero;
        Assert(dialog._armyInfo == null, "", "AoWHeroInfoDlg.pas", 80);
        dialog._done = done;
        dialog._armyInfo = eventValue;
        dialog._armyInfo.AddRef();
        var text = eventValue.Messages.Text;
        dialog.Message.Items.Text = text;
        hero = dialog._armyInfo.ArmyView.Hero(dialog._armyInfo.Selection, RosterTables.AllHeroTypes);
        if (hero != null)
        {
            dialog.HeroName.Text = hero.HeroName;
            var level = hero.GetLevel();
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.LevelFormat), [IntToStr(level)]);
            dialog.HeroLevel.Text = format;
        }
        else
        {
            dialog.HeroName.Text = "";
            dialog.HeroLevel.Text = "";
        }

        dialog.Dlg.Visible = true;
        dialog._detailsOpen = false;
    }

}
