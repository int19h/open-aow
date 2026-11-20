#nullable disable
// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public partial class CombatImageLibraryTerrain(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

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
            if (Resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                // Defense lives on the combat resource. Another resource throws, as the old cast did.
                CombatImageLibraryTerrainResource combatResource = GetResource<CombatImageLibraryTerrainResource>();
                if (message.Param[0].Int < combatResource.Defense)
                {
                    MapFieldMessageParameters param = message.Param;
                    param[0] = combatResource.Defense;
                    message.Param = param;
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
            if (_shadow is not null)
            {
                _ = visibilityList.AddSortChild(sortValue: _shadow.GetShowPriority(), child: _shadow);
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
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        Resource.ShowComplexImage(pixelXSurface, pixelYSurface, lockedSurface, 40, (uint)(Owner.HexX + (Owner.HexY << 4)));
    }

    // CombatILTer.pas:87
    public sbyte GetDefense() => GetResource<CombatImageLibraryTerrainResource>().Defense;

    // CombatILTer.pas:90
    public sbyte GetBurnValue() => GetResource<CombatImageLibraryTerrainResource>().BurnValue;

    // CombatILTer.pas:118
    public void DestroyShadow()
    {
        if (_shadow is not null)
        {
            if (GetVisible())
            {
                _ = Resource.Engine.HexSpriteMap.DynamicVisibleHexSprite.RemoveChild(_shadow);
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // CombatILTer.pas:128
    public void CreateShadow()
    {
        if (Resource.ImageSequenceList.GetImageSequence(40) is not null)
        {
            if (_shadow is not null)
            {
                DestroyShadow();
            }

            if (StrategicMap.Current is null || StrategicMap.Current.ShowUnitShadows)
            {
                _shadow = new CombatImageLibraryTerrainShadow(null)
                {
                    Holder = CreationHolder(),
                    Terrain = this
                };
            }
        }
    }

}
