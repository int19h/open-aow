
using Aow.Rtl;

namespace Aow.Engine.Hash;

// the abstract table interface
public partial class StringHashTable
{
    // HashTabU.pas:84
    public virtual void Add(object Item, string Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // HashTabU.pas:85
    public virtual void Delete(string Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // HashTabU.pas:86
    public virtual object Find(string Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // HashTabU.pas:87
    public virtual void Clear()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
