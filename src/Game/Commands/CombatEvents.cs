#nullable disable

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Commands;

internal static class CombatEvents
{
    internal static void MakeDamageEffectHexSpriteList(int X, int Y, Set<DamageType> damageTypes, bool resisted)
    {
        void MakeEffect(BattleEffectType effectType)
        {
            int count = Random(5) + 8;
            for (int i = 1; i <= count - 1; i++)
            {
                BattleEffectSprite effect = CombatMap.Current.AddEffect(effectType);
                float speed = (float)((Random(10) + 10) * 0.1);
                float gravity = (float)0.5;
                float vz = resisted || effectType == BattleEffectType.DamMagic ? (float)((Random(50) + 50) * 0.1) : (float)((Random(25) + 25) * 0.1);

                float z = 0;
                Point3DList path = effect.Path;
                EffectPaths.MakeArcPath(X, Y, i * 360 / count, ref z, speed, ref vz, gravity, path);
                effect.Path = path;
            }
        }

        X += 16;
        if (damageTypes.Contains(DamageType.Lightning))
        {
            MakeEffect(BattleEffectType.DamLightning);
        }

        if (damageTypes.Contains(DamageType.Fire))
        {
            MakeEffect(BattleEffectType.DamFire);
        }

        if (damageTypes.Contains(DamageType.Poison))
        {
            MakeEffect(BattleEffectType.DamPoison);
        }

        if (damageTypes.Contains(DamageType.Magic))
        {
            MakeEffect(BattleEffectType.DamMagic);
        }

        if (damageTypes.Contains(DamageType.Holy))
        {
            MakeEffect(BattleEffectType.DamHoly);
        }

        if (damageTypes.Contains(DamageType.Death))
        {
            MakeEffect(BattleEffectType.DamDeath);
        }

        if (damageTypes.Contains(DamageType.Cold))
        {
            MakeEffect(BattleEffectType.DamCold);
        }
    }

    internal static TacticalBattleEndTurnTokenEvent CreateTacticalBattleCancelEndTurnTokenEvent(sbyte player, GameEngine engine)
    {
        TacticalBattleEndTurnTokenEvent result = !StrategicMap.Current.Players.GetPlayers(player).GetBusy()
            ? new TacticalBattleEndTurnTokenEvent(null) { Holder = engine }
            : null;
        return result;
    }

}
