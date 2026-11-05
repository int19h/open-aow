// Translated from RndHex.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Hexagon;

namespace Aow.Units.RndHex
{
    // { The RndHex unit of HSEPack.dpl: the random hexagon resource (TRndHexagonResource, ClassID
//   $20154): placing it changes the terrain under the cursor to its terrain type through the map,
//   which picks one of the hexagons of that terrain; its resource user is created the same way. }
    public partial class TRndHexagonResource : THexagonResource
    {
        public TRndHexagonResource(TECustomNode owner) : base(owner)
        {}

        public new const int ClassID = 131412;

        // RndHex.pas:38
        public override TEObject CreateResourceUser()
        {
            TEObject Result = default;
            var call__1 = FTerrainList.GetTerrain(0);
            var call__2 = FEngine.HSSet.HexagonControl.CreateMO(call__1);
            Result = call__2;
            return Result;
        }

        // RndHex.pas:43
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            int Lhx = default;
            int Radius = default;
            int Xhx = default;
            int Yhx = default;
            if (Msg.MsgID == 65541)
            {
                Lhx = Msg.Data[1];
                Radius = Msg.Param[1].Value;
                HPtoHX(Msg.Value, Msg.Data[0], ref Xhx, ref Yhx);
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

        // RndHex.pas:33
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Random Hexagon";
            return Result;
        }

    }

    public static partial class RndHex
    {
        // RndHex.pas:61
        public static void RegisterRandomHexagon(TEngine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<TRndHexagonResource>()]);
        }
    }
}
