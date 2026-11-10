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

    public TacticalCombatUnitHS FUnitHS;
    public Set<CombatMoveModeElement> FMoveFlags;

    // AoWTC.pas:2716
    public override void ProcessMove(MoveControlSettings Settings)
    {
        // An army sprite does not reach this override. Any other sprite is absent.
        FUnitHS = Settings.FArmyHS is TacticalCombatUnitHS unit ? unit : null;
        FMoveFlags = ((CombatMoveControlSettings)Settings).FMoveFlags;
        base.ProcessMove(Settings);
    }

    // intf_P5_01.inc:120

}
