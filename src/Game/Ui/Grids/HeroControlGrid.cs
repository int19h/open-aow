// Translated from HeroCtrl.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Ui.Grids;

// indices, show level, show status, stream, map, sort, unselect event
public partial class HeroControlGrid : CustomImageDrawGrid
{
    public IntegerList FIndices;
    public bool FShowLevel;
    public bool FReservedDisplay;
    public bool FShowStatus;
    public bool FUnplacedOnly;
    public MemoryStream FStream;
    public AoWHSMap FMap;
    public byte FSort;
    public NotifyEvent<object> FOnUnselect;

    // HeroCtrl.pas:134
    public override void Dispose()
    {
        /* TODO: Implement Grids.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // HeroCtrl.pas:122
    // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
    public HeroControlGrid(Component owner) : base(owner)
    {

        DefaultColWidth = 155;
        DefaultRowHeight = 46;
        GrowDirection = ILGrowDirection.Vertical;
        ScrollBars = CustomGrid.ScrollStyle.Vertical;
        FIndices = new IntegerList();
        FStream = new MemoryStream();
        FSort = 0;
    }

    // HeroCtrl.pas:263
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    // HeroCtrl.pas:142
    protected internal override void MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        /* TODO: Implement Grids.MouseCoord in the platform or editor service. */
        throw new NotImplementedException();
    }

    // HeroCtrl.pas:156
    public void SetAoWHSMap(AoWHSMap Value)
    {
        if (Value != FMap)
        {
            if (FMap != null)
            {
                FMap.FHeroControl.FOnChanged -= HeroControlChanged;
                FMap.FHeroControl.FOnHeroChanged -= HeroChanged;
            }

            FMap = Value;
            if (FMap != null)
            {
                FMap.FHeroControl.FOnChanged += HeroControlChanged;
                FMap.FHeroControl.FOnHeroChanged += HeroChanged;
            }

            UpdateHeroes();
        }
    }

    // HeroCtrl.pas:175
    public void HeroControlChanged(object Sender)
    {
        UpdateHeroes();
    }

    // HeroCtrl.pas:180
    public void HeroChanged(object Sender)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // HeroCtrl.pas:186
    public Hero GetHero(int Index)
    {
        Hero Result;
        // { Original checks only the upper bound. }
        if (Index < FIndices.Count)
        {
            var indice = FIndices[Index];
            Result = FMap.FHeroControl.GetHero(indice);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // HeroCtrl.pas:193
    public Hero GetSelectedHero()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // HeroCtrl.pas:202
    public void SetSelectedHero(Hero Value)
    {
        int I;
        I = FMap.FHeroControl.IndexOf(Value);
        if (I != -1)
        {
            I = FIndices.IndexOf(I);
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
    public NotifyEvent<object> OnUnselect { get => FOnUnselect; set => FOnUnselect = value; }

    [Published]
    public bool ShowHeroLevel { get => FShowLevel; set => FShowLevel = value; }

    public partial class GridMapView
    {
        public ItemControl ItemControl;
        public HeroControl HeroControl;
        public PlayerList Players;
        public EventList LeaderEvent;
    }

    public struct RawView
    {
        public FixedArray_object_512 Fields;
    }

    public struct NodeView
    {
        public FixedArray_byte_8 Reserved;
        public DelphiList Children;
    }

    [StructLayout(LayoutKind.Explicit, Size = 49)]
    public struct EngineView
    {
        [FieldOffset(0)]
        public FixedArray_byte_48 Reserved;
        [FieldOffset(48)]
        public byte Status;
    }

    public struct HeroView
    {
        public FixedArray_byte_96 Reserved00;
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }

        public FixedArray_byte_4 Reserved64;
        public byte Race;
        public FixedArray_byte_15 Reserved69;
        public bool Hidden;
    }

    public struct ItemView
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
