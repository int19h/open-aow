#nullable disable

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

internal static class CombatEvents
{
    internal static void MakeDamageEffectHexSpriteList(int X, int Y, Set<DamageType> damageTypes, bool resisted)
    {
        void MakeEffect(BattleEffectType effectType)
        {
            BattleEffectSprite effect;
            int count;
            float z = default;
            float speed = default;
            float vz = default;
            float gravity = default;
            count = Random(5) + 8;
            for (int i = 1; i <= count - 1; i++)
            {
                effect = CombatMap.Current.AddEffect(effectType);
                speed = (float)((Random(10) + 10) * 0.1);
                gravity = (float)0.5;
                if (resisted || effectType == BattleEffectType.DamMagic)
                {
                    vz = (float)((Random(50) + 50) * 0.1);
                }
                else
                {
                    vz = (float)((Random(25) + 25) * 0.1);
                }

                z = 0;
                var path = effect.Path;
                EffectPaths.MakeArcPath(X, Y, i * 360 / count, ref z, ref speed, ref vz, ref gravity, ref path);
                effect.Path = path;
            }
        }

        X = X + 16;
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
        TacticalBattleEndTurnTokenEvent result;
        if (!StrategicMap.Current.Players.GetPlayers(player).GetBusy())
        {
            result = new TacticalBattleEndTurnTokenEvent(null) { Holder = engine };
        }
        else
        {
            result = null;
        }

        return result;
    }

}
