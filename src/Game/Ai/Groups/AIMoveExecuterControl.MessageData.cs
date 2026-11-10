
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

public partial class AIMoveExecuterControl
{
    public AIMoveExecuterMove CreateMove(AIMoveExecuterArmy army, byte selection, sbyte x, sbyte y,
        sbyte level, Set<CreateMoveModeElement> mode)
    {
        byte result = 0;
        return CreateMove(army, selection, x, y, level, mode, ref result);
    }
}
