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
public partial class TControlWin : TDataModule
{
    public TControlWin(TComponent owner) : base(owner)
    { }

    public TAOWWindow ControlWindow;
    public TAOWLabel CWTitle;
    public TAOWButton CWMinIcon;
    public TAoWFrame Unit1Frame;
    public TAoWFrame Unit2Frame;
    public TAoWFrame Unit3Frame;
    public TAoWFrame Unit4Frame;
    public TAoWFrame Unit5Frame;
    public TAoWFrame Unit6Frame;
    public TAoWFrame Unit7Frame;
    public TAoWFrame Unit8Frame;
    public TAOWImage CTitleIcon;
    public TAOWLabel MovesLbl;
    public TAOWLabel PartyStatusLbl;
    public TAOWButton Close;
    public TAOWImage U1MIcon;
    public TAOWImage U2MIcon;
    public TAOWImage U3MIcon;
    public TAOWImage U4MIcon;
    public TAOWImage U5MIcon;
    public TAOWImage U6MIcon;
    public TAOWImage U7MIcon;
    public TAOWImage U8MIcon;
    public TAOWImage MoraleMask;
    public TAOWImage MoveIcon;
    public TAOWPanel PartyT1Pnl;
    public TAOWPanel PartyBPnl;
    public TAOWPanel PartyT2Pnl;
    public TAOWPanel PartyT3Pnl;
    public TAOWPanel PartyT4Pnl;
    public TAoWGauge Unit1Gge;
    public TAoWGauge Unit2Gge;
    public TAoWGauge Unit3Gge;
    public TAoWGauge Unit4Gge;
    public TAoWGauge Unit5Gge;
    public TAoWGauge Unit6Gge;
    public TAoWGauge Unit7Gge;
    public TAoWGauge Unit8Gge;
    public TAOWCheckBox Unit1Chk;
    public TAOWCheckBox Unit2Chk;
    public TAOWCheckBox Unit3Chk;
    public TAOWCheckBox Unit5Chk;
    public TAOWCheckBox Unit6Chk;
    public TAOWCheckBox Unit7Chk;
    public TAOWCheckBox Unit4Chk;
    public TAOWCheckBox Unit8Chk;
    public TAoWFrame Emp1Fr;
    public TAoWFrame Emp2Fr;
    public TAoWFrame Emp3Fr;
    public TAoWFrame Emp4Fr;
    public TAoWFrame Emp5Fr;
    public TAoWFrame Emp6Fr;
    public TAoWFrame Emp7Fr;
    public TAoWFrame Emp8Fr;
    public TAoWFrame Emp1Chk;
    public TAoWFrame Emp2Chk;
    public TAoWFrame Emp3Chk;
    public TAoWFrame Emp4Chk;
    public TAoWFrame Emp5Chk;
    public TAoWFrame Emp6Chk;
    public TAoWFrame Emp7Chk;
    public TAoWFrame Emp8Chk;
    public TAOWPanel PartyIPnl;
    public TAOWLabel PartyILbl;
    public TAOWButton Center;
    public TAOWButton Help;
    public TAOWButton PPrev;
    public TAOWButton PMove;
    public TAOWButton PNext;
    public TAOWPanel PControlPnl;
    public TAOWCheckBox PGuard;
    public TAoWFrame AoWFrame2;
    public TAOWPanel PartyB2Pbl;
    public TAoWFrame AoWFrame1;
    public TIvTranslator Translator;
    public TAOWLabel PartyActionLbl1;
    public TAOWLabel PartyActionLbl2;
    public byte FSelection;

