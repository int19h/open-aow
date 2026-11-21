#nullable disable warnings
// Translated from CombatObstacle.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World;

public class CombatObstacle(CustomNode owner) : MultiHexMapObject(owner)
{

    // CombatObstacle.pas:94
    public new const int ClassId = 2_229_546;

    private CombatObstacleShadow? _shadow;
    private CombatObstacleOverlay? _overlay;

    // CombatObstacle.pas:97
    public override void Dispose()
    {
        DestroyShadow();
        DestroyOverlay();
        base.Dispose();
    }

    // CombatObstacle.pas:110
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // CombatObstacle.pas:116
    public override void RemoveFromMap()
    {
        MapLevel mapLevel = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(mapLevel, StoredHexX, StoredHexY);
    }

    // CombatObstacle.pas:229
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59))
        {
            if (base.Resource.TerrainList.GetOverlay(info.HexNumber) == 8)
            {
                moveResult.Code = -1;
                moveResult.Done = true;
            }
        }
    }

    // CombatObstacle.pas:208
    public override void Activate()
    {
        base.Activate();
        CreateShadow();
        CreateOverlay();
    }

    // CombatObstacle.pas:215
    public override void Deactivate()
    {
        DestroyShadow();
        DestroyOverlay();
        base.Deactivate();
    }

    // CombatObstacle.pas:222
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        base.UpdateMapField(info);
        if (info.HexNumber < base.Resource.TerrainList.Count)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOnFlag;
        }
    }

    // CombatObstacle.pas:193
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatuses.Visible))
        {
            base.MakeVisible(visibilityList);
            if (_shadow is not null)
            {
                _ = visibilityList.AddSortChild(sortValue: _shadow.GetShowPriority(), child: _shadow);
            }

            if (_overlay is not null)
            {
                _ = visibilityList.AddSortChild(sortValue: _overlay.GetShowPriority(), child: _overlay);
            }
        }
    }

    // CombatObstacle.pas:107
    public override MapObjectResourceList GetResourceList(GameEngine? engine) => GamePack.Current.CombatResources;

    // CombatObstacle.pas:104
    public override bool CanPlace(MapContainer? mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel) => true;

    // CombatObstacle.pas:203
    public override bool PlaceHex(MapContainer? mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel) => base.PlaceHex(mapContainer, hexX, hexY, hexLevel);

    // CombatObstacle.pas:125
    public void CreateShadow()
    {
        if (base.Resource.ImageSequenceList.GetImageSequence(40) is not null)
        {
            if (_shadow is not null)
            {
                DestroyShadow();
            }

            if (StrategicMap.Current is null || StrategicMap.Current.ShowUnitShadows)
            {
                _shadow = new CombatObstacleShadow(null)
                {
                    Holder = CreationHolder(),
                    Obstacle = this
                };
            }
        }
    }

    // CombatObstacle.pas:138
    public void DestroyShadow()
    {
        if (_shadow is not null)
        {
            if (Visible)
            {
                _ = CombatMap.Current.DynamicVisibleHexSprite.RemoveChild(_shadow);
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // CombatObstacle.pas:148
    public void CreateOverlay()
    {
        if (base.Resource.ImageSequenceList.GetImageSequence(41) is not null)
        {
            if (_overlay is not null)
            {
                DestroyOverlay();
            }

            if (StrategicMap.Current is null || StrategicMap.Current.ShowUnitShadows)
            {
                _overlay = new CombatObstacleOverlay(null)
                {
                    Holder = CreationHolder(),
                    Obstacle = this
                };
            }
        }
    }

    // CombatObstacle.pas:161
    public void DestroyOverlay()
    {
        if (_overlay is not null)
        {
            if (Visible)
            {
                _ = CombatMap.Current.DynamicVisibleHexSprite.RemoveChild(_overlay);
            }

            _overlay?.Dispose();
            _overlay = null;
        }
    }

    // CombatObstacle.pas:171
    public void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int x, out int y);
        pixelXSurface = x - pixelXSurface;
        pixelYSurface = y - pixelYSurface;
        base.Resource.ShowComplexImage(pixelXSurface, pixelYSurface, lockedSurface, index: 40, offset: (uint)(Owner.HexX + (Owner.HexY << 4)));
    }

    // CombatObstacle.pas:182
    public void ShowOverlay(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int x, out int y);
        pixelXSurface = x - pixelXSurface;
        pixelYSurface = y - pixelYSurface;
        base.Resource.ShowComplexImage(pixelXSurface, pixelYSurface, lockedSurface, index: 41, offset: (uint)(Owner.HexX + (Owner.HexY << 4)));
    }

    public new class Resource(CustomNode owner) : MultiHexMapObjectResource(owner)
    {

        // CombatObstacle.pas:239
        public new const int ClassId = 2_229_547;

        // CombatObstacle.pas:245
        public override int ResourceUserClassId() => CombatObstacle.ClassId;

        // CombatObstacle.pas:242
        public override string ResourceEditName() => $"Combat {AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ObstacleText)}";

    }
}
