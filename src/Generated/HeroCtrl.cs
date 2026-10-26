// Generated from HeroCtrl.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.HeroCtrl
{
    // { THeroControlGrid (instance size 504, VMT 55808AFC); $1DC indices, $1E0 show level, $1E2 show status, $1E4 stream, $1E8 map, $1EC sort, $1F0 unselect event. }
    public partial class THeroControlGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
        public Aow.Units.Engine.TIntegerList FIndices;
        // { +$1DC }
        public bool FShowLevel;
        // { $1E0 }
        public bool FReservedDisplay;
        public bool FShowStatus;
        // { $1E2 }
        public bool FUnplacedOnly;
        // { +$1E3 }
        public Aow.Rtl.TMemoryStream FStream;
        // { +$1E4 }
        public Aow.Units.AoWE.TAoWHSMap FMap;
        // { +$1E8 }
        public byte FSort;
        // { +$1EC }
        public Aow.Units.FixedArray_byte_3 FReserved1ED;
        public Aow.Rtl.TNotifyEvent FOnUnselect;
        // HeroCtrl.pas:134
        // { +$1F0 }
        public override void Destroy()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Destroy */
            throw new System.NotImplementedException();
        }

        // HeroCtrl.pas:122
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            DefaultColWidth = 155;
            DefaultRowHeight = 46;
            GrowDirection = Aow.Units.ImageLib.TILGrowDirection.ilVertical;
            ScrollBars = Aow.Rtl.TScrollStyle.ssVertical;
            var call__1 = Aow.Units.Engine.TIntegerList.Create();
            FIndices = call__1;
            var call__2 = new Aow.Rtl.TMemoryStream();
            FStream = call__2;
            FSort = 0;
        }

        // HeroCtrl.pas:263
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Canvas */
            throw new System.NotImplementedException();
        }

        // HeroCtrl.pas:142
        protected internal override void MouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            /* TODO(T-shim) Platform or editor boundary Grids.MouseCoord */
            throw new System.NotImplementedException();
        }

        // HeroCtrl.pas:156
        public void SetAoWHSMap(Aow.Units.AoWE.TAoWHSMap Value)
        {
            if (Value != FMap)
            {
                if (FMap != null)
                {
                    FMap.FHeroControl.FOnChanged.Remove(HeroControlChanged);
                    FMap.FHeroControl.FOnHeroChanged.Remove(HeroChanged);
                }

                FMap = Value;
                if (FMap != null)
                {
                    FMap.FHeroControl.FOnChanged.Add(HeroControlChanged);
                    FMap.FHeroControl.FOnHeroChanged.Add(HeroChanged);
                }

                UpdateHeroes();
            }
        }

        // HeroCtrl.pas:175
        public void HeroControlChanged(Aow.Rtl.TObject Sender)
        {
            UpdateHeroes();
        }

        // HeroCtrl.pas:180
        public void HeroChanged(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // HeroCtrl.pas:186
        public Aow.Units.AoWE.THero GetHero(int Index)
        {
            Aow.Units.AoWE.THero Result = default;
            // { Original checks only the upper bound. }
            if (Index < FIndices.Count)
            {
                var call__3 = FIndices[Index];
                var call__4 = Aow.Units.HeroCtrl.HeroCtrl.ControlHeroAt(FMap.FHeroControl, call__3);
                Result = call__4;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HeroCtrl.pas:193
        public Aow.Units.AoWE.THero GetSelectedHero()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Col */
            throw new System.NotImplementedException();
        }

        // HeroCtrl.pas:202
        public void SetSelectedHero(Aow.Units.AoWE.THero Value)
        {
            int I = default;
            var call__5 = Aow.Units.HeroCtrl.HeroCtrl.HeroIndexOf(FMap.FHeroControl, Value);
            I = call__5;
            if (I != -1)
            {
                var call__6 = FIndices.IndexOf(I);
                I = call__6;
                if (I != -1)
                {
                    SelectIndex(I);
                }
            }
        }

        // HeroCtrl.pas:214
        public void Unselect()
        {
            SelectionEnabled = false;
            if (FOnUnselect != null)
            {
                FOnUnselect(this);
            }
        }

        // HeroCtrl.pas:220
        public void UpdateHeroes()
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // HeroCtrl.pas:245
        public void SortOnName()
        {
            if (FSort != 0)
            {
                FSort = 0;
                UpdateHeroes();
            }
        }

        // HeroCtrl.pas:254
        public void SortOnLevel()
        {
            if (FSort != 1)
            {
                FSort = 1;
                UpdateHeroes();
            }
        }

        // HeroCtrl.pas:356
        public void DeleteHero()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Bottom */
            throw new System.NotImplementedException();
        }

        // HeroCtrl.pas:379
        public void RemoveFromMap()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Top */
            throw new System.NotImplementedException();
        }

        [Published]
        public bool EditHeroEnabled { get => FReservedDisplay; set => FReservedDisplay = value; }

        [Published]
        public bool ShowStatus { get => FShowStatus; set => FShowStatus = value; }

        [Published]
        public bool OnlyFreeHeroes { get => FUnplacedOnly; set => FUnplacedOnly = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnUnselect { get => FOnUnselect; set => FOnUnselect = value; }

        [Published]
        public bool ShowHeroLevel { get => FShowLevel; set => FShowLevel = value; }

        // HeroCtrl.pas:30
        public static new Aow.Units.HeroCtrl.THeroControlGrid Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.HeroCtrl.THeroControlGrid o = new Aow.Units.HeroCtrl.THeroControlGrid();
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

    public static partial class HeroCtrl
    {
        // HeroCtrl.pas:397
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.HeroCtrl.THeroControlGrid)]);
        }

        public static readonly Aow.Rtl.AnsiString GridUsedRStr = (AnsiString)"Used";
        // HeroCtrl.pas:89
        // { needs: typed results for the existing draft methods. These tail jumps keep their ABI. }
        public static Aow.Units.AoWE.THero HeroAt(Aow.Units.AoWE.THeroList List, int Index)
        {
            Aow.Units.AoWE.THero Result = default;
            Result = List.GetHero(Index);
            return Result;
        }

        // HeroCtrl.pas:91
        public static Aow.Units.AoWE.THero ControlHeroAt(Aow.Units.AoWE.THeroControl Control, int Index)
        {
            Aow.Units.AoWE.THero Result = default;
            Result = Control.GetHero(Index);
            return Result;
        }

        // HeroCtrl.pas:93
        public static int HeroCount(Aow.Units.AoWE.THeroControl Control)
        {
            int Result = default;
            Result = Control.GetCount();
            return Result;
        }

        // HeroCtrl.pas:95
        public static int HeroIndexOf(Aow.Units.AoWE.THeroControl Control, Aow.Units.AoWE.THero Hero)
        {
            int Result = default;
            Result = Control.IndexOf(Hero);
            return Result;
        }

        // HeroCtrl.pas:97
        public static sbyte HeroLevel(Aow.Units.AoWE.THero Hero)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)Hero.GetLevel());
            return Result;
        }

        // HeroCtrl.pas:99
        public static Aow.Units.AoWE.TItem ItemAt(Aow.Units.AoWE.TItemList List, int Index)
        {
            Aow.Units.AoWE.TItem Result = default;
            Result = List.GetItem(Index);
            return Result;
        }

        // HeroCtrl.pas:101
        public static Aow.Units.AoWE.TItem ControlItemAt(Aow.Units.AoWE.TItemControl Control, int Index)
        {
            Aow.Units.AoWE.TItem Result = default;
            Result = Control.GetItem(Index);
            return Result;
        }

        // HeroCtrl.pas:103
        public static int ItemCount(Aow.Units.AoWE.TItemControl Control)
        {
            int Result = default;
            Result = Control.GetCount();
            return Result;
        }

        // HeroCtrl.pas:105
        public static Aow.Units.AoWE.TPlayer PlayerAt(Aow.Units.AoWE.TPlayerList List, int Index)
        {
            Aow.Units.AoWE.TPlayer Result = default;
            Result = List.GetPlayers(Index);
            return Result;
        }

        // HeroCtrl.pas:107
        public static int RaceToPlayer(Aow.Units.AoWE.TPlayerList List, byte Race)
        {
            int Result = default;
            Result = List.RaceToPlayer(unchecked((sbyte)Race));
            return Result;
        }

        // HeroCtrl.pas:110
        public static int HeroControlCompareName(int A, int B)
        {
            int Result = default;
            var call__7 = Aow.Units.HeroCtrl.HeroCtrl.ControlHeroAt(Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl, B);
            var call__8 = Aow.Units.HeroCtrl.HeroCtrl.ControlHeroAt(Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl, A);
            Result = Aow.Rtl.Rtl.CompareText(call__8.FHeroName, call__7.FHeroName);
            return Result;
        }

        // HeroCtrl.pas:115
        public static int HeroControlCompareLevel(int A, int B)
        {
            int Result = default;
            var call__9 = Aow.Units.HeroCtrl.HeroCtrl.ControlHeroAt(Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl, A);
            var call__10 = Aow.Units.HeroCtrl.HeroCtrl.HeroLevel(call__9);
            var call__11 = Aow.Units.HeroCtrl.HeroCtrl.ControlHeroAt(Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl, B);
            var call__12 = Aow.Units.HeroCtrl.HeroCtrl.HeroLevel(call__11);
            Result = call__10 - call__12;
            if (Result == 0)
            {
                Result = Aow.Units.HeroCtrl.HeroCtrl.HeroControlCompareName(A, B);
            }

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
