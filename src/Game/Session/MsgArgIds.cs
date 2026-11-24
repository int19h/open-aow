using Aow.Rtl;

namespace Aow.Game.Session;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromPlayer(PlayerId? player) => new(player.ToOriginal());
        public static MsgArg FromRace(RaceId? race) => new(race.ToOriginal());
        public static MsgArg FromScenario(ScenarioId? scenario) => new(scenario.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public PlayerId? Player => PlayerId.FromOriginal(argument.Int);
        public RaceId? Race => RaceId.FromOriginal(argument.Int);
        public ScenarioId? Scenario => ScenarioId.FromOriginal(argument.Int);
    }
}
