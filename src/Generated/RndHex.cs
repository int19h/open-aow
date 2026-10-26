// Generated from RndHex.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;

namespace Aow.Units.RndHex
{
    // { The RndHex unit of HSEPack.dpl: the random hexagon resource (TRndHexagonResource, ClassID
//   $20154): placing it changes the terrain under the cursor to its terrain type through the map,
//   which picks one of the hexagons of that terrain; its resource user is created the same way. }
    // { RndHex.TRndHexagonResource (instance size 56) }
    public partial class TRndHexagonResource : Aow.Units.Hexagon.THexagonResource
    {
        // RndHex.pas:38
        public override Aow.Units.Engine.TEObject CreateResourceUser()
        {
            Aow.Units.Engine.TEObject Result = default;
            var call__1 = FTerrainList.GetTerrain(0);
            var call__2 = FEngine.HSSet.HexagonControl.CreateMO(unchecked((sbyte)call__1));
            Result = call__2;
            return Result;
        }

        // RndHex.pas:43
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            int Lhx = default;
            int Radius = default;
            int Xhx = default;
            int Yhx = default;
            if (Msg.MsgID == 65541)
            {
                Lhx = Msg.Data[1];
                Radius = Msg.Param[1].Value;
                Aow.Units.HSEngine.HSEngine.HPtoHX(Msg.Value, Msg.Data[0], ref Xhx, ref Yhx);
                var call__3 = FTerrainList.GetTerrain(0);
                FEngine.HSMap.ChangeTerrain(null, Xhx, Yhx, Lhx, Radius, call__3);
                Msg.MsgID = 0;
                Msg.Result = 0;
            }

            if (Msg.MsgID != 65556)
            {
                base.MsgProc(ref Msg, Sender);
            }
        }

        public new partial class Meta : Aow.Units.Hexagon.THexagonResource.Meta
        {
            // RndHex.pas:28
            // { TRndHexagonResource }
            public override int ClassID()
            {
                int Result = default;
                Result = 131412;
                return Result;
            }

            // RndHex.pas:33
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Random Hexagon";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.RndHex.TRndHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
            // Hexagon.pas:32
            public new Aow.Units.RndHex.TRndHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.RndHex.TRndHexagonResource o = (Aow.Units.RndHex.TRndHexagonResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    public static partial class RndHex
    {
        // RndHex.pas:61
        public static void RegisterRandomHexagon(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.RndHex.TRndHexagonResource>()]);
        }
    }
}
