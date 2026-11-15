// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public partial class CombatImageLibraryTerrain : ImageLibraryTerrainMapObject
{
    public CombatImageLibraryTerrain(CustomNode owner) : base(owner)
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
            if (Resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                if (message.Param[0].Int < ((CombatImageLibraryTerrainResource)Resource).Defense)
                {
                    message.Param[0] = ((CombatImageLibraryTerrainResource)Resource).Defense;
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
                visibilityList.AddSortChild(sortValue: _shadow.GetShowPriority(), child: _shadow);
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
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out pixelX, out pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        Resource.ShowComplexImage(pixelXSurface, pixelYSurface, lockedSurface, 40, (uint)(Owner.HexX + (Owner.HexY << 4)));
    }

    // CombatILTer.pas:87
    public sbyte GetDefense() => ((CombatImageLibraryTerrainResource)Resource).Defense;

    // CombatILTer.pas:90
    public sbyte GetBurnValue() => ((CombatImageLibraryTerrainResource)Resource).BurnValue;

    // CombatILTer.pas:118
    public void DestroyShadow()
    {
        if (_shadow != null)
        {
            if (GetVisible())
            {
                Resource.Engine.HexSpriteMap.DynamicVisibleHexSprite.RemoveChild(_shadow);
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // CombatILTer.pas:128
    public void CreateShadow()
    {
        if (Resource.ImageSequenceList.GetImageSequence(40) != null)
        {
            if (_shadow != null)
            {
                DestroyShadow();
            }

            if (StrategicMap.Current == null || StrategicMap.Current.ShowUnitShadows)
            {
                _shadow = new CombatImageLibraryTerrainShadow(null) { Holder = CreationHolder() };
                _shadow.Terrain = this;
            }
        }
    }

}
