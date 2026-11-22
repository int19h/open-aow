#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Units;

// Race. HeroType. Name. Alignment. Hidden. base statistics. Abilities. FaceIndex
public class HeroResource : TroopResource
{
    public new const int ClassId = 131_633;

    // Hero.PAS:2308
    public HeroResource(CustomNode? owner) : base(owner)
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
    public int Race { get; private set; }
    public HeroType HeroType { get; set; }
    public Alignment Alignment { get; private set; }
    public bool Hidden { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int Damage { get; private set; }
    public int Hits { get; private set; }
    public int Moves { get; private set; }
    public int Resistance { get; private set; }

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
        Race = persister.UnsignedByte(name: "Race", value: Race.TruncateToSByte());
        HeroType = persister.Enum("HeroType", value: HeroType);
        Alignment = persister.Enum("Alignment", value: Alignment);
        if (Alignment > Alignment.Unaligned)
        {
            Alignment = Alignment.Neutral;
        }

        Hidden = persister.Boolean("Hidden", value: Hidden);
        Attack = persister.UnsignedByte(name: "Attack", value: Attack.TruncateToSByte());
        Defense = persister.UnsignedByte(name: "Defense", value: Defense.TruncateToSByte());
        Damage = persister.UnsignedByte(name: "Damage", value: Damage.TruncateToSByte());
        Hits = persister.UnsignedByte(name: "Hits", value: Hits.TruncateToSByte());
        Moves = persister.UnsignedByte(name: "Moves", value: Moves.TruncateToSByte());
        Resistance = persister.UnsignedByte(name: "Resistance", value: Resistance.TruncateToSByte());
        persister.Object("Abilities", objectValue: Abilities);
        FaceIndex = persister.Int32("FaceIndex", value: FaceIndex);
    }

    // Hero.PAS:2326
    public override int ResourceUserClassId() => Hero.ClassId;

    // Hero.PAS:2329
    public int HeroResourceIndex() => Owner.FindChildIndex(this);

    // Hero.PAS:2332
    public int UsedSkillPoints()
    {
        int result = (Attack * 5) + (Defense * 5) + (Damage * 10) + (Resistance * 5) + (Hits * 5) + (Moves * 2);
        for (int i = Abilities.OwnAbilityCount - 1; i >= 0; i--)
        {
            if (Abilities.GetAbilitySetAt(i))
            {
                result += GamePack.Current.AbilityControl[i].GetSkillPoints(Abilities);
            }
        }

        return result;
    }
}
