// Translated from IndependentAG.pas.
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

public partial class IndependentRegistrar
{
    // IndependentAG.pas:136
    public static void MapLoaded(HSMap Sender)
    {
        if (Sender == AoWHSMap.Current)
        {
            AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<GuardAIGroup>(), AIGroupClass.Of<GuardAreaAIGroup>(), AIGroupClass.Of<PatrolAIGroup>(), AIGroupClass.Of<RefugeAIGroup>(), AIGroupClass.Of<RaidAIGroup>(), AIGroupClass.Of<ScoutAIGroup>()]);
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
