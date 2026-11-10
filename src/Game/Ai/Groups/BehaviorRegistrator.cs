// Translated from BehaviorAG.pas.
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Ai.Groups;

public partial class BehaviorRegistrator
{
    // BehaviorAG.pas:136
    public static void MapLoaded(HSMap Sender)
    {
        if (Sender == AoWHSMap.Current)
        {
            AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<DefenderAG>(), AIGroupClass.Of<AggressorAG>(), AIGroupClass.Of<ExpanderAG>(), AIGroupClass.Of<ScorcherAG>(), AIGroupClass.Of<NormalAG>(), AIGroupClass.Of<SkirmishAG>()]);
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
