
using Aow.Game.World.Map;

namespace Aow.Headless;

internal static class TacticalInput
{
    public static void Install(StrategicMap world, HexSpriteMapViewer viewer)
    {
        world.FCombatMapCreatedEvents += _ =>
        {
            var combat = world.CombatMap;
            viewer.HexSpriteMap = combat;
            // TTBWindow.TCTurnButtonAnimDone completes the turn banner before CPU input.
            combat.FChangeTurnEvents += _ => combat.EndTurnPending = true;
        };
        world.FCombatMapDestroyedEvents += _ => viewer.HexSpriteMap = world;
    }
}
