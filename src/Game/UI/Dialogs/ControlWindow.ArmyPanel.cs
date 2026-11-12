#nullable disable
using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class ControlWindow
{
    // Original AoW.exe loads the three-space string. The recovered UpdateArmy source has one space
    // at all eight labels
    public static string UnitMoveText(int points) => "   " + RtlUnit.IntToStr(points);

    public void UpdateArmy()
    {
        var checks = new[] { Unit1CheckBox, Unit2CheckBox, Unit3CheckBox, Unit4CheckBox, Unit5CheckBox, Unit6CheckBox, Unit7CheckBox, Unit8CheckBox };
        var emptyFrames = new[] { EmptySlot1Frame, EmptySlot2Frame, EmptySlot3Frame, EmptySlot4Frame, EmptySlot5Frame, EmptySlot6Frame, EmptySlot7Frame, EmptySlot8Frame };
        var emptyChecks = new[] { EmptyCheck1Frame, EmptyCheck2Frame, EmptyCheck3Frame, EmptyCheck4Frame, EmptyCheck5Frame, EmptyCheck6Frame, EmptyCheck7Frame, EmptyCheck8Frame };
        var frames = new[] { Unit1Frame, Unit2Frame, Unit3Frame, Unit4Frame, Unit5Frame, Unit6Frame, Unit7Frame, Unit8Frame };
        var icons = new[] { Unit1MovesIcon, Unit2MovesIcon, Unit3MovesIcon, Unit4MovesIcon, Unit5MovesIcon, Unit6MovesIcon, Unit7MovesIcon, Unit8MovesIcon };
        var gauges = new[] { Unit1Gauge, Unit2Gauge, Unit3Gauge, Unit4Gauge, Unit5Gauge, Unit6Gauge, Unit7Gauge, Unit8Gauge };
        for (int i = 0; i < 8; i++)
        {
            checks[i].Checked = false;
            emptyFrames[i].Visible = emptyChecks[i].Visible = true;
            frames[i].ImageLibraryIndexFrame = 12;
        }
        PartyActionLabel1.Visible = PartyActionLabel2.Visible = false;
        var army = DialogHosts.General.UnitList;
        bool readOnly = army.Player != AoWHSMap.Current.SeatedPlayer;
        if (army.Player == 0)
        {
            PartyMovesPanel.Visible = false;
            PartyInfoPanel.Visible = true;
            PartyInfoLabel.Text = "";
            readOnly = true;
            PartyActionLabel1.Visible = PartyActionLabel2.Visible = true;
            PartyActionLabel1.Text = AbilityRules.TranslateResourceString(s_partyActionText);
            PartyActionLabel2.Text = AbilityRules.TranslateResourceString(((int)army.GetBehavior(255) & 127) switch
            {
                1 => s_patrollingText,
                2 or 3 => s_guardingText,
                4 => s_scoutingText,
                5 => s_seekingRefugeText,
                6 => s_raidingText,
                7 => s_huntingText,
                8 => s_protectingText,
                9 => s_fortifiedText,
                10 => s_berserkText,
                _ => s_wanderingText
            });
        }
        else if (readOnly)
        {
            PartyMovesPanel.Visible = false;
            PartyInfoPanel.Visible = true;
            PartyInfoLabel.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_partyOwnerFormat), [AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[AoWHSMap.Current.Players.GetPlayers(army.Player).PlayerRace])]);
            PartyControlPanel.Visible = false;
        }
        else
        {
            PartyMovesPanel.Visible = true;
            PartyInfoPanel.Visible = false;
            MoveIcon.Visible = true;
            MoveIcon.CurrentImage = DialogHosts.General.MoveTypesToInt32(army.GetPartyMoves(255));
            MovesLabel.Text = RtlUnit.IntToStr(army.MovePoints(255));
            DialogHosts.ControlWindow.CheckEnd();
            PartyControlPanel.Visible = true;
            PartyGuardCheckBox.Checked = army.UserMode.Contains(ArmyUserModeElement.Guard);
        }
        for (int i = 0; i < 8; i++)
        {
            int index = DialogHosts.General.SelectedUnits[i];
            if (index == -1) continue;
            byte selection = (byte)(1 << index);
            icons[i].CurrentImage = DialogHosts.General.MoveTypesToInt32(army.MoveTypes(selection));
            var troop = army.GetUnit(index);
            gauges[i].Max = troop.GetHits();
            gauges[i].Position = troop.GetHitPoints();
            checks[i].Text = UnitMoveText(army.MovePoints(selection));
            emptyFrames[i].Visible = false;
            checks[i].Visible = !readOnly;
            emptyChecks[i].Visible = readOnly;
        }
        if (!DialogHosts.TaskBar.PartyButton.Visible) MainWindow.Visible = true;
        DialogHosts.TaskBar.PartyButton.Visible = true;
    }
}
