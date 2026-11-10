// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Collections;

// the abstract table interface
public partial class StringHashTable
{
    // Engine.pas:804
    public virtual void Add(object Item, string Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:805
    public virtual void Delete(string Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:806
    public virtual object Find(string Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:807
    public virtual void Clear()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
