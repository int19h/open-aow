
namespace Aow.Formats.Tests;

/// <summary>Every test sets the process error mode first, so a crash ends the process silently.</summary>
public abstract class FormatsTest
{
    protected FormatsTest() => ProcessSafety.Initialize();
}
