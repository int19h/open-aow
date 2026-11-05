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
            FIndices = new TIntegerList();
            FStream = new TMemoryStream();
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
            THero Result;
            // { Original checks only the upper bound. }
            if (Index < FIndices.Count)
            {
                var indice = FIndices[Index];
                Result = FHeroes.GetHero(indice);
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
            int I;
            I = FHeroes.FChildren.IndexOf(Value);
            if (I != -1)
            {
                I = FIndices.IndexOf(I);
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
            THero Hero;
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
            var hero = SortHeroList.GetHero(B);
            int Result = CompareText(SortHeroList.GetHero(A).FHeroName, hero.FHeroName);
            // { Original uses object addresses to break ties. }
            if (Result == 0)
            {
                var sortHeroListHero = SortHeroList.GetHero(A);
                var hero2 = SortHeroList.GetHero(B);
                Result = ObjectToInt(sortHeroListHero) - ObjectToInt(hero2);
            }

            return Result;
        }

        // HeroGrid.pas:108
        public static int HeroGridCompareLevel(int A, int B)
        {
            int Result = SortHeroList.GetHero(A).GetLevel() - SortHeroList.GetHero(B).GetLevel();
            if (Result == 0)
            {
                var hero = SortHeroList.GetHero(A);
                var sortHeroListHero = SortHeroList.GetHero(B);
                Result = ObjectToInt(hero) - ObjectToInt(sortHeroListHero);
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
