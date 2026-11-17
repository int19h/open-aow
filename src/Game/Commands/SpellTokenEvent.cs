// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (spell ID, source ID, player, mana)
public partial class SpellTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135936;

    public int EventSpellId { get; protected set; }
    public int EventSourceId { get; set; }
    public byte EventPlayerIndex { get; protected set; }
    public int EventMana { get; protected set; }

    // Spells.PAS:423
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EventSpellId = persister.Int32("EventSpellId", value: EventSpellId);
        EventSourceId = persister.Int32("EventSourceId", value: EventSourceId);
        EventPlayerIndex = persister.Byte("EventPlayerIndex", value: EventPlayerIndex);
        EventMana = persister.Int32("EventMana", value: EventMana);
    }

    // Spells.PAS:431
    public void Initialize(int spellId, byte playerIndex, int sourceId, int mana)
    {
        EventSpellId = spellId;
        EventSourceId = sourceId;
        EventPlayerIndex = playerIndex;
        EventMana = mana;
    }

    // NetworkE.pas:230
    public Troop GetSource() => StrategicMap.Current.UnitControl.FindUnit(EventSourceId);

    public virtual bool Validate(Hero source)
    {
        string reason = "";
        return (byte)source.Player == EventPlayerIndex && source.CanCastSpellInstantly(EventSpellId, EventMana, ref reason);
    }

    public virtual bool Process(Troop source, int value) => GamePack.Current.SpellControl.GetSpell(EventSpellId).ExecuteTokenEvent(this, source, value);

    public virtual void PlayDefaultCastEffects(int unused1, int unused2)
    {
        MapCoordinate coordinate10 = GetSource().GetLocation();
        sbyte x = coordinate10.X;
        sbyte y = coordinate10.Y;
        sbyte l = coordinate10.MapLevel;
        Spell spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
        spell.PlayDefaultCastEffects(GetSource().Player, x, y, l);
    }

    public override ExecuteResult Execute(TokenExecutor executor)
    {
        _ = base.Execute(executor);
        // ExecuteCount binds tighter than the null test. A non-hero with a later count still reaches Process.
        Troop source = StrategicMap.Current.UnitControl.FindUnit(EventSourceId);
        return source == null
            ? ExecuteResult.DoneHalt
            : executor.ExecuteCount == 0 && (source is not Hero hero || !Validate(hero))
            ? ExecuteResult.DoneHalt
            : Process(source, executor.ExecuteCount) ? ExecuteResult.Retry : ExecuteResult.Done;
    }
}
