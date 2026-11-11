
using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

internal static class CombatEvents
{
    internal static void MakeDEHSList(int X, int Y, Set<DamageType> DamageTypes, bool Resisted)
    {
        void MakeEffect(BattleEffectType EffectType)
        {
            BattleEffectSprite Effect;
            int Count;
            float Z = default;
            float Speed = default;
            float VZ = default;
            float Gravity = default;
            Count = Random(5) + 8;
            for (int I = 1; I <= Count - 1; I++)
            {
                Effect = CombatMap.Current.AddEffect(EffectType);
                Speed = (float)((double)(Random(10) + 10) * 0.1);
                Gravity = (float)0.5;
                if (Resisted || EffectType == BattleEffectType.DamMagic)
                {
                    VZ = (float)((double)(Random(50) + 50) * 0.1);
                }
                else
                {
                    VZ = (float)((double)(Random(25) + 25) * 0.1);
                }

                Z = 0;
                EffectPaths.MakeArcPath(X, Y, I * 360 / Count, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
            }
        }

        X = X + 16;
        if (DamageTypes.Contains(DamageType.Lightning))
        {
            MakeEffect(BattleEffectType.DamLightning);
        }

        if (DamageTypes.Contains(DamageType.Fire))
        {
            MakeEffect(BattleEffectType.DamFire);
        }

        if (DamageTypes.Contains(DamageType.Poison))
        {
            MakeEffect(BattleEffectType.DamPoison);
        }

        if (DamageTypes.Contains(DamageType.Magic))
        {
            MakeEffect(BattleEffectType.DamMagic);
        }

        if (DamageTypes.Contains(DamageType.Holy))
        {
            MakeEffect(BattleEffectType.DamHoly);
        }

        if (DamageTypes.Contains(DamageType.Death))
        {
            MakeEffect(BattleEffectType.DamDeath);
        }

        if (DamageTypes.Contains(DamageType.Cold))
        {
            MakeEffect(BattleEffectType.DamCold);
        }
    }

    internal static TacticalBattleEndTurnTokenEvent CreateTCCancelEndTurnTE(sbyte Player)
    {
        TacticalBattleEndTurnTokenEvent Result;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy())
        {
            Result = new TacticalBattleEndTurnTokenEvent(null);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
