// Translated from FleeLoc.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World;

public partial class FleeLocation : MapObject
{
    public FleeLocation(ECustomNode owner) : base(owner)
    {
    }

    // FleeLoc.pas:76
    public new const int ClassId = 2229544;

    private bool _highlighted;
    private int _alpha;
    private int _alphaStep;

    // FleeLoc.pas:71

    // FleeLoc.pas:82
    public override int GetShowPriority() => 1744830464 + (HexY << 13) + HexX;

    // FleeLoc.pas:151
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        if (_alpha > 0)
        {
            HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref X, ref Y);
            _resource.ImageLibrary.Get(0).ShowBlended(X - pixelXSurface - 8, Y - pixelYSurface, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[_alpha]);
        }
    }

    // FleeLoc.pas:120
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (info.HexNumber == 0)
        {
            if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetSide() == CombatSideIndex.Attacker)
            {
                moveResult.Code = 25;
            }
            else
            {
                moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][info.MapField.OverlayType - -1];
            }

            moveResult.Done = true;
        }
    }

    // FleeLoc.pas:111
    public override void Activate()
    {
        base.Activate();
        CombatMap.Current.NewFrameEvents += NewFrame;
        _highlighted = CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting);
        if (_highlighted)
        {
            _alpha = 50;
        }
        else
        {
            _alpha = 0;
        }

        CombatMap.Current.FleeLocations.Add(this);
    }

    // FleeLoc.pas:65
    public override void Deactivate()
    {
        CombatMap.Current.NewFrameEvents -= NewFrame;
        base.Deactivate();
    }

    // FleeLoc.pas:90
    public override bool CanSelect(int pixelX, int pixelY) => true;

    // FleeLoc.pas:79
    public override MapObjectResourceList GetResourceList(GameEngine engine) => AoWHSSet.Current.CombatResources;

    // FleeLoc.pas:87
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel) => true;

    // FleeLoc.pas:93
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        EngineObject oldHex;
        MapField mapField;
        if (!GetEditMode())
        {
            mapField = mapContainer.GetField(hexX, hexY, hexLevel);
            oldHex = mapField.FindOwnedChild(132439);
            if (oldHex != null)
            {
                oldHex?.Dispose();
                AoWHSSet.Current.CombatHexes.Place(CombatMap.Current.MapContainer, hexX, hexY, 0, 1);
            }
        }

        return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
    }

    // FleeLoc.pas:132
    public void NewFrame(object sender)
    {
        bool highlight;
        highlight = false;
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            highlight = true;
        }
        else
        {
            if (CombatMap.Current.UnitSelection.GetTacticalBattleUnitHexSprite() != null && CombatMap.Current.UnitSelection.GetTacticalBattleUnitHexSprite().Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer && CombatMap.Current.UnitSelection.GetTacticalBattleUnitHexSprite().Unit.CombatPlayer != CombatMap.Current.Combat.CombatData.DefenderPlayer)
            {
                highlight = true;
            }
        }

        if (highlight != _highlighted)
        {
            _highlighted = highlight;
            if (_highlighted)
            {
                _alphaStep = 5;
            }
            else
            {
                _alphaStep = -5;
            }
        }
        else
        {
            if ((_alpha > 0 && _alphaStep < 0) || (_alpha < 50 && _alphaStep > 0))
            {
                _alpha = _alpha + _alphaStep;
            }
        }
    }

    public new partial class Resource : MapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // FleeLoc.pas:163
        public new const int ClassId = 2229545;

        // FleeLoc.pas:169
        public override int ResourceUserClassId() => 2229544;

        // FleeLoc.pas:166
        public override string ResourceEditName() => "Combat Flee Location";

    }

    // FleeLoc.pas:27

}
