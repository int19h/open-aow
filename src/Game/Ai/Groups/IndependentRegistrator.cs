// Translated from IndependentAG.pas.
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Ai.Groups;

public partial class IndependentRegistrator
{
    // IndependentAG.pas:136
    public static void MapLoaded(HSMap Sender)
    {
        if (Sender == AoWHSMap.Current)
        {
            AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<GuardAG>(), AIGroupClass.Of<GuardAreaAG>(), AIGroupClass.Of<PatrolAG>(), AIGroupClass.Of<RefugeAG>(), AIGroupClass.Of<RaidAG>(), AIGroupClass.Of<ScoutAG>()]);
        }
    }

    internal static void SubscribeMapLoaded()
    {
        if (GameTables.ValidateAoWEngine())
            GameTables.AoWEngine.OnHSMLoaded += MapLoaded;
    }

    internal static void UnsubscribeMapLoaded()
    {
        if (GameTables.AoWEngine != null)
            GameTables.AoWEngine.OnHSMLoaded -= MapLoaded;
    }
}
