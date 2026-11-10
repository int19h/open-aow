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

namespace Aow.Game.Ui.Grids
{

    // show level, show status, stream, map
    public partial class TLeaderGrid : TCustomImageDrawGrid
    {
        public bool FShowLevel;
        public bool FReserved1DD;
        public bool FShowStatus;
        public MemoryStream FStream;
        public TAoWHSMap FMap;

        // LeadrGrd.pas:111
        public override void Dispose()
        {
            /* TODO: Implement Grids.Destroy in the platform or editor service. */
            throw new NotImplementedException();
        }

        // LeadrGrd.pas:101
        // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
        public TLeaderGrid(TComponent owner) : base(owner)
        {

            DefaultColWidth = 155;
            DefaultRowHeight = 56;
            GrowDirection = TILGrowDirection.ilVertical;
            ScrollBars = TScrollStyle.ssVertical;
            FStream = new MemoryStream();
        }

        // LeadrGrd.pas:166
        protected internal override void DrawCell(int ACol, int ARow, TRect ARect, TSet<TGridDrawState> AState)
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
        public void SetAoWHSMap(TAoWHSMap Value)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
        }

        // LeadrGrd.pas:134
        public TLeader GetLeader(sbyte Player)
        {
            TLeader Result;
            TAoWHSMap Map;
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
        public TLeader GetSelectedLeader()
        {
            /* TODO: Implement Grids.Col in the platform or editor service. */
            throw new NotImplementedException();
        }

        // LeadrGrd.pas:155
        public void SetSelectedLeader(TLeader Value)
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
            TLeader Leader;
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
    }
}

namespace Aow.Game.Ui.Grids.LeadrGrd
{

    [LegacyObject]
    public partial class TGridMapView
    {
        public TItemControl ItemControl;
        public THeroControl HeroControl;
        public TPlayerList Players;
        public TEventList LeaderEvent;

        public TGridMapView()
        {
            CaptureAddress.Assign(this);
        }
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

    public static partial class LeadrGrdUnit
    {
        public static readonly string GridUsedRStr = "Used";
        public static readonly string GridPlayerRStr = "Player";

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
