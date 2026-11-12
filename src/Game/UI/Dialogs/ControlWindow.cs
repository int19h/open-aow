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
    public Frame Unit1Frame;
    public Frame Unit2Frame;
    public Frame Unit3Frame;
    public Frame Unit4Frame;
    public Frame Unit5Frame;
    public Frame Unit6Frame;
    public Frame Unit7Frame;
    public Frame Unit8Frame;
    public Label MovesLbl;
    public Label PartyStatusLbl;
    public Image U1MIcon;
    public Image U2MIcon;
    public Image U3MIcon;
    public Image U4MIcon;
    public Image U5MIcon;
    public Image U6MIcon;
    public Image U7MIcon;
    public Image U8MIcon;
    public Image MoveIcon;
    public Panel PartyBPnl;
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
    public Panel PControlPnl;
    public CheckBox PGuard;
    public Label PartyActionLbl1;
    public Label PartyActionLbl2;
    private byte _selection;

    // AOWControlWin.pas:294
    public void DrawUnit(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        if (DialogHosts.General.SelectedUnits[index] != -1)
        {
            if (DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[index]).GetPreviewImage() != null)
            {
                lockedSurface.ClipRectangle = R;
                // { Original tests the unsorted index when checking the background library. }
                if ((int)DialogHosts.General.UnitList.GetUnit(index).GetAlignment() < DialogHosts.UIGraphics.UnitBGs.Count)
                {
                    var alignment = DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[index]).GetAlignment();
                    var unitBackground = DialogHosts.UIGraphics.UnitBGs[(int)alignment];
                    unitBackground.ShowClipped(R.Left + (R.Right - R.Left - unitBackground.Width) / 2, R.Top + (R.Bottom - R.Top - unitBackground.Height) / 2, lockedSurface);
                }

                var previewImage = DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[index]).GetPreviewImage();
                var previewImage2 = DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[index]).GetPreviewImage();
                DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[index]).Show(R.Left + (R.Right - R.Left - previewImage2.Width) / 2, R.Top + (R.Bottom - R.Top - previewImage.Height) / 2, lockedSurface, DialogHosts.General.UnitShowInfo);
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
        DialogHosts.TaskBar.ConButton.Visible = false;
    }

    // AOWControlWin.pas:324
    public void ControlWindowResize()
    {
        this.MainWindow.Alignment.RightOffset = this.MainWindow.WindowManager.WinWidth - this.MainWindow.WinLeft - this.MainWindow.WinWidth;
        this.MainWindow.Alignment.TopOffset = this.MainWindow.WinTop;
    }

    // AOWControlWin.pas:333
    public void Unit1FrameMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        if (DialogHosts.General.UnitList != null && DialogHosts.General.SelectedUnits[slot] > -1 && DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[slot]) != null)
        {
            var troop = DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[slot]);
            DialogHosts.General.SelectedUnit = troop;
            if (button == MouseButton.Left)
            {
                DialogHosts.UnitWindow.ShowUnit();
            }
        }
    }

    // AOWControlWin.pas:348
    public void Unit1ChkChange(CheckBox sender)
    {
        CheckStart();
        AoWHSMap.Current.SelectedArmy.SetSelection(_selection);
        CheckEnd();
        if (sender.Checked)
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
        _selection = 0;
        if (!Unit1Chk.Checked && DialogHosts.General.SelectedUnits[0] != -1)
        {
            _selection = (byte)(_selection | (1 << DialogHosts.General.SelectedUnits[0]));
        }

        if (!Unit2Chk.Checked && DialogHosts.General.SelectedUnits[1] != -1)
        {
            _selection = (byte)(_selection | (1 << DialogHosts.General.SelectedUnits[1]));
        }

        if (!Unit3Chk.Checked && DialogHosts.General.SelectedUnits[2] != -1)
        {
            _selection = (byte)(_selection | (1 << DialogHosts.General.SelectedUnits[2]));
        }

        if (!Unit4Chk.Checked && DialogHosts.General.SelectedUnits[3] != -1)
        {
            _selection = (byte)(_selection | (1 << DialogHosts.General.SelectedUnits[3]));
        }

        if (!Unit5Chk.Checked && DialogHosts.General.SelectedUnits[4] != -1)
        {
            _selection = (byte)(_selection | (1 << DialogHosts.General.SelectedUnits[4]));
        }

        if (!Unit6Chk.Checked && DialogHosts.General.SelectedUnits[5] != -1)
        {
            _selection = (byte)(_selection | (1 << DialogHosts.General.SelectedUnits[5]));
        }

        if (!Unit7Chk.Checked && DialogHosts.General.SelectedUnits[6] != -1)
        {
            _selection = (byte)(_selection | (1 << DialogHosts.General.SelectedUnits[6]));
        }

        if (!Unit8Chk.Checked && DialogHosts.General.SelectedUnits[7] != -1)
        {
            _selection = (byte)(_selection | (1 << DialogHosts.General.SelectedUnits[7]));
        }
    }

    // AOWControlWin.pas:375
    public void CheckEnd()
    {
        _selection = AoWHSMap.Current.SelectedArmy.GetSelection();
        Unit1Chk.Checked = DialogHosts.General.SelectedUnits[0] != -1 && (_selection & (1 << DialogHosts.General.SelectedUnits[0])) == 0;
        Unit2Chk.Checked = DialogHosts.General.SelectedUnits[1] != -1 && (_selection & (1 << DialogHosts.General.SelectedUnits[1])) == 0;
        Unit3Chk.Checked = DialogHosts.General.SelectedUnits[2] != -1 && (_selection & (1 << DialogHosts.General.SelectedUnits[2])) == 0;
        Unit4Chk.Checked = DialogHosts.General.SelectedUnits[3] != -1 && (_selection & (1 << DialogHosts.General.SelectedUnits[3])) == 0;
        Unit5Chk.Checked = DialogHosts.General.SelectedUnits[4] != -1 && (_selection & (1 << DialogHosts.General.SelectedUnits[4])) == 0;
        Unit6Chk.Checked = DialogHosts.General.SelectedUnits[5] != -1 && (_selection & (1 << DialogHosts.General.SelectedUnits[5])) == 0;
        Unit7Chk.Checked = DialogHosts.General.SelectedUnits[6] != -1 && (_selection & (1 << DialogHosts.General.SelectedUnits[6])) == 0;
        Unit8Chk.Checked = DialogHosts.General.SelectedUnits[7] != -1 && (_selection & (1 << DialogHosts.General.SelectedUnits[7])) == 0;
        if (Unit1Chk.Checked)
        {
            Unit1Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit1Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit2Chk.Checked)
        {
            Unit2Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit2Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit3Chk.Checked)
        {
            Unit3Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit3Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit4Chk.Checked)
        {
            Unit4Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit4Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit5Chk.Checked)
        {
            Unit5Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit5Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit6Chk.Checked)
        {
            Unit6Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit6Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit7Chk.Checked)
        {
            Unit7Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit7Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit8Chk.Checked)
        {
            Unit8Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit8Frame.ImageLibraryIndexFrame = 12;
        }

        var selection = AoWHSMap.Current.SelectedArmy.GetSelection();
        var partyMove = DialogHosts.General.UnitList.GetPartyMoves(selection);
        var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(partyMove);
        MoveIcon.CurrentImage = moveTypesToInt32;
        var fSelectedArmySelection = AoWHSMap.Current.SelectedArmy.GetSelection();
        var movePoints = DialogHosts.General.UnitList.MovePoints(fSelectedArmySelection);
        MovesLbl.Text = IntToStr(movePoints);
        var selection2 = AoWHSMap.Current.SelectedArmy.GetSelection();
        var partyStatus = AoWHSMap.Current.SelectedArmy.GetArmyHexSprite().Army.GetPartyStatus(selection2);
        var partyStatusText = AbilityRules.TranslateResourceString(s_partyStatusTexts[(int)partyStatus]);
        PartyStatusLbl.Text = partyStatusText;
    }

    // AOWControlWin.pas:407
    public void Unit1ChkMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right)
        {
            CheckStart();
            AoWHSMap.Current.SelectedArmy.SetSelection((byte)~_selection);
            CheckEnd();
        }
    }

    // AOWControlWin.pas:415
    public void CenterClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var army = DialogHosts.General.UnitList;
        AoWHSMap.Current.CenterViewTo(army.X, army.Y, army.MapLevel);

        var army2 = DialogHosts.General.UnitList;
        AoWHSMap.Current.PlayCenterAnimation(army2.X, army2.Y, army2.MapLevel);
    }

    // AOWControlWin.pas:422
    public void Unit1FramePreDraw(Frame sender, LockedSurface lockedSurface, int slot)
    {
        Rect saveRectangle;
        UIComponent component;
        if (DialogHosts.General.UnitList != null)
        {
            component = sender;
            saveRectangle = lockedSurface.ClipRectangle;
            var rectangle = new Rect(component.OwnerComponent.WinLeft + component.WinLeft + 4, component.OwnerComponent.WinTop + component.WinTop + 3, component.OwnerComponent.WinLeft + component.WinLeft + component.WinWidth - 3, component.OwnerComponent.WinTop + component.WinTop + component.WinHeight - 7);
            DrawUnit(slot, rectangle, ref lockedSurface);
            lockedSurface.ClipRectangle = saveRectangle;
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
        AoWHSMap.Current.SelectPreviousArmy();
    }

    // AOWControlWin.pas:447
    public void PNextClick()
    {
        AoWHSMap.Current.SelectNextArmy();
    }

    // AOWControlWin.pas:452
    public void PMoveClick()
    {
        AoWHSMap.Current.SelectedArmy.ExecuteMove();
    }

    // AOWControlWin.pas:457
    public void PGuardChange()
    {
        if (PGuard.Checked)
        {
            if (!DialogHosts.General.UnitList.UserMode.Contains(ArmyUserModeElement.Guard))
            {
                DialogHosts.General.UnitList.UserMode = DialogHosts.General.UnitList.UserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            }
        }
        else
        {
            if (DialogHosts.General.UnitList.UserMode.Contains(ArmyUserModeElement.Guard))
            {
                DialogHosts.General.UnitList.UserMode = DialogHosts.General.UnitList.UserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            }
        }
    }

    private static readonly string s_partyOwnerFormat = "Party Owner: %s";
    private static StringArray5 s_partyStatusTexts = StringArray5.From(["Unruly", "Unrest", "Stable", "Content", "Cheerful"]);
    private static readonly string s_partyActionText = "Party Action:";
    private static readonly string s_patrollingText = "Patrolling";
    private static readonly string s_guardingText = "Guarding";
    private static readonly string s_scoutingText = "Scouting";
    private static readonly string s_seekingRefugeText = "Seeking Refuge";
    private static readonly string s_raidingText = "Raiding";
    private static readonly string s_huntingText = "Hunting";
    private static readonly string s_protectingText = "Protecting";
    private static readonly string s_fortifiedText = "Fortified";
    private static readonly string s_berserkText = "Berserk";
    private static readonly string s_wanderingText = "Wandering";
}
