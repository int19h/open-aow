#nullable disable
// Translated from MagicSanctuary.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// OverlayHexSprite
public class MagicSanctuary : Structure
{
    public MagicSanctuary(CustomNode owner) : base(owner)
    {
        _overlayHexSprite = new MagicSanctuaryOverlayHexSprite(null)
        {
            Holder = CreationHolder(),
            Structure = this
        };
    }

    // MagicSanctuary.pas:124
    public new const int ClassId = 133274;

    private readonly MagicSanctuaryOverlayHexSprite _overlayHexSprite;

    // MagicSanctuary.pas:117
    public override void Dispose()
    {
        _ = StrategicMap.Current.DynamicVisibleHexSprite.RemoveChild(_overlayHexSprite);
        _overlayHexSprite?.Dispose();
        base.Dispose();
    }

    // MagicSanctuary.pas:136
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (base.Resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
        {
            if (message.MessageId == 4393)
            {
                MapFieldMessageParameters param = message.Param;
                param[0] = 1;
                message.Param = param;
            }
        }
    }

    // MagicSanctuary.pas:127
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatusElement.Visible))
        {
            base.MakeVisible(visibilityList);
            _overlayHexSprite.MakeVisible(visibilityList);
        }
    }

    // MagicSanctuary.pas:143
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        _overlayHexSprite.FadeUp();
    }

    public new class Resource : StructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = false;
            Razeable = false;
            ConnectRoads = false;
        }

        // MagicSanctuary.pas:160
        public new const int ClassId = 133275;

        // MagicSanctuary.pas:163
        public override int ResourceUserClassId() => 133274;

        // MagicSanctuary.pas:157
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_magicSanctuaryText);

        private static readonly string s_magicSanctuaryText = "Magic Sanctuary";

    }
}
