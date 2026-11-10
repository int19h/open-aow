// Translated from AoWHeroJoinDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWHeroJoinDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class HeroJoinDlg : DataModule
{
    public HeroJoinDlg(global::Aow.Rtl.Component owner) : base(owner)
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
    public AOWButton MiddleBtn;
    public AoWFrame BCeltL;
    public AOWButton RightBtn;
    public AOWButton LeftBtn;
    public AOWWindow Dlg;
    public AOWLabel HeroName;
    public AOWLabel HeroLevel;
    public AOWImage FaceFrame;
    public AOWLabel DetailsLbl;
    public AOWMemo JoinMemo;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public JoinEventLog FJoinEvent;
    public bool FDetailsOpen;
    public byte FSelection;
    public UnitUserList FUnits;
    public AbstractUnit FFaceUnit;

    // AoWHeroJoinDlg.pas:68
    public bool ValidateJoin()
    {
        bool Result = FJoinEvent.Validate();
        if (!Result)
        {
            Dlg.Visible = false;
        }

        return Result;
    }

    // AoWHeroJoinDlg.pas:74
    public void FinishDialog()
    {
        EventLog Event;
        ExecuteDoneEvent Done;
        FUnits.Clear();
        if (FDetailsOpen)
        {
            FDetailsOpen = false;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Priority = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Priority;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = false;
        }

        Done = FDone;
        Event = FJoinEvent;
        FDone = null;
        FJoinEvent = null;
        if (Done != null)
        {
            Done(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWHeroJoinDlg.pas:94
    public void DrawUnit(int Index, in global::Aow.Rtl.Rect R, ref SurfaceDesc Surface)
    {
        Alignment Alignment;
        int X;
        int Y;
        global::Aow.Rtl.Rect OldClip;
        P2UnitShowInfo Info;
        if (Index < FUnits.GetCount())
        {
            OldClip = Surface.ClipRect;
            Surface.ClipRect = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            Alignment = FUnits.GetUnit(Index).GetAlignment();
            if ((int)Alignment < Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                var image = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            FUnits.GetUnit(Index).ShowEx(X - 33, Y - 35, Surface, Info);
            if ((((int)(((uint)FSelection) >> Index)) & 1) == 0)
            {
                var image = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Surface.ClipRect = OldClip;
        }
    }

    // AoWHeroJoinDlg.pas:122
    public void HeroJoinDlgCreate()
    {
        FUnits = new UnitUserList(null);
    }

    // AoWHeroJoinDlg.pas:127
    public void HeroJoinDlgDestroy()
    {
        FUnits?.Dispose();
    }

    // AoWHeroJoinDlg.pas:132
    public void LeftBtnClick()
    {
        string Reason = "";
        if (ValidateJoin())
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            if (!FJoinEvent.Accept(ref Reason))
            {
                Aow.Game.World.Map.AoWHSMap.Current.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }
    }

    // AoWHeroJoinDlg.pas:144
    public void MiddleBtnClick()
    {
        string Reason = "";
        if (ValidateJoin())
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            if (!FJoinEvent.Reject(ref Reason))
            {
                Aow.Game.World.Map.AoWHSMap.Current.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }
    }

    // AoWHeroJoinDlg.pas:156
    public void RightBtnClick()
    {
        if (ValidateJoin())
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWHeroJoinDlg.pas:165
    public void FaceFramePreDraw(SurfaceDesc Surface)
    {
        global::Aow.Rtl.Rect R = default;
        if (ValidateJoin())
        {
            R.Left = FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = FaceFrame.WinWidth + R.Left;
            R.Top = FaceFrame.WinTop + Dlg.WinTop;
            R.Bottom = FaceFrame.WinHeight + R.Top;
            if (FFaceUnit.GetFace() != null)
            {
                FFaceUnit.GetFace().ShowStretched(R, Surface);
            }
        }
    }

    // AoWHeroJoinDlg.pas:179
    public void U1PreDraw(AoWFrame Sender, SurfaceDesc Surface, int Slot)
    {
        global::Aow.Rtl.Rect R = default;
        if (ValidateJoin())
        {
            R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
            R.Right = Sender.WinWidth + R.Left;
            R.Top = Sender.WinTop + UnitPnl.WinTop;
            R.Bottom = Sender.WinHeight + R.Top;

            DrawUnit(Slot, R, ref Surface);
        }
    }

    // AoWHeroJoinDlg.pas:196
    public void U1MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y, int Slot)
    {
        if (ValidateJoin())
        {
            var count = FUnits.GetCount();
            if (Slot < count)
            {
                var unit = FUnits.GetUnit(Slot);
                Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = unit;
                if (Button == MouseButton.Left)
                {
                    FDetailsOpen = true;
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
                }
            }
        }
    }

    // AoWHeroJoinDlg.pas:211
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWHeroJoinDlg.pas:216
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void UpdateHeroJoin(HeroJoinDlg Dialog)
    {
        if (Dialog.ValidateJoin())
        {
            Dialog.FUnits.Clear();
            Dialog.FFaceUnit = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
            Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
            var level = ((Hero)Dialog.FFaceUnit).GetLevel();
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LevelXRStr), [IntToStr(level)]);
            Dialog.HeroLevel.Text = format;
            Dialog.HeroName.Text = ((Hero)Dialog.FFaceUnit).FHeroName;
            var text = Dialog.FJoinEvent.FJoinMessage.Text;
            Dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowHeroJoin(HeroJoinDlg Dialog, HeroJoinEventLog Event, ExecuteDoneEvent Done)
    {
        Aow.Game.Ui.Dialogs.HeroJoinDlg.ResetHeroJoin(Dialog);
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        Dialog.MiddleBtn.Visible = false;
        var cancel = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CancelRStr);
        Dialog.RightBtn.Text = cancel;
        Aow.Game.Ui.Dialogs.HeroJoinDlg.UpdateHeroJoin(Dialog);
        Dialog.Dlg.Visible = true;
    }

    internal static void ShowHeroNegotiation(HeroJoinDlg Dialog, NegotiateRequestEventLog Event, ExecuteDoneEvent Done)
    {
        Aow.Game.Ui.Dialogs.HeroJoinDlg.ResetHeroJoin(Dialog);
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        var attack = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AttackRStr);
        Dialog.MiddleBtn.Text = attack;
        var cancel = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CancelRStr);
        Dialog.RightBtn.Text = cancel;
        Aow.Game.Ui.Dialogs.HeroJoinDlg.UpdateHeroJoin(Dialog);
        Dialog.Dlg.Visible = true;
    }

    internal static void ResetHeroJoin(HeroJoinDlg Dialog)
    {
        Dialog.LeftBtn.Visible = true;
        Dialog.MiddleBtn.Visible = true;
        Dialog.RightBtn.Visible = true;
    }

    internal static void ShowPrisonerHero(HeroJoinDlg Dialog, PrisonersJoinEventLog Event, ExecuteDoneEvent Done)
    {
        Aow.Game.Ui.Dialogs.HeroJoinDlg.ResetHeroJoin(Dialog);
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        Dialog.MiddleBtn.Visible = false;
        var reject = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.RejectRStr);
        Dialog.RightBtn.Text = reject;
        Aow.Game.Ui.Dialogs.HeroJoinDlg.UpdateHeroJoin(Dialog);
        Dialog.Dlg.Visible = true;
    }

}
