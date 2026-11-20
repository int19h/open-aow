namespace Aow.Rtl;

// A rectangle (Delphi TRect). RectImage is its stored record.
public struct Rect(int left, int top, int right, int bottom)
{
    public int Left { get; set; } = left;
    public int Top { get; set; } = top;
    public int Right { get; set; } = right;
    public int Bottom { get; set; } = bottom;

    public readonly bool Intersect(in Rect right, out Rect result)
    {
        Rect intersection = new() { Left = Math.Max(Left, right.Left), Top = Math.Max(Top, right.Top), Right = Math.Min(Right, right.Right), Bottom = Math.Min(Bottom, right.Bottom) };
        bool nonempty = intersection.Right > intersection.Left && intersection.Bottom > intersection.Top;
        result = nonempty ? intersection : default;
        return nonempty;
    }

    public readonly bool Union(in Rect right, out Rect result)
    {
        bool a = Right > Left && Bottom > Top, b = right.Right > right.Left && right.Bottom > right.Top;
        result = !a ? b ? right : default : !b ? this : new Rect { Left = Math.Min(Left, right.Left), Top = Math.Min(Top, right.Top), Right = Math.Max(Right, right.Right), Bottom = Math.Max(Bottom, right.Bottom) };
        return a || b;
    }
}
