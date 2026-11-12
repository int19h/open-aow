// Translated from AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (spell ID, source/target IDs, spell data, mana)
public partial class UnitSpellCombatAction : CombatAction
{
    public new const int ClassId = 131779;

    public UnitSpellCombatAction(ECustomNode owner) : base(owner)
    { }

    public int SpellId;
    public byte SourceId;
    public byte TargetId;

    public int[] SpellData { get; } = new int[2];

    public int Mana;

    // Spells.PAS:1102
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref SpellId, 20);
        stream.rwByte(ref SourceId, 21);
        stream.rwByte(ref TargetId, 22);
        stream.rwData(MemoryMarshal.AsBytes((Span<int>)SpellData).Slice(0, 8), 8, 23);
        stream.rwInteger(ref Mana, 24);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("SpellID", ref SpellId);
        persister.Byte("SourceID", ref SourceId);
        persister.Byte("TargetID", ref TargetId);
        PersistSupport.Int32Array(persister, "SpellData", SpellData);
        persister.Int32("Mana", ref Mana);
    }

    // Spells.PAS:1111
    public override void Execute(Battle combat)
    {
        ((UnitSpell)GamePack.Current.SpellControl.GetSpell(SpellId)).ExecuteCombatAction(combat, this);
    }

    // Spells.PAS:1115
    public override bool Play(CombatViewer viewer)
    {
        Execute(viewer.Combat);
        return false;
    }

    // intf_P3_02.inc:38

}
