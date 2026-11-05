// Translated from AoWTer.pas.
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.Terrain;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWTer
{
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
                catch (Exception)
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
                var call__1 = FResource.TerrainList.Terrain[Msg.HN];
                if (call__1 >= 0)
                {
                    Msg.Result = ((TAoWTerrainResource)FResource).FMapFieldResponse;
                    return;
                }
            }
            else
            {
                if (Msg.MsgID == 4357)
                {
                    var call__2 = FResource.TerrainList.Terrain[Msg.HN];
                    if (call__2 >= 0)
                    {
                        ValidateDenseVegetationBurned(Msg.Param[0].Ref);
                    }
                }
            }
        }

        // AoWTer.pas:67
        public void ValidateDeadTreeRegrow()
        {
            TTerrainMOR R = default;
            TECustomNode OldOwner = default;
            var call__3 = Owner.FindOwnedChild(132990);
            if (call__3 == null)
            {
                var call__4 = AoWHSMap.Random(20);
                if (call__4 == 0)
                {
                    var call__5 = FindCompatibleResource(((TTerrainMOR)FResource).TerrainType, 1, 0);
                    R = call__5;
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

    public static partial class AoWTer
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
}
