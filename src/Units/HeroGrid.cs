// Translated from HeroGrid.pas.
using System.Runtime.InteropServices;
using Aow.Game.Draw.Images;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids
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
        public MemoryStream FStream;
        public byte FSort;

        // HeroGrid.pas:126
        public override void Dispose()
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
            FStream = new MemoryStream();
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
            FSort = Mode;
            HeroGridUnit.SortHeroList = FHeroes;
            switch (FSort)
            {
                case 0:
                    FIndices.SortStable(HeroGridUnit.HeroGridCompareName);
                    break;
                case 1:
                    FIndices.SortStable(HeroGridUnit.HeroGridCompareLevel);
                    break;
            }

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
        public void HeroesUpdated()
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
}

namespace Aow.Game.Ui.Grids.HeroGrid
{

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
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }

        public FixedArray_byte_4 Reserved64;
        public byte Race;
        public FixedArray_byte_15 Reserved69;
        public bool Hidden;
    }

    public struct TItemView
    {
        public FixedArray_byte_24 Reserved00;
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }

        public FixedArray_byte_24 Reserved1C;
        public byte Kind;
        public FixedArray_byte_15 Reserved35;
        public bool Hidden;
    }
}

namespace Aow.Game.Ui.Grids
{

    public static partial class HeroGridUnit
    {
        public static readonly string GridUsedRStr = "Used";

        public static THeroList SortHeroList;

        // HeroGrid.pas:102
        public static int HeroGridCompareName(int A, int B)
        {
            var hero = SortHeroList.GetHero(B);
            return CompareText(SortHeroList.GetHero(A).FHeroName, hero.FHeroName);
        }

        // HeroGrid.pas:108
        public static int HeroGridCompareLevel(int A, int B)
        {
            return SortHeroList.GetHero(A).GetLevel() - SortHeroList.GetHero(B).GetLevel();
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
