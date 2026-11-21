#nullable disable
// Translated from CityWall.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Terrain;

public class CityWallDoor(CustomNode owner) : CityWall(owner)
{

    // CityWall.pas:381
    public new const int ClassId = 2_229_510;

    // CityWall.pas:428
    public override sbyte GetOverlay(int index) => -1;

    // CityWall.pas:431
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) || CombatMap.Current.Combat is null)
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
        else
        {
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            TacticalBattleUnitHexSprite combatUnit = CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
            if (combatUnit is not null)
            {
                if (combatUnit.Unit.Side != CombatSideIndex.Defender)
                {
                    combatUnit = null;
                }
            }

            bool condition = HitPoints == 0;
            if (!condition)
            {
                int indexOfPlayer = CombatMap.Current.Combat.CombatData.Players.IndexOfPlayer((byte)CombatMap.Current.CurrentPlayer);
                condition = CombatMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)indexOfPlayer).SideIndex != 1 && combatUnit is null;
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
                if (((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Wall))
                {
                    moveResult.Code = 25;
                    moveResult.Done = true;
                }
                else
                {
                    moveResult.Code = ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.WallCrushing) && HexX == ((MoveControl)info.Sender).Destination.X && HexY == ((MoveControl)info.Sender).Destination.Y
                        ? 0
                        : ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(1) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(66) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Side == CombatSideIndex.Defender
                            ? ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][-1 - MovementTables.FirstOverlay]
                            : ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59)
                                ? -1
                                : ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay];
                }
            }

            moveResult.Done = true;
        }
    }

    // CityWall.pas:409
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        return info.HexNumber == 0
            ? HitPoints == 0 || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Side == CombatSideIndex.Defender || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(66) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(36) || ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Climbing) || ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Wall)
                ? 1
                : ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59)
                    ? 0
                    : ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay] != 0 ? 1 : 0
            : 1;
    }

    public new class Resource(CustomNode owner) : CityWall.Resource(owner)
    {

        // CityWall.pas:451
        public new const int ClassId = 2_229_511;

        // CityWall.pas:457
        public override int ResourceUserClassId() => 2_229_510;

        // CityWall.pas:454
        public override string ResourceEditName() => "Combat City Door";

    }
}
