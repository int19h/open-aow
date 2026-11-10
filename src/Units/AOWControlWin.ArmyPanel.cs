#nullable disable
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class TControlWin
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
            frames[i].ILIndexFrame = 12;
        }
        PartyActionLbl1.Visible = PartyActionLbl2.Visible = false;
        var army = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList;
        bool readOnly = army.FPlayer != Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer;
        if (army.FPlayer == 0)
        {
            PartyBPnl.Visible = false;
            PartyIPnl.Visible = true;
            PartyILbl.Text = "";
            readOnly = true;
            PartyActionLbl1.Visible = PartyActionLbl2.Visible = true;
            PartyActionLbl1.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TControlWin.PartyActionRStr);
            PartyActionLbl2.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(((int)army.GetBehavior(255) & 127) switch
            {
                1 => Aow.Game.Ui.Dialogs.TControlWin.PatrollingRStr,
                2 or 3 => Aow.Game.Ui.Dialogs.TControlWin.GuardingRStr,
                4 => Aow.Game.Ui.Dialogs.TControlWin.ScoutingRStr,
                5 => Aow.Game.Ui.Dialogs.TControlWin.SeekingRefugeRStr,
                6 => Aow.Game.Ui.Dialogs.TControlWin.RaidingRStr,
                7 => Aow.Game.Ui.Dialogs.TControlWin.HuntingRStr,
                8 => Aow.Game.Ui.Dialogs.TControlWin.ProtectingRStr,
                9 => Aow.Game.Ui.Dialogs.TControlWin.FortifiedRStr,
                10 => Aow.Game.Ui.Dialogs.TControlWin.BerserkRStr,
                _ => Aow.Game.Ui.Dialogs.TControlWin.WanderingRStr
            });
        }
        else if (readOnly)
        {
            PartyBPnl.Visible = false;
            PartyIPnl.Visible = true;
            PartyILbl.Text = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TControlWin.PartyOwnerXRStr), [Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(army.FPlayer).FPlayerRace])]);
            PControlPnl.Visible = false;
        }
        else
        {
            PartyBPnl.Visible = true;
            PartyIPnl.Visible = false;
            MoveIcon.Visible = true;
            MoveIcon.CurrentImage = Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(army.GetPartyMoves(255));
            MovesLbl.Text = RtlUnit.IntToStr(army.MovePoints(255));
            Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.CheckEnd();
            PControlPnl.Visible = true;
            PGuard.Checked = army.FUserMode.Contains(TArmyUserModeElement.aumGuard);
        }
        for (int i = 0; i < 8; i++)
        {
            int index = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[i];
            if (index == -1) continue;
            byte selection = (byte)(1 << index);
            icons[i].CurrentImage = Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(army.MoveTypes(selection));
            var unit = army.GetUnit(index);
            gauges[i].Max = unit.GetHits();
            gauges[i].Position = unit.GetHitPoints();
            checks[i].Text = UnitMoveText(army.MovePoints(selection));
            emptyFrames[i].Visible = false;
            checks[i].Visible = !readOnly;
            emptyChecks[i].Visible = readOnly;
        }
        if (!Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ConButton.Visible) ControlWindow.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ConButton.Visible = true;
    }
}
