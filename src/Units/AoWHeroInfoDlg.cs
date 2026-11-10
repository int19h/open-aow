// Translated from AoWHeroInfoDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AoWHeroInfoDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// done callback, army event, unit-details flag
public partial class HeroInfoDlg : DataModule
{
    public HeroInfoDlg(Component owner) : base(owner)
    { }

    public AOWPanel UnitPnl;
    public AoWFrame U1;
    public AoWFrame U2;
    public AoWFrame U4;
    public AoWFrame U5;
    public AoWFrame U3;
    public AoWFrame U6;
    public AoWFrame U7;
    public AoWFrame U8;
    public AOWButton CloseBtn;
    public AoWFrame BCeltL;
    public AOWWindow Dlg;
    public AOWLabel HeroName;
    public AOWLabel HeroLevel;
    public AOWImage FaceFrame;
    public AOWLabel DetailsLbl;
    public AOWMemo Message;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public ArmyInfoEventLog FArmyInfo;
    public bool FDetailsOpen;

    // AoWHeroInfoDlg.pas:99
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        EventLog Event;
        if (FDetailsOpen)
        {
            FDetailsOpen = false;
            DialogHosts.UnitWindow.UnitWin.Priority = DialogHosts.ControlWin.ControlWindow.Priority;
            DialogHosts.UnitWindow.UnitWin.Visible = false;
        }

        Done = FDone;
        Event = FArmyInfo;
        FDone = null;
        FArmyInfo = null;
        if (Done != null)
        {
            Done(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWHeroInfoDlg.pas:118
    public void DrawUnit(int Index, in Rect R, ref SurfaceDesc Surface)
    {
        Alignment Alignment;
        int X;
        int Y;
        Rect OldClip;
        P2UnitShowInfo Info;
        if (Index < FArmyInfo.FArmyView.GetCount())
        {
            OldClip = Surface.ClipRect;
            Surface.ClipRect = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            Alignment = FArmyInfo.FArmyView.GetUnit(Index).GetAlignment();
            if ((int)Alignment < DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                var image = DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            FArmyInfo.FArmyView.GetUnit(Index).ShowEx(X - 33, Y - 35, Surface, Info);
            if ((((int)(((uint)FArmyInfo.FSelection) >> Index)) & 1) == 0)
            {
                var image = DialogHosts.IntGfxMod.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Surface.ClipRect = OldClip;
        }
    }

    // AoWHeroInfoDlg.pas:146
    public void CloseBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWHeroInfoDlg.pas:152
    public void FaceFramePreDraw(SurfaceDesc Surface)
    {
        Rect R = default;
        Hero Hero;
        int I;
        R.Left = this.FaceFrame.WinLeft + Dlg.WinLeft;
        R.Right = this.FaceFrame.WinWidth + R.Left;
        R.Top = this.FaceFrame.WinTop + Dlg.WinTop;
        R.Bottom = this.FaceFrame.WinHeight + R.Top;

        Hero = FArmyInfo.FArmyView.Hero(FArmyInfo.FSelection, RosterTables.AllHeroTypes);
        if (Hero.GetFace() != null)
        {
            Hero.ShowFace(R, Surface);
        }
        else
        {
            I = 0;
            while (((1 << I) & FArmyInfo.FSelection) == 0 && I != FArmyInfo.FArmyView.GetCount())
            {
                I += 1;
            }

            if (I < FArmyInfo.FArmyView.GetCount())
            {
                FArmyInfo.FArmyView.GetUnit(I).ShowFace(R, Surface);
            }
        }
    }

    // AoWHeroInfoDlg.pas:175
    public void U1PreDraw(AoWFrame Sender, SurfaceDesc Surface, int Slot)
    {
        Rect R = default;
        R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
        R.Right = Sender.WinWidth + R.Left;
        R.Top = Sender.WinTop + UnitPnl.WinTop;
        R.Bottom = Sender.WinHeight + R.Top;

        DrawUnit(Slot, R, ref Surface);
    }

    // AoWHeroInfoDlg.pas:189
    public void U1MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y, int Slot)
    {
        var count = FArmyInfo.FArmyView.GetCount();
        if (Slot < count)
        {
            var unit = FArmyInfo.FArmyView.GetUnit(Slot);
            DialogHosts.General.FSelectedUnit = unit;
            if (Button == MouseButton.Left)
            {
                FDetailsOpen = true;
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
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowHeroInfo(HeroInfoDlg Dialog, ArmyInfoEventLog Event, ExecuteDoneEvent Done)
    {
        Hero Hero;
        Assert(Dialog.FArmyInfo == null, "", "AoWHeroInfoDlg.pas", 80);
        Dialog.FDone = Done;
        Dialog.FArmyInfo = Event;
        Dialog.FArmyInfo.AddRef();
        var text = Event.FMessages.Text;
        Dialog.Message.Items.Text = text;
        Hero = Dialog.FArmyInfo.FArmyView.Hero(Dialog.FArmyInfo.FSelection, RosterTables.AllHeroTypes);
        if (Hero != null)
        {
            Dialog.HeroName.Text = Hero.FHeroName;
            var level = Hero.GetLevel();
            var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.LevelXRStr), [IntToStr(level)]);
            Dialog.HeroLevel.Text = format;
        }
        else
        {
            Dialog.HeroName.Text = "";
            Dialog.HeroLevel.Text = "";
        }

        Dialog.Dlg.Visible = true;
        Dialog.FDetailsOpen = false;
    }

}
