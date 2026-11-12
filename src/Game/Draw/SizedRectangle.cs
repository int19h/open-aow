namespace Aow.Game.Draw;

// A rectangle given by its left and top corner and its size. The interface image library draws
// into one. The Delphi code passed a TRect whose Right and Bottom carried the width and height.
public readonly record struct SizedRectangle(int Left, int Top, int Width, int Height);
