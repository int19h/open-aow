using Aow.Rtl;
namespace Aow.Graphics;

public sealed record StretchRows(Ptr<byte> Source, int[] Offsets);
