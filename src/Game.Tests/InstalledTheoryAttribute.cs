
namespace Aow.Game.Tests;

public sealed class InstalledTheoryAttribute : TheoryAttribute
{
    public InstalledTheoryAttribute()
    {
        if (InstalledSave.SkipReason is string reason) Skip = reason;
    }
}
