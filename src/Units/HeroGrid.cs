// Translated from HeroGrid.pas.
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

namespace Aow.Units.HeroGrid
{
    // { THeroGrid (instance size 496, VMT 558012E8); $1DC indices, $1E0 heroes, $1E4 show level, $1E6 show status, $1E8 stream, $1EC sort. }
    public partial class THeroGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
        public Aow.Units.Engine.TIntegerList FIndices;
        // { +$1DC }
        public Aow.Units.AoWE.THeroList FHeroes;
        // { +$1E0 }
        public bool FShowLevel;
        // { $1E4 }
        public bool FEditable;
        // { $1E5; AoWEd 00424B1C/00424BEA }
        public bool FShowStatus;
        // { $1E6 }
        public bool FUnplacedOnly;
        // { +$1E7 }
        public Aow.Rtl.TMemoryStream FStream;
        // { +$1E8 }
        public byte FSort;
        // { +$1EC }
        public Aow.Units.FixedArray_byte_3 FReserved1ED;
        // HeroGrid.pas:126
        public override void Destroy()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Destroy */
            throw new System.NotImplementedException();
        }

        // HeroGrid.pas:114
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
            FSort = 1;
        }

        // HeroGrid.pas:226
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Canvas */
            throw new System.NotImplementedException();
        }

        // HeroGrid.pas:134
        public override void DeleteCell()
        {
            if (EditMode.Contains(Aow.Units.ImageLib.TILGridEditModeElement.igDelete))
            {
                DeleteSelectedHero();
            }
        }

        // HeroGrid.pas:139
        public override void DelCell()
        {
            if (EditMode.Contains(Aow.Units.ImageLib.TILGridEditModeElement.igDel))
            {
                DeleteSelectedHero();
            }
        }

        // HeroGrid.pas:144
        public Aow.Units.AoWE.THero GetHero(int Index)
        {
            Aow.Units.AoWE.THero Result = default;
            // { Original checks only the upper bound. }
            if (Index < FIndices.Count)
            {
                var call__3 = FIndices[Index];
                var call__4 = Aow.Units.HeroGrid.HeroGrid.HeroAt(FHeroes, call__3);
                Result = call__4;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HeroGrid.pas:151
        public Aow.Units.AoWE.THero GetSelectedHero()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Col */
            throw new System.NotImplementedException();
        }

        // HeroGrid.pas:160
        public void SetSelectedHero(Aow.Units.AoWE.THero Value)
        {
            int I = default;
            var call__5 = FHeroes.FChildren.IndexOf(Value);
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

        // HeroGrid.pas:172
        public void SortHeroes(byte Mode)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // HeroGrid.pas:183
        public void UpdateHeroes()
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // HeroGrid.pas:202
        public void SortOnName()
        {
            SortHeroes(0);
        }

        // HeroGrid.pas:207
        public void SortOnLevel()
        {
            SortHeroes(1);
        }

        // HeroGrid.pas:212
        public void HeroesUpdated(Aow.Rtl.TObject Sender)
        {
            UpdateHeroes();
        }

        // HeroGrid.pas:217
        public void SetHeroes(Aow.Units.AoWE.THeroList Value)
        {
            if (Value != FHeroes)
            {
                FHeroes = Value;
                UpdateHeroes();
            }
        }

        // HeroGrid.pas:308
        public void DeleteHero(int Index)
        {
            Aow.Units.AoWE.THero Hero = default;
            Hero = GetHero(Index);
            if (Hero != null)
            {
                FHeroes.RemoveChild(Hero);
                UpdateHeroes();
            }
        }

        // HeroGrid.pas:320
        public void DeleteSelectedHero()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Col */
            throw new System.NotImplementedException();
        }

        // { AoWEd 00424BEA }
        [Published]
        public bool EditHeroEnabled { get => FEditable; set => FEditable = value; }

        [Published]
        public bool ShowStatus { get => FShowStatus; set => FShowStatus = value; }

        [Published]
        public bool OnlyFreeHeroes { get => FUnplacedOnly; set => FUnplacedOnly = value; }

        [Published]
        public bool ShowHeroLevel { get => FShowLevel; set => FShowLevel = value; }

        // HeroGrid.pas:29
        public static new Aow.Units.HeroGrid.THeroGrid Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.HeroGrid.THeroGrid o = new Aow.Units.HeroGrid.THeroGrid();
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

    public static partial class HeroGrid
    {
        // HeroGrid.pas:325
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.HeroGrid.THeroGrid)]);
        }

        public static readonly Aow.Rtl.AnsiString GridUsedRStr = (AnsiString)"Used";
        // HeroGrid.pas:79
        // { needs: typed results for the existing draft methods. These tail jumps keep their ABI. }
        public static Aow.Units.AoWE.THero HeroAt(Aow.Units.AoWE.THeroList List, int Index)
        {
            Aow.Units.AoWE.THero Result = default;
            Result = List.GetHero(Index);
            return Result;
        }

        // HeroGrid.pas:81
        public static Aow.Units.AoWE.THero ControlHeroAt(Aow.Units.AoWE.THeroControl Control, int Index)
        {
            Aow.Units.AoWE.THero Result = default;
            Result = Control.GetHero(Index);
            return Result;
        }

        // HeroGrid.pas:83
        public static int HeroCount(Aow.Units.AoWE.THeroControl Control)
        {
            int Result = default;
            Result = Control.GetCount();
            return Result;
        }

        // HeroGrid.pas:85
        public static int HeroIndexOf(Aow.Units.AoWE.THeroControl Control, Aow.Units.AoWE.THero Hero)
        {
            int Result = default;
            Result = Control.IndexOf(Hero);
            return Result;
        }

        // HeroGrid.pas:87
        public static sbyte HeroLevel(Aow.Units.AoWE.THero Hero)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)Hero.GetLevel());
            return Result;
        }

        // HeroGrid.pas:89
        public static Aow.Units.AoWE.TItem ItemAt(Aow.Units.AoWE.TItemList List, int Index)
        {
            Aow.Units.AoWE.TItem Result = default;
            Result = List.GetItem(Index);
            return Result;
        }

        // HeroGrid.pas:91
        public static Aow.Units.AoWE.TItem ControlItemAt(Aow.Units.AoWE.TItemControl Control, int Index)
        {
            Aow.Units.AoWE.TItem Result = default;
            Result = Control.GetItem(Index);
            return Result;
        }

        // HeroGrid.pas:93
        public static int ItemCount(Aow.Units.AoWE.TItemControl Control)
        {
            int Result = default;
            Result = Control.GetCount();
            return Result;
        }

        // HeroGrid.pas:95
        public static Aow.Units.AoWE.TPlayer PlayerAt(Aow.Units.AoWE.TPlayerList List, int Index)
        {
            Aow.Units.AoWE.TPlayer Result = default;
            Result = List.GetPlayers(Index);
            return Result;
        }

        // HeroGrid.pas:97
        public static int RaceToPlayer(Aow.Units.AoWE.TPlayerList List, byte Race)
        {
            int Result = default;
            Result = List.RaceToPlayer(unchecked((sbyte)Race));
            return Result;
        }

        public static Aow.Units.AoWE.THeroList SortHeroList;
        // HeroGrid.pas:102
        public static int HeroGridCompareName(int A, int B)
        {
            int Result = default;
            var call__7 = Aow.Units.HeroGrid.HeroGrid.HeroAt(Aow.Units.HeroGrid.HeroGrid.SortHeroList, B);
            var call__8 = Aow.Units.HeroGrid.HeroGrid.HeroAt(Aow.Units.HeroGrid.HeroGrid.SortHeroList, A);
            Result = Aow.Rtl.Rtl.CompareText(call__8.FHeroName, call__7.FHeroName);
            // { Original uses object addresses to break ties. }
            if (Result == 0)
            {
                var call__9 = Aow.Units.HeroGrid.HeroGrid.HeroAt(Aow.Units.HeroGrid.HeroGrid.SortHeroList, A);
                var call__10 = Aow.Units.HeroGrid.HeroGrid.HeroAt(Aow.Units.HeroGrid.HeroGrid.SortHeroList, B);
                Result = Aow.Rtl.Rtl.ObjectToInt(call__9) - Aow.Rtl.Rtl.ObjectToInt(call__10);
            }

            return Result;
        }

        // HeroGrid.pas:108
        public static int HeroGridCompareLevel(int A, int B)
        {
            int Result = default;
            var call__11 = Aow.Units.HeroGrid.HeroGrid.HeroAt(Aow.Units.HeroGrid.HeroGrid.SortHeroList, A);
            var call__12 = Aow.Units.HeroGrid.HeroGrid.HeroLevel(call__11);
            var call__13 = Aow.Units.HeroGrid.HeroGrid.HeroAt(Aow.Units.HeroGrid.HeroGrid.SortHeroList, B);
            var call__14 = Aow.Units.HeroGrid.HeroGrid.HeroLevel(call__13);
            Result = call__12 - call__14;
            if (Result == 0)
            {
                var call__15 = Aow.Units.HeroGrid.HeroGrid.HeroAt(Aow.Units.HeroGrid.HeroGrid.SortHeroList, A);
                var call__16 = Aow.Units.HeroGrid.HeroGrid.HeroAt(Aow.Units.HeroGrid.HeroGrid.SortHeroList, B);
                Result = Aow.Rtl.Rtl.ObjectToInt(call__15) - Aow.Rtl.Rtl.ObjectToInt(call__16);
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
