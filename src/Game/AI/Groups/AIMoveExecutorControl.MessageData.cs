
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class AIMoveExecutorControl
{
    public AIMoveExecutorMove CreateMove(AIMoveExecutorArmy army, byte selection, sbyte x, sbyte y,
        sbyte level, Set<CreateMoveModeElement> mode)
    {
        byte result = 0;
        return CreateMove(army, selection, x, y, level, mode, ref result);
    }
}
