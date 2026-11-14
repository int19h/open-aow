// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

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

    public int SpellId { get; set; }
    public byte SourceId { get; set; }
    public byte TargetId { get; set; }

    public int[] SpellData { get; } = new int[2];

    public int Mana { get; set; }

    // Spells.PAS:1102
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SpellId = persister.Int32("SpellId", SpellId);
        SourceId = persister.Byte("SourceId", SourceId);
        TargetId = persister.Byte("TargetId", TargetId);
        PersistSupport.Int32Array(persister, "SpellData", SpellData);
        Mana = persister.Int32("Mana", Mana);
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
