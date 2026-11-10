// Translated from MagicSanctuary.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// FOverlayHS
public partial class MagicSanctuary : Structure
{
    public MagicSanctuary(ECustomNode owner) : base(owner)
    {
        FOverlayHS = new MagicSanctuaryOverlayHS(null);
        FOverlayHS.FStructure = this;
    }

    // MagicSanctuary.pas:124
    public new const int ClassID = 133274;

    public MagicSanctuaryOverlayHS FOverlayHS;

    // MagicSanctuary.pas:117
    public override void Dispose()
    {
        AoWHSMap.Current.DynamicVisibleHS.RemoveChild(FOverlayHS);
        FOverlayHS?.Dispose();
        base.Dispose();
    }

    // MagicSanctuary.pas:110

    // MagicSanctuary.pas:136
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
        {
            if (Msg.MsgID == 4393)
            {
                Msg.Param[0] = 1;
            }
        }
    }

    // MagicSanctuary.pas:127
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if (!FStatus.Contains(MapObjectStatusElement.Visible))
        {
            base.MakeVisible(VisibilityList);
            FOverlayHS.MakeVisible(VisibilityList);
        }
    }

    // MagicSanctuary.pas:143
    public override void ArmyPlaced(ArmyHS PlacedArmy)
    {
        base.ArmyPlaced(PlacedArmy);
        FOverlayHS.FadeUp();
    }

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
            FRazeable = false;
            FConnectRoads = false;
        }

        // MagicSanctuary.pas:160
        public new const int ClassID = 133275;

        // MagicSanctuary.pas:149

        // MagicSanctuary.pas:163
        public override int ResourceUserClassID() => 133274;

        // MagicSanctuary.pas:43

        // MagicSanctuary.pas:157
        public override string ResourceEditName() => AbilityRules.TranslateRStr(MagicSanctuaryRStr);

        private static readonly string MagicSanctuaryRStr = "Magic Sanctuary";

    }

    // MagicSanctuary.pas:33

}
