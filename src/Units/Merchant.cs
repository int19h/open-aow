// Translated from Merchant.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TMerchant : TStructure
{
    public TMerchant(TECustomNode owner) : base(owner)
    {
    }

    // Merchant.pas:38
    public new const int ClassID = 131338;

    // Merchant.pas:35

    // Merchant.pas:18

}

public partial class TMerchantResource : TStructureResource
{
    public TMerchantResource(TECustomNode owner) : base(owner)
    {
        FTerrainBound = false;
    }

    // Merchant.pas:47
    public new const int ClassID = 131339;

    // Merchant.pas:41

    // Merchant.pas:53
    public override int ResourceUserClassID() => 131338;

    // Merchant.pas:25

    // Merchant.pas:50
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TMerchantResource.MerchantRStr);

    private static readonly string MerchantRStr = "Merchant";

}
