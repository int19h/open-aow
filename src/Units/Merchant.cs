// Translated from Merchant.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class Merchant : Structure
{
    public Merchant(ECustomNode owner) : base(owner)
    {
    }

    // Merchant.pas:38
    public new const int ClassID = 131338;

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
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
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.Merchant.Resource.MerchantRStr);

        private static readonly string MerchantRStr = "Merchant";

    }

    // Merchant.pas:35

    // Merchant.pas:18

}
