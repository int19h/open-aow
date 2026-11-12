// Translated from AoWTC.pas.
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Combat;

public partial class CombatMoveControl : MoveControl
{

    public CombatMoveControl(HSMap owner) : base(owner)
    { }

    public TacticalBattleUnitHexSprite UnitHexSprite { get; set; }
    public Set<CombatMoveModeElement> MoveFlags { get; private set; }

    // AoWTC.pas:2716
    public override void ProcessMove(MoveControlSettings settings)
    {
        // An army sprite does not reach this override. Any other sprite is absent.
        UnitHexSprite = settings.ArmyHexSprite is TacticalBattleUnitHexSprite unit ? unit : null;
        MoveFlags = ((CombatMoveControlSettings)settings).MoveFlags;
        base.ProcessMove(settings);
    }

    // intf_P5_01.inc:120

}
