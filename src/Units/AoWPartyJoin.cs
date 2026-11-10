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
// Translated from AoWPartyJoin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class TPartyJoinDlg : TDataModule
{
    public TPartyJoinDlg(TComponent owner) : base(owner)
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
    public TAOWButton MiddleBtn;
    public TAOWImage TitleIcon;
    public TAOWLabel Title;
    public TAoWFrame BCeltL;
    public TAOWButton RightBtn;
    public TAOWButton LeftBtn;
    public TAOWWindow Dlg;
    public TAoWFrame UnitLine1;
    public TAoWFrame UnitLine2;
    public TAOWMemo JoinMemo;
    public TAOWLabel Header;
    public TAOWImage FaceFrame;
    public TAOWLabel DetailsLbl;
    public TAOWButton Close;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TJoinEventLog FJoinEvent;
    public bool FDetailsOpen;
    public byte FSelection;
    public TUnitUserList FUnits;
    public TAbstractUnit FFaceUnit;

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
        TEventLog Event;
        TExecuteDoneEvent Done;
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

    // AoWPartyJoin.pas:99
    public void DrawUnit(int Index, in TRect R, ref TSurfaceDesc Surface)
    {
        TAlignment Alignment;
        int X;
        int Y;
        TRect OldClip;
        TP2UnitShowInfo Info;
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
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
            FUnits.GetUnit(Index).ShowEx(X - 33, Y - 35, Surface, Info);
            if ((((int)(((uint)FSelection) >> Index)) & 1) == 0)
            {
                var image = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Surface.ClipRect = OldClip;
        }
    }

    // AoWPartyJoin.pas:127
    public void PartyJoinDlgCreate()
    {
        FUnits = new TUnitUserList(null);
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
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            if (!FJoinEvent.Accept(ref Reason))
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
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
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            if (!FJoinEvent.Reject(ref Reason))
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
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
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWPartyJoin.pas:170
    public void CloseClick()
    {
        if (ValidateJoin())
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWPartyJoin.pas:179
    public void FaceFramePreDraw(TSurfaceDesc Surface)
    {
        TRect R = default;
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
    public void U1PreDraw(TAoWFrame Sender, TSurfaceDesc Surface, int Slot)
    {
        TRect R = default;
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
    public void U1MouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y, int Slot)
    {
        if (ValidateJoin())
        {
            var count = FUnits.GetCount();
            if (Slot < count)
            {
                var unit = FUnits.GetUnit(Slot);
                Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = unit;
                if (Button == TMouseButton.mbLeft)
                {
                    FDetailsOpen = true;
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
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
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ResetPartyJoin(TPartyJoinDlg Dialog)
    {
        Dialog.LeftBtn.Visible = true;
        Dialog.MiddleBtn.Visible = true;
        Dialog.RightBtn.Visible = true;
        var partyJoin = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.ScenarioText.PartyJoinRStr);
        Dialog.Title.Text = partyJoin;
    }

    internal static void UpdatePartyJoin(TPartyJoinDlg Dialog)
    {
        if (Dialog.ValidateJoin())
        {
            Dialog.FUnits.Clear();
            Dialog.FFaceUnit = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
            Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
            var text = Dialog.FJoinEvent.FJoinMessage.Text;
            Dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowPartyJoin(TPartyJoinDlg Dialog, TNegotiateRequestEventLog Event, TExecuteDoneEvent Done)
    {
        Aow.Game.Ui.Dialogs.TPartyJoinDlg.ResetPartyJoin(Dialog);
        Dialog.Dlg.Visible = true;
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        var attack = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AttackRStr);
        Dialog.MiddleBtn.Text = attack;
        var cancel = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CancelRStr);
        Dialog.RightBtn.Text = cancel;
        Aow.Game.Ui.Dialogs.TPartyJoinDlg.UpdatePartyJoin(Dialog);
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(70, true, true);
    }

    internal static void ShowPrisonersJoin(TPartyJoinDlg Dialog, TPrisonersJoinEventLog Event, TExecuteDoneEvent Done)
    {
        Aow.Game.Ui.Dialogs.TPartyJoinDlg.ResetPartyJoin(Dialog);
        Dialog.Dlg.Visible = true;
        var prisonerLiberation = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TDungeon.PrisonerLiberationRStr);
        Dialog.Title.Text = prisonerLiberation;
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        Dialog.MiddleBtn.Visible = false;
        var reject = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.RejectRStr);
        Dialog.RightBtn.Text = reject;
        Aow.Game.Ui.Dialogs.TPartyJoinDlg.UpdatePartyJoin(Dialog);
    }

}
