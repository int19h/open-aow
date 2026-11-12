// Translated from CityWall.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Terrain;

public partial class CityWallDoor : CityWall
{
    public CityWallDoor(ECustomNode owner) : base(owner)
    { }

    // CityWall.pas:381
    public new const int ClassId = 2229510;

    // CityWall.pas:428
    public override sbyte GetOverlay(int index) => -1;

    // CityWall.pas:431
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        TacticalBattleUnitHexSprite combatUnit;
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || CombatMap.Current.Combat == null)
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
        else
        {
            var hexY = GetHexY();
            var hexX = GetHexX();
            combatUnit = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).FindOwnedChild(2228484);
            if (combatUnit != null)
            {
                if (combatUnit.Unit.GetSide() != CombatSideIndex.Defender)
                {
                    combatUnit = null;
                }
            }

            bool condition = HitPoints == 0;
            if (!condition)
            {
                var indexOfPlayer = CombatMap.Current.Combat.CombatData.Players.IndexOfPlayer((byte)CombatMap.Current.CurrentPlayer);
                condition = CombatMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)indexOfPlayer).SideIndex != 1 && combatUnit == null;
            }

            if (condition)
            {
                base.Show(pixelXSurface, pixelYSurface, lockedSurface);
            }
        }
    }

    // CityWall.pas:384
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (info.HexNumber == 0)
        {
            if (HitPoints == 0)
            {
                moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][6 - MovementTables.FirstOverlay];
            }
            else
            {
                if (((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Wall))
                {
                    moveResult.Code = 25;
                    moveResult.Done = true;
                }
                else
                {
                    if (((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.WallCrushing) && GetHexX() == ((MoveControl)info.Sender).DestinationX && GetHexY() == ((MoveControl)info.Sender).DestinationY)
                    {
                        moveResult.Code = 0;
                    }
                    else
                    {
                        if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(1) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(66) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetSide() == CombatSideIndex.Defender)
                        {
                            moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][-1 - MovementTables.FirstOverlay];
                        }
                        else
                        {
                            if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59))
                            {
                                moveResult.Code = -1;
                            }
                            else
                            {
                                moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay];
                            }
                        }
                    }
                }
            }

            moveResult.Done = true;
        }
    }

    // CityWall.pas:409
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        int result;
        if (info.HexNumber == 0)
        {
            if (HitPoints == 0 || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetSide() == CombatSideIndex.Defender || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(66) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(36) || ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Climbing) || ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Wall))
            {
                result = 1;
            }
            else
            {
                if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59))
                {
                    result = 0;
                }
                else
                {
                    if (((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay] != 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
        }
        else
        {
            result = 1;
        }

        return result;
    }

    public new partial class Resource : CityWall.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CityWall.pas:451
        public new const int ClassId = 2229511;

        // CityWall.pas:457
        public override int ResourceUserClassId() => 2229510;

        // CityWall.pas:454
        public override string ResourceEditName() => "Combat City Door";

    }

}
