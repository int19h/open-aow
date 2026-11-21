#nullable disable
// Translated from AoWTC.pas.
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Combat;

public class CombatMoveControl(GameMap owner) : MoveControl(owner)
{
    public TacticalBattleUnitHexSprite UnitHexSprite { get; set; }
    public CombatMoveModes MoveFlags { get; private set; }

    // AoWTC.pas:2716
    public override void ProcessMove(MoveControlSettings settings)
    {
        // An army sprite does not reach this override. Any other sprite is absent.
        UnitHexSprite = settings.ArmyHexSprite is TacticalBattleUnitHexSprite unit ? unit : null;
        // The combat selector and the battle AI pass combat move settings. Another settings object
        // fails. Null stays null and the flag read fails on it.
        MoveFlags = ((CombatMoveControlSettings)settings).MoveFlags;
        base.ProcessMove(settings);
    }
}
