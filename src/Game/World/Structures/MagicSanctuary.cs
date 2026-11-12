// Translated from MagicSanctuary.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// OverlayHexSprite
public partial class MagicSanctuary : Structure
{
    public MagicSanctuary(ECustomNode owner) : base(owner)
    {
        OverlayHexSprite = new MagicSanctuaryOverlayHexSprite(null);
        OverlayHexSprite.Structure = this;
    }

    // MagicSanctuary.pas:124
    public new const int ClassId = 133274;

    public MagicSanctuaryOverlayHexSprite OverlayHexSprite;

    // MagicSanctuary.pas:117
    public override void Dispose()
    {
        AoWHSMap.Current.DynamicVisibleHexSprite.RemoveChild(OverlayHexSprite);
        OverlayHexSprite?.Dispose();
        base.Dispose();
    }

    // MagicSanctuary.pas:110

    // MagicSanctuary.pas:136
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (_resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
        {
            if (message.MessageId == 4393)
            {
                message.Param[0] = 1;
            }
        }
    }

    // MagicSanctuary.pas:127
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!_status.Contains(MapObjectStatusElement.Visible))
        {
            base.MakeVisible(visibilityList);
            OverlayHexSprite.MakeVisible(visibilityList);
        }
    }

    // MagicSanctuary.pas:143
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        OverlayHexSprite.FadeUp();
    }

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            TerrainBound = false;
            Razeable = false;
            ConnectRoads = false;
        }

        // MagicSanctuary.pas:160
        public new const int ClassId = 133275;

        // MagicSanctuary.pas:149

        // MagicSanctuary.pas:163
        public override int ResourceUserClassId() => 133274;

        // MagicSanctuary.pas:43

        // MagicSanctuary.pas:157
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_magicSanctuaryText);

        private static readonly string s_magicSanctuaryText = "Magic Sanctuary";

    }

    // MagicSanctuary.pas:33

}
