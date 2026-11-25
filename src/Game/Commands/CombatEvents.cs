#nullable disable warnings

using Aow.Game.Combat;

using Aow.Game.Draw;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Commands;

internal static class CombatEvents
{
    internal static void MakeDamageEffectHexSpriteList(int X, int Y, DamageTypes damageTypes, bool resisted, GameEngine engine)
    {
        void MakeEffect(BattleEffectType effectType)
        {
            int count = Random(5) + 8;
            for (int i = 1; i <= count - 1; i++)
            {
                BattleEffectSprite effect = engine.CombatMap.AddEffect(effectType);
                float speed = (float)((Random(10) + 10) * 0.1);
                float gravity = (float)0.5;
                float vz = resisted || effectType == BattleEffectType.DamMagic ? (float)((Random(50) + 50) * 0.1) : (float)((Random(25) + 25) * 0.1);

                float z = 0;
                Point3DList path = effect.Path;
                EffectPaths.MakeArcPath(X, Y, angle: i * 360 / count, ref z, speed, ref vz, gravity, list: path);
                effect.Path = path;
            }
        }

        X += 16;
        if (damageTypes.Contains(DamageTypes.Lightning))
        {
            MakeEffect(BattleEffectType.DamLightning);
        }

        if (damageTypes.Contains(DamageTypes.Fire))
        {
            MakeEffect(BattleEffectType.DamFire);
        }

        if (damageTypes.Contains(DamageTypes.Poison))
        {
            MakeEffect(BattleEffectType.DamPoison);
        }

        if (damageTypes.Contains(DamageTypes.Magic))
        {
            MakeEffect(BattleEffectType.DamMagic);
        }

        if (damageTypes.Contains(DamageTypes.Holy))
        {
            MakeEffect(BattleEffectType.DamHoly);
        }

        if (damageTypes.Contains(DamageTypes.Death))
        {
            MakeEffect(BattleEffectType.DamDeath);
        }

        if (damageTypes.Contains(DamageTypes.Cold))
        {
            MakeEffect(BattleEffectType.DamCold);
        }
    }
}
