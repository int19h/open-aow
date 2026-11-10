// Translated from ReflectingPool.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

// FCenterFrame (-1 when inactive)
public partial class TReflectingPool : TPlayerStructure
{
    public TReflectingPool(TECustomNode owner) : base(owner)
    {
        FCenterFrame = -1;
    }

    // ReflectingPool.pas:58
    public new const int ClassID = 132670;

    public int FCenterFrame;

    // ReflectingPool.pas:80
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
    }

    // ReflectingPool.pas:52

    // ReflectingPool.pas:241
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        if (Msg.MsgID == 4362 && FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
        {
            if (VisibilityRange() >= Msg.Param[0].Int && PoolVisionPlayer((sbyte)Msg.Param[1].Int))
            {
                Msg.Result = 1;
            }
        }
        else
        {
            if (Msg.MsgID == 4365 && FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
            {
                if (VisibilityRange() >= Msg.Param[0].Int)
                {
                    NotifyPlayerChanged(Msg.MapField, Msg.Param[0].Int);
                }
            }
            else
            {
                base.MapFieldMsgProc(ref Msg);
            }
        }
    }

    // ReflectingPool.pas:83
    public override void Activate()
    {
        base.Activate();
        if (FPlayer != -1)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FKnownStructures.AddChild(this);
        }
    }

    // ReflectingPool.pas:89
    public override void Deactivate()
    {
        base.Deactivate();
        DeactivatePoolCentering();
        if (FPlayer != -1)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FKnownStructures.RemoveChild(this);
        }
    }

    // ReflectingPool.pas:143
    public override void SetPlayer(sbyte Player)
    {
        if (Player != FPlayer)
        {
            if (FPlayer != -1)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FKnownStructures.RemoveChild(this);
            }

            base.SetPlayer(Player);
            if (FPlayer != -1)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FKnownStructures.AddChild(this);
            }

            UpdateReflectingPoolsExploration();
            if (FPlayer != -1 && FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FKnownStructures.Count == 1)
            {
                ActivatePoolCentering();
            }
        }
    }

    // ReflectingPool.pas:69
    public override int VisibilityRange()
    {
        int Result = 8;
        if (GetLhx() != 0 || FPlayer > 0 && (((byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 8) != 0)
        {
            Result = (int)(((uint)(Result + 1)) >> 1);
        }

        return Result;
    }

    // ReflectingPool.pas:185
    public override void UpdateUnfog()
    {
        int Range;
        int X = default;
        int Y = default;
        if (FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moPlacedOnMap))
        {
            if (FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moActivated) && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer != -1)
            {
                if (PoolVisionPlayer(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer))
                {
                    Range = VisibilityRange();
                }
                else
                {
                    Range = 0;
                }
            }
            else
            {
                Range = 0;
            }

            if (Range != FUnfogRange)
            {
                if (FUnfogRange != 0)
                {
                    for (int I = 0, last = FResource.TerrainList.Count - 1; I <= last; I++)
                    {
                        if (FResource.TerrainList.GetTerrain(I) >= 0)
                        {
                            Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FogArea(X, Y, ((TMapField)Owner).Lhx, FUnfogRange);
                        }
                    }
                }

                FUnfogRange = (byte)Range;
                if (FUnfogRange != 0)
                {
                    for (int I = 0, last = FResource.TerrainList.Count - 1; I <= last; I++)
                    {
                        if (FResource.TerrainList.GetTerrain(I) >= 0)
                        {
                            Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UnFogArea(X, Y, ((TMapField)Owner).Lhx, FUnfogRange);
                        }
                    }
                }
            }
        }
    }

    // ReflectingPool.pas:157
    public override void UpdateExploration()
    {
        int X = default;
        int Y = default;
        if (FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moPlacedOnMap))
        {
            for (int P = 1, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count - 1; P <= last; P++)
            {
                if (PoolVisionPlayer((sbyte)P))
                {
                    for (int I = 0, lastI = FResource.TerrainList.Count - 1; I <= lastI; I++)
                    {
                        if (FResource.TerrainList.GetTerrain(I) >= 0)
                        {
                            Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                            var visibilityRange = VisibilityRange();
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ExploreArea(X, Y, ((TMapField)Owner).Lhx, (sbyte)P, visibilityRange);
                        }
                    }
                }
            }
        }
    }

    // ReflectingPool.pas:77
    public override int GetDefaultAITargetPriority() => 1;

    // ReflectingPool.pas:61
    public void ListReflectingPools(ref TPlayerStructureList List)
    {
        for (int P = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count - 1; P >= 0; P--)
        {
            for (int I = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(P).FKnownStructures.Count - 1; I >= 0; I--)
            {
                var structure = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(P).FKnownStructures.GetStructure(I);
                List.AddChild(structure);
            }
        }
    }

    // ReflectingPool.pas:99
    public void NewFrame(object Sender)
    {
        TPlayerStructureList List = default;
        TPlayerStructure Pool = default;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == FPlayer)
        {
            FCenterFrame += 1;
            if (FCenterFrame % 8 == 0)
            {
                List = new TPlayerStructureList(null);
                ListReflectingPools(ref List);
                List.RemoveChild(this);
                List.SortStable(Aow.Game.World.Structures.TReflectingPool.ComparePools);
                var count = List.Count;
                if (FCenterFrame / 8 < count)
                {
                    Pool = List.GetStructure(FCenterFrame / 8);
                    var lhx = Pool.GetLhx();
                    var yhx = Pool.GetYhx();
                    var xhx = Pool.GetXhx();
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(xhx, yhx, lhx);
                }
                else
                {
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(xhx, yhx, lhx);
                    DeactivatePoolCentering();
                }

                List?.Dispose();
            }
        }
        else
        {
            DeactivatePoolCentering();
        }
    }

    // ReflectingPool.pas:127
    public void ActivatePoolCentering()
    {
        DeactivatePoolCentering();
        FCenterFrame = 0;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.NewFrameEvents += NewFrame;
    }

    // ReflectingPool.pas:134
    public void DeactivatePoolCentering()
    {
        if (FCenterFrame != -1)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.NewFrameEvents -= NewFrame;
            FCenterFrame = -1;
        }
    }

    // ReflectingPool.pas:171
    public void UpdateReflectingPoolsExploration()
    {
        TPlayer Player;
        for (int P = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count - 1; P >= 0; P--)
        {
            Player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(P);
            for (int I = Player.FKnownStructures.Count - 1; I >= 0; I--)
            {
                Player.FKnownStructures.GetStructure(I).UpdateExploration();
                Player.FKnownStructures.GetStructure(I).UpdateUnfog();
            }
        }
    }

    // ReflectingPool.pas:217
    public bool PoolVisionPlayer(sbyte Player)
    {
        int P;
        bool Result = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FKnownStructures.Count != 0;
        if (!Result)
        {
            P = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count;
            while (P > 1 && !Result)
            {
                P -= 1;
                Result = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(P).FKnownStructures.Count != 0 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(P).FDiplomaticRelations.GetDiplomaticRelation(Player) == TDiplomaticRelation.drAlliance;
            }
        }

        return Result;
    }

    // ReflectingPool.pas:233
    public void NotifyPlayerChanged(TMapField MapField, int Distance)
    {
        for (int P = 1, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count - 1; P <= last; P++)
        {
            if (PoolVisionPlayer((sbyte)P))
            {
                var trueVisionRange = TrueVisionRange();
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UpdatePlayerSeen(P, MapField.Xhx, MapField.Yhx, MapField.Lhx, Distance, trueVisionRange);
            }
        }
    }

    internal static int ComparePools(object Item1, object Item2)
    {
        return 0;
    }

    // ReflectingPool.pas:20

    // ReflectingPool.pas:20

}

public partial class TReflectingPoolResource : TPlayerStructureResource
{
    public TReflectingPoolResource(TECustomNode owner) : base(owner)
    {
        FTerrainBound = true;
        FConnectRoads = false;
        FClaimed = false;
        FRazeable = false;
    }

    // ReflectingPool.pas:266
    public new const int ClassID = 132671;

    // ReflectingPool.pas:254

    // ReflectingPool.pas:269
    public override int ResourceUserClassID() => 132670;

    // ReflectingPool.pas:42

    // ReflectingPool.pas:263
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TReflectingPoolResource.ReflectingPoolRStr);

    private static readonly string ReflectingPoolRStr = "Reflecting Pool";

}
