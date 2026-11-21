#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// UnitId. Player. statistic upgrades. Abilities
public class HeroUpgradeTokenEvent : TokenEvent
{
    public new const int ClassId = 135_965;

    public int UnitId { get; set; }
    public sbyte Player { get; set; }
    public sbyte Attack { get; set; }
    public sbyte Defense { get; set; }
    public sbyte Resistance { get; set; }
    public sbyte Damage { get; set; }
    public sbyte Moves { get; set; }
    public sbyte Hits { get; set; }

    public AbilityOwner Abilities { get; }

    // Hero.PAS:7
    public override void Dispose()
    {
        Abilities?.Dispose();
        base.Dispose();
    }

    // Hero.PAS:13
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Attack = persister.UnsignedByte(name: "Attack", value: Attack);
        Defense = persister.UnsignedByte(name: "Defense", value: Defense);
        Resistance = persister.UnsignedByte(name: "Resistance", value: Resistance);
        Damage = persister.UnsignedByte(name: "Damage", value: Damage);
        Moves = persister.UnsignedByte(name: "Moves", value: Moves);
        Hits = persister.UnsignedByte(name: "Hits", value: Hits);
        persister.Object("Abilities", objectValue: Abilities);
        UnitId = persister.Int32("UnitId", value: UnitId);
        Player = persister.UnsignedByte(name: "Player", value: Player);
    }

    // Hero.PAS:1
    public HeroUpgradeTokenEvent(CustomNode? owner) : base(owner) => Abilities = new AbilityOwner(null) { Holder = CreationHolder() };

    // Hero.PAS:30
    public override ExecuteResult Execute(TokenExecutor? tokenEvent)
    {
        ExecuteResult result = base.Execute(tokenEvent);
        Hero hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(UnitId);
        if (hero is not null && hero.GetHitPoints() != 0 && hero.Player == Player)
        {
            hero.SetUnitAttack((sbyte)(hero.UnitAttack + Attack));
            hero.SetUnitDefense((sbyte)(hero.UnitDefense + Defense));
            hero.SetUnitResistance((sbyte)(hero.UnitResistance + Resistance));
            hero.SetUnitDamage((sbyte)(hero.UnitDamage + Damage));
            hero.SetUnitMoves((sbyte)(hero.UnitMoves + Moves));
            hero.SetUnitHits((sbyte)(hero.UnitHits + Hits));
            hero.UpdateDefaultAbilities(Abilities);
            hero.UpgradePending = false;
        }

        return result;
    }

    // Hero.PAS:48
    public void Setup(Hero hero)
    {
        UnitId = hero.UnitId;
        Player = hero.Player;
    }
}
