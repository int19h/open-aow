
namespace Aow.Game.Tests;

public sealed class InstalledFactAttribute : FactAttribute
{
    public InstalledFactAttribute()
    {
        if (InstalledSave.SkipReason is string reason) Skip = reason;
    }
}
