// Translated from MainAG.pas.
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Ai.Groups;

// no fields. map-load registration callback
public partial class MainRegistrator
{
    // MainAG.pas:115
    public static void MapLoaded(HSMap Sender)
    {
        if (Sender == AoWHSMap.Current)
        {
            AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<PassiveAG>(), AIGroupClass.Of<SuicidalAG>(), AIGroupClass.Of<FortifyAG>(), AIGroupClass.Of<TransportAG>()]);
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
