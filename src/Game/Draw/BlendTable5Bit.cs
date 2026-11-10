// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// Table (32 x 32 bytes, row I at byte I * 32)
public partial class BlendTable5Bit : BlendTable
{
    // The rows of the table, so Table[I][J] reads and writes the byte of `Table[I, J]`.
    public readonly struct Rows(byte[] bytes)
    {
        public Span<byte> this[int I] => bytes.AsSpan(I * 32, 32);
    }

    public new Rows Table => new(FTable);

    // The 32 by 32 bytes of the table in one span, as the blenders read it.
    public ReadOnlySpan<byte> Bytes => FTable;

    // GFXE.pas:9609
    public BlendTable5Bit()
    {
        Initialize(5, new byte[32 * 32]);
    }

    // GFXE.pas:9621
    public override BlendTable Copy()
    {
        BlendTable Result = new BlendTable5Bit();
        Result.Assign(this);
        return Result;
    }

}
