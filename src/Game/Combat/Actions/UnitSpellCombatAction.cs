// Translated from AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (spell ID, source/target IDs, spell data, mana)
public partial class UnitSpellCombatAction : CombatAction
{
    public new const int ClassID = 131779;

    public UnitSpellCombatAction(ECustomNode owner) : base(owner)
    { }

    public int FSpellID;
    public byte FSourceID;
    public byte FTargetID;
    public Int32Array2 FSpellData;
    public int FMana;

    // Spells.PAS:1102
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FSpellID, 20);
        Stream.rwByte(ref FSourceID, 21);
        Stream.rwByte(ref FTargetID, 22);
        Stream.rwData(MemoryMarshal.AsBytes((Span<int>)FSpellData).Slice(0, 8), 8, 23);
        Stream.rwInteger(ref FMana, 24);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("SpellID", ref FSpellID);
        persister.Byte("SourceID", ref FSourceID);
        persister.Byte("TargetID", ref FTargetID);
        PersistSupport.Int32Array(persister, "SpellData", FSpellData);
        persister.Int("Mana", ref FMana);
    }

    // Spells.PAS:1111
    public override void Execute(Battle Combat)
    {
        ((UnitSpell)AoWHSSet.Current.FSpellControl.GetSpell(FSpellID)).ExecuteCA(Combat, this);
    }

    // Spells.PAS:1115
    public override bool Play(CombatViewer Viewer)
    {
        Execute(Viewer.FCombat);
        return false;
    }

    // intf_P3_02.inc:38

}
