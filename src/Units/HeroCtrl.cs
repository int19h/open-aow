// Translated from HeroCtrl.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.ImageLib;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.HeroCtrl
{
    // indices, show level, show status, stream, map, sort, unselect event
    public partial class THeroControlGrid : TCustomImageDrawGrid
    {
        public TIntegerList FIndices;
        public bool FShowLevel;
        public bool FReservedDisplay;
        public bool FShowStatus;
        public bool FUnplacedOnly;
        public TMemoryStream FStream;
        public TAoWHSMap FMap;
        public byte FSort;
        public TNotifyEvent FOnUnselect;
        // HeroCtrl.pas:134
        public override void Destroy()
        {
            /* TODO: Implement Grids.Destroy in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroCtrl.pas:122
        // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
        public THeroControlGrid(TComponent owner) : base(owner)
        {

            DefaultColWidth = 155;
            DefaultRowHeight = 46;
            GrowDirection = TILGrowDirection.ilVertical;
            ScrollBars = TScrollStyle.ssVertical;
            var call__1 = new TIntegerList();
            FIndices = call__1;
            var call__2 = new TMemoryStream();
            FStream = call__2;
            FSort = 0;
        }

        // HeroCtrl.pas:263
        protected internal override void DrawCell(int ACol, int ARow, TRect ARect, TSet<TGridDrawState> AState)
        {
            /* TODO: Implement Controls.Canvas in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroCtrl.pas:142
        protected internal override void MouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            /* TODO: Implement Grids.MouseCoord in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroCtrl.pas:156
        public void SetAoWHSMap(TAoWHSMap Value)
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
        public void HeroControlChanged(TObject Sender)
        {
            UpdateHeroes();
        }

        // HeroCtrl.pas:180
        public void HeroChanged(TObject Sender)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroCtrl.pas:186
        public THero GetHero(int Index)
        {
            THero Result = default;
            // { Original checks only the upper bound. }
            if (Index < FIndices.Count)
            {
                var call__3 = FIndices[Index];
                var call__4 = FMap.FHeroControl.GetHero(call__3);
                Result = call__4;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HeroCtrl.pas:193
        public THero GetSelectedHero()
        {
            /* TODO: Implement Grids.Col in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroCtrl.pas:202
        public void SetSelectedHero(THero Value)
        {
            int I = default;
            var call__5 = FMap.FHeroControl.IndexOf(Value);
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
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
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
            /* TODO: Implement Grids.Bottom in the platform or editor service. */
            throw new NotImplementedException();
        }

        // HeroCtrl.pas:379
        public void RemoveFromMap()
        {
            /* TODO: Implement Grids.Top in the platform or editor service. */
            throw new NotImplementedException();
        }

        [Published]
        public bool EditHeroEnabled { get => FReservedDisplay; set => FReservedDisplay = value; }

        [Published]
        public bool ShowStatus { get => FShowStatus; set => FShowStatus = value; }

        [Published]
        public bool OnlyFreeHeroes { get => FUnplacedOnly; set => FUnplacedOnly = value; }

        [Published]
        public TNotifyEvent OnUnselect { get => FOnUnselect; set => FOnUnselect = value; }

        [Published]
        public bool ShowHeroLevel { get => FShowLevel; set => FShowLevel = value; }
    }

    public partial class TGridMapView : TObject
    {
        public TItemControl ItemControl;
        public THeroControl HeroControl;
        public TPlayerList Players;
        public TEventList LeaderEvent;
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

    public static partial class HeroCtrl
    {
        // HeroCtrl.pas:397
        public static void Register()
        {
            RegisterComponents((AnsiString)"AoW", [typeof(THeroControlGrid)]);
        }

        public static readonly AnsiString GridUsedRStr = (AnsiString)"Used";

        // HeroCtrl.pas:110
        public static int HeroControlCompareName(int A, int B)
        {
            int Result = default;
            var call__7 = AoWHSMap.FHeroControl.GetHero(B);
            var call__8 = AoWHSMap.FHeroControl.GetHero(A);
            Result = CompareText(call__8.FHeroName, call__7.FHeroName);
            return Result;
        }

        // HeroCtrl.pas:115
        public static int HeroControlCompareLevel(int A, int B)
        {
            int Result = default;
            var call__9 = AoWHSMap.FHeroControl.GetHero(A);
            var call__10 = call__9.GetLevel();
            var call__11 = AoWHSMap.FHeroControl.GetHero(B);
            var call__12 = call__11.GetLevel();
            Result = call__10 - call__12;
            if (Result == 0)
            {
                Result = HeroControlCompareName(A, B);
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
