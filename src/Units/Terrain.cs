// Translated from Terrain.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

// FDecorationType (-1 none), FTerrainType. persisted as ids $12 and $11
public partial class TerrainMOR : MultiHexMOR
{
    public TerrainMOR(ECustomNode owner) : base(owner)
    {
        FDecorationType = -1;
    }

    // Terrain.pas:327
    public new const int ClassID = 131431;

    internal sbyte FDecorationType;
    internal byte FTerrainType;

    // Terrain.pas:310
    public override void ReadWrite(EStorageStream Stream)
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
    public override string ResourceEditName() => TerrainStr;

    private static readonly string TerrainStr = "Terrain";

}

public partial class TerrainMO : MultiHexMO
{
    public TerrainMO(ECustomNode owner) : base(owner)
    {
    }

    // Terrain.pas:65
    public new const int ClassID = 131430;

    // Terrain.pas:60

    // Terrain.pas:179
    public override bool CanChangeTerrain(in HSSenderInfo Info)
    {
        bool Result;
        if (this != Info.Sender)
        {
            if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
            {
                var terrain = FResource.TerrainList.GetTerrain(Info.HN);
                if (GetMostFrequentMapFieldTerrainType((MapLevel)Owner.Owner, FXhx, FYhx, terrain) == Info.MapField.TerrainType)
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
    public override void ChangeTerrain(in HSTerrainInfo Info)
    {
        sbyte TerrainType;
        sbyte NewTerrainType;
        TerrainMOR R;
        ECustomNode OldOwner;
        TerrainType = FResource.TerrainList.GetTerrain(Info.HN);
        if (TerrainType >= 0 && TerrainType != Info.TerrainType)
        {
            var terrain = FResource.TerrainList.GetTerrain(Info.HN);
            NewTerrainType = GetMostFrequentMapFieldTerrainType((MapLevel)Owner.Owner, FXhx, FYhx, terrain);
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
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
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
                Msg.Result = ((TerrainMOR)FResource).FDecorationType;
            }
        }
    }

    // Terrain.pas:291
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        if (Result && !GetEditMode())
        {
            OverrideTerrain();
        }

        return Result;
    }

    // Terrain.pas:226
    public virtual void ClearTerrain(MapField MapField, int HN)
    {
        int Xhx;
        int Yhx;
        int Lhx;
        int X = default;
        int Y = default;
        TerrainList TerrainList;
        HSMap Map;
        TerrainMOR R;
        EObjectList Resources;
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
                if (Resources.List[I] is TerrainMOR terrain)
                {
                    R = terrain;
                    Found = R.EClass.ClassID == FResource.EClass.ClassID && R.FDecorationType == ((TerrainMOR)FResource).FDecorationType && R.FTerrainType == ((TerrainMOR)FResource).FTerrainType && R.TerrainList.UsedTerrain() == 1 && R.TerrainList.GetTerrain(0) >= 0 && GetOverlay(0) == R.TerrainList.GetOverlay(0);
                }
            }

            if (Found)
            {
                Xhx = GetXhx();
                Yhx = GetYhx();
                Lhx = GetLhx();
                Map = ((MapLevel)Owner.Owner).Map;
                TerrainList = FResource.TerrainList;
                Dispose();
                I = 0;
                int last = TerrainList.Count - 1;
                for (; I <= last; I += 1)
                {
                    if (TerrainList.GetTerrain(I) >= 0 && I != HN)
                    {
                        HexGeometry.CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
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
    public virtual TerrainMOR FindDecorationResource(byte DecorationType)
    {
        TerrainMOR Result;
        DelphiList Candidates;
        EObjectList Resources;
        TerrainMOR R;
        Candidates = new DelphiList();
        Resources = GetResourceList(FResource.Engine).FChildren;
        for (int I = 0, last = Resources.Count - 1; I <= last; I++)
        {
            if (Resources.List[I] is TerrainMOR terrain)
            {
                R = terrain;
                if (R.EClass.ClassID == FResource.EClass.ClassID && R.FTerrainType == ((TerrainMOR)FResource).FTerrainType && R.FDecorationType == DecorationType && R != FResource && R.TerrainList.CompareTerrain(FResource.TerrainList, Set<TerrainCompareModeElement>.Of(TerrainCompareModeElement.SameOverlayType)))
                {
                    Candidates.Add(R);
                }
            }
        }

        if (Candidates.Count != 0)
        {
            var random = Random(Candidates.Count);
            Result = (TerrainMOR)Candidates.List[random];
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
        MapLevel Level;
        int Count;
        HNWalkData Walk = default;
        bool Result = true;
        Level = (MapLevel)Owner.Owner;
        Count = GetTerrainCount();
        HNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (Walk.HN < Count && Result)
        {
            if (Walk.HN != HN && GetTerrain(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Result = Level.GetField(Walk.X, Walk.Y).CanChangeTerrainEx(this);
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }

        return Result;
    }

    // Terrain.pas:19

}
