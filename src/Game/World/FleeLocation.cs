#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from FleeLoc.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World;

public class FleeLocation(CustomNode owner) : MapObject(owner)
{

    // FleeLoc.pas:76
    public new const int ClassId = 2_229_544;

    private bool _highlighted;
    private int _alpha;
    private int _alphaStep;

    // FleeLoc.pas:82
    public override int GetShowPriority() => 1_744_830_464 + (HexY << 13) + HexX;

    // FleeLoc.pas:151
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (_alpha > 0)
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int x, out int y);
            base.Resource.ImageLibrary.Get(0).ShowBlended(x - pixelXSurface - 8, y - pixelYSurface, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[_alpha]);
        }
    }

    // FleeLoc.pas:120
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (info.HexNumber == 0)
        {
            moveResult.Code = ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Side == CombatSideIndex.Attacker
                ? 25
                : ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainNumber][info.MapField.OverlayType.ToOriginal() - MovementTables.FirstOverlay];

            moveResult.Done = true;
        }
    }

    // FleeLoc.pas:111
    public override void Activate()
    {
        base.Activate();
        CombatMap.Current.FrameStarted += NewFrame;
        _highlighted = CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting);
        _alpha = _highlighted ? 50 : 0;

        CombatMap.Current.FleeLocations.Add(this);
    }

    // FleeLoc.pas:65
    public override void Deactivate()
    {
        CombatMap.Current.FrameStarted -= NewFrame;
        base.Deactivate();
    }

    // FleeLoc.pas:90
    public override bool CanSelect(int pixelX, int pixelY) => true;

    // FleeLoc.pas:79
    public override MapObjectResourceList GetResourceList(GameEngine engine) => GamePack.Current.CombatResources;

    // FleeLoc.pas:87
    public override bool CanPlace(MapContainer mapContainer, int hexX, int hexY, int hexLevel) => true;

    // FleeLoc.pas:93
    public override bool PlaceHex(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        if (!EditMode)
        {
            MapField mapField = mapContainer.GetField(hexX, hexY, hexLevel);
            EngineObject oldHex = mapField.FindOwnedChild(Hexes.BorderHex.ClassId);
            if (oldHex is not null)
            {
                oldHex?.Dispose();
                _ = GamePack.Current.CombatHexes.Place(CombatMap.Current.MapContainer, hexX, hexY, L: 0, terrainType: 1);
            }
        }

        return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
    }

    // FleeLoc.pas:132
    public void NewFrame(object sender)
    {
        bool highlight = false;
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            highlight = true;
        }
        else
        {
            if (CombatMap.Current.UnitSelection.TacticalBattleUnitHexSprite is not null && CombatMap.Current.UnitSelection.TacticalBattleUnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer && CombatMap.Current.UnitSelection.TacticalBattleUnitHexSprite.Unit.CombatPlayer != CombatMap.Current.Combat.CombatData.DefenderPlayer)
            {
                highlight = true;
            }
        }

        if (highlight != _highlighted)
        {
            _highlighted = highlight;
            _alphaStep = _highlighted ? 5 : -5;
        }
        else
        {
            if ((_alpha > 0 && _alphaStep < 0) || (_alpha < 50 && _alphaStep > 0))
            {
                _alpha += _alphaStep;
            }
        }
    }

    public new class Resource(CustomNode owner) : MapObjectResource(owner)
    {

        // FleeLoc.pas:163
        public new const int ClassId = 2_229_545;

        // FleeLoc.pas:169
        public override int ResourceUserClassId() => FleeLocation.ClassId;

        // FleeLoc.pas:166
        public override string ResourceEditName() => "Combat Flee Location";

    }
}
