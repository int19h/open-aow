#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Commands;

// UnitId. Player. statistic upgrades. Abilities
public class HeroUpgradeTokenEvent : TokenEvent
{
    public new const int ClassId = 135_965;

    public int UnitId { get; set; }
    public PlayerId? Player { get; set; } = PlayerId.Independent;
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Resistance { get; set; }
    public int Damage { get; set; }
    public int Moves { get; set; }
    public int Hits { get; set; }

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
        Attack = persister.UnsignedByte(name: "Attack", value: Attack.TruncateToSByte());
        Defense = persister.UnsignedByte(name: "Defense", value: Defense.TruncateToSByte());
        Resistance = persister.UnsignedByte(name: "Resistance", value: Resistance.TruncateToSByte());
        Damage = persister.UnsignedByte(name: "Damage", value: Damage.TruncateToSByte());
        Moves = persister.UnsignedByte(name: "Moves", value: Moves.TruncateToSByte());
        Hits = persister.UnsignedByte(name: "Hits", value: Hits.TruncateToSByte());
        persister.Object("Abilities", objectValue: Abilities);
        UnitId = persister.Int32("UnitId", value: UnitId);
        Player = persister.OptionalPlayer(name: "Player", value: Player, width: IdWidth.UnsignedByte, fallback: 0);
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
            hero.SetUnitAttack((hero.UnitAttack + Attack).TruncateToSByte());
            hero.SetUnitDefense((hero.UnitDefense + Defense).TruncateToSByte());
            hero.SetUnitResistance((hero.UnitResistance + Resistance).TruncateToSByte());
            hero.SetUnitDamage((hero.UnitDamage + Damage).TruncateToSByte());
            hero.SetUnitMoves((hero.UnitMoves + Moves).TruncateToSByte());
            hero.SetUnitHits((hero.UnitHits + Hits).TruncateToSByte());
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
