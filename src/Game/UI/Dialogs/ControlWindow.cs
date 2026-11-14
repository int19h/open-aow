// Translated from AOWControlWin.pas.
using System.Collections.Immutable;

using Aow.Game.Draw;

using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// published controls. inverted army selection mask
public partial class ControlWindow : UIModule
{
    public ControlWindow(UIContext ui) : base(ui)
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
    public Label MovesLabel;
    public Label PartyStatusLabel;
    public Image Unit1MovesIcon;
    public Image Unit2MovesIcon;
    public Image Unit3MovesIcon;
    public Image Unit4MovesIcon;
    public Image Unit5MovesIcon;
    public Image Unit6MovesIcon;
    public Image Unit7MovesIcon;
    public Image Unit8MovesIcon;
    public Image MoveIcon;
    public Panel PartyMovesPanel;
    public Gauge Unit1Gauge;
    public Gauge Unit2Gauge;
    public Gauge Unit3Gauge;
    public Gauge Unit4Gauge;
    public Gauge Unit5Gauge;
    public Gauge Unit6Gauge;
    public Gauge Unit7Gauge;
    public Gauge Unit8Gauge;
    public CheckBox Unit1CheckBox;
    public CheckBox Unit2CheckBox;
    public CheckBox Unit3CheckBox;
    public CheckBox Unit5CheckBox;
    public CheckBox Unit6CheckBox;
    public CheckBox Unit7CheckBox;
    public CheckBox Unit4CheckBox;
    public CheckBox Unit8CheckBox;
    public Frame EmptySlot1Frame;
    public Frame EmptySlot2Frame;
    public Frame EmptySlot3Frame;
    public Frame EmptySlot4Frame;
    public Frame EmptySlot5Frame;
    public Frame EmptySlot6Frame;
    public Frame EmptySlot7Frame;
    public Frame EmptySlot8Frame;
    public Frame EmptyCheck1Frame;
    public Frame EmptyCheck2Frame;
    public Frame EmptyCheck3Frame;
    public Frame EmptyCheck4Frame;
    public Frame EmptyCheck5Frame;
    public Frame EmptyCheck6Frame;
    public Frame EmptyCheck7Frame;
    public Frame EmptyCheck8Frame;
    public Panel PartyInfoPanel;
    public Label PartyInfoLabel;
    public Panel PartyControlPanel;
    public CheckBox PartyGuardCheckBox;
    public Label PartyActionLabel1;
    public Label PartyActionLabel2;
    private byte _selection;

