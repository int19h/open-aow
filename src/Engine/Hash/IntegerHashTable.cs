
using Aow.Rtl;

namespace Aow.Engine.Hash;

// the abstract table interface
public partial class IntegerHashTable
{
    // HashTabU.pas:47
    public virtual void Add(object Item, int Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // HashTabU.pas:48
    public virtual void Delete(int Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // HashTabU.pas:49
    public virtual object Find(int Key)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // HashTabU.pas:50
    public virtual void Clear()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
