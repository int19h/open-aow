// Translated from AOWControlWin.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AOWControlWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// published controls. inverted army selection mask
public partial class ControlWin : DataModule
{
    public ControlWin(Component owner) : base(owner)
    { }

    public AOWWindow ControlWindow;
    public AOWLabel CWTitle;
    public AOWButton CWMinIcon;
    public AoWFrame Unit1Frame;
    public AoWFrame Unit2Frame;
    public AoWFrame Unit3Frame;
    public AoWFrame Unit4Frame;
    public AoWFrame Unit5Frame;
    public AoWFrame Unit6Frame;
    public AoWFrame Unit7Frame;
    public AoWFrame Unit8Frame;
    public AOWImage CTitleIcon;
    public AOWLabel MovesLbl;
    public AOWLabel PartyStatusLbl;
    public AOWButton Close;
    public AOWImage U1MIcon;
    public AOWImage U2MIcon;
    public AOWImage U3MIcon;
    public AOWImage U4MIcon;
    public AOWImage U5MIcon;
    public AOWImage U6MIcon;
    public AOWImage U7MIcon;
    public AOWImage U8MIcon;
    public AOWImage MoraleMask;
    public AOWImage MoveIcon;
    public AOWPanel PartyT1Pnl;
    public AOWPanel PartyBPnl;
    public AOWPanel PartyT2Pnl;
    public AOWPanel PartyT3Pnl;
    public AOWPanel PartyT4Pnl;
    public AoWGauge Unit1Gge;
    public AoWGauge Unit2Gge;
    public AoWGauge Unit3Gge;
    public AoWGauge Unit4Gge;
    public AoWGauge Unit5Gge;
    public AoWGauge Unit6Gge;
    public AoWGauge Unit7Gge;
    public AoWGauge Unit8Gge;
    public AOWCheckBox Unit1Chk;
    public AOWCheckBox Unit2Chk;
    public AOWCheckBox Unit3Chk;
    public AOWCheckBox Unit5Chk;
    public AOWCheckBox Unit6Chk;
    public AOWCheckBox Unit7Chk;
    public AOWCheckBox Unit4Chk;
    public AOWCheckBox Unit8Chk;
    public AoWFrame Emp1Fr;
    public AoWFrame Emp2Fr;
    public AoWFrame Emp3Fr;
    public AoWFrame Emp4Fr;
    public AoWFrame Emp5Fr;
    public AoWFrame Emp6Fr;
    public AoWFrame Emp7Fr;
    public AoWFrame Emp8Fr;
    public AoWFrame Emp1Chk;
    public AoWFrame Emp2Chk;
    public AoWFrame Emp3Chk;
    public AoWFrame Emp4Chk;
    public AoWFrame Emp5Chk;
    public AoWFrame Emp6Chk;
    public AoWFrame Emp7Chk;
    public AoWFrame Emp8Chk;
    public AOWPanel PartyIPnl;
    public AOWLabel PartyILbl;
    public AOWButton Center;
    public AOWButton Help;
    public AOWButton PPrev;
    public AOWButton PMove;
    public AOWButton PNext;
    public AOWPanel PControlPnl;
    public AOWCheckBox PGuard;
    public AoWFrame AoWFrame2;
    public AOWPanel PartyB2Pbl;
    public AoWFrame AoWFrame1;
    public IvTranslator Translator;
    public AOWLabel PartyActionLbl1;
    public AOWLabel PartyActionLbl2;
    public byte FSelection;

    // AOWControlWin.pas:294
    public void DrawUnit(int Index, in Rect R, ref SurfaceDesc SD)
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
        ControlWindow.Visible = false;
    }

    // AOWControlWin.pas:318
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        ControlWindow.Visible = false;
        DialogHosts.TBWindow.ConButton.Visible = false;
    }

    // AOWControlWin.pas:324
    public void ControlWindowResize()
    {
        this.ControlWindow.Alignment.RightOffset = this.ControlWindow.WinManager.WinWidth - this.ControlWindow.WinLeft - this.ControlWindow.WinWidth;
        this.ControlWindow.Alignment.TopOffset = this.ControlWindow.WinTop;
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
    public void Unit1ChkChange(AOWCheckBox Sender)
    {
        CheckStart();
        World.Map.AoWHSMap.Current.FSelectedArmy.SetSelection(FSelection);
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
        FSelection = World.Map.AoWHSMap.Current.FSelectedArmy.GetSelection();
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

        var selection = World.Map.AoWHSMap.Current.FSelectedArmy.GetSelection();
        var partyMove = DialogHosts.General.FUnitList.GetPartyMoves(selection);
        var mTtoInt = DialogHosts.General.MTtoInt(partyMove);
        MoveIcon.CurrentImage = mTtoInt;
        var fSelectedArmySelection = World.Map.AoWHSMap.Current.FSelectedArmy.GetSelection();
        var movePoints = DialogHosts.General.FUnitList.MovePoints(fSelectedArmySelection);
        MovesLbl.Text = IntToStr(movePoints);
        var selection2 = World.Map.AoWHSMap.Current.FSelectedArmy.GetSelection();
        var partyStatus = World.Map.AoWHSMap.Current.FSelectedArmy.GetArmyHS().FArmy.GetPartyStatus(selection2);
        var partyStatusText = AbilityRules.TranslateRStr(TPartyStatusRStr[(int)partyStatus]);
        PartyStatusLbl.Text = partyStatusText;
    }

    // AOWControlWin.pas:407
    public void Unit1ChkMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (Button == MouseButton.Right)
        {
            CheckStart();
            World.Map.AoWHSMap.Current.FSelectedArmy.SetSelection((byte)~FSelection);
            CheckEnd();
        }
    }

    // AOWControlWin.pas:415
    public void CenterClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var army = DialogHosts.General.FUnitList;
        World.Map.AoWHSMap.Current.CenterViewTo(army.FX, army.FY, army.FL);

        var army2 = DialogHosts.General.FUnitList;
        World.Map.AoWHSMap.Current.PlayCenterAnimation(army2.FX, army2.FY, army2.FL);
    }

    // AOWControlWin.pas:422
    public void Unit1FramePreDraw(AoWFrame Sender, SurfaceDesc SurfDesc, int Slot)
    {
        Rect SaveRect;
        AoWComponent Component;
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
        World.Map.AoWHSMap.Current.SelectPrevArmy();
    }

    // AOWControlWin.pas:447
    public void PNextClick()
    {
        World.Map.AoWHSMap.Current.SelectNextArmy();
    }

    // AOWControlWin.pas:452
    public void PMoveClick()
    {
        World.Map.AoWHSMap.Current.FSelectedArmy.ExecuteMove();
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
    private static Engine.Buffers.FixedArray_string_5 TPartyStatusRStr = Engine.Buffers.FixedArray_string_5.From(["Unruly", "Unrest", "Stable", "Content", "Cheerful"]);
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