    // AOWControlWin.pas:294
    public void DrawUnit(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        if (UI.General.SelectedUnits[index] != -1)
        {
            if (UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).GetPreviewImage() != null)
            {
                lockedSurface.ClipRectangle = R;
                // { Original tests the unsorted index when checking the background library. }
                if ((int)UI.General.UnitList.Units.GetUnit(index).GetAlignment() < UI.Graphics.UnitBackgrounds.Count)
                {
                    var alignment = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).GetAlignment();
                    var unitBackground = UI.Graphics.UnitBackgrounds[(int)alignment];
                    unitBackground.ShowClipped(R.Left + (R.Right - R.Left - unitBackground.Width) / 2, R.Top + (R.Bottom - R.Top - unitBackground.Height) / 2, lockedSurface);
                }

                var previewImage = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).GetPreviewImage();
                var previewImage2 = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).GetPreviewImage();
                UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).Show(R.Left + (R.Right - R.Left - previewImage2.Width) / 2, R.Top + (R.Bottom - R.Top - previewImage.Height) / 2, lockedSurface, UI.General.UnitShowInfo);
            }
        }
    }

    // AOWControlWin.pas:312
    public void CWMinIconClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AOWControlWin.pas:318
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
        UI.TaskBar.PartyButton.Visible = false;
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
        if (UI.General.UnitList != null && UI.General.SelectedUnits[slot] > -1 && UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[slot]) != null)
        {
            var troop = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[slot]);
            UI.General.SelectedUnit = troop;
            if (button == MouseButton.Left)
            {
                UI.UnitWindow.ShowUnit();
            }
        }
    }

    // AOWControlWin.pas:348
    public void Unit1ChkChange(CheckBox sender)
    {
        CheckStart();
        StrategicMap.Current.SelectedArmy.SetSelection(_selection);
        CheckEnd();
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AOWControlWin.pas:354
    public void CheckStart()
    {
        _selection = 0;
        if (!Unit1CheckBox.Checked && UI.General.SelectedUnits[0] != -1)
        {
            _selection = (byte)(_selection | (1 << UI.General.SelectedUnits[0]));
        }

        if (!Unit2CheckBox.Checked && UI.General.SelectedUnits[1] != -1)
        {
            _selection = (byte)(_selection | (1 << UI.General.SelectedUnits[1]));
        }

        if (!Unit3CheckBox.Checked && UI.General.SelectedUnits[2] != -1)
        {
            _selection = (byte)(_selection | (1 << UI.General.SelectedUnits[2]));
        }

        if (!Unit4CheckBox.Checked && UI.General.SelectedUnits[3] != -1)
        {
            _selection = (byte)(_selection | (1 << UI.General.SelectedUnits[3]));
        }

        if (!Unit5CheckBox.Checked && UI.General.SelectedUnits[4] != -1)
        {
            _selection = (byte)(_selection | (1 << UI.General.SelectedUnits[4]));
        }

        if (!Unit6CheckBox.Checked && UI.General.SelectedUnits[5] != -1)
        {
            _selection = (byte)(_selection | (1 << UI.General.SelectedUnits[5]));
        }

        if (!Unit7CheckBox.Checked && UI.General.SelectedUnits[6] != -1)
        {
            _selection = (byte)(_selection | (1 << UI.General.SelectedUnits[6]));
        }

        if (!Unit8CheckBox.Checked && UI.General.SelectedUnits[7] != -1)
        {
            _selection = (byte)(_selection | (1 << UI.General.SelectedUnits[7]));
        }
    }

    // AOWControlWin.pas:375
    public void CheckEnd()
    {
        _selection = StrategicMap.Current.SelectedArmy.GetSelection();
        Unit1CheckBox.Checked = UI.General.SelectedUnits[0] != -1 && (_selection & (1 << UI.General.SelectedUnits[0])) == 0;
        Unit2CheckBox.Checked = UI.General.SelectedUnits[1] != -1 && (_selection & (1 << UI.General.SelectedUnits[1])) == 0;
        Unit3CheckBox.Checked = UI.General.SelectedUnits[2] != -1 && (_selection & (1 << UI.General.SelectedUnits[2])) == 0;
        Unit4CheckBox.Checked = UI.General.SelectedUnits[3] != -1 && (_selection & (1 << UI.General.SelectedUnits[3])) == 0;
        Unit5CheckBox.Checked = UI.General.SelectedUnits[4] != -1 && (_selection & (1 << UI.General.SelectedUnits[4])) == 0;
        Unit6CheckBox.Checked = UI.General.SelectedUnits[5] != -1 && (_selection & (1 << UI.General.SelectedUnits[5])) == 0;
        Unit7CheckBox.Checked = UI.General.SelectedUnits[6] != -1 && (_selection & (1 << UI.General.SelectedUnits[6])) == 0;
        Unit8CheckBox.Checked = UI.General.SelectedUnits[7] != -1 && (_selection & (1 << UI.General.SelectedUnits[7])) == 0;
        if (Unit1CheckBox.Checked)
        {
            Unit1Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit1Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit2CheckBox.Checked)
        {
            Unit2Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit2Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit3CheckBox.Checked)
        {
            Unit3Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit3Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit4CheckBox.Checked)
        {
            Unit4Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit4Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit5CheckBox.Checked)
        {
            Unit5Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit5Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit6CheckBox.Checked)
        {
            Unit6Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit6Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit7CheckBox.Checked)
        {
            Unit7Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit7Frame.ImageLibraryIndexFrame = 12;
        }

        if (Unit8CheckBox.Checked)
        {
            Unit8Frame.ImageLibraryIndexFrame = 26;
        }
        else
        {
            Unit8Frame.ImageLibraryIndexFrame = 12;
        }

        var selection = StrategicMap.Current.SelectedArmy.GetSelection();
        var partyMove = UI.General.UnitList.GetPartyMoves(selection);
        var moveTypesToInt32 = UI.General.MoveTypesToInt32(partyMove);
        MoveIcon.CurrentImage = moveTypesToInt32;
        var fSelectedArmySelection = StrategicMap.Current.SelectedArmy.GetSelection();
        var movePoints = UI.General.UnitList.MovePoints(fSelectedArmySelection);
        MovesLabel.Text = IntToStr(movePoints);
        var selection2 = StrategicMap.Current.SelectedArmy.GetSelection();
        var partyStatus = StrategicMap.Current.SelectedArmy.GetArmyHexSprite().Army.GetPartyStatus(selection2);
        var partyStatusText = UI.Engine.TranslateResourceString(s_partyStatusTexts[(int)partyStatus]);
        PartyStatusLabel.Text = partyStatusText;
    }

    // AOWControlWin.pas:407
    public void Unit1ChkMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right)
        {
            CheckStart();
            StrategicMap.Current.SelectedArmy.SetSelection((byte)~_selection);
            CheckEnd();
        }
    }

    // AOWControlWin.pas:415
    public void CenterClick()
    {
        UI.General.PlayButtonSound(0);
        var army = UI.General.UnitList;
        StrategicMap.Current.CenterViewTo(army.X, army.Y, army.MapLevel);

        var army2 = UI.General.UnitList;
        StrategicMap.Current.PlayCenterAnimation(army2.X, army2.Y, army2.MapLevel);
    }

    // AOWControlWin.pas:422
    public void Unit1FramePreDraw(Frame sender, LockedSurface lockedSurface, int slot)
    {
        Rect saveRectangle;
        UIComponent component;
        if (UI.General.UnitList != null)
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
        UI.HelpWindow.ShowHelp(503);
    }

    // AOWControlWin.pas:442
    public void PPrevClick()
    {
        StrategicMap.Current.SelectPreviousArmy();
    }

    // AOWControlWin.pas:447
    public void PNextClick()
    {
        StrategicMap.Current.SelectNextArmy();
    }

    // AOWControlWin.pas:452
    public void PMoveClick()
    {
        StrategicMap.Current.SelectedArmy.ExecuteMove();
    }

    // AOWControlWin.pas:457
    public void PGuardChange()
    {
        if (PartyGuardCheckBox.Checked)
        {
            if (!UI.General.UnitList.UserMode.Contains(ArmyUserModeElement.Guard))
            {
                UI.General.UnitList.UserMode = UI.General.UnitList.UserMode + Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            }
        }
        else
        {
            if (UI.General.UnitList.UserMode.Contains(ArmyUserModeElement.Guard))
            {
                UI.General.UnitList.UserMode = UI.General.UnitList.UserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            }
        }
    }

    private static readonly string s_partyOwnerFormat = "Party Owner: %s";
    private static readonly ImmutableArray<string> s_partyStatusTexts = ["Unruly", "Unrest", "Stable", "Content", "Cheerful"];
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
