// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public partial class CombatImageLibraryTerrain : ImageLibraryTerrainMapObject
{
    public CombatImageLibraryTerrain(ECustomNode owner) : base(owner)
    { }

    // CombatILTer.pas:105
    public new const int ClassId = 2228534;

    private CombatImageLibraryTerrainShadow _shadow;

    // CombatILTer.pas:99
    public override void Dispose()
    {
        DestroyShadow();
        base.Dispose();
    }

    // CombatILTer.pas:166
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 135169)
        {
            if (_resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                if (message.Param[0].Int < ((CombatImageLibraryTerrainResource)_resource).Defense)
                {
                    message.Param[0] = ((CombatImageLibraryTerrainResource)_resource).Defense;
                }
            }
        }
    }

    // CombatILTer.pas:151
    public override void Activate()
    {
        base.Activate();
        CreateShadow();
    }

    // CombatILTer.pas:157
    public override void Deactivate()
    {
        DestroyShadow();
        base.Deactivate();
    }

    // CombatILTer.pas:141
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatusElement.Visible))
        {
            base.MakeVisible(visibilityList);
            if (_shadow != null)
            {
                var showPriority = _shadow.GetShowPriority();
                visibilityList.AddSortChild(_shadow, showPriority);
            }
        }
    }

    // CombatILTer.pas:163
    public override MapObjectResourceList GetResourceList(GameEngine engine) => GamePack.Current.CombatResources;

    // CombatILTer.pas:93
    public override bool CanPlaceOnHexSprite(HexSprite hexSprite) => base.CanPlaceOnHexSprite(hexSprite) || hexSprite is AbstractRoad;

    // CombatILTer.pas:108
    public virtual void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(_hexX, _hexY, ref pixelX, ref pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        _resource.ShowComplexImage(pixelXSurface, pixelYSurface, lockedSurface, 40, (uint)(Owner.HexX + (Owner.HexY << 4)));
    }

    // CombatILTer.pas:87
    public sbyte GetDefense() => ((CombatImageLibraryTerrainResource)_resource).Defense;

    // CombatILTer.pas:90
    public sbyte GetBurnValue() => ((CombatImageLibraryTerrainResource)_resource).BurnValue;

    // CombatILTer.pas:118
    public void DestroyShadow()
    {
        if (_shadow != null)
        {
            if (GetVisible())
            {
                _resource.Engine.HexSpriteMap.DynamicVisibleHexSprite.RemoveChild(_shadow);
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // CombatILTer.pas:128
    public void CreateShadow()
    {
        if (_resource.ImageSequenceList.GetImageSequence(40) != null)
        {
            if (_shadow != null)
            {
                DestroyShadow();
            }

            if (StrategicMap.Current == null || StrategicMap.Current.ShowUnitShadows)
            {
                _shadow = new CombatImageLibraryTerrainShadow(null);
                _shadow._terrain = this;
            }
        }
    }

}
