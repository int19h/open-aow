// Translated from MainAG.pas.
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// no fields. map-load registration callback
public partial class MainRegistrar
{
    // MainAG.pas:115
    public static void MapLoaded(HSMap Sender)
    {
        if (Sender == AoWHSMap.Current)
        {
            AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<PassiveAIGroup>(), AIGroupClass.Of<SuicidalAIGroup>(), AIGroupClass.Of<FortifyAIGroup>(), AIGroupClass.Of<TransportAIGroup>()]);
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
