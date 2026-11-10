// Translated from Terrain.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

// FDecorationType (-1 none), FTerrainType. persisted as ids $12 and $11
public partial class TTerrainMOR : Aow.Game.World.TMultiHexMOR
{
    public TTerrainMOR(TECustomNode owner) : base(owner)
    {
        FDecorationType = -1;
    }

    // Terrain.pas:327
    public new const int ClassID = 131431;

    internal sbyte FDecorationType;
    internal byte FTerrainType;

    // Terrain.pas:310
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FTerrainType, 17);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDecorationType), 18);
    }

    // Terrain.pas:299

    // Terrain.pas:322
    public override int ResourceUserClassID() => 131430;

    public sbyte DecorationType { get => FDecorationType; set => FDecorationType = value; }
    public byte TerrainType { get => FTerrainType; set => FTerrainType = value; }

    // Terrain.pas:42

    // Terrain.pas:317
    public override string ResourceEditName() => Aow.Game.World.Hexes.TTerrainMOR.TerrainStr;

    private static readonly string TerrainStr = "Terrain";

}

public partial class TTerrainMO : Aow.Game.World.TMultiHexMO
{
    public TTerrainMO(TECustomNode owner) : base(owner)
    {
    }

    // Terrain.pas:65
    public new const int ClassID = 131430;

    // Terrain.pas:60

    // Terrain.pas:179
    public override bool CanChangeTerrain(in THSSenderInfo Info)
    {
        bool Result;
        if (this != Info.Sender)
        {
            if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
            {
                var terrain = FResource.TerrainList.GetTerrain(Info.HN);
                if (GetMostFrequentMapFieldTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx, terrain) == Info.MapField.TerrainType)
                {
                    Result = CanChangeOwnTerrain(Info.HN);
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Terrain.pas:195
    public override void ChangeTerrain(in THSTerrainInfo Info)
    {
        sbyte TerrainType;
        sbyte NewTerrainType;
        TTerrainMOR R;
        TECustomNode OldOwner;
        TerrainType = FResource.TerrainList.GetTerrain(Info.HN);
        if (TerrainType >= 0 && TerrainType != Info.TerrainType)
        {
            var terrain = FResource.TerrainList.GetTerrain(Info.HN);
            NewTerrainType = GetMostFrequentMapFieldTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx, terrain);
            if (NewTerrainType != TerrainType)
            {
                R = FindTerrainResource((byte)NewTerrainType);
                if (R != null)
                {
                    OldOwner = Owner;
                    RemoveFromMap();
                    SetResource(R);
                    Owner = OldOwner;
                    PlaceOnMap();
                    if (FResource.Level <= 15)
                    {
                        FResource.Engine.HSMap.StaticSceneInvalid = true;
                    }

                    OverrideTerrain();
                }
                else
                {
                    Dispose();
                }
            }
        }
    }

    // Terrain.pas:275
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        if (Msg.MsgID == 16)
        {
            if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
            {
                ClearTerrain(Msg.MapField, Msg.HN);
            }
            else
            {
                if (Msg.HN == 0)
                {
                    Dispose();
                }
            }
        }
        else
        {
            base.MapFieldMsgProc(ref Msg);
        }

        if (Msg.MsgID == 17)
        {
            if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
            {
                Msg.Result = ((TTerrainMOR)FResource).FDecorationType;
            }
        }
    }

    // Terrain.pas:291
    public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        if (Result && !GetEditMode())
        {
            OverrideTerrain();
        }

        return Result;
    }

    // Terrain.pas:226
    public virtual void ClearTerrain(TMapField MapField, int HN)
    {
        int Xhx;
        int Yhx;
        int Lhx;
        int X = default;
        int Y = default;
        TTerrainList TerrainList;
        THSMap Map;
        TTerrainMOR R;
        TEObjectList Resources;
        int I;
        bool Found;
        if (FResource.TerrainList.UsedTerrain() > 1)
        {
            R = null;
            Resources = GetResourceList(FResource.Engine).FChildren;
            I = Resources.Count;
            Found = false;
            while (!Found && I != 0)
            {
                I -= 1;
                if (Resources.List[I] is TTerrainMOR terrain)
                {
                    R = terrain;
                    Found = R.EClass.ClassID == FResource.EClass.ClassID && R.FDecorationType == ((TTerrainMOR)FResource).FDecorationType && R.FTerrainType == ((TTerrainMOR)FResource).FTerrainType && R.TerrainList.UsedTerrain() == 1 && R.TerrainList.GetTerrain(0) >= 0 && GetOverlay(0) == R.TerrainList.GetOverlay(0);
                }
            }

            if (Found)
            {
                Xhx = GetXhx();
                Yhx = GetYhx();
                Lhx = GetLhx();
                Map = ((TMapLevel)Owner.Owner).Map;
                TerrainList = FResource.TerrainList;
                Dispose();
                I = 0;
                int last = TerrainList.Count - 1;
                for (; I <= last; I += 1)
                {
                    if (TerrainList.GetTerrain(I) >= 0 && I != HN)
                    {
                        Aow.Game.World.Map.HexGeometry.CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
                        R.PlaceResourceUser(Map, (sbyte)X, (sbyte)Y, (sbyte)Lhx);
                    }
                }
            }
            else
            {
                Dispose();
            }
        }
        else
        {
            Dispose();
        }
    }

    // Terrain.pas:70
    public virtual TTerrainMOR FindDecorationResource(byte DecorationType)
    {
        TTerrainMOR Result;
        TList Candidates;
        TEObjectList Resources;
        TTerrainMOR R;
        Candidates = new TList();
        Resources = GetResourceList(FResource.Engine).FChildren;
        for (int I = 0, last = Resources.Count - 1; I <= last; I++)
        {
            if (Resources.List[I] is TTerrainMOR terrain)
            {
                R = terrain;
                if (R.EClass.ClassID == FResource.EClass.ClassID && R.FTerrainType == ((TTerrainMOR)FResource).FTerrainType && R.FDecorationType == DecorationType && R != FResource && R.TerrainList.CompareTerrain(FResource.TerrainList, TSet<TTerrainCompareModeElement>.Of(TTerrainCompareModeElement.cmSameOverlayType)))
                {
                    Candidates.Add(R);
                }
            }
        }

        if (Candidates.Count != 0)
        {
            var random = Random(Candidates.Count);
            Result = (TTerrainMOR)Candidates.List[random];
        }
        else
        {
            Result = null;
        }

        Candidates?.Dispose();
        return Result;
    }

    // Terrain.pas:160
    public bool CanChangeOwnTerrain(int HN)
    {
        TMapLevel Level;
        int Count;
        THNWalkData Walk = default;
        bool Result = true;
        Level = (TMapLevel)Owner.Owner;
        Count = GetTerrainCount();
        Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (Walk.HN < Count && Result)
        {
            if (Walk.HN != HN && GetTerrain(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Result = Level.GetField(Walk.X, Walk.Y).CanChangeTerrainEx(this);
            }

            Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
        }

        return Result;
    }

    // Terrain.pas:19

}
