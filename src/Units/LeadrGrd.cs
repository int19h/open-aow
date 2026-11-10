// Translated from LeadrGrd.pas.
using System.Runtime.InteropServices;
using Aow.Game.Draw.Images;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

// show level, show status, stream, map
public partial class LeaderGrid : CustomImageDrawGrid
{
    public bool FShowLevel;
    public bool FReserved1DD;
    public bool FShowStatus;
    public MemoryStream FStream;
    public AoWHSMap FMap;

    // LeadrGrd.pas:111
    public override void Dispose()
    {
        /* TODO: Implement Grids.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // LeadrGrd.pas:101
    // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
    public LeaderGrid(Component owner) : base(owner)
    {

        DefaultColWidth = 155;
        DefaultRowHeight = 56;
        GrowDirection = ILGrowDirection.Vertical;
        ScrollBars = Aow.Rtl.CustomGrid.ScrollStyle.Vertical;
        FStream = new MemoryStream();
    }

    // LeadrGrd.pas:166
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    // LeadrGrd.pas:118
    public void LeadersUpdated()
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // LeadrGrd.pas:123
    public void SetAoWHSMap(AoWHSMap Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // LeadrGrd.pas:134
    public Leader GetLeader(sbyte Player)
    {
        Leader Result;
        AoWHSMap Map;
        Map = FMap;
        if (Map != null && Player > 0 && Map.FPlayers.Count > Player)
        {
            Result = FMap.FPlayers.GetPlayers(Player).FLeader;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // LeadrGrd.pas:144
    public Leader GetSelectedLeader()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // LeadrGrd.pas:155
    public void SetSelectedLeader(Leader Value)
    {
        int I;
        if (FMap != null)
        {
            I = FMap.FPlayers.RaceToPlayer((sbyte)(byte)Value.FRace);
            if (I != -1)
            {
                SelectIndex(I - 1);
            }
        }
    }

    // LeadrGrd.pas:263
    public void RemoveFromMap()
    {
        Leader Leader;
        Leader = GetSelectedLeader();
        if (Leader != null)
        {
            Leader.SetOwner(null);
        }
    }

    [Published]
    public bool EditLeaderEnabled { get => FReserved1DD; set => FReserved1DD = value; }

    [Published]
    public bool ShowStatus { get => FShowStatus; set => FShowStatus = value; }

    [Published]
    public bool ShowLevel { get => FShowLevel; set => FShowLevel = value; }

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
