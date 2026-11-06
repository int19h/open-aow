// Translated from AoWTer.pas.
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.Terrain;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.AoWE;

namespace Aow.Units.AoWTer;

public partial class TAoWTerrain : TTerrainMO
{
    public TAoWTerrain(TECustomNode owner) : base(owner)
    {}

    // AoWTer.pas:89
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985)
        {
            try
            {
                if (((TTerrainMOR)FResource).DecorationType == 3)
                {
                    ValidateDeadTreeRegrow();
                }
            }
            catch
            {
            }
        }
    }

    // AoWTer.pas:100
    // { Original ignores regrowth failures. }
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4386)
        {
            if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                Msg.Result = ((TAoWTerrainResource)FResource).FMapFieldResponse;
                return;
            }
        }
        else
        {
            if (Msg.MsgID == 4357)
            {
                if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
                {
                    ValidateDenseVegetationBurned(Msg.Param[0].Ref);
                }
            }
        }
    }

    // AoWTer.pas:67
    public void ValidateDeadTreeRegrow()
    {
        TTerrainMOR R;
        TECustomNode OldOwner;
        if (Owner.FindOwnedChild(132990) == null)
        {
            if (AoWHSMap.Random(20) == 0)
            {
                R = FindCompatibleResource(((TTerrainMOR)FResource).TerrainType, 1, 0);
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
            }
        }
    }

    // AoWTer.pas:45. Message $1105 carries a TFire whose byte $1E is FAge.
    public void ValidateDenseVegetationBurned(object info)
    {
        if (((TFire)info).FAge < 2 || FResource.TerrainList.Overlay[0] != 1) return;
        var resource = FindCompatibleResource(((TTerrainMOR)FResource).TerrainType, 3, 3);
        if (resource == null) return;
        UpdateTerrain((TMapLevel)Owner.Owner, GetXhx(), GetYhx());
        var owner = Owner;
        RemoveFromMap();
        SetResource(resource);
        Owner = owner;
        PlaceOnMap();
        if (FResource.Level <= 15) FResource.Engine.HSMap.StaticSceneInvalid = true;
        OverrideTerrain();
    }
}

// FMapFieldResponse (stream id 20)
public partial class TAoWTerrainResource : TTerrainMOR
{
    public TAoWTerrainResource(TECustomNode owner) : base(owner)
    {}

    public int FMapFieldResponse;
    // AoWTer.pas:116
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FMapFieldResponse, 20);
    }
}

[StructLayout(LayoutKind.Explicit, Size = 31)]
public struct TBurnInfo
{
    [FieldOffset(0)]
    public FixedArray_byte_30 Reserved;
    [FieldOffset(30)]
    public byte Severity;
}

public static partial class AoWTerUnit
{
    // AoWTer.pas:122
    public static void RegisterAoWTerrain(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TAoWTerrain>(), EClass.Of<TAoWTerrainResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
