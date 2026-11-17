#nullable disable
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
    public TownQuake(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_townQuakeText);
        SpellId = 71;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy);
    }

    // CitySpells.pas:820
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result;
        City targetCity;
        ArmyHSList armies;
        if (executeCount == 0)
        {
            targetCity = eventValue.GetSelectedCity();
            PlayDefaultCityEffects(source, targetCity);
            targetCity.StartTownQuake();
            _ = targetCity.ValidateObservation(source.Player);
            result = true;
        }
        else
        {
            if (executeCount == 50)
            {
                targetCity = eventValue.GetSelectedCity();
                targetCity.StopTownQuake();
                armies = new ArmyHSList(null) { Holder = CreationHolder() };
                if (StrategicMap.Current.Random(10) < 7)
                {
                    targetCity.SetWallType(0);
                }

                _ = targetCity.ListArmies(armies);
                for (int i = 0, last = armies.Count - 1; i <= last; i++)
                {
                    TriggerArmyDamage(source, armies.GetArmyHexSprite(i));
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
        result = result && !targetCity.Enchantments.Contains(CityEnchantmentId.AntiMagicShell);

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
        log.Player = source.Player;
        string townQuake = AbilityRules.TranslateResourceString(GetEngine(), s_townQuakeText);
        _ = log.ArmyMessages.Add(townQuake);
        log.ArmyIcon = 2;
        log.ArmyView.Setup(army.Army, 255);
        using (army.Army.BeginUpdate())
        {
            damaged = false;
            for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
            {
                troop = army.Army.Units.GetUnit(i);
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

                _ = log.DamageValues.Add((byte)damage);
                _ = log.DamageTypes.Add(7);
            }

            if (damaged)
            {
                log.ValidateGameOver();
                EventLog.DistributeLocationEventLog(y: army.GetHexY(), level: army.GetHexLevel(), x: army.GetHexX(), log: log, player: army.Army.Player, execute: true);
            }

            _ = log.Release();
            army.Army.Validate();
        }
    }

    private static readonly string s_townQuakeDamageText = "Town Quake Damage";
    private static readonly string s_townQuakeText = "Town Quake";
}
