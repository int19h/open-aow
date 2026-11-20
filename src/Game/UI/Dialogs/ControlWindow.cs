#nullable disable
// Translated from AOWControlWin.pas.
using System.Collections.Immutable;

using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.UI.Dialogs;

// published controls. inverted army selection mask
public sealed partial class ControlWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; private set; }
    public Frame Unit1Frame { get; private set; }
    public Frame Unit2Frame { get; private set; }
    public Frame Unit3Frame { get; private set; }
    public Frame Unit4Frame { get; private set; }
    public Frame Unit5Frame { get; private set; }
    public Frame Unit6Frame { get; private set; }
    public Frame Unit7Frame { get; private set; }
    public Frame Unit8Frame { get; private set; }
    public Label MovesLabel { get; private set; }
    public Label PartyStatusLabel { get; private set; }
    public Image Unit1MovesIcon { get; private set; }
    public Image Unit2MovesIcon { get; private set; }
    public Image Unit3MovesIcon { get; private set; }
    public Image Unit4MovesIcon { get; private set; }
    public Image Unit5MovesIcon { get; private set; }
    public Image Unit6MovesIcon { get; private set; }
    public Image Unit7MovesIcon { get; private set; }
    public Image Unit8MovesIcon { get; private set; }
    public Image MoveIcon { get; private set; }
    public Panel PartyMovesPanel { get; private set; }
    public Gauge Unit1Gauge { get; private set; }
    public Gauge Unit2Gauge { get; private set; }
    public Gauge Unit3Gauge { get; private set; }
    public Gauge Unit4Gauge { get; private set; }
    public Gauge Unit5Gauge { get; private set; }
    public Gauge Unit6Gauge { get; private set; }
    public Gauge Unit7Gauge { get; private set; }
    public Gauge Unit8Gauge { get; private set; }
    public CheckBox Unit1CheckBox { get; private set; }
    public CheckBox Unit2CheckBox { get; private set; }
    public CheckBox Unit3CheckBox { get; private set; }
    public CheckBox Unit5CheckBox { get; private set; }
    public CheckBox Unit6CheckBox { get; private set; }
    public CheckBox Unit7CheckBox { get; private set; }
    public CheckBox Unit4CheckBox { get; private set; }
    public CheckBox Unit8CheckBox { get; private set; }
    public Frame EmptySlot1Frame { get; private set; }
    public Frame EmptySlot2Frame { get; private set; }
    public Frame EmptySlot3Frame { get; private set; }
    public Frame EmptySlot4Frame { get; private set; }
    public Frame EmptySlot5Frame { get; private set; }
    public Frame EmptySlot6Frame { get; private set; }
    public Frame EmptySlot7Frame { get; private set; }
    public Frame EmptySlot8Frame { get; private set; }
    public Frame EmptyCheck1Frame { get; private set; }
    public Frame EmptyCheck2Frame { get; private set; }
    public Frame EmptyCheck3Frame { get; private set; }
    public Frame EmptyCheck4Frame { get; private set; }
    public Frame EmptyCheck5Frame { get; private set; }
    public Frame EmptyCheck6Frame { get; private set; }
    public Frame EmptyCheck7Frame { get; private set; }
    public Frame EmptyCheck8Frame { get; private set; }
    public Panel PartyInfoPanel { get; private set; }
    public Label PartyInfoLabel { get; private set; }
    public Panel PartyControlPanel { get; private set; }
    public CheckBox PartyGuardCheckBox { get; private set; }
    public Label PartyActionLabel1 { get; private set; }
    public Label PartyActionLabel2 { get; private set; }

    private byte _selection;

    // AOWControlWin.pas:294
    public void DrawUnit(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        if (UI.General.SelectedUnits[index] != -1)
        {
            if (UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).GetPreviewImage() is not null)
            {
                lockedSurface.ClipRectangle = R;
                // { Original tests the unsorted index when checking the background library. }
                if ((int)UI.General.UnitList.Units.GetUnit(index).GetAlignment() < UI.Graphics.UnitBackgrounds.Count)
                {
                    Units.Alignment alignment = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).GetAlignment();
                    LibraryImage unitBackground = UI.Graphics.UnitBackgrounds[(int)alignment];
                    unitBackground.ShowClipped(R.Left + ((R.Right - R.Left - unitBackground.Width) / 2), R.Top + ((R.Bottom - R.Top - unitBackground.Height) / 2), lockedSurface);
                }

                LibraryImage previewImage = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).GetPreviewImage();
                LibraryImage previewImage2 = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).GetPreviewImage();
                _ = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[index]).Show(R.Left + ((R.Right - R.Left - previewImage2.Width) / 2), R.Top + ((R.Bottom - R.Top - previewImage.Height) / 2), lockedSurface, info: UI.General.UnitShowInfo);
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
        MainWindow.Alignment.RightOffset = MainWindow.WindowManager.WinWidth - MainWindow.WinLeft - MainWindow.WinWidth;
        MainWindow.Alignment.TopOffset = MainWindow.WinTop;
    }

    // AOWControlWin.pas:333
    public void Unit1FrameMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        if (UI.General.UnitList is not null && UI.General.SelectedUnits[slot] > -1 && UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[slot]) is not null)
        {
            Troop troop = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[slot]);
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
        _selection = StrategicMap.Current.SelectedArmy.Selection;
        Unit1CheckBox.Checked = UI.General.SelectedUnits[0] != -1 && (_selection & (1 << UI.General.SelectedUnits[0])) == 0;
        Unit2CheckBox.Checked = UI.General.SelectedUnits[1] != -1 && (_selection & (1 << UI.General.SelectedUnits[1])) == 0;
        Unit3CheckBox.Checked = UI.General.SelectedUnits[2] != -1 && (_selection & (1 << UI.General.SelectedUnits[2])) == 0;
        Unit4CheckBox.Checked = UI.General.SelectedUnits[3] != -1 && (_selection & (1 << UI.General.SelectedUnits[3])) == 0;
        Unit5CheckBox.Checked = UI.General.SelectedUnits[4] != -1 && (_selection & (1 << UI.General.SelectedUnits[4])) == 0;
        Unit6CheckBox.Checked = UI.General.SelectedUnits[5] != -1 && (_selection & (1 << UI.General.SelectedUnits[5])) == 0;
        Unit7CheckBox.Checked = UI.General.SelectedUnits[6] != -1 && (_selection & (1 << UI.General.SelectedUnits[6])) == 0;
        Unit8CheckBox.Checked = UI.General.SelectedUnits[7] != -1 && (_selection & (1 << UI.General.SelectedUnits[7])) == 0;
        Unit1Frame.ImageLibraryIndexFrame = Unit1CheckBox.Checked ? 26 : 12;

        Unit2Frame.ImageLibraryIndexFrame = Unit2CheckBox.Checked ? 26 : 12;

        Unit3Frame.ImageLibraryIndexFrame = Unit3CheckBox.Checked ? 26 : 12;

        Unit4Frame.ImageLibraryIndexFrame = Unit4CheckBox.Checked ? 26 : 12;

        Unit5Frame.ImageLibraryIndexFrame = Unit5CheckBox.Checked ? 26 : 12;

        Unit6Frame.ImageLibraryIndexFrame = Unit6CheckBox.Checked ? 26 : 12;

        Unit7Frame.ImageLibraryIndexFrame = Unit7CheckBox.Checked ? 26 : 12;

        Unit8Frame.ImageLibraryIndexFrame = Unit8CheckBox.Checked ? 26 : 12;

        byte selection = StrategicMap.Current.SelectedArmy.Selection;
        Set<MoveType> partyMove = UI.General.UnitList.GetPartyMoves(selection);
        int moveTypesToInt32 = UI.General.MoveTypesToInt32(partyMove);
        MoveIcon.CurrentImage = moveTypesToInt32;
        byte fSelectedArmySelection = StrategicMap.Current.SelectedArmy.Selection;
        sbyte movePoints = UI.General.UnitList.MovePoints(fSelectedArmySelection);
        MovesLabel.Text = IntToStr(movePoints);
        byte selection2 = StrategicMap.Current.SelectedArmy.Selection;
        Army.PartyStatus partyStatus = StrategicMap.Current.SelectedArmy.ArmyHexSprite.Army.GetPartyStatus(selection2);
        string partyStatusText = UI.Engine.TranslateResourceString(s_partyStatusTexts[(int)partyStatus]);
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
        Army army = UI.General.UnitList;
        StrategicMap.Current.CenterViewTo(army.Location.X, army.Location.Y, army.Location.MapLevel);

        Army army2 = UI.General.UnitList;
        StrategicMap.Current.PlayCenterAnimation(army2.Location.X, army2.Location.Y, L: army2.Location.MapLevel);
    }

    // AOWControlWin.pas:422
    public void Unit1FramePreDraw(Frame sender, LockedSurface lockedSurface, int slot)
    {
        if (UI.General.UnitList is not null)
        {
            UIComponent component = sender;
            Rect saveRectangle = lockedSurface.ClipRectangle;
            Rect rectangle = new(component.OwnerComponent.WinLeft + component.WinLeft + 4, component.OwnerComponent.WinTop + component.WinTop + 3, component.OwnerComponent.WinLeft + component.WinLeft + component.WinWidth - 3, component.OwnerComponent.WinTop + component.WinTop + component.WinHeight - 7);
            DrawUnit(slot, R: rectangle, ref lockedSurface);
            lockedSurface.ClipRectangle = saveRectangle;
        }
    }

    // AOWControlWin.pas:437
    public void HelpClick() => UI.HelpWindow.ShowHelp(503);

    // AOWControlWin.pas:442
    public void PPrevClick() => StrategicMap.Current.SelectPreviousArmy();

    // AOWControlWin.pas:447
    public void PNextClick() => StrategicMap.Current.SelectNextArmy();

    // AOWControlWin.pas:452
    public void PMoveClick() => StrategicMap.Current.SelectedArmy.ExecuteMove();

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
