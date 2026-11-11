// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class TownQuake : CityTargetSpell
{
    // CitySpells.pas:764
    public TownQuake(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(TownQuakeRStr);
        FSpellID = 71;
        FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy);
    }

    // CitySpells.pas:820
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        bool Result;
        City TargetCity;
        ArmyHSList Armies;
        if (ExecuteCount == 0)
        {
            TargetCity = ((CityTargetSpellTokenEvent)Event).GetSelectedCity();
            PlayDefaultCityEffects(Source, TargetCity);
            TargetCity.StartTownQuake();
            TargetCity.ValidateObservation(Source.FPlayer);
            Result = true;
        }
        else
        {
            if (ExecuteCount == 50)
            {
                TargetCity = ((CityTargetSpellTokenEvent)Event).GetSelectedCity();
                TargetCity.StopTownQuake();
                Armies = new ArmyHSList(null);
                if (AoWHSMap.Current.Random(10) < 7)
                {
                    TargetCity.SetWallType(0);
                }

                TargetCity.ListArmies(Armies);
                for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                {
                    TriggerArmyDamage(Source, Armies.GetArmyHS(I));
                }

                Armies?.Dispose();
                CastingDone(Source, Event.FEventMana);
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // CitySpells.pas:812
    public override bool ValidTargetCity(Troop Source, City TargetCity)
    {
        bool Result = base.ValidTargetCity(Source, TargetCity);
        if (!Result || TargetCity.FEnchantments.Contains(CityEnchantmentId.AntiMagicShell))
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // CitySpells.pas:773
    public void TriggerArmyDamage(Troop Source, ArmyHexSprite Army)
    {
        ArmyEventLog Log;
        bool Damaged;
        int Damage;
        Troop AUnit;
        Log = new ArmyEventLog(null);
        Log.FArmyText = AbilityRules.TranslateRStr(TownQuakeDamageRStr);
        Log.FPlayer = Source.FPlayer;
        var townQuake = AbilityRules.TranslateRStr(TownQuakeRStr);
        Log.FArmyMessages.Add(townQuake);
        Log.FArmyIcon = 2;
        Log.FArmyView.Setup(Army.FArmy, 255);
        Army.FArmy.BeginUpdate();
        Damaged = false;
        for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
        {
            AUnit = Army.FArmy.GetUnit(I);
            Damage = AUnit.ExecuteDamageRole(7, 5, Set<DamageType>.Of(DamageType.Physical));
            if (Damage > AUnit.GetHitPoints())
            {
                Damage = AUnit.GetHitPoints();
            }

            if (Damage > 0)
            {
                AUnit.SetHitPoints((sbyte)(AUnit.GetHitPoints() - Damage));
                Damaged = true;
            }

            Log.FDamageValues.Add((byte)Damage);
            Log.FDamageTypes.Add(7);
        }

        if (Damaged)
        {
            Log.ValidateGameOver();
            var yhx = Army.GetYhx();
            var lhx = Army.GetLhx();
            var xhx = Army.GetXhx();
            EventLog.DistributeLocationEventLog(Log, Army.FArmy.FPlayer, xhx, yhx, lhx, true);
        }

        Log.Release();
        Army.FArmy.Validate();
        Army.FArmy.EndUpdate();
    }

    private static readonly string TownQuakeDamageRStr = "Town Quake Damage";
    private static readonly string TownQuakeRStr = "Town Quake";
}
