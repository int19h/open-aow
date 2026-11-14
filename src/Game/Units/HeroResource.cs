// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// Race. HeroType. Name. Alignment. Hidden. base statistics. Abilities. FaceIndex
public partial class HeroResource : TroopResource
{
    public new const int ClassId = 131633;

    // Hero.PAS:2308
    public HeroResource(ECustomNode owner) : base(owner)
    {
        Abilities = new HeroResourceAbilityOwner(null) { Holder = CreationHolder() };
        Attack = 3;
        Defense = 3;
        Damage = 3;
        Hits = 5;
        Moves = 28;
        Resistance = 2;
    }

    public string Name { get; set; } = "";
    public sbyte Race { get; private set; }
    public HeroType HeroType { get; set; }
    public Alignment Alignment { get; private set; }
    public bool Hidden { get; private set; }
    public sbyte Attack { get; private set; }
    public sbyte Defense { get; private set; }
    public sbyte Damage { get; private set; }
    public sbyte Hits { get; private set; }
    public sbyte Moves { get; private set; }
    public sbyte Resistance { get; private set; }

    public AbilityOwner Abilities { get; }

    public int FaceIndex { get; private set; }

    // Hero.PAS:2320
    public override void Dispose()
    {
        Abilities?.Dispose();
        base.Dispose();
    }

    // Hero.PAS:2341
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Name = persister.String("Name", value: Name);
        Race = PersistSupport.UnsignedByte(persister, name: "Race", value: Race);
        HeroType = persister.Enum("HeroType", value: HeroType);
        Alignment = persister.Enum("Alignment", value: Alignment);
        if (Alignment > Alignment.None)
        {
            Alignment = Alignment.Neutral;
        }

        Hidden = persister.Boolean("Hidden", value: Hidden);
        Attack = PersistSupport.UnsignedByte(persister, name: "Attack", value: Attack);
        Defense = PersistSupport.UnsignedByte(persister, name: "Defense", value: Defense);
        Damage = PersistSupport.UnsignedByte(persister, name: "Damage", value: Damage);
        Hits = PersistSupport.UnsignedByte(persister, name: "Hits", value: Hits);
        Moves = PersistSupport.UnsignedByte(persister, name: "Moves", value: Moves);
        Resistance = PersistSupport.UnsignedByte(persister, name: "Resistance", value: Resistance);
        persister.Object("Abilities", objectValue: Abilities);
        FaceIndex = persister.Int32("FaceIndex", value: FaceIndex);
    }

    // Hero.PAS:2326
    public override int ResourceUserClassId() => 131632;

    // Hero.PAS:2329
    public int HeroResourceIndex() => Owner.FindChildIndex(this);

    // Hero.PAS:2332
    public int UsedSkillPoints()
    {
        int result = Attack * 5 + Defense * 5 + Damage * 10 + Resistance * 5 + Hits * 5 + Moves * 2;
        for (int I = Abilities.GetAbCount() - 1; I >= 0; I--)
        {
            if (Abilities.GetAbilitySetAt(I))
            {
                result += GamePack.Current.AbilityControl.GetAbility(I).GetSkillPoints(Abilities);
            }
        }

        return result;
    }

    // intf_P2_07.inc:343

}
