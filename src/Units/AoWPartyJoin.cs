// Translated from AoWPartyJoin.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWPartyJoin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class PartyJoinDlg : DataModule
{
    public PartyJoinDlg(Component owner) : base(owner)
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
    public AOWImage TitleIcon;
    public AOWLabel Title;
    public AoWFrame BCeltL;
    public AOWButton RightBtn;
    public AOWButton LeftBtn;
    public AOWWindow Dlg;
    public AoWFrame UnitLine1;
    public AoWFrame UnitLine2;
    public AOWMemo JoinMemo;
    public AOWLabel Header;
    public AOWImage FaceFrame;
    public AOWLabel DetailsLbl;
    public AOWButton Close;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public JoinEventLog FJoinEvent;
    public bool FDetailsOpen;
    public byte FSelection;
    public UnitUserList FUnits;
    public AbstractUnit FFaceUnit;

    // AoWPartyJoin.pas:73
    public bool ValidateJoin()
    {
        bool Result = FJoinEvent.Validate();
        if (!Result)
        {
            Dlg.Visible = false;
        }

        return Result;
    }

    // AoWPartyJoin.pas:79
    public void FinishDialog()
    {
        EventLog Event;
        ExecuteDoneEvent Done;
        FUnits.Clear();
        if (FDetailsOpen)
        {
            FDetailsOpen = false;
            DialogHosts.UnitWindow.UnitWin.Priority = DialogHosts.ControlWin.ControlWindow.Priority;
            DialogHosts.UnitWindow.UnitWin.Visible = false;
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

    // AoWPartyJoin.pas:99
    public void DrawUnit(int Index, in Rect R, ref SurfaceDesc Surface)
    {
        Alignment Alignment;
        int X;
        int Y;
        Rect OldClip;
        P2UnitShowInfo Info;
        if (Index < FUnits.GetCount())
        {
            OldClip = Surface.ClipRect;
            Surface.ClipRect = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            Alignment = FUnits.GetUnit(Index).GetAlignment();
            if ((int)Alignment < DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                var image = DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            FUnits.GetUnit(Index).ShowEx(X - 33, Y - 35, Surface, Info);
            if ((((int)(((uint)FSelection) >> Index)) & 1) == 0)
            {
                var image = DialogHosts.IntGfxMod.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Surface.ClipRect = OldClip;
        }
    }

    // AoWPartyJoin.pas:127
    public void PartyJoinDlgCreate()
    {
        FUnits = new UnitUserList(null);
    }

    // AoWPartyJoin.pas:132
    public void PartyJoinDlgDestroy()
    {
        FUnits?.Dispose();
    }

    // AoWPartyJoin.pas:137
    public void LeftBtnClick()
    {
        string Reason = "";
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
            if (!FJoinEvent.Accept(ref Reason))
            {
                AoWHSMap.Current.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }
    }

    // AoWPartyJoin.pas:149
    public void MiddleBtnClick()
    {
        string Reason = "";
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
            if (!FJoinEvent.Reject(ref Reason))
            {
                AoWHSMap.Current.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }
    }

    // AoWPartyJoin.pas:161
    public void RightBtnClick()
    {
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWPartyJoin.pas:170
    public void CloseClick()
    {
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWPartyJoin.pas:179
    public void FaceFramePreDraw(SurfaceDesc Surface)
    {
        Rect R = default;
        if (ValidateJoin())
        {
            R.Left = FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = FaceFrame.WinWidth + R.Left;
            R.Top = FaceFrame.WinTop + Dlg.WinTop;
            R.Bottom = FaceFrame.WinHeight + R.Top;
            if (FFaceUnit != null)
            {
                if (FFaceUnit.GetFace() != null)
                {
                    FFaceUnit.GetFace().ShowStretched(R, Surface);
                }
            }
        }
    }

    // AoWPartyJoin.pas:194
    public void U1PreDraw(AoWFrame Sender, SurfaceDesc Surface, int Slot)
    {
        Rect R = default;
        if (ValidateJoin())
        {
            R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
            R.Right = Sender.WinWidth + R.Left;
            R.Top = Sender.WinTop + UnitPnl.WinTop;
            R.Bottom = Sender.WinHeight + R.Top;

            DrawUnit(Slot, R, ref Surface);
        }
    }

    // AoWPartyJoin.pas:211
    public void U1MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y, int Slot)
    {
        if (ValidateJoin())
        {
            var count = FUnits.GetCount();
            if (Slot < count)
            {
                var unit = FUnits.GetUnit(Slot);
                DialogHosts.General.FSelectedUnit = unit;
                if (Button == MouseButton.Left)
                {
                    FDetailsOpen = true;
                    DialogHosts.UnitWindow.ShowUnit();
                    DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
                }
            }
        }
    }

    // AoWPartyJoin.pas:226
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWPartyJoin.pas:231
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ResetPartyJoin(PartyJoinDlg Dialog)
    {
        Dialog.LeftBtn.Visible = true;
        Dialog.MiddleBtn.Visible = true;
        Dialog.RightBtn.Visible = true;
        var partyJoin = AbilityRules.TranslateRStr(ScenarioText.PartyJoinRStr);
        Dialog.Title.Text = partyJoin;
    }

    internal static void UpdatePartyJoin(PartyJoinDlg Dialog)
    {
        if (Dialog.ValidateJoin())
        {
            Dialog.FUnits.Clear();
            Dialog.FFaceUnit = AoWHSMap.Current.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
            Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
            var text = Dialog.FJoinEvent.FJoinMessage.Text;
            Dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowPartyJoin(PartyJoinDlg Dialog, NegotiateRequestEventLog Event, ExecuteDoneEvent Done)
    {
        ResetPartyJoin(Dialog);
        Dialog.Dlg.Visible = true;
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = AbilityRules.TranslateRStr(GameText.AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        var attack = AbilityRules.TranslateRStr(GameText.AttackRStr);
        Dialog.MiddleBtn.Text = attack;
        var cancel = AbilityRules.TranslateRStr(GameText.CancelRStr);
        Dialog.RightBtn.Text = cancel;
        UpdatePartyJoin(Dialog);
        AoWHSMap.Current.FTutorialControl.ShowMessage(70, true, true);
    }

    internal static void ShowPrisonersJoin(PartyJoinDlg Dialog, PrisonersJoinEventLog Event, ExecuteDoneEvent Done)
    {
        ResetPartyJoin(Dialog);
        Dialog.Dlg.Visible = true;
        var prisonerLiberation = AbilityRules.TranslateRStr(Dungeon.PrisonerLiberationRStr);
        Dialog.Title.Text = prisonerLiberation;
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = AbilityRules.TranslateRStr(GameText.AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        Dialog.MiddleBtn.Visible = false;
        var reject = AbilityRules.TranslateRStr(GameText.RejectRStr);
        Dialog.RightBtn.Text = reject;
        UpdatePartyJoin(Dialog);
    }

}
