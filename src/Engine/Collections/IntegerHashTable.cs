// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Collections;

// the abstract table interface
public partial class IntegerHashTable
{
    // Engine.pas:771
    public virtual void Add(object Item, int Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:772
    public virtual void Delete(int Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:773
    public virtual object Find(int Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:774
    public virtual void Clear()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
