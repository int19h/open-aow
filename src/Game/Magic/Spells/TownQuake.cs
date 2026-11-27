#nullable disable warnings
// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class TownQuake : CityTargetSpell
{
    // CitySpells.pas:764
    public TownQuake(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_townQuakeText);
        SpellId = 71;
        CityTargetMode = CityTargetModes.Enemy;
    }

    // CitySpells.pas:820
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        City targetCity;
        if (executeCount == 0)
        {
            targetCity = eventValue.GetSelectedCity();
            PlayDefaultCityEffects(source, targetCity);
            targetCity.StartTownQuake();
            _ = targetCity.ValidateObservation(source.Player);
            return true;
        }
        else
        {
            if (executeCount == 50)
            {
                targetCity = eventValue.GetSelectedCity();
                targetCity.StopTownQuake();
                ArmyHSList armies = new(null) { Holder = CreationHolder() };
                if (GetEngine().StrategicMap.Random(10) < 7)
                {
                    targetCity.SetWallType(0);
                }

                _ = targetCity.ListArmies(armies);
                for (int i = 0, last = armies.Count - 1; i <= last; i++)
                {
                    TriggerArmyDamage(source, army: armies[i]);
                }

                armies?.Dispose();
                CastingDone(source, mana: eventValue.EventMana);
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // CitySpells.pas:812
    public override bool ValidTargetCity(Troop source, City? targetCity)
    {
        bool result = base.ValidTargetCity(source, targetCity);
        return result && !targetCity.Enchantments.Contains(CityEnchantmentIds.AntiMagicShell);
    }

    // CitySpells.pas:773
    public void TriggerArmyDamage(Troop source, ArmyHexSprite army)
    {
        ArmyEventLog log = new(null, holder: CreationHolder())
        {
            ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: s_townQuakeDamageText),
            Player = source.Player
        };
        string townQuake = AbilityRules.TranslateResourceString(GetEngine(), S: s_townQuakeText);
        _ = log.ArmyMessages.Add(townQuake);
        log.ArmyIcon = 2;
        log.ArmyView.Setup(army.Army, selection: 255);
        using (army.Army.BeginUpdate())
        {
            bool damaged = false;
            for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
            {
                Troop? troop = army.Army.Units.GetUnit(i);
                int damage = troop.ExecuteDamageRole(7, damage: 5, damageTypes: DamageTypes.Physical);
                if (damage > troop.GetHitPoints())
                {
                    damage = troop.GetHitPoints();
                }

                if (damage > 0)
                {
                    troop.SetHitPoints((sbyte)(troop.GetHitPoints() - damage));
                    damaged = true;
                }

                _ = log.DamageValues.Add(damage.TruncateToByte());
                _ = log.DamageTypes.Add(7);
            }

            if (damaged)
            {
                log.ValidateGameOver();
                EventLog.DistributeLocationEventLog(y: army.HexY, level: army.HexLevel, x: army.HexX, log: log, player: army.Army.Player, execute: true);
            }

            _ = log.Release();
            army.Army.Validate();
        }
    }

    private static readonly string s_townQuakeDamageText = "Town Quake Damage";
    private static readonly string s_townQuakeText = "Town Quake";
}
