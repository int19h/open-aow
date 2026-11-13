// Translated from HSEngine.pas.
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Movement;

// The steps of a path (Delphi TXYLList with the move cost in each item's flag byte), with the
// default item of TXYLList and its run-length encoded stream form.
public class PathStepList : DefaultItemList<PathStep>
{
    // HSEngine.pas:1578
    public PathStepList()
    {
        Default = PathStep.None;
    }

    // HSEngine.pas:1614
    public int AddMapCoordinate(sbyte X, sbyte Y, sbyte L) => Add(new PathStep(Location: new MapCoordinate(X, Y, L), MoveCost: 0));

    // HSEngine.pas:1709
    public int IndexOfMapCoordinate(sbyte X, sbyte Y, sbyte L)
    {
        int result = 0;
        while (result < Count && (List[result].Location.X != X || List[result].Location.Y != Y || List[result].Location.MapLevel != L))
        {
            result += 1;
        }

        if (result == Count)
        {
            result = -1;
        }

        return result;
    }

    // HSEngine.pas:1828
    // The default, the count, then each item, where an item equal to the default is followed by
    // the length of its run of defaults. Items are the four bytes of PathStepImage, compared as
    // steps, which covers all four bytes.
    public void LoadFromStream(Stream stream)
    {
        int count = default;
        int run = default;
        Clear();
        Default = ReadStep(stream);
        stream.ReadExactly(Bytes.Of(ref count, 4));
        SetCapacity(count);
        while (count != Count)
        {
            var item = ReadStep(stream);
            if (item == Default)
            {
                stream.ReadExactly(Bytes.Of(ref run, 4));
                SetCount(Count + run);
            }
            else
            {
                Add(item);
            }
        }
    }

    // HSEngine.pas:1850
    public void SaveToStream(Stream stream)
    {
        int I;
        int run = default;
        int count = Count;
        int DefaultRun()
        {
            int J;
            int result = 0;
            J = I;
            while (J < Count && List[J] == Default)
            {
                result += 1;
                J += 1;
            }

            return result;
        }

        I = 0;
        WriteStep(stream, Default);
        stream.Write(Bytes.Of(ref count, 4));
        while (Count != I)
        {
            run = DefaultRun();
            if (run > 0)
            {
                WriteStep(stream, Default);
                stream.Write(Bytes.Of(ref run, 4));
                I += run;
            }
            else
            {
                WriteStep(stream, List[I]);
                I += 1;
            }
        }
    }

    private static PathStep ReadStep(Stream stream) => PathStepImage.Read(stream).ToValue();

    private static void WriteStep(Stream stream, PathStep step) => PathStepImage.From(step).Write(stream);
}
