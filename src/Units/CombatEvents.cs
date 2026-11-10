using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
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
    internal static void MakeDEHSList(int X, int Y, TSet<TDamageType> DamageTypes, bool Resisted)
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
                Effect = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.AddEffect(EffectType);
                Speed = (float)((double)(Random(10) + 10) * 0.1);
                Gravity = (float)0.5;
                if (Resisted || EffectType == TfxType.fxDamMagic)
                {
                    VZ = (float)((double)(Random(50) + 50) * 0.1);
                }
                else
                {
                    VZ = (float)((double)(Random(25) + 25) * 0.1);
                }

                Z = 0;
                Aow.Game.Draw.EffectPaths.MakeArcPath(X, Y, I * 360 / Count, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
            }
        }

        X = X + 16;
        if (DamageTypes.Contains(TDamageType.dtLightning))
        {
            MakeEffect(TfxType.fxDamLightning);
        }

        if (DamageTypes.Contains(TDamageType.dtFire))
        {
            MakeEffect(TfxType.fxDamFire);
        }

        if (DamageTypes.Contains(TDamageType.dtPoison))
        {
            MakeEffect(TfxType.fxDamPoison);
        }

        if (DamageTypes.Contains(TDamageType.dtMagic))
        {
            MakeEffect(TfxType.fxDamMagic);
        }

        if (DamageTypes.Contains(TDamageType.dtHoly))
        {
            MakeEffect(TfxType.fxDamHoly);
        }

        if (DamageTypes.Contains(TDamageType.dtDeath))
        {
            MakeEffect(TfxType.fxDamDeath);
        }

        if (DamageTypes.Contains(TDamageType.dtCold))
        {
            MakeEffect(TfxType.fxDamCold);
        }
    }

    internal static TCEndTurnTE CreateTCCancelEndTurnTE(sbyte Player)
    {
        TCEndTurnTE Result;
        if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).GetBusy())
        {
            Result = new TCEndTurnTE(null);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
