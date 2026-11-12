// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

namespace Aow.Game.Magic;

public partial class CosmeticSurgeryEnchantment : UnitEnchantment
{
    public new const int ClassId = 135964;

    // AoWAb.PAS:885
    public CosmeticSurgeryEnchantment(ECustomNode owner) : base(owner)
    {
    }

    public int FaceIndex;

    // AoWAb.PAS:895
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref FaceIndex, 25);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        FaceIndex = persister.Int32("FaceIndex", FaceIndex);
    }

    // AoWAb.PAS:901
    public override string GetName()
    {
        Troop source;
        source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        // { Original supplies an argument to the unformatted 'Cosmagic Surgery' text. }
        var name = source.GetName();
        return AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CosmeticSurgeryText), [name]);
    }

    // intf_P4_02.inc:846

}
