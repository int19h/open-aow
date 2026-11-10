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
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Session.E2CampaignSupportUnit;
using static Aow.Game.Ui.Dialogs.aowIntGfxModUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// done callback, army event, unit-details flag
public partial class THeroInfoDlg : TDataModule
{
    public THeroInfoDlg(TComponent owner) : base(owner)
    { }

    public TAOWPanel UnitPnl;
    public TAoWFrame U1;
    public TAoWFrame U2;
    public TAoWFrame U4;
    public TAoWFrame U5;
    public TAoWFrame U3;
    public TAoWFrame U6;
    public TAoWFrame U7;
    public TAoWFrame U8;
    public TAOWButton CloseBtn;
    public TAoWFrame BCeltL;
    public TAOWWindow Dlg;
    public TAOWLabel HeroName;
    public TAOWLabel HeroLevel;
    public TAOWImage FaceFrame;
    public TAOWLabel DetailsLbl;
    public TAOWMemo Message;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TArmyInfoEventLog FArmyInfo;
    public bool FDetailsOpen;

    // AoWHeroInfoDlg.pas:99
    public void FinishDialog()
    {
        TExecuteDoneEvent Done;
        TEventLog Event;
        if (FDetailsOpen)
        {
            FDetailsOpen = false;
            AoWUnitWindowUnit.UnitWindow.UnitWin.Priority = AOWControlWinUnit.ControlWin.ControlWindow.Priority;
            AoWUnitWindowUnit.UnitWindow.UnitWin.Visible = false;
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
    public void DrawUnit(int Index, in TRect R, ref TSurfaceDesc Surface)
    {
        TAlignment Alignment;
        int X;
        int Y;
        TRect OldClip;
        TP2UnitShowInfo Info;
        if (Index < FArmyInfo.FArmyView.GetCount())
        {
            OldClip = Surface.ClipRect;
            Surface.ClipRect = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            Alignment = FArmyInfo.FArmyView.GetUnit(Index).GetAlignment();
            if ((int)Alignment < IntGfxMod.UnitBGs.Count)
            {
                var image = IntGfxMod.UnitBGs[(int)Alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
            FArmyInfo.FArmyView.GetUnit(Index).ShowEx(X - 33, Y - 35, Surface, Info);
            if ((((int)(((uint)FArmyInfo.FSelection) >> Index)) & 1) == 0)
            {
                var image = IntGfxMod.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Surface.ClipRect = OldClip;
        }
    }

    // AoWHeroInfoDlg.pas:146
    public void CloseBtnClick()
    {
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWHeroInfoDlg.pas:152
    public void FaceFramePreDraw(TSurfaceDesc Surface)
    {
        TRect R = default;
        THero Hero;
        int I;
        R.Left = this.FaceFrame.WinLeft + Dlg.WinLeft;
        R.Right = this.FaceFrame.WinWidth + R.Left;
        R.Top = this.FaceFrame.WinTop + Dlg.WinTop;
        R.Bottom = this.FaceFrame.WinHeight + R.Top;

        Hero = FArmyInfo.FArmyView.Hero(FArmyInfo.FSelection, AllHeroTypes);
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
    public void U1PreDraw(TAoWFrame Sender, TSurfaceDesc Surface, int Slot)
    {
        TRect R = default;
        R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
        R.Right = Sender.WinWidth + R.Left;
        R.Top = Sender.WinTop + UnitPnl.WinTop;
        R.Bottom = Sender.WinHeight + R.Top;

        DrawUnit(Slot, R, ref Surface);
    }

    // AoWHeroInfoDlg.pas:189
    public void U1MouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y, int Slot)
    {
        var count = FArmyInfo.FArmyView.GetCount();
        if (Slot < count)
        {
            var unit = FArmyInfo.FArmyView.GetUnit(Slot);
            General.FSelectedUnit = unit;
            if (Button == TMouseButton.mbLeft)
            {
                FDetailsOpen = true;
                AoWUnitWindowUnit.UnitWindow.ShowUnit();
                AoWUnitWindowUnit.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
            }
        }
    }

    // AoWHeroInfoDlg.pas:203
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWHeroInfoDlg.pas:208
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWHeroInfoDlgUnit
{
    // AoWHeroInfoDlg.pas:61
    public static void ShowHeroInfo(THeroInfoDlg Dialog, TArmyInfoEventLog Event, TExecuteDoneEvent Done)
    {
        THero Hero;
        Assert(Dialog.FArmyInfo == null, "", "AoWHeroInfoDlg.pas", 80);
        Dialog.FDone = Done;
        Dialog.FArmyInfo = Event;
        Dialog.FArmyInfo.AddRef();
        var text = Event.FMessages.Text;
        Dialog.Message.Items.Text = text;
        Hero = Dialog.FArmyInfo.FArmyView.Hero(Dialog.FArmyInfo.FSelection, AllHeroTypes);
        if (Hero != null)
        {
            Dialog.HeroName.Text = Hero.FHeroName;
            var level = Hero.GetLevel();
            var format = AoWEUnit.Format(TranslateRStr(LevelXRStr), [IntToStr(level)]);
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
