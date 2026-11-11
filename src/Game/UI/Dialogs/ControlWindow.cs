// Translated from AOWControlWin.pas.
using Aow.Engine.Buffers;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// published controls. inverted army selection mask
public partial class ControlWindow : DataModule
{
    public ControlWindow(Component owner) : base(owner)
    { }

    public Window MainWindow;
    public Label CWTitle;
    public Button CWMinIcon;
    public Frame Unit1Frame;
    public Frame Unit2Frame;
    public Frame Unit3Frame;
    public Frame Unit4Frame;
    public Frame Unit5Frame;
    public Frame Unit6Frame;
    public Frame Unit7Frame;
    public Frame Unit8Frame;
    public Image CTitleIcon;
    public Label MovesLbl;
    public Label PartyStatusLbl;
    public Button Close;
    public Image U1MIcon;
    public Image U2MIcon;
    public Image U3MIcon;
    public Image U4MIcon;
    public Image U5MIcon;
    public Image U6MIcon;
    public Image U7MIcon;
    public Image U8MIcon;
    public Image MoraleMask;
    public Image MoveIcon;
    public Panel PartyT1Pnl;
    public Panel PartyBPnl;
    public Panel PartyT2Pnl;
    public Panel PartyT3Pnl;
    public Panel PartyT4Pnl;
    public Gauge Unit1Gge;
    public Gauge Unit2Gge;
    public Gauge Unit3Gge;
    public Gauge Unit4Gge;
    public Gauge Unit5Gge;
    public Gauge Unit6Gge;
    public Gauge Unit7Gge;
    public Gauge Unit8Gge;
    public CheckBox Unit1Chk;
    public CheckBox Unit2Chk;
    public CheckBox Unit3Chk;
    public CheckBox Unit5Chk;
    public CheckBox Unit6Chk;
    public CheckBox Unit7Chk;
    public CheckBox Unit4Chk;
    public CheckBox Unit8Chk;
    public Frame Emp1Fr;
    public Frame Emp2Fr;
    public Frame Emp3Fr;
    public Frame Emp4Fr;
    public Frame Emp5Fr;
    public Frame Emp6Fr;
    public Frame Emp7Fr;
    public Frame Emp8Fr;
    public Frame Emp1Chk;
    public Frame Emp2Chk;
    public Frame Emp3Chk;
    public Frame Emp4Chk;
    public Frame Emp5Chk;
    public Frame Emp6Chk;
    public Frame Emp7Chk;
    public Frame Emp8Chk;
    public Panel PartyIPnl;
    public Label PartyILbl;
    public Button Center;
    public Button Help;
    public Button PPrev;
    public Button PMove;
    public Button PNext;
    public Panel PControlPnl;
    public CheckBox PGuard;
    public Frame AoWFrame2;
    public Panel PartyB2Pbl;
    public Frame AoWFrame1;
    public IvTranslator Translator;
    public Label PartyActionLbl1;
    public Label PartyActionLbl2;
    public byte FSelection;

