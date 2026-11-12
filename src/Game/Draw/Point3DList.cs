
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FList, the owned points
public partial class Point3DList : IDisposable
{

    internal List<Point3D> _list;

    // aowFX.pas:93
    public Point3DList()
    {
        _list = new List<Point3D>();
    }

    // aowFX.pas:98
    public virtual void Dispose()
    {
        for (int I = 0, last = _list.Count - 1; I <= last; I++)
        {
            _ = GetXYZ(I);
        }

        _list?.Clear();
    }

    // aowFX.pas:108
    public void Clear()
    {
        for (int I = 0, last = _list.Count - 1; I <= last; I++)
        {
            _ = GetXYZ(I);
        }

        _list.Clear();
    }

    // aowFX.pas:117
    public void Add(Point3D xYZ)
    {
        _list.Add(xYZ);
    }

    // aowFX.pas:122
    public void Delete(int index)
    {
        _list.RemoveAt(index);
    }

    // aowFX.pas:128
    public Point3D GetXYZ(int index)
    {
        Point3D result;
        if (index >= 0 && index < _list.Count)
        {
            result = _list[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // aowFX.pas:136
    public int GetCount() => _list.Count;

    public int Count { get => GetCount(); }

    public Point3D this[int index] { get => GetXYZ(index); }

    internal static void ScatterPath(ref Point3DList list)
    {
        int rx;
        int ry;
        int deltaX;
        int deltaY;
        Point3D xYZ;
        deltaX = 0;
        deltaY = 0;
        for (int I = 1, last = list.Count - 2; I <= last; I++)
        {
            xYZ = list[I];
            rx = Random(3);
            ry = Random(3);
            if (rx - deltaX > 1)
            {
                rx = deltaX + 1;
            }

            if (deltaX - rx > 1)
            {
                rx = deltaX - 1;
            }

            if (ry - deltaY > 1)
            {
                ry = deltaY + 1;
            }

            if (deltaY - ry > 1)
            {
                ry = deltaY - 1;
            }

            xYZ.X = xYZ.X + rx - 1;
            xYZ.Y = xYZ.Y + ry - 1;
            deltaX = rx;
            deltaY = ry;
        }
    }

}