    // AOWControlWin.pas:294
    public void DrawUnit(int Index, in TRect R, ref TSurfaceDesc SD)
    {
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Index] != -1)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Index]).GetPreviewImage() != null)
            {
                SD.ClipRect = R;
                // { Original tests the unsorted index when checking the background library. }
                if ((int)Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Index).GetAlignment() < Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs.Count)
                {
                    var alignment = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Index]).GetAlignment();
                    var unitBG = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[(int)alignment];
                    unitBG.ShowClipped(R.Left + (R.Right - R.Left - unitBG.Width) / 2, R.Top + (R.Bottom - R.Top - unitBG.Height) / 2, SD);
                }

                var previewImage = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Index]).GetPreviewImage();
                var previewImage2 = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Index]).GetPreviewImage();
                Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Index]).ShowEx(R.Left + (R.Right - R.Left - previewImage2.Width) / 2, R.Top + (R.Bottom - R.Top - previewImage.Height) / 2, SD, Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitShowInfo);
            }
        }
    }

    // AOWControlWin.pas:312
    public void CWMinIconClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        ControlWindow.Visible = false;
    }

    // AOWControlWin.pas:318
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        ControlWindow.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ConButton.Visible = false;
    }

    // AOWControlWin.pas:324
    public void ControlWindowResize()
    {
        this.ControlWindow.Alignment.RightOffset = this.ControlWindow.WinManager.WinWidth - this.ControlWindow.WinLeft - this.ControlWindow.WinWidth;
        this.ControlWindow.Alignment.TopOffset = this.ControlWindow.WinTop;
    }

    // AOWControlWin.pas:333
    public void Unit1FrameMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y, int Slot)
    {
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList != null && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Slot] > -1 && Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Slot]) != null)
        {
            var unit = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Slot]);
            Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = unit;
            if (Button == TMouseButton.mbLeft)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
            }
        }
    }

    // AOWControlWin.pas:348
    public void Unit1ChkChange(TAOWCheckBox Sender)
    {
        CheckStart();
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.SetSelection(FSelection);
        CheckEnd();
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AOWControlWin.pas:354
    public void CheckStart()
    {
        FSelection = 0;
        if (!Unit1Chk.Checked && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[0] != -1)
        {
            FSelection = (byte)(FSelection | (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[0]));
        }

        if (!Unit2Chk.Checked && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[1] != -1)
        {
            FSelection = (byte)(FSelection | (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[1]));
        }

        if (!Unit3Chk.Checked && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[2] != -1)
        {
            FSelection = (byte)(FSelection | (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[2]));
        }

        if (!Unit4Chk.Checked && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[3] != -1)
        {
            FSelection = (byte)(FSelection | (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[3]));
        }

        if (!Unit5Chk.Checked && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[4] != -1)
        {
            FSelection = (byte)(FSelection | (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[4]));
        }

        if (!Unit6Chk.Checked && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[5] != -1)
        {
            FSelection = (byte)(FSelection | (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[5]));
        }

        if (!Unit7Chk.Checked && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[6] != -1)
        {
            FSelection = (byte)(FSelection | (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[6]));
        }

        if (!Unit8Chk.Checked && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[7] != -1)
        {
            FSelection = (byte)(FSelection | (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[7]));
        }
    }

    // AOWControlWin.pas:375
    public void CheckEnd()
    {
        FSelection = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.GetSelection();
        Unit1Chk.Checked = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[0] != -1 && (FSelection & (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[0])) == 0;
        Unit2Chk.Checked = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[1] != -1 && (FSelection & (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[1])) == 0;
        Unit3Chk.Checked = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[2] != -1 && (FSelection & (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[2])) == 0;
        Unit4Chk.Checked = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[3] != -1 && (FSelection & (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[3])) == 0;
        Unit5Chk.Checked = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[4] != -1 && (FSelection & (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[4])) == 0;
        Unit6Chk.Checked = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[5] != -1 && (FSelection & (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[5])) == 0;
        Unit7Chk.Checked = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[6] != -1 && (FSelection & (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[6])) == 0;
        Unit8Chk.Checked = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[7] != -1 && (FSelection & (1 << Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[7])) == 0;
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

        var selection = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.GetSelection();
        var partyMove = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetPartyMoves(selection);
        var mTtoInt = Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(partyMove);
        MoveIcon.CurrentImage = mTtoInt;
        var fSelectedArmySelection = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.GetSelection();
        var movePoints = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.MovePoints(fSelectedArmySelection);
        MovesLbl.Text = IntToStr(movePoints);
        var selection2 = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.GetSelection();
        var partyStatus = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.GetArmyHS().FArmy.GetPartyStatus(selection2);
        var partyStatusText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TControlWin.TPartyStatusRStr[(int)partyStatus]);
        PartyStatusLbl.Text = partyStatusText;
    }

    // AOWControlWin.pas:407
    public void Unit1ChkMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Button == TMouseButton.mbRight)
        {
            CheckStart();
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.SetSelection((byte)~FSelection);
            CheckEnd();
        }
    }

    // AOWControlWin.pas:415
    public void CenterClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        var army = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(army.FX, army.FY, army.FL);

        var army2 = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlayCenterAnimation(army2.FX, army2.FY, army2.FL);
    }

    // AOWControlWin.pas:422
    public void Unit1FramePreDraw(TAoWFrame Sender, TSurfaceDesc SurfDesc, int Slot)
    {
        TRect SaveRect;
        TAoWComponent Component;
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList != null)
        {
            Component = Sender;
            SaveRect = SurfDesc.ClipRect;
            var rect = Rect(Component.AOWOwner.WinLeft + Component.WinLeft + 4, Component.AOWOwner.WinTop + Component.WinTop + 3, Component.AOWOwner.WinLeft + Component.WinLeft + Component.WinWidth - 3, Component.AOWOwner.WinTop + Component.WinTop + Component.WinHeight - 7);
            DrawUnit(Slot, rect, ref SurfDesc);
            SurfDesc.ClipRect = SaveRect;
        }
    }

    // AOWControlWin.pas:437
    public void HelpClick()
    {
        Aow.Game.Ui.Dialogs.THelpWindow.ShowHelp(503);
    }

    // AOWControlWin.pas:442
    public void PPrevClick()
    {
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SelectPrevArmy();
    }

    // AOWControlWin.pas:447
    public void PNextClick()
    {
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SelectNextArmy();
    }

    // AOWControlWin.pas:452
    public void PMoveClick()
    {
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.ExecuteMove();
    }

    // AOWControlWin.pas:457
    public void PGuardChange()
    {
        if (PGuard.Checked)
        {
            if (!Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.FUserMode.Contains(TArmyUserModeElement.aumGuard))
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.FUserMode = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.FUserMode + TSet<TArmyUserModeElement>.Of(TArmyUserModeElement.aumGuard);
            }
        }
        else
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.FUserMode.Contains(TArmyUserModeElement.aumGuard))
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.FUserMode = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.FUserMode - TSet<TArmyUserModeElement>.Of(TArmyUserModeElement.aumGuard);
            }
        }
    }

    private static readonly string PartyOwnerXRStr = "Party Owner: %s";
    private static global::Aow.Game.Engine.Buffers.FixedArray_string_5 TPartyStatusRStr = global::Aow.Game.Engine.Buffers.FixedArray_string_5.From(["Unruly", "Unrest", "Stable", "Content", "Cheerful"]);
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