    // AOWControlWin.pas:294
    public void DrawUnit(int Index, in Rect R, ref LockedSurface SD)
    {
        if (DialogHosts.General.FSelectedUnits[Index] != -1)
        {
            if (DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[Index]).GetPreviewImage() != null)
            {
                SD.ClipRect = R;
                // { Original tests the unsorted index when checking the background library. }
                if ((int)DialogHosts.General.FUnitList.GetUnit(Index).GetAlignment() < DialogHosts.IntGfxMod.UnitBGs.Count)
                {
                    var alignment = DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[Index]).GetAlignment();
                    var unitBG = DialogHosts.IntGfxMod.UnitBGs[(int)alignment];
                    unitBG.ShowClipped(R.Left + (R.Right - R.Left - unitBG.Width) / 2, R.Top + (R.Bottom - R.Top - unitBG.Height) / 2, SD);
                }

                var previewImage = DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[Index]).GetPreviewImage();
                var previewImage2 = DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[Index]).GetPreviewImage();
                DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[Index]).ShowEx(R.Left + (R.Right - R.Left - previewImage2.Width) / 2, R.Top + (R.Bottom - R.Top - previewImage.Height) / 2, SD, DialogHosts.General.FUnitShowInfo);
            }
        }
    }

    // AOWControlWin.pas:312
    public void CWMinIconClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AOWControlWin.pas:318
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        MainWindow.Visible = false;
        DialogHosts.TBWindow.ConButton.Visible = false;
    }

    // AOWControlWin.pas:324
    public void ControlWindowResize()
    {
        this.MainWindow.Alignment.RightOffset = this.MainWindow.WinManager.WinWidth - this.MainWindow.WinLeft - this.MainWindow.WinWidth;
        this.MainWindow.Alignment.TopOffset = this.MainWindow.WinTop;
    }

    // AOWControlWin.pas:333
    public void Unit1FrameMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y, int Slot)
    {
        if (DialogHosts.General.FUnitList != null && DialogHosts.General.FSelectedUnits[Slot] > -1 && DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[Slot]) != null)
        {
            var unit = DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[Slot]);
            DialogHosts.General.FSelectedUnit = unit;
            if (Button == MouseButton.Left)
            {
                DialogHosts.UnitWindow.ShowUnit();
            }
        }
    }

    // AOWControlWin.pas:348
    public void Unit1ChkChange(CheckBox Sender)
    {
        CheckStart();
        AoWHSMap.Current.FSelectedArmy.SetSelection(FSelection);
        CheckEnd();
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AOWControlWin.pas:354
    public void CheckStart()
    {
        FSelection = 0;
        if (!Unit1Chk.Checked && DialogHosts.General.FSelectedUnits[0] != -1)
        {
            FSelection = (byte)(FSelection | (1 << DialogHosts.General.FSelectedUnits[0]));
        }

        if (!Unit2Chk.Checked && DialogHosts.General.FSelectedUnits[1] != -1)
        {
            FSelection = (byte)(FSelection | (1 << DialogHosts.General.FSelectedUnits[1]));
        }

        if (!Unit3Chk.Checked && DialogHosts.General.FSelectedUnits[2] != -1)
        {
            FSelection = (byte)(FSelection | (1 << DialogHosts.General.FSelectedUnits[2]));
        }

        if (!Unit4Chk.Checked && DialogHosts.General.FSelectedUnits[3] != -1)
        {
            FSelection = (byte)(FSelection | (1 << DialogHosts.General.FSelectedUnits[3]));
        }

        if (!Unit5Chk.Checked && DialogHosts.General.FSelectedUnits[4] != -1)
        {
            FSelection = (byte)(FSelection | (1 << DialogHosts.General.FSelectedUnits[4]));
        }

        if (!Unit6Chk.Checked && DialogHosts.General.FSelectedUnits[5] != -1)
        {
            FSelection = (byte)(FSelection | (1 << DialogHosts.General.FSelectedUnits[5]));
        }

        if (!Unit7Chk.Checked && DialogHosts.General.FSelectedUnits[6] != -1)
        {
            FSelection = (byte)(FSelection | (1 << DialogHosts.General.FSelectedUnits[6]));
        }

        if (!Unit8Chk.Checked && DialogHosts.General.FSelectedUnits[7] != -1)
        {
            FSelection = (byte)(FSelection | (1 << DialogHosts.General.FSelectedUnits[7]));
        }
    }

    // AOWControlWin.pas:375
    public void CheckEnd()
    {
        FSelection = AoWHSMap.Current.FSelectedArmy.GetSelection();
        Unit1Chk.Checked = DialogHosts.General.FSelectedUnits[0] != -1 && (FSelection & (1 << DialogHosts.General.FSelectedUnits[0])) == 0;
        Unit2Chk.Checked = DialogHosts.General.FSelectedUnits[1] != -1 && (FSelection & (1 << DialogHosts.General.FSelectedUnits[1])) == 0;
        Unit3Chk.Checked = DialogHosts.General.FSelectedUnits[2] != -1 && (FSelection & (1 << DialogHosts.General.FSelectedUnits[2])) == 0;
        Unit4Chk.Checked = DialogHosts.General.FSelectedUnits[3] != -1 && (FSelection & (1 << DialogHosts.General.FSelectedUnits[3])) == 0;
        Unit5Chk.Checked = DialogHosts.General.FSelectedUnits[4] != -1 && (FSelection & (1 << DialogHosts.General.FSelectedUnits[4])) == 0;
        Unit6Chk.Checked = DialogHosts.General.FSelectedUnits[5] != -1 && (FSelection & (1 << DialogHosts.General.FSelectedUnits[5])) == 0;
        Unit7Chk.Checked = DialogHosts.General.FSelectedUnits[6] != -1 && (FSelection & (1 << DialogHosts.General.FSelectedUnits[6])) == 0;
        Unit8Chk.Checked = DialogHosts.General.FSelectedUnits[7] != -1 && (FSelection & (1 << DialogHosts.General.FSelectedUnits[7])) == 0;
        if (Unit1Chk.Checked)
        {
            Unit1Frame.ILIndexFrame = 26;
        }
        else
        {
            Unit1Frame.ILIndexFrame = 12;
        }

        if (Unit2Chk.Checked)
        {
            Unit2Frame.ILIndexFrame = 26;
        }
        else
        {
            Unit2Frame.ILIndexFrame = 12;
        }

        if (Unit3Chk.Checked)
        {
            Unit3Frame.ILIndexFrame = 26;
        }
        else
        {
            Unit3Frame.ILIndexFrame = 12;
        }

        if (Unit4Chk.Checked)
        {
            Unit4Frame.ILIndexFrame = 26;
        }
        else
        {
            Unit4Frame.ILIndexFrame = 12;
        }

        if (Unit5Chk.Checked)
        {
            Unit5Frame.ILIndexFrame = 26;
        }
        else
        {
            Unit5Frame.ILIndexFrame = 12;
        }

        if (Unit6Chk.Checked)
        {
            Unit6Frame.ILIndexFrame = 26;
        }
        else
        {
            Unit6Frame.ILIndexFrame = 12;
        }

        if (Unit7Chk.Checked)
        {
            Unit7Frame.ILIndexFrame = 26;
        }
        else
        {
            Unit7Frame.ILIndexFrame = 12;
        }

        if (Unit8Chk.Checked)
        {
            Unit8Frame.ILIndexFrame = 26;
        }
        else
        {
            Unit8Frame.ILIndexFrame = 12;
        }

        var selection = AoWHSMap.Current.FSelectedArmy.GetSelection();
        var partyMove = DialogHosts.General.FUnitList.GetPartyMoves(selection);
        var mTtoInt = DialogHosts.General.MTtoInt(partyMove);
        MoveIcon.CurrentImage = mTtoInt;
        var fSelectedArmySelection = AoWHSMap.Current.FSelectedArmy.GetSelection();
        var movePoints = DialogHosts.General.FUnitList.MovePoints(fSelectedArmySelection);
        MovesLbl.Text = IntToStr(movePoints);
        var selection2 = AoWHSMap.Current.FSelectedArmy.GetSelection();
        var partyStatus = AoWHSMap.Current.FSelectedArmy.GetArmyHS().FArmy.GetPartyStatus(selection2);
        var partyStatusText = AbilityRules.TranslateRStr(TPartyStatusRStr[(int)partyStatus]);
        PartyStatusLbl.Text = partyStatusText;
    }

    // AOWControlWin.pas:407
    public void Unit1ChkMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (Button == MouseButton.Right)
        {
            CheckStart();
            AoWHSMap.Current.FSelectedArmy.SetSelection((byte)~FSelection);
            CheckEnd();
        }
    }

    // AOWControlWin.pas:415
    public void CenterClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var army = DialogHosts.General.FUnitList;
        AoWHSMap.Current.CenterViewTo(army.FX, army.FY, army.FL);

        var army2 = DialogHosts.General.FUnitList;
        AoWHSMap.Current.PlayCenterAnimation(army2.FX, army2.FY, army2.FL);
    }

    // AOWControlWin.pas:422
    public void Unit1FramePreDraw(Frame Sender, LockedSurface SurfDesc, int Slot)
    {
        Rect SaveRect;
        UIComponent Component;
        if (DialogHosts.General.FUnitList != null)
        {
            Component = Sender;
            SaveRect = SurfDesc.ClipRect;
            var rect = new Rect(Component.AOWOwner.WinLeft + Component.WinLeft + 4, Component.AOWOwner.WinTop + Component.WinTop + 3, Component.AOWOwner.WinLeft + Component.WinLeft + Component.WinWidth - 3, Component.AOWOwner.WinTop + Component.WinTop + Component.WinHeight - 7);
            DrawUnit(Slot, rect, ref SurfDesc);
            SurfDesc.ClipRect = SaveRect;
        }
    }

    // AOWControlWin.pas:437
    public void HelpClick()
    {
        HelpWindow.ShowHelp(503);
    }

    // AOWControlWin.pas:442
    public void PPrevClick()
    {
        AoWHSMap.Current.SelectPrevArmy();
    }

    // AOWControlWin.pas:447
    public void PNextClick()
    {
        AoWHSMap.Current.SelectNextArmy();
    }

    // AOWControlWin.pas:452
    public void PMoveClick()
    {
        AoWHSMap.Current.FSelectedArmy.ExecuteMove();
    }

    // AOWControlWin.pas:457
    public void PGuardChange()
    {
        if (PGuard.Checked)
        {
            if (!DialogHosts.General.FUnitList.FUserMode.Contains(ArmyUserModeElement.Guard))
            {
                DialogHosts.General.FUnitList.FUserMode = DialogHosts.General.FUnitList.FUserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            }
        }
        else
        {
            if (DialogHosts.General.FUnitList.FUserMode.Contains(ArmyUserModeElement.Guard))
            {
                DialogHosts.General.FUnitList.FUserMode = DialogHosts.General.FUnitList.FUserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            }
        }
    }

    private static readonly string PartyOwnerXRStr = "Party Owner: %s";
    private static StringArray5 TPartyStatusRStr = StringArray5.From(["Unruly", "Unrest", "Stable", "Content", "Cheerful"]);
    private static readonly string PartyActionRStr = "Party Action:";
    private static readonly string PatrollingRStr = "Patrolling";
    private static readonly string GuardingRStr = "Guarding";
    private static readonly string ScoutingRStr = "Scouting";
    private static readonly string SeekingRefugeRStr = "Seeking Refuge";
    private static readonly string RaidingRStr = "Raiding";
    private static readonly string HuntingRStr = "Hunting";
    private static readonly string ProtectingRStr = "Protecting";
    private static readonly string FortifiedRStr = "Fortified";
    private static readonly string BerserkRStr = "Berserk";
    private static readonly string WanderingRStr = "Wandering";
}
