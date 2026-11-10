using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

internal static class CombatEvents
{
    internal static void MakeDEHSList(int X, int Y, Set<DamageType> DamageTypes, bool Resisted)
    {
        void MakeEffect(TfxType EffectType)
        {
            TfxHS Effect;
            int Count;
            float Z = default;
            float Speed = default;
            float VZ = default;
            float Gravity = default;
            Count = Random(5) + 8;
            for (int I = 1; I <= Count - 1; I++)
            {
                Effect = AoWCombatMap.Current.AddEffect(EffectType);
                Speed = (float)((double)(Random(10) + 10) * 0.1);
                Gravity = (float)0.5;
                if (Resisted || EffectType == TfxType.DamMagic)
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
            MakeEffect(TfxType.DamLightning);
        }

        if (DamageTypes.Contains(DamageType.Fire))
        {
            MakeEffect(TfxType.DamFire);
        }

        if (DamageTypes.Contains(DamageType.Poison))
        {
            MakeEffect(TfxType.DamPoison);
        }

        if (DamageTypes.Contains(DamageType.Magic))
        {
            MakeEffect(TfxType.DamMagic);
        }

        if (DamageTypes.Contains(DamageType.Holy))
        {
            MakeEffect(TfxType.DamHoly);
        }

        if (DamageTypes.Contains(DamageType.Death))
        {
            MakeEffect(TfxType.DamDeath);
        }

        if (DamageTypes.Contains(DamageType.Cold))
        {
            MakeEffect(TfxType.DamCold);
        }
    }

    internal static CEndTurnTE CreateTCCancelEndTurnTE(sbyte Player)
    {
        CEndTurnTE Result;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy())
        {
            Result = new CEndTurnTE(null);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
