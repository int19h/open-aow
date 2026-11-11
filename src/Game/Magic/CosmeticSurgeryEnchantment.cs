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
    public new const int ClassID = 135964;

    // AoWAb.PAS:885
    public CosmeticSurgeryEnchantment(ECustomNode owner) : base(owner)
    {
    }

    public int FFaceIndex;

    // AoWAb.PAS:895
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FFaceIndex, 25);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("FaceIndex", ref FFaceIndex);
    }

    // AoWAb.PAS:901
    public override string GetName()
    {
        Troop Source;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        // { Original supplies an argument to the unformatted 'Cosmagic Surgery' text. }
        var name = Source.GetName();
        return AbilityRules.Format(AbilityRules.TranslateRStr(GameText.CosmeticSurgeryRStr), [name]);
    }

    // intf_P4_02.inc:846

}
