using Aow.Game.Combat;
using Aow.Game.World.Map;

namespace Aow.Headless;

internal static class TacticalInput
{
    public static void Install(TAoWHSMap world, THSMapViewer viewer)
    {
        world.FCombatMapCreatedEvents += _ =>
        {
            var combat = (TAoWCombatMap)world.FCombatMap;
            viewer.HSMap = combat;
            // TTBWindow.TCTurnButtonAnimDone completes the turn banner before CPU input.
            combat.FChangeTurnEvents += _ => combat.FEndTurnPending = true;
        };
        world.FCombatMapDestroyedEvents += _ => viewer.HSMap = world;
    }
}
