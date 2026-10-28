// Translated from LeadrGrd.pas.
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

namespace Aow.Units.LeadrGrd
{
    // { TLeaderGrid (instance size 488, VMT 55803A10); $1DC show level, $1DE show status, $1E0 stream, $1E4 map. }
    public partial class TLeaderGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
        public bool FShowLevel;
        // { $1DC }
        public bool FReserved1DD;
        public bool FShowStatus;
        // { $1DE }
        public byte FReserved1DF;
        public Aow.Rtl.TMemoryStream FStream;
        // { +$1E0 }
        public Aow.Units.AoWE.TAoWHSMap FMap;
        // LeadrGrd.pas:111
        // { +$1E4 }
        public override void Destroy()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Destroy */
            throw new System.NotImplementedException();
        }

        // LeadrGrd.pas:101
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            DefaultColWidth = 155;
            DefaultRowHeight = 56;
            GrowDirection = Aow.Units.ImageLib.TILGrowDirection.ilVertical;
            ScrollBars = Aow.Rtl.TScrollStyle.ssVertical;
            var call__1 = new Aow.Rtl.TMemoryStream();
            FStream = call__1;
        }

        // LeadrGrd.pas:166
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Canvas */
            throw new System.NotImplementedException();
        }

        // LeadrGrd.pas:118
        public void LeadersUpdated(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // LeadrGrd.pas:123
        public void SetAoWHSMap(Aow.Units.AoWE.TAoWHSMap Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // LeadrGrd.pas:134
        public Aow.Units.AoWE.TLeader GetLeader(sbyte Player)
        {
            Aow.Units.AoWE.TLeader Result = default;
            Aow.Units.AoWE.TAoWHSMap Map = default;
            Map = FMap;
            bool bool__2 = (Map != null) && (Player > 0);
            if (bool__2)
            {
                var call__3 = Map.FPlayers.Count;
                bool__2 = (call__3 > Player);
            }

            if (bool__2)
            {
                var call__4 = Aow.Units.LeadrGrd.LeadrGrd.PlayerAt(FMap.FPlayers, Player);
                Result = call__4.FLeader;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // LeadrGrd.pas:144
        public Aow.Units.AoWE.TLeader GetSelectedLeader()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Col */
            throw new System.NotImplementedException();
        }

        // LeadrGrd.pas:155
        public void SetSelectedLeader(Aow.Units.AoWE.TLeader Value)
        {
            int I = default;
            if (FMap != null)
            {
                var call__5 = Aow.Units.LeadrGrd.LeadrGrd.RaceToPlayer(FMap.FPlayers, unchecked((byte)Value.FRace));
                I = call__5;
                if (I != -1)
                {
                    SelectIndex(I - 1);
                }
            }
        }

        // LeadrGrd.pas:263
        public void RemoveFromMap()
        {
            Aow.Units.AoWE.TLeader Leader = default;
            Leader = GetSelectedLeader();
            if (Leader != null)
            {
                Leader.SetOwner(null);
            }
        }

        // { AoWEd 0042D0F8 }
        [Published]
        public bool EditLeaderEnabled { get => FReserved1DD; set => FReserved1DD = value; }

        [Published]
        public bool ShowStatus { get => FShowStatus; set => FShowStatus = value; }

        [Published]
        public bool ShowLevel { get => FShowLevel; set => FShowLevel = value; }

        // LeadrGrd.pas:26
        public static new Aow.Units.LeadrGrd.TLeaderGrid Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.LeadrGrd.TLeaderGrid o = new Aow.Units.LeadrGrd.TLeaderGrid();
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

    public partial class TGridMapView : Aow.Rtl.TObject
    {
        public Aow.Units.FixedArray_byte_240 Reserved04;
        public Aow.Units.AoWE.TItemControl ItemControl;
        public Aow.Units.AoWE.THeroControl HeroControl;
        public Aow.Units.FixedArray_byte_68 ReservedFC;
        public Aow.Units.AoWE.TPlayerList Players;
        public Aow.Units.FixedArray_byte_56 Reserved144;
        public Aow.Units.Engine.TEventList LeaderEvent;
    }

    public struct TRawView
    {
        public Aow.Units.FixedArray_object_512 Fields;
    }

    public struct TNodeView
    {
        public Aow.Units.FixedArray_byte_8 Reserved;
        public Aow.Rtl.TList Children;
    }

    [StructLayout(LayoutKind.Explicit, Size = 49)]
    public struct TEngineView
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_48 Reserved;
        [FieldOffset(48)]
        public byte Status;
    }

    public struct THeroView
    {
        public Aow.Units.FixedArray_byte_96 Reserved00;
        public Aow.Rtl.AnsiString Name;
        public Aow.Units.FixedArray_byte_4 Reserved64;
        public byte Race;
        public Aow.Units.FixedArray_byte_15 Reserved69;
        public bool Hidden;
    }

    public struct TItemView
    {
        public Aow.Units.FixedArray_byte_24 Reserved00;
        public Aow.Rtl.AnsiString Name;
        public Aow.Units.FixedArray_byte_24 Reserved1C;
        public byte Kind;
        public Aow.Units.FixedArray_byte_15 Reserved35;
        public bool Hidden;
    }

    public static partial class LeadrGrd
    {
        // LeadrGrd.pas:271
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.LeadrGrd.TLeaderGrid)]);
        }

        public static readonly Aow.Rtl.AnsiString GridUsedRStr = (AnsiString)"Used";
        public static readonly Aow.Rtl.AnsiString GridPlayerRStr = (AnsiString)"Player";
        // LeadrGrd.pas:79
        // { needs: typed results for the existing draft methods. These tail jumps keep their ABI. }
        public static Aow.Units.AoWE.THero HeroAt(Aow.Units.AoWE.THeroList List, int Index)
        {
            Aow.Units.AoWE.THero Result = default;
            Result = List.GetHero(Index);
            return Result;
        }

        // LeadrGrd.pas:81
        public static Aow.Units.AoWE.THero ControlHeroAt(Aow.Units.AoWE.THeroControl Control, int Index)
        {
            Aow.Units.AoWE.THero Result = default;
            Result = Control.GetHero(Index);
            return Result;
        }

        // LeadrGrd.pas:83
        public static int HeroCount(Aow.Units.AoWE.THeroControl Control)
        {
            int Result = default;
            Result = Control.GetCount();
            return Result;
        }

        // LeadrGrd.pas:85
        public static int HeroIndexOf(Aow.Units.AoWE.THeroControl Control, Aow.Units.AoWE.THero Hero)
        {
            int Result = default;
            Result = Control.IndexOf(Hero);
            return Result;
        }

        // LeadrGrd.pas:87
        public static sbyte HeroLevel(Aow.Units.AoWE.THero Hero)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)Hero.GetLevel());
            return Result;
        }

        // LeadrGrd.pas:89
        public static Aow.Units.AoWE.TItem ItemAt(Aow.Units.AoWE.TItemList List, int Index)
        {
            Aow.Units.AoWE.TItem Result = default;
            Result = List.GetItem(Index);
            return Result;
        }

        // LeadrGrd.pas:91
        public static Aow.Units.AoWE.TItem ControlItemAt(Aow.Units.AoWE.TItemControl Control, int Index)
        {
            Aow.Units.AoWE.TItem Result = default;
            Result = Control.GetItem(Index);
            return Result;
        }

        // LeadrGrd.pas:93
        public static int ItemCount(Aow.Units.AoWE.TItemControl Control)
        {
            int Result = default;
            Result = Control.GetCount();
            return Result;
        }

        // LeadrGrd.pas:95
        public static Aow.Units.AoWE.TPlayer PlayerAt(Aow.Units.AoWE.TPlayerList List, int Index)
        {
            Aow.Units.AoWE.TPlayer Result = default;
            Result = List.GetPlayers(Index);
            return Result;
        }

        // LeadrGrd.pas:97
        public static int RaceToPlayer(Aow.Units.AoWE.TPlayerList List, byte Race)
        {
            int Result = default;
            Result = List.RaceToPlayer(unchecked((sbyte)Race));
            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
