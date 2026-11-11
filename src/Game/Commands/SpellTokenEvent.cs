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

    public new const int ClassID = 135936;

    public int FEventSpellID;
    public int FEventSourceID;
    public byte FEventPlayerIndex;
    public int FEventMana;

    // Spells.PAS:423
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FEventSpellID, 15);
        Stream.rwInteger(ref FEventSourceID, 16);
        Stream.rwByte(ref FEventPlayerIndex, 17);
        Stream.rwInteger(ref FEventMana, 18);
    }

    // Spells.PAS:431
    public void Initialize(int SpellID, byte PlayerIndex, int SourceID, int Mana)
    {
        FEventSpellID = SpellID;
        FEventSourceID = SourceID;
        FEventPlayerIndex = PlayerIndex;
        FEventMana = Mana;
    }

    // NetworkE.pas:230
    public Troop GetSource() => AoWHSMap.Current.FUnitControl.FindUnit(FEventSourceID);

    public virtual bool Validate(Hero source)
    {
        string reason = "";
        try { return (byte)source.FPlayer == FEventPlayerIndex && source.CanCastSpellInstantly(FEventSpellID, FEventMana, ref reason); }
        finally { reason = ""; }
    }

    public virtual bool Process(Troop source, int value) => AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).ExecuteTE(this, source, value);

    public virtual void PlayDefaultCastEffects(int unused1, int unused2)
    {
        sbyte x = 0, y = 0, l = 0;
        GetSource().GetLocation(ref x, ref y, ref l);
        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
        spell.PlayDefaultCastEffects(GetSource().FPlayer, x, y, l);
    }

    public override ExecuteResult Execute(TokenExecutor executer)
    {
        base.Execute(executer);
        // ExecuteCount binds tighter than the null test. A non-hero with a later count still reaches Process.
        var source = AoWHSMap.Current.FUnitControl.FindUnit(FEventSourceID);
        if (source == null)
        {
            return ExecuteResult.DoneHalt;
        }

        if (executer.ExecuteCount == 0 && (source is not Hero hero || !Validate(hero)))
        {
            return ExecuteResult.DoneHalt;
        }

        return Process(source, executer.ExecuteCount) ? ExecuteResult.Retry : ExecuteResult.Done;
    }
}
