// Translated from CityWall.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Terrain;

public partial class CityWallDoor : CityWall
{
    public CityWallDoor(ECustomNode owner) : base(owner)
    { }

    // CityWall.pas:381
    public new const int ClassID = 2229510;

    // CityWall.pas:428
    public override sbyte GetOverlay(int Index) => -1;

    // CityWall.pas:431
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        TacticalCombatUnitHS CombatUnit;
        if (AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting) || AoWCombatMap.Current.FCombat == null)
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
        else
        {
            var yhx = GetYhx();
            var xhx = GetXhx();
            CombatUnit = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetMapLevel(0).GetField(xhx, yhx).FindOwnedChild(2228484);
            if (CombatUnit != null)
            {
                if (CombatUnit.FUnit.GetSide() != CombatSideIndex.Defender)
                {
                    CombatUnit = null;
                }
            }

            bool condition = FHitPoints == 0;
            if (!condition)
            {
                var indexOfPlayer = AoWCombatMap.Current.FCombat.FCombatData.FPlayers.IndexOfPlayer((byte)AoWCombatMap.Current.FCurrentPlayer);
                condition = AoWCombatMap.Current.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)indexOfPlayer).FSideIndex != 1 && CombatUnit == null;
            }

            if (condition)
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }
    }

    // CityWall.pas:384
    public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
    {
        if (Info.HN == 0)
        {
            if (FHitPoints == 0)
            {
                MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
            }
            else
            {
                if (((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Wall))
                {
                    MoveResult.Code = 25;
                    MoveResult.Done = true;
                }
                else
                {
                    if (((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.WallCrushing) && GetXhx() == ((MoveControl)Info.Sender).FDestinationX && GetYhx() == ((MoveControl)Info.Sender).FDestinationY)
                    {
                        MoveResult.Code = 0;
                    }
                    else
                    {
                        if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == CombatSideIndex.Defender)
                        {
                            MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                        }
                        else
                        {
                            if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                            {
                                MoveResult.Code = -1;
                            }
                            else
                            {
                                MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                            }
                        }
                    }
                }
            }

            MoveResult.Done = true;
        }
    }

    // CityWall.pas:409
    public override int CanMoveOver(in HSSenderInfo Info)
    {
        int Result;
        if (Info.HN == 0)
        {
            if (FHitPoints == 0 || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == CombatSideIndex.Defender || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36) || ((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Climbing) || ((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Wall))
            {
                Result = 1;
            }
            else
            {
                if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                {
                    Result = 0;
                }
                else
                {
                    if (((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
                    {
                        Result = 1;
                    }
                    else
                    {
                        Result = 0;
                    }
                }
            }
        }
        else
        {
            Result = 1;
        }

        return Result;
    }

    public new partial class Resource : CityWall.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CityWall.pas:451
        public new const int ClassID = 2229511;

        // CityWall.pas:457
        public override int ResourceUserClassID() => 2229510;

        // CityWall.pas:454
        public override string ResourceEditName() => "Combat City Door";

    }

}
