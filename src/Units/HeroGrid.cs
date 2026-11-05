// Translated from HeroGrid.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.ImageLib;
using Aow.Units.AoWE;

namespace Aow.Units.HeroGrid
{
    // indices, heroes, show level, show status, stream, sort
    public partial class THeroGrid : TCustomImageDrawGrid
    {
        public TIntegerList FIndices;
        public THeroList FHeroes;
        public bool FShowLevel;
        public bool FEditable;
        public bool FShowStatus;
        public bool FUnplacedOnly;
        public TMemoryStream FStream;
        public byte FSort;
        // HeroGrid.pas:126
        public override void Destroy()
        {
            /* TODO: Implement Grids.Destroy in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroGrid.pas:114
        // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
        public THeroGrid(TComponent owner) : base(owner)
        {

            DefaultColWidth = 155;
            DefaultRowHeight = 46;
            GrowDirection = TILGrowDirection.ilVertical;
            ScrollBars = TScrollStyle.ssVertical;
            var call__1 = new TIntegerList();
            FIndices = call__1;
            var call__2 = new TMemoryStream();
            FStream = call__2;
            FSort = 1;
        }

        // HeroGrid.pas:226
        protected internal override void DrawCell(int ACol, int ARow, TRect ARect, TSet<TGridDrawState> AState)
        {
            /* TODO: Implement Controls.Canvas in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroGrid.pas:134
        public override void DeleteCell()
        {
            if (EditMode.Contains(TILGridEditModeElement.igDelete))
            {
                DeleteSelectedHero();
            }
        }

        // HeroGrid.pas:139
        public override void DelCell()
        {
            if (EditMode.Contains(TILGridEditModeElement.igDel))
            {
                DeleteSelectedHero();
            }
        }

        // HeroGrid.pas:144
        public THero GetHero(int Index)
        {
            THero Result = default;
            // { Original checks only the upper bound. }
            if (Index < FIndices.Count)
            {
                var call__3 = FIndices[Index];
                var call__4 = FHeroes.GetHero(call__3);
                Result = call__4;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HeroGrid.pas:151
        public THero GetSelectedHero()
        {
            /* TODO: Implement Grids.Col in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroGrid.pas:160
        public void SetSelectedHero(THero Value)
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
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroGrid.pas:183
        public void UpdateHeroes()
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
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
        public void HeroesUpdated(TObject Sender)
        {
            UpdateHeroes();
        }

        // HeroGrid.pas:217
        public void SetHeroes(THeroList Value)
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
            THero Hero = default;
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
            /* TODO: Implement Grids.Col in the platform or editor service. */
            throw new NotImplementedException();
        }

        [Published]
        public bool EditHeroEnabled { get => FEditable; set => FEditable = value; }

        [Published]
        public bool ShowStatus { get => FShowStatus; set => FShowStatus = value; }

        [Published]
        public bool OnlyFreeHeroes { get => FUnplacedOnly; set => FUnplacedOnly = value; }

        [Published]
        public bool ShowHeroLevel { get => FShowLevel; set => FShowLevel = value; }
    }

    public struct TRawView
    {
        public FixedArray_object_512 Fields;
    }

    public struct TNodeView
    {
        public FixedArray_byte_8 Reserved;
        public TList Children;
    }

    [StructLayout(LayoutKind.Explicit, Size = 49)]
    public struct TEngineView
    {
        [FieldOffset(0)]
        public FixedArray_byte_48 Reserved;
        [FieldOffset(48)]
        public byte Status;
    }

    public struct THeroView
    {
        public FixedArray_byte_96 Reserved00;
        public AnsiString Name;
        public FixedArray_byte_4 Reserved64;
        public byte Race;
        public FixedArray_byte_15 Reserved69;
        public bool Hidden;
    }

    public struct TItemView
    {
        public FixedArray_byte_24 Reserved00;
        public AnsiString Name;
        public FixedArray_byte_24 Reserved1C;
        public byte Kind;
        public FixedArray_byte_15 Reserved35;
        public bool Hidden;
    }

    public static partial class HeroGrid
    {
        // HeroGrid.pas:325
        public static void Register()
        {
            RegisterComponents((AnsiString)"AoW", [typeof(THeroGrid)]);
        }

        public static readonly AnsiString GridUsedRStr = (AnsiString)"Used";

        public static THeroList SortHeroList;
        // HeroGrid.pas:102
        public static int HeroGridCompareName(int A, int B)
        {
            int Result = default;
            var call__7 = SortHeroList.GetHero(B);
            var call__8 = SortHeroList.GetHero(A);
            Result = CompareText(call__8.FHeroName, call__7.FHeroName);
            // { Original uses object addresses to break ties. }
            if (Result == 0)
            {
                var call__9 = SortHeroList.GetHero(A);
                var call__10 = SortHeroList.GetHero(B);
                Result = ObjectToInt(call__9) - ObjectToInt(call__10);
            }

            return Result;
        }

        // HeroGrid.pas:108
        public static int HeroGridCompareLevel(int A, int B)
        {
            int Result = default;
            var call__11 = SortHeroList.GetHero(A);
            var call__12 = call__11.GetLevel();
            var call__13 = SortHeroList.GetHero(B);
            var call__14 = call__13.GetLevel();
            Result = call__12 - call__14;
            if (Result == 0)
            {
                var call__15 = SortHeroList.GetHero(A);
                var call__16 = SortHeroList.GetHero(B);
                Result = ObjectToInt(call__15) - ObjectToInt(call__16);
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
