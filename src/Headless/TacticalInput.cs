#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Combat;
using Aow.Game.World.Map;

namespace Aow.Headless;

internal static class TacticalInput
{
    public static void Install(StrategicMap world, HexSpriteMapViewer viewer)
    {
        world.CombatMapCreated += _ =>
        {
            CombatMap combat = world.CombatMap;
            viewer.HexSpriteMap = combat;
            // TTBWindow.TCTurnButtonAnimDone completes the turn banner before CPU input.
            combat.TurnChanged += _ => combat.EndTurnPending = true;
        };
        world.CombatMapDestroyed += _ => viewer.HexSpriteMap = world;
    }
}
