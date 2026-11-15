// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (spell ID, source ID, player, mana)
public partial class SpellTokenEvent : TokenEvent
{
    public SpellTokenEvent(CustomNode owner) : base(owner)
    { }

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
        try { return (byte)source.Player == EventPlayerIndex && source.CanCastSpellInstantly(EventSpellId, EventMana, ref reason); }
        finally { reason = ""; }
    }

    public virtual bool Process(Troop source, int value) => GamePack.Current.SpellControl.GetSpell(EventSpellId).ExecuteTokenEvent(this, source, value);

    public virtual void PlayDefaultCastEffects(int unused1, int unused2)
    {
        sbyte x = 0, y = 0, l = 0;
        GetSource().GetLocation(out x, out y, out l);
        var spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
        spell.PlayDefaultCastEffects(GetSource().Player, x, y, l);
    }

    public override ExecuteResult Execute(TokenExecutor executor)
    {
        base.Execute(executor);
        // ExecuteCount binds tighter than the null test. A non-hero with a later count still reaches Process.
        var source = StrategicMap.Current.UnitControl.FindUnit(EventSourceId);
        if (source == null)
        {
            return ExecuteResult.DoneHalt;
        }

        if (executor.ExecuteCount == 0 && (source is not Hero hero || !Validate(hero)))
        {
            return ExecuteResult.DoneHalt;
        }

        return Process(source, executor.ExecuteCount) ? ExecuteResult.Retry : ExecuteResult.Done;
    }
}
