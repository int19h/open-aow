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
        var checks = new[] { Unit1Chk, Unit2Chk, Unit3Chk, Unit4Chk, Unit5Chk, Unit6Chk, Unit7Chk, Unit8Chk };
        var emptyFrames = new[] { Emp1Fr, Emp2Fr, Emp3Fr, Emp4Fr, Emp5Fr, Emp6Fr, Emp7Fr, Emp8Fr };
        var emptyChecks = new[] { Emp1Chk, Emp2Chk, Emp3Chk, Emp4Chk, Emp5Chk, Emp6Chk, Emp7Chk, Emp8Chk };
        var frames = new[] { Unit1Frame, Unit2Frame, Unit3Frame, Unit4Frame, Unit5Frame, Unit6Frame, Unit7Frame, Unit8Frame };
        var icons = new[] { U1MIcon, U2MIcon, U3MIcon, U4MIcon, U5MIcon, U6MIcon, U7MIcon, U8MIcon };
        var gauges = new[] { Unit1Gge, Unit2Gge, Unit3Gge, Unit4Gge, Unit5Gge, Unit6Gge, Unit7Gge, Unit8Gge };
        for (int i = 0; i < 8; i++)
        {
            checks[i].Checked = false;
            emptyFrames[i].Visible = emptyChecks[i].Visible = true;
            frames[i].ImageLibraryIndexFrame = 12;
        }
        PartyActionLbl1.Visible = PartyActionLbl2.Visible = false;
        var army = DialogHosts.General.UnitList;
        bool readOnly = army.Player != AoWHSMap.Current.SeatedPlayer;
        if (army.Player == 0)
        {
            PartyBPnl.Visible = false;
            PartyIPnl.Visible = true;
            PartyILbl.Text = "";
            readOnly = true;
            PartyActionLbl1.Visible = PartyActionLbl2.Visible = true;
            PartyActionLbl1.Text = AbilityRules.TranslateResourceString(s_partyActionText);
            PartyActionLbl2.Text = AbilityRules.TranslateResourceString(((int)army.GetBehavior(255) & 127) switch
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
            PartyBPnl.Visible = false;
            PartyIPnl.Visible = true;
            PartyILbl.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_partyOwnerFormat), [AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[AoWHSMap.Current.Players.GetPlayers(army.Player).PlayerRace])]);
            PControlPnl.Visible = false;
        }
        else
        {
            PartyBPnl.Visible = true;
            PartyIPnl.Visible = false;
            MoveIcon.Visible = true;
            MoveIcon.CurrentImage = DialogHosts.General.MoveTypesToInt32(army.GetPartyMoves(255));
            MovesLbl.Text = RtlUnit.IntToStr(army.MovePoints(255));
            DialogHosts.ControlWindow.CheckEnd();
            PControlPnl.Visible = true;
            PGuard.Checked = army.UserMode.Contains(ArmyUserModeElement.Guard);
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
        if (!DialogHosts.TaskBar.ConButton.Visible) MainWindow.Visible = true;
        DialogHosts.TaskBar.ConButton.Visible = true;
    }
}
