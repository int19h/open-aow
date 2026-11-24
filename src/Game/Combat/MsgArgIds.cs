using Aow.Rtl;

namespace Aow.Game.Combat;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromCombatPlayer(CombatPlayerId? player) => new(player.ToOriginal());
        public static MsgArg FromCombatObject(CombatObjectId? objectId) => new(objectId.ToOriginal());
        public static MsgArg FromCpu(CpuId? cpu) => new(cpu.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public CombatPlayerId? CombatPlayer => CombatPlayerId.FromOriginal(argument.Int);
        public CombatObjectId? CombatObject => CombatObjectId.FromOriginal(argument.Int);
        public CpuId? Cpu => CpuId.FromOriginal(argument.Int);
    }
}
