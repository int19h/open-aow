
namespace Aow.Game.Tests;

public sealed class CorpusFactAttribute : FactAttribute
{
    public CorpusFactAttribute()
    {
        if (PersistCorpusTests.SkipReason is string reason) Skip = reason;
    }
}
