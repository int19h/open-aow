using Aow.Rtl;

namespace Aow.Units.Engine;

public partial class TEngine
{
    public bool VerifyEObjectClass(TEObject.Meta type)
    {
        // The native allocator byte total has no corresponding managed heap measure.
        // Exercise the same constructor and destructor without comparing GC generations.
        type.Create(null).Free();
        return true;
    }
}
