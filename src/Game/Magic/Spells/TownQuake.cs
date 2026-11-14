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
    public TownQuake(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_townQuakeText);
        SpellId = 71;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy);
    }

    // CitySpells.pas:820
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result;
        City targetCity;
        ArmyHSList armies;
        if (executeCount == 0)
        {
            targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
            PlayDefaultCityEffects(source, targetCity);
            targetCity.StartTownQuake();
            targetCity.ValidateObservation(source.Player);
            result = true;
        }
        else
        {
            if (executeCount == 50)
            {
                targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
                targetCity.StopTownQuake();
                armies = new ArmyHSList(null) { Holder = CreationHolder() };
                if (StrategicMap.Current.Random(10) < 7)
                {
                    targetCity.SetWallType(0);
                }

                targetCity.ListArmies(armies);
                for (int I = 0, last = armies.Count - 1; I <= last; I++)
                {
                    TriggerArmyDamage(source, armies.GetArmyHexSprite(I));
                }

                armies?.Dispose();
                CastingDone(source, eventValue.EventMana);
                result = false;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    // CitySpells.pas:812
    public override bool ValidTargetCity(Troop source, City targetCity)
    {
        bool result = base.ValidTargetCity(source, targetCity);
        if (!result || targetCity.Enchantments.Contains(CityEnchantmentId.AntiMagicShell))
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // CitySpells.pas:773
    public void TriggerArmyDamage(Troop source, ArmyHexSprite army)
    {
        ArmyEventLog log;
        bool damaged;
        int damage;
        Troop troop;
        log = new ArmyEventLog(null) { Holder = CreationHolder() };
        log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_townQuakeDamageText);
        log._player = source.Player;
        var townQuake = AbilityRules.TranslateResourceString(GetEngine(), s_townQuakeText);
        log.ArmyMessages.Add(townQuake);
        log.ArmyIcon = 2;
        log.ArmyView.Setup(army.Army, 255);
        army.Army.BeginUpdate();
        damaged = false;
        for (int I = 0, last = army.Army.Units.Count - 1; I <= last; I++)
        {
            troop = army.Army.Units.GetUnit(I);
            damage = troop.ExecuteDamageRole(7, 5, Set<DamageType>.Of(DamageType.Physical));
            if (damage > troop.GetHitPoints())
            {
                damage = troop.GetHitPoints();
            }

            if (damage > 0)
            {
                troop.SetHitPoints((sbyte)(troop.GetHitPoints() - damage));
                damaged = true;
            }

            log.DamageValues.Add((byte)damage);
            log.DamageTypes.Add(7);
        }

        if (damaged)
        {
            log.ValidateGameOver();
            var hexY = army.GetHexY();
            var hexLevel = army.GetHexLevel();
            var hexX = army.GetHexX();
            EventLog.DistributeLocationEventLog(log, army.Army.Player, hexX, hexY, hexLevel, true);
        }

        log.Release();
        army.Army.Validate();
        army.Army.EndUpdate();
    }

    private static readonly string s_townQuakeDamageText = "Town Quake Damage";
    private static readonly string s_townQuakeText = "Town Quake";
}
