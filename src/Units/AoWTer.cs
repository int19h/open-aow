// Translated from AoWTer.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWTer
{
    public partial class TAoWTerrain : Aow.Units.Terrain.TTerrainMO
    {
        public TAoWTerrain(TECustomNode owner) : base(owner)
        {}

        // AoWTer.pas:89
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001985)
            {
                try
                {
                    if (unchecked((short)((Aow.Units.Terrain.TTerrainMOR)FResource).DecorationType) == 3)
                    {
                        ValidateDeadTreeRegrow();
                    }
                }
                catch (System.Exception)
                {
                }
            }
        }

        // AoWTer.pas:100
        // { Original ignores regrowth failures. }
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4386)
            {
                var call__1 = FResource.TerrainList.Terrain[Msg.HN];
                if (call__1 >= 0)
                {
                    Msg.Result = ((Aow.Units.AoWTer.TAoWTerrainResource)FResource).FMapFieldResponse;
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
            Aow.Units.Terrain.TTerrainMOR R = default;
            Aow.Units.Engine.TECustomNode OldOwner = default;
            var call__3 = Owner.FindOwnedChild(132990);
            if (call__3 == null)
            {
                var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(20);
                if (call__4 == 0)
                {
                    var call__5 = FindCompatibleResource(unchecked((byte)((Aow.Units.Terrain.TTerrainMOR)FResource).TerrainType), 1, 0);
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
    public partial class TAoWTerrainResource : Aow.Units.Terrain.TTerrainMOR
    {
        public TAoWTerrainResource(TECustomNode owner) : base(owner)
        {}

        public int FMapFieldResponse;
        // AoWTer.pas:116
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FMapFieldResponse, 20);
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 31)]
    public struct TBurnInfo
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_30 Reserved;
        [FieldOffset(30)]
        public byte Severity;
    }

    public static partial class AoWTer
    {
        // AoWTer.pas:122
        public static void RegisterAoWTerrain(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AoWTer.TAoWTerrain>(), Aow.Units.Engine.EClass.Of<Aow.Units.AoWTer.TAoWTerrainResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
