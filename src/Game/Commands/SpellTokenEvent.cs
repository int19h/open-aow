// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (spell ID, source ID, player, mana)
public partial class SpellTokenEvent : TokenEvent
{
    public SpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135936;

    public int EventSpellId;
    public int EventSourceId;
    public byte EventPlayerIndex;
    public int EventMana;

    // Spells.PAS:423
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref EventSpellId, 15);
        stream.Int32(ref EventSourceId, 16);
        stream.Byte(ref EventPlayerIndex, 17);
        stream.Int32(ref EventMana, 18);
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
