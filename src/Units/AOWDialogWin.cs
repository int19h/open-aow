// Translated from AOWDialogWin.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.AOWDialogWin;

public partial class TDWin : TDataModule
{
    public TDWin(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dialog;
    public TAOWLabel DTitle;
    public TAOWLabel DMessage;
    public TAOWButton DButton;
    public TAOWWindow DebugWin;
    public TAOWLabel DebugDTitle;
    public TAOWButton DebugDOkBtn;
    public TAOWLabel FrameLbl;
    public TAOWLabel DebugDfrTitle;
    public TAOWMemo DebugMemo;
    public TAOWVScrollBar DMemoScroll;
    public TAOWPanel okPnl;
    public TAOWPanel yesPnl;
    public TAOWButton yesBtn;
    public TAOWButton noBtn;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TAoWFrame AoWFrame1;
    public TAoWFrame SignBLine;
    public TAOWImage SIcon;
    public TIvTranslator Translator;
    public byte FAction;
    public bool FSkipCombatChoice;
    // AOWDialogWin.pas:121
    public void Confirm(string Title, string Text, byte Action)
    {
        FAction = Action;
        DTitle.Text = Title;
        DMessage.Text = Text;
        yesPnl.Visible = true;
        okPnl.Visible = false;
        Dialog.Visible = true;
    }

    // AOWDialogWin.pas:113
    public void AddDebugMessage(string Text)
    {
        DebugMemo.Items.Add(Text);
        if (DebugMemo.Items.Count > 99)
        {
            DebugMemo.Items.Delete(0);
        }

        DebugMemo.FSized = false;
        DMemoScroll.Position = DMemoScroll.Max;
    }

    // AOWDialogWin.pas:60
    public void DButtonClick(TObject Sender)
    {
        General.PlayButtonSound(0);
        Dialog.Visible = false;
    }

    // AOWDialogWin.pas:67
    public void DebugDOkBtnClick()
    {
        General.PlayButtonSound(0);
        DebugWin.Visible = false;
    }

    // AOWDialogWin.pas:73
    public void yesBtnClick(TObject Sender)
    {
        string Reason = "";
        TStructure Structure;
        General.PlayButtonSound(0);
        switch (FAction)
        {
            case 0:
            case 1:
                break;
            case 2:
                General.FCity.Loot();
                break;
            case 3:
                if (General.FCity.CanRaze(AoWHSMap.FSeatedPlayer, ref Reason))
                {
                    if (General.FCity.Raze(AoWHSMap.FSeatedPlayer))
                    {
                        Structure = General.FStructure;
                        aowMapEvents.aowMapEvents.MapEvents.TheMapStructureDeselected(General.FStructure);
                        aowMapEvents.aowMapEvents.MapEvents.TheMapStructureSelected(Structure);
                    }
                }
                else
                {
                    AoWHSMap.ShowWarning(Reason);
                }

                break;
            case 4:
                AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).Surrender();
                break;
            case 5:
                AoWWorldMap.AoWWorldMap.WorldMap.WorldPnl.Visible = false;
                General.ReturnToTitle();
                break;
            case 6:
                if (AoWCombatMap != null)
                {
                    AoWCombatMap.Surrender(AoWCombatMap.FSeatedCombatPlayer);
                }

                break;
            case 7:
                if (General.FSelectedUnit != null)
                {
                    if (General.FSelectedUnit.CanDisband(ref Reason))
                    {
                        if (General.FSelectedUnit.Disband())
                        {
                            AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
                        }
                    }
                    else
                    {
                        AoWHSMap.ShowWarning(Reason);
                    }
                }

                break;
        }

        Dialog.Visible = false;
    }

    // AOWDialogWin.pas:104
    public void DialogKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        switch (Key)
        {
            case 27:
                DButtonClick(this);
                break;
            case 13:
                if (yesPnl.Visible)
                {
                    yesBtnClick(this);
                }
                else
                {
                    DButtonClick(this);
                }

                break;
        }
    }
}

public static partial class AOWDialogWin
{
}
