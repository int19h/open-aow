#nullable disable
using Aow.Rtl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.E3Globals.E3Globals;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AOWControlWin;

public partial class TControlWin
{
    // Original AoW.exe 004055E6 loads the three-space string at 00405D94.
    // The recovered UpdateArmy source has one space at all eight labels.
    public static AnsiString UnitMoveText(int points) => "   " + Aow.Rtl.Rtl.IntToStr(points);

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
        var army = General.FUnitList;
        bool readOnly = army.FPlayer != AoWHSMap.FSeatedPlayer;
        if (army.FPlayer == 0)
        {
            PartyBPnl.Visible = false;
            PartyIPnl.Visible = true;
            PartyILbl.Text = "";
            readOnly = true;
            PartyActionLbl1.Visible = PartyActionLbl2.Visible = true;
            PartyActionLbl1.Text = TranslateRStr(PartyActionRStr);
            PartyActionLbl2.Text = TranslateRStr(((int)army.GetBehavior(255) & 127) switch
            {
                1 => PatrollingRStr, 2 or 3 => GuardingRStr, 4 => ScoutingRStr,
                5 => SeekingRefugeRStr, 6 => RaidingRStr, 7 => HuntingRStr,
                8 => ProtectingRStr, 9 => FortifiedRStr, 10 => Aow.Units.E3Globals.E3Globals.BerserkRStr, _ => WanderingRStr
            });
        }
        else if (readOnly)
        {
            PartyBPnl.Visible = false;
            PartyIPnl.Visible = true;
            PartyILbl.Text = Aow.Units.AoWE.AoWE.Format(TranslateRStr(PartyOwnerXRStr), [TranslateRStr(RaceNamesPluralRStr[AoWHSMap.FPlayers.GetPlayers(army.FPlayer).FPlayerRace])]);
            PControlPnl.Visible = false;
        }
        else
        {
            PartyBPnl.Visible = true;
            PartyIPnl.Visible = false;
            MoveIcon.Visible = true;
            MoveIcon.CurrentImage = General.MTtoInt(army.GetPartyMoves(255));
            MovesLbl.Text = Aow.Rtl.Rtl.IntToStr(army.MovePoints(255));
            AOWControlWin.ControlWin.CheckEnd();
            PControlPnl.Visible = true;
            PGuard.Checked = army.FUserMode.Contains(TArmyUserModeElement.aumGuard);
        }
        for (int i = 0; i < 8; i++)
        {
            int index = General.FSelectedUnits[i];
            if (index == -1) continue;
            byte selection = unchecked((byte)(1 << index));
            icons[i].CurrentImage = General.MTtoInt(army.MoveTypes(selection));
            var unit = army.GetUnit(index);
            gauges[i].Max = unit.GetHits();
            gauges[i].Position = unit.GetHitPoints();
            checks[i].Text = UnitMoveText(army.MovePoints(selection));
            emptyFrames[i].Visible = false;
            checks[i].Visible = !readOnly;
            emptyChecks[i].Visible = readOnly;
        }
        if (!Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ConButton.Visible) ControlWindow.Visible = true;
        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ConButton.Visible = true;
    }
}
