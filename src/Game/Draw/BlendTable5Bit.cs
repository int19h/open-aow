// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// Table (32 x 32 bytes, row I at byte I * 32)
public partial class BlendTable5Bit : BlendTable
{
    // The rows of the table. RowView[I][J] reads and writes the byte at row I and column J.
    public readonly struct Rows(byte[] bytes)
    {
        public Span<byte> this[int I] => bytes.AsSpan(I * 32, 32);
    }

    public Rows RowView => new(Table);

    // GFXE.pas:9609
    public BlendTable5Bit()
    {
        Initialize(5, new byte[32 * 32]);
    }

    // GFXE.pas:9621
    public override BlendTable Copy()
    {
        BlendTable result = new BlendTable5Bit();
        result.Assign(this);
        return result;
    }

}
