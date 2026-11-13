using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

namespace Aow.Game.World.Movement;

// The move points of each terrain and overlay (Delphi TMovePointTable, array[0..15, -1..14] of
// Shortint). A table is a value: the code copies whole tables. The overlay index starts at -1, so an
// index subtracts MovementTables.FirstOverlay: table[terrain][overlay - FirstOverlay].
[InlineArray(TerrainCount)]
public struct MovePointTable
{
    internal const int TerrainCount = 16;
    internal const int OverlayCount = 16;

    // The entry count, the byte size of the Delphi table.
    internal const int EntryCount = TerrainCount * OverlayCount;

    private Array16<sbyte> _terrain0;

    // Every entry set to one value (the Delphi FillChar of the whole table).
    public void Fill(sbyte points)
    {
        for (int terrain = 0; terrain < TerrainCount; terrain++)
        {
            for (int overlay = 0; overlay < OverlayCount; overlay++)
            {
                this[terrain][overlay] = points;
            }
        }
    }

    // MoveT.PAS:1, MaximizeMovePointTable: each entry of overlays -1 to 11 takes the larger cost of
    // the two tables, and -1 (impassable) in either table wins. Overlays 12 to 14 keep this table's
    // entry.
    public void Maximize(in MovePointTable other)
    {
        for (int overlay = 0; overlay <= 12; overlay++)
        {
            for (int terrain = 0; terrain < TerrainCount; terrain++)
            {
                sbyte oldValue = this[terrain][overlay];
                sbyte value = other[terrain][overlay];
                if (oldValue != -1 && (oldValue < value || value == -1))
                {
                    this[terrain][overlay] = value;
                }
            }
        }
    }

    // The entry at a byte offset of the Delphi table, row by row: terrain offset / 16, overlay index
    // offset % 16. The original reads some tables through a view that starts one byte into the table
    // (Cave.pas:269, GlobalTargetSpells.pas:1834), and this names that read.
    public readonly sbyte AtOffset(int offset) => this[offset / OverlayCount][offset % OverlayCount];
}
