// Translated from AoWTer.pas.
using System.Runtime.InteropServices;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;

namespace Aow.Game.World.Hexes;

public partial class AoWTerrain : TerrainMO
{
    public AoWTerrain(ECustomNode owner) : base(owner)
    { }

    // AoWTer.pas:89
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985)
        {
            try
            {
                if (((TerrainMOR)FResource).DecorationType == 3)
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
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4386)
        {
            if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                Msg.Result = ((Resource)FResource).FMapFieldResponse;
                return;
            }
        }
        else
        {
            if (Msg.MsgID == 4357)
            {
                if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
                {
                    ValidateDenseVegetationBurned(Msg.Param[0].As<Fire>());
                }
            }
        }
    }

    // AoWTer.pas:67
    public void ValidateDeadTreeRegrow()
    {
        TerrainMOR R;
        ECustomNode OldOwner;
        if (Owner.FindOwnedChild(132990) == null)
        {
            if (AoWHSMap.Current.Random(20) == 0)
            {
                R = FindCompatibleResource(((TerrainMOR)FResource).TerrainType, 1, 0);
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
    public void ValidateDenseVegetationBurned(Fire info)
    {
        if (info.FAge < 2 || FResource.TerrainList.Overlay[0] != 1) return;
        var resource = FindCompatibleResource(((TerrainMOR)FResource).TerrainType, 3, 3);
        if (resource == null) return;
        UpdateTerrain((MapLevel)Owner.Owner, GetXhx(), GetYhx());
        var owner = Owner;
        RemoveFromMap();
        SetResource(resource);
        Owner = owner;
        PlaceOnMap();
        if (FResource.Level <= 15) FResource.Engine.HSMap.StaticSceneInvalid = true;
        OverrideTerrain();
    }

    // FMapFieldResponse (stream id 20)
    public new partial class Resource : TerrainMOR
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public int FMapFieldResponse;

        // AoWTer.pas:116
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FMapFieldResponse, 20);
        }
    }
}

[StructLayout(LayoutKind.Explicit, Size = 31)]
public struct BurnInfo
{
    [FieldOffset(0)]
    public FixedArray_byte_30 Reserved;
    [FieldOffset(30)]
    public byte Severity;
}
